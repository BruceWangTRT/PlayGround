namespace ClassLibraryA
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class DelimitedFile : IDisposable
    {
        #region Static Members

        public static DelimitedFileDefinition DefaultDelimitedFileDefinition { get; set; }
        public static bool UseLambdas { get; set; }
        public static bool UseTasks { get; set; }
        public static bool FastIndexOfAny { get; set; }

        static DelimitedFile()
        {
            DefaultDelimitedFileDefinition = new DelimitedFileDefinition
            {
                EndOfLine = "\r\n",
                //FieldSeparator = (char) 9, //TAB
                FieldSeparator = '|', //TAB

                TextQualifier = char.MinValue
            };
            UseLambdas = true;
            UseTasks = true;
            FastIndexOfAny = true;
        }

        #endregion

        protected internal Stream BaseStream;
        protected static DateTime DateTimeZero = new DateTime();


        public static IEnumerable<T> Read<T>(DelimitedFileSource delimitedFileSource) where T : new()
        {
            var delimitedFileReader = new DelimitedFileReader<T>(delimitedFileSource);
            return (IEnumerable<T>)delimitedFileReader;
        }

        public char FieldSeparator { get; private set; }
        public char TextQualifier { get; private set; }
        public IEnumerable<String> Columns { get; private set; }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            // overriden in derived classes
        }
    }

    public class DelimitedFile<T> : DelimitedFile
    {
        private readonly char fieldSeparator;
        private readonly string fieldSeparatorAsString;
        private readonly char[] invalidCharsInFields;
        private readonly StreamWriter streamWriter;
        private readonly char textQualifier;
        private readonly String[] columns;
        private Func<T, object>[] getters;
        private readonly bool[] isInvalidCharInFields;
        private int linesToWrite;
        private readonly BlockingCollection<string> delimitedLinesToWrite = new BlockingCollection<string>(5000);
        private readonly Thread writeDelimitedLinesTask;
        private Task addAsyncTask;

        public DelimitedFile(DelimitedFileDestination delimitedFileDestination)
            : this(delimitedFileDestination, null)
        {
        }

        public DelimitedFile()
        {
        }

        public DelimitedFile(DelimitedFileDestination delimitedFileDestination,
            DelimitedFileDefinition delimitedFileDefinition)
        {
            if (delimitedFileDefinition == null)
                delimitedFileDefinition = DefaultDelimitedFileDefinition;
            this.columns = (delimitedFileDefinition.Columns ?? InferColumns(typeof(T))).ToArray();
            this.fieldSeparator = delimitedFileDefinition.FieldSeparator;
            this.fieldSeparatorAsString = this.fieldSeparator.ToString(CultureInfo.InvariantCulture);
            this.textQualifier = delimitedFileDefinition.TextQualifier;
            this.streamWriter = delimitedFileDestination.StreamWriter;

            this.invalidCharsInFields = new[] { '\r', '\n', this.textQualifier, this.fieldSeparator };
            this.isInvalidCharInFields = new bool[256];

            foreach (var c in this.invalidCharsInFields)
            {
                this.isInvalidCharInFields[c] = true;
            }
            this.WriteHeader();

            this.CreateGetters();
            if (DelimitedFile.UseTasks)
            {
                writeDelimitedLinesTask = new Thread((o) => this.WriteDelimitedLines());
                writeDelimitedLinesTask.Start();
            }
            this.addAsyncTask = Task.Factory.StartNew(() => { });

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                addAsyncTask.Wait();
                if (delimitedLinesToWrite != null)
                {
                    delimitedLinesToWrite.CompleteAdding();
                }
                if (writeDelimitedLinesTask != null)
                    writeDelimitedLinesTask.Join();
                this.streamWriter.Close();
            }
        }

        protected static IEnumerable<string> InferColumns(Type recordType)
        {
            var columns = recordType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(pi => pi.GetIndexParameters().Length == 0
                             && pi.GetSetMethod() != null
                             && !Attribute.IsDefined(pi, typeof(DelimitedFileIgnorePropertyAttribute)))
                .Select(pi => pi.Name)
                .Concat(recordType
                    .GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Where(fi => !Attribute.IsDefined(fi, typeof(DelimitedFileIgnorePropertyAttribute)))
                    .Select(fi => fi.Name))
                .ToList();
            return columns;
        }

        private void WriteDelimitedLines()
        {
            int written = 0;
            foreach (var delimitedLine in delimitedLinesToWrite.GetConsumingEnumerable())
            {
                this.streamWriter.WriteLine(delimitedLine);
                written++;
            }
            Interlocked.Add(ref this.linesToWrite, -written);
        }

        public void Append(T record)
        {

            if (DelimitedFile.UseTasks)
            {
                var linesWaiting = Interlocked.Increment(ref this.linesToWrite);
                Action<Task> addRecord = (t) =>
                {
                    var delimitedLine = this.ToDelimitedRecord(record);
                    this.delimitedLinesToWrite.Add(delimitedLine);
                };

                if (linesWaiting < 10000)
                    this.addAsyncTask = this.addAsyncTask.ContinueWith(addRecord);
                else
                    addRecord(null);
            }
            else
            {
                var delimitedLine = this.ToDelimitedRecord(record);
                this.streamWriter.WriteLine(delimitedLine);
            }
        }

        private static Func<T, object> FindGetter(string c, bool staticMember)
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase |
                        (staticMember ? BindingFlags.Static : BindingFlags.Instance);
            Func<T, object> func = null;
            PropertyInfo pi = typeof(T).GetProperty(c, flags);
            FieldInfo fi = typeof(T).GetField(c, flags);

            if (UseLambdas)
            {
                Expression expr = null;
                ParameterExpression parameter = Expression.Parameter(typeof(T), "r");
                Type type = null;

                if (pi != null)
                {
                    type = pi.PropertyType;
                    expr = Expression.Property(parameter, pi.Name);
                }
                else if (fi != null)
                {
                    type = fi.FieldType;
                    expr = Expression.Field(parameter, fi.Name);
                }
                if (expr != null)
                {
                    Expression<Func<T, object>> lambda;
                    if (type.IsValueType)
                    {
                        lambda = Expression.Lambda<Func<T, object>>(Expression.TypeAs(expr, typeof(object)), parameter);
                    }
                    else
                    {
                        lambda = Expression.Lambda<Func<T, object>>(expr, parameter);
                    }
                    func = lambda.Compile();
                }
            }
            else
            {
                if (pi != null)
                    func = o => pi.GetValue(o, null);
                else if (fi != null)
                    func = o => fi.GetValue(o);
            }
            return func;
        }

        private void CreateGetters()
        {
            var list = new List<Func<T, object>>();

            foreach (var columnName in columns)
            {
                Func<T, Object> func = null;
                var propertyName = (columnName.IndexOf(' ') < 0 ? columnName : columnName.Replace(" ", ""));
                func = FindGetter(columnName, false) ?? FindGetter(columnName, true);

                list.Add(func);
            }
            getters = list.ToArray();
        }

        private string ToDelimitedRecord(T record)
        {
            if (record == null)
                throw new ArgumentException("Cannot be null", "record");

            string[] values = new string[getters.Length];

            for (int i = 0; i < getters.Length; i++)
            {
                var getter = getters[i];
                object fieldValue = getter == null ? null : getter(record);
                values[i] = ToDelimitedString(fieldValue);
            }
            return string.Join(fieldSeparatorAsString, values);

        }

        private string ToDelimitedString(object o)
        {
            if (o != null)
            {
                string valueString = o as string ?? Convert.ToString(o, CultureInfo.CurrentUICulture);
                if (RequiresQuotes(valueString))
                {
                    var line = new StringBuilder();
                    line.Append(this.textQualifier);
                    foreach (char c in valueString)
                    {
                        if (c == this.textQualifier)
                            line.Append(c); // double the double quotes
                        line.Append(c);
                    }
                    line.Append(this.textQualifier);
                    return line.ToString();
                }
                else
                    return valueString;
            }
            return string.Empty;
        }

        private bool RequiresQuotes(string valueString)
        {
            if (DelimitedFile.FastIndexOfAny)
            {
                var len = valueString.Length;
                for (int i = 0; i < len; i++)
                {
                    char c = valueString[i];
                    if (c <= 255 && this.isInvalidCharInFields[c])
                        return true;
                }
                return false;
            }
            else
            {
                return valueString.IndexOfAny(this.invalidCharsInFields) >= 0;
            }
        }

        private void WriteHeader()
        {
            var line = new StringBuilder();
            for (int i = 0; i < columns.Length; i++)
            {
                if (i > 0)
                    line.Append(fieldSeparator);
                line.Append(ToDelimitedString(columns[i]));
            }
            streamWriter.WriteLine(line.ToString());
        }
    }

    internal class DelimitedFileReader<T> : DelimitedFile, IEnumerable<T>, IEnumerator<T>
        where T : new()
    {
        private readonly Dictionary<Type, List<Action<T, String>>> allSetters =
            new Dictionary<Type, List<Action<T, String>>>();

        private string[] columns;
        private char curChar;
        private int len;
        private string line;
        private int pos;
        private T record;
        private readonly char fieldSeparator;
        private readonly TextReader textReader;
        private readonly char textQualifier;
        private readonly StringBuilder parseFieldResult = new StringBuilder();

        public DelimitedFileReader(DelimitedFileSource delimitedFileSource)
            : this(delimitedFileSource, null)
        {
        }

        public DelimitedFileReader(DelimitedFileSource delimitedFileSource,
            DelimitedFileDefinition delimitedFileDefinition)
        {
            var streamReader = delimitedFileSource.TextReader as StreamReader;
            if (streamReader != null)
                this.BaseStream = streamReader.BaseStream;
            if (delimitedFileDefinition == null)
                delimitedFileDefinition = DefaultDelimitedFileDefinition;
            this.fieldSeparator = delimitedFileDefinition.FieldSeparator;
            this.textQualifier = delimitedFileDefinition.TextQualifier;

            this.textReader = delimitedFileSource.TextReader;
            // new FileStream(delimitedFileSource.TextReader, FileMode.Open);

            this.ReadHeader(delimitedFileDefinition.Header);

        }

        public T Current
        {
            get { return this.record; }
        }

        public bool Eof
        {
            get { return this.line == null; }
        }

        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                this.textReader.Dispose();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            this.ReadNextLine();
            if (this.line == null && (this.line = this.textReader.ReadLine()) == null)
            {
                this.record = default(T);
            }
            else
            {
                this.record = new T();
                Type recordType = typeof(T);
                List<Action<T, String>> setters;
                if (!this.allSetters.TryGetValue(recordType, out setters))
                {
                    setters = this.CreateSetters();
                    this.allSetters[recordType] = setters;
                }

                var fieldValues = new string[setters.Count];
                for (int i = 0; i < setters.Count; i++)
                {
                    fieldValues[i] = this.ParseField();
                    if (this.curChar == this.fieldSeparator)
                        this.NextChar();
                    else
                        break;
                }
                for (int i = 0; i < setters.Count; i++)
                {
                    var setter = setters[i];
                    if (setter != null)
                    {
                        setter(this.record, fieldValues[i]);
                    }
                }
            }
            return (this.record != null);
        }


        public void Reset()
        {
            throw new NotImplementedException("Cannot reset DelimitedFileReader enumeration.");
        }

        private static Action<T, string> EmitSetValueAction(MemberInfo mi, Func<string, object> func)
        {
            ParameterExpression paramExpObj = Expression.Parameter(typeof(object), "obj");
            ParameterExpression paramExpT = Expression.Parameter(typeof(T), "instance");

            {
                var pi = mi as PropertyInfo;
                if (pi != null)
                {
                    if (DelimitedFile.UseLambdas)
                    {
                        var callExpr = Expression.Call(
                            paramExpT,
                            pi.GetSetMethod(),
                            Expression.ConvertChecked(paramExpObj, pi.PropertyType));
                        var setter = Expression.Lambda<Action<T, object>>(
                            callExpr,
                            paramExpT,
                            paramExpObj).Compile();
                        return (o, s) => setter(o, func(s));
                    }
                    return (o, v) => pi.SetValue(o, (object)func(v), null);

                }
            }
            {
                var fi = mi as FieldInfo;
                if (fi != null)
                {
                    if (DelimitedFile.UseLambdas)
                    {
                        //ParameterExpression valueExp = Expression.Parameter(typeof(string), "value");
                        var valueExp = Expression.ConvertChecked(paramExpObj, fi.FieldType);

                        // Expression.Property can be used here as well
                        MemberExpression fieldExp = Expression.Field(paramExpT, fi);
                        BinaryExpression assignExp = Expression.Assign(fieldExp, valueExp);

                        var setter = Expression.Lambda<Action<T, object>>
                            (assignExp, paramExpT, paramExpObj).Compile();

                        return (o, s) => setter(o, func(s));
                    }
                    return ((o, v) => fi.SetValue(o, func(v)));
                }
            }
            throw new NotImplementedException();
        }

        private static Action<T, string> FindSetter(string c, bool staticMember)
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase |
                        (staticMember ? BindingFlags.Static : BindingFlags.Instance);
            Action<T, string> action = null;
            PropertyInfo pi = typeof(T).GetProperty(c, flags);
            if (pi != null)
            {
                var pFunc = StringToObject(pi.PropertyType);
                action = EmitSetValueAction(pi, pFunc);
            }
            FieldInfo fi = typeof(T).GetField(c, flags);
            if (fi != null)
            {
                var fFunc = StringToObject(fi.FieldType);
                action = EmitSetValueAction(fi, fFunc);
            }
            return action;
        }

        private static Func<string, object> StringToObject(Type propertyType)
        {
            if (propertyType == typeof(string))
                return (s) => s ?? String.Empty;
            else if (propertyType == typeof(Int32))
                return (s) => String.IsNullOrEmpty(s) ? 0 : Int32.Parse(s);
            else if (propertyType == typeof(Int16))
                return (s) => String.IsNullOrEmpty(s) ? 0 : Int16.Parse(s);
            if (propertyType == typeof(DateTime))
                return (s) => String.IsNullOrEmpty(s) ? DateTimeZero : DateTime.Parse(s);
            else if (propertyType == typeof(decimal))
                return (s) => string.IsNullOrWhiteSpace(s) ? decimal.Zero : decimal.Parse(s);
            else if (propertyType == typeof(decimal?))
                return (s) => string.IsNullOrWhiteSpace(s) ? (decimal?)null : decimal.Parse(s);
            else if (propertyType == typeof(double))
                return (s) => string.IsNullOrWhiteSpace(s) ? 0D : double.Parse(s);
            else if (propertyType == typeof(bool))
                return (s) => !String.IsNullOrEmpty(s) && Int32.Parse(s)==1;
            else
                throw new NotImplementedException();
        }

        private List<Action<T, string>> CreateSetters()
        {
            var list = new List<Action<T, string>>();
            for (int i = 0; i < this.columns.Length; i++)
            {
                string columnName = this.columns[i];
                Action<T, string> action = null;
                if (columnName.IndexOf(' ') >= 0)
                    columnName = columnName.Replace(" ", "");
                action = FindSetter(columnName, false) ?? FindSetter(columnName, true);

                list.Add(action);
            }
            return list;
        }

        private void NextChar()
        {
            if (this.pos < this.len)
            {
                this.pos++;
                this.curChar = this.pos < this.len ? this.line[this.pos] : '\0';
            }
        }

        private void ParseEndOfLine()
        {
            throw new NotImplementedException();
        }


        private string ParseField()
        {
            parseFieldResult.Length = 0;
            if (this.line == null || this.pos >= this.len)
                return null;
            while (this.curChar == ' ')
            {
                this.NextChar();
            }
            if (this.curChar == this.textQualifier)
            {
                this.NextChar();
                while (this.curChar != 0)
                {
                    if (this.curChar == this.textQualifier)
                    {
                        this.NextChar();
                        if (this.curChar == this.textQualifier)
                        {
                            this.NextChar();
                            parseFieldResult.Append(this.textQualifier);
                        }
                        else
                            return parseFieldResult.ToString();
                    }
                    else if (this.curChar == '\0')
                    {
                        if (this.line == null)
                            return parseFieldResult.ToString();
                        this.ReadNextLine();
                    }
                    else
                    {
                        parseFieldResult.Append(this.curChar);
                        this.NextChar();
                    }
                }
            }
            else
            {
                while (this.curChar != 0 && this.curChar != this.fieldSeparator && this.curChar != '\r' &&
                       this.curChar != '\n')
                {
                    parseFieldResult.Append(this.curChar);
                    this.NextChar();
                }
            }
            return parseFieldResult.ToString();
        }

        private void ReadHeader(string header)
        {
            if (header == null)
            {
                this.ReadNextLine();
            }
            else
            {
                // we read the first line from the given header
                this.line = header;
                this.pos = -1;
                this.len = this.line.Length;
                this.NextChar();
            }

            var readColumns = new List<string>();
            string columnName;
            while ((columnName = this.ParseField()) != null)
            {
                readColumns.Add(columnName);
                if (this.curChar == this.fieldSeparator)
                    this.NextChar();
                else
                    break;
            }
            this.columns = readColumns.ToArray();
        }

        private void ReadNextLine()
        {
            this.line = this.textReader.ReadLine();
            this.pos = -1;
            if (this.line == null)
            {
                this.len = 0;
                this.curChar = '\0';
            }
            else
            {
                this.len = this.line.Length;
                this.NextChar();
            }
        }
    }

    public class DelimitedFileDefinition
    {
        public string Header { get; set; }
        public char FieldSeparator { get; set; }
        public char TextQualifier { get; set; }
        public IEnumerable<String> Columns { get; set; }
        public string EndOfLine { get; set; }

        public DelimitedFileDefinition()
        {
            if (DelimitedFile.DefaultDelimitedFileDefinition != null)
            {
                FieldSeparator = DelimitedFile.DefaultDelimitedFileDefinition.FieldSeparator;
                TextQualifier = DelimitedFile.DefaultDelimitedFileDefinition.TextQualifier;
                EndOfLine = DelimitedFile.DefaultDelimitedFileDefinition.EndOfLine;
            }
        }
    }

    public class DelimitedFileSource
    {
        public readonly TextReader TextReader;

        public static implicit operator DelimitedFileSource(DelimitedFile file)
        {
            return new DelimitedFileSource(file);
        }

        public static implicit operator DelimitedFileSource(string path)
        {
            return new DelimitedFileSource(path);
        }

        public static implicit operator DelimitedFileSource(TextReader textReader)
        {
            return new DelimitedFileSource(textReader);
        }

        public DelimitedFileSource(TextReader textReader)
        {
            this.TextReader = textReader;
        }

        public DelimitedFileSource(Stream stream)
        {
            this.TextReader = new StreamReader(stream);
        }

        public DelimitedFileSource(string path)
        {
            this.TextReader = new StreamReader(path);
        }

        public DelimitedFileSource(DelimitedFile delimitedFile)
        {
            this.TextReader = new StreamReader(delimitedFile.BaseStream);
        }
    }

    public class DelimitedFileDestination
    {
        public StreamWriter StreamWriter;

        public static implicit operator DelimitedFileDestination(string path)
        {
            return new DelimitedFileDestination(path);
        }

        private DelimitedFileDestination(StreamWriter streamWriter)
        {
            this.StreamWriter = streamWriter;
        }

        private DelimitedFileDestination(Stream stream)
        {
            this.StreamWriter = new StreamWriter(stream);
        }

        public DelimitedFileDestination(string fullName)
        {
            FixDelimitedFileName(ref fullName);
            this.StreamWriter = new StreamWriter(fullName);
        }

        private static void FixDelimitedFileName(ref string fullName)
        {
            fullName = Path.GetFullPath(fullName);
            var path = Path.GetDirectoryName(fullName);
            if (path != null && !Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!String.Equals(Path.GetExtension(fullName), ".tsv"))
                fullName += ".tsv";
        }
    }

    public static class DelimitedFileLinqExtensions
    {
        public static void ToDelimitedFile<T>(this IEnumerable<T> source, DelimitedFileDestination delimitedFileDestination)
        {
            source.ToDelimitedFile(delimitedFileDestination, null);
        }

        public static void ToDelimitedFile<T>(this IEnumerable<T> source, DelimitedFileDestination delimitedFileDestination,
            DelimitedFileDefinition delimitedFileDefinition)
        {
            using (var delimitedFile = new DelimitedFile<T>(delimitedFileDestination, delimitedFileDefinition))
            {
                foreach (var record in source)
                {
                    delimitedFile.Append(record);
                }
            }
        }

    }

    public class DelimitedFileIgnorePropertyAttribute : Attribute
    {
        public override string ToString()
        {
            return "Ignore Property";
        }
    }
}