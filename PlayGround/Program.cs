using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PlayGround.Model;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using SharedComponent.Configurations;
using NodaTime;
using EO.Pdf;

namespace PlayGround
{
    class Program
    {
        public static void Main(string[] args)
        {
            //SumTest();
            //DataSetTest();
            //GetRangeTest();
            //DecimalToStringTest();
            //TimeZoneTest();
            //PhoneNumberValidationTest();
            //DateTimeConstrutorTest();
            //SwitchScopeTest();
            //DefaultDecimalTest();
            //TakeTest();
            //SearchOneWayResponseTest();
            //Regex1H01MTest();
            //DateTimeParseTest();
            //SharedConfigTest();
            //SkipTakeTest();
            //ConvertAllTest();
            //DictValuesTest();
            //TimeSpanTest();
            //FirstOrDefaultTest();
            //AirportCityMapUpdateAirportNameScriptGenerator();
            //CleanUpAirlinesInTravelPreferenceAttributeValues();
            //CleanUpAirCraftsInTravelPreferenceAttributeValues();
            //ReadAirlinesFromXlsxFileAndGenerateSQLScripts();
            //DoubleInitializingList();
            //NullAsSomething();
            //PdfTest();
            //PdfTestXTimes(5);
            //CultureTest();
            //NestedIfTest();
            NestedForTest();
            Console.ReadLine();
        }

        private static void NestedForTest()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 2; j++)
                {
                    Console.WriteLine(string.Format("i = {0}, j ={1}",i, j));
                }
        }

        private static void NestedIfTest()
        {
            var input = Console.ReadLine();
            var condition1 = string.IsNullOrWhiteSpace(input);
            var condition2 = !string.IsNullOrWhiteSpace(input);
            if (condition1)
                if (condition2)
                    Console.WriteLine("if - if");
                else
                    Console.WriteLine("if - else");
            else
                Console.WriteLine("else");
        }

        private static void CultureTest()
        {
            var i = 0;
            // Displays several properties of the neutral cultures.
            Console.WriteLine("CULTURE ISO ISO WIN DISPLAYNAME                              ENGLISHNAME                              NativeName");
            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                Console.Write("{0,-11}", ci.Name);
                Console.Write(" {0,-3}", ci.TwoLetterISOLanguageName);
                Console.Write(" {0,-3}", ci.ThreeLetterISOLanguageName);
                Console.Write(" {0,-3}", ci.ThreeLetterWindowsLanguageName);
                Console.Write(" {0,-40}", ci.DisplayName);
                Console.Write(" {0,-40}", ci.EnglishName);
                Console.WriteLine("{0,-40}",ci.NativeName);
                i++;
                //if (i > 20) break;
            }
        }

        private static void PdfTestXTimes(int x)
        {
            for (int i = 0; i < x; i++)
            {
                PdfTest();
            }
        }

        private static void PdfTest()
        {
            var html = File.ReadAllText("booking_hotel_confirmation.html");
            var stopWatch = Stopwatch.StartNew();
            HtmlToPdf.Options.PageSize = PdfPageSizes.Letter;
            HtmlToPdf.Options.OutputArea = new RectangleF(0f, 0f, 8.5f, 11f);
            HtmlToPdf.ConvertHtml(html, "hotel.pdf");
            //var converter = new SelectPdf.HtmlToPdf();
            ////converter.Options.JavaScriptEnabled = false;
            ////SelectPdf.HtmlToPdfOptions.MaximumConcurrentConversions = 2;
            //converter.Options.InternalLinksEnabled = false;
            //converter.Options.PdfDocumentInformation.Author = "HelloGbye";
            //converter.Options.PdfPageSize = SelectPdf.PdfPageSize.Letter;
            //converter.Options.JpegCompressionEnabled = false;
            ////converter.Options.PdfCompressionLevel = SelectPdf.PdfCompressionLevel.NoCompression;
            //var pdf = converter.ConvertHtmlString(html);
            //pdf.Save("hotel.pdf");
            //pdf.Close();
            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);
        }

        private static void NullAsSomething()
        {
            string a = "asdfasfd";
            string b = null;
            var x = a as string;
            var y = b as string;
            Console.WriteLine(x);
            Console.WriteLine(y);
        }
        private static void DoubleInitializingList()
        {
            var listA = new List<string> {"A1", "A2", "A3"};
            var listX = new List<string>(listA) {"X"};
            Console.WriteLine(string.Concat(listX));
            //var listB = new List<string> { "B1", "B2", "B3" };
            //var listC = new List<string> { "C1", "C2", "C3" };
            //var listD = new List<string> { "D1", "D2", "D3" };
            var lists = new List<List<string>> {listA};//, listB, listC, listD };
            var x = CartesianProduct(lists);
            x.ForEach(l => Console.WriteLine(string.Concat(l)));
        }
        private static List<List<T>> CartesianProduct<T>(List<List<T>> lists)
        {
            var expandedResult = new List<List<T>>();

            if (lists == null || lists.Count == 0)
            {
                expandedResult.Add(new List<T>());
                return expandedResult;
            }

            var list = lists.First();
            lists.Remove(list);
            var expandedList = CartesianProduct(lists);
            foreach (var element in list)
            {
                var temp = expandedList.Select(exp => new List<T>(exp) {element});
                expandedResult.AddRange(temp);
            }
            return expandedResult;
        }
        private static void ReadAirlinesFromXlsxFileAndGenerateSQLScripts()
        {
            DataTable sheetData = new DataTable();
            using (OleDbConnection conn = ReturnConnection("../../Airlines - 2016-02-09.xlsx"))
            {
                conn.Open();
                // retrieve the data using data adapter
                OleDbDataAdapter sheetAdapter = new OleDbDataAdapter("select * from [Airlines-2016-02-09$]", conn);
                sheetAdapter.Fill(sheetData);
            }

            var attributeValuesDict = new Dictionary<string,int>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["HelloGbyeDev"].ConnectionString))
            {
                var query = "SELECT [TravelPreferenceAttributeValueId],[Code] FROM [HelloGByeDev].[MasterData].[TravelPreferenceAttributeValues] WHERE [TravelPreferenceAttributeId]=1";
                var command = new SqlCommand(query, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        attributeValuesDict.Add(reader.GetString(1),reader.GetInt32(0));
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
            }

            var missingInTable = new List<int>();
            var added = new List<string>();
            using (var writer = new StreamWriter("../../UpdateAirlines.sql"))
            {
                var updateTemplate =
                  "UPDATE [MasterData].[TravelPreferenceAttributeValues]" +
                  "SET [Description]='{0}', [AlternateDescription]='{1}'" +
                  "WHERE [TravelPreferenceAttributeId]=1 AND [Code]='{2}'";
                var insertTemplate =
                    "INSERT INTO [MasterData].[TravelPreferenceAttributeValues]" +
                    "([TravelPreferenceAttributeValueId],[TravelPreferenceAttributeId],[Code],[Description],[AlternateDescription],[Order])" +
                    "VALUES ({0},1,'{1}','{2}','{3}',0)";
                var deleteTemplate =
                    "DELETE FROM [HelloGByeDev].[MasterData].[TravelPreferenceAttributeValues] WHERE [TravelPreferenceAttributeValueId]={0}";
                var reverseDict = new Dictionary<string, int>();
                foreach (DataRow row in sheetData.Rows)
                {
                    var iataCode = Convert.ToString(row["IATA Code"]);
                    var description = Convert.ToString(row["Primary Name"]);
                    var alternateDescription = Convert.ToString(row["Secondary Name"]);
                    if (attributeValuesDict.ContainsKey(iataCode))
                    {
                        writer.WriteLine(updateTemplate, description, alternateDescription, iataCode);
                        attributeValuesDict.Remove(iataCode);
                    }
                    else
                    {
                        added.Add(iataCode);
                        writer.WriteLine(insertTemplate, 9, iataCode, description, alternateDescription);
                    }
                }
                foreach (var kvp in attributeValuesDict)
                {
                    writer.WriteLine(deleteTemplate, kvp.Value);
                }
            }
        }

        private static OleDbConnection ReturnConnection(string fileName)
        {
            return new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + @";Extended Properties='Excel 12.0 xml;HDR=YES;'");
        }

        private static void CleanUpAirlinesInTravelPreferenceAttributeValues()
        {
            CleanUpTravelPreferenceAttributeValuesById(1, "Airline");
        }

        private static void CleanUpAirCraftsInTravelPreferenceAttributeValues()
        {
            CleanUpTravelPreferenceAttributeValuesById(2, "Aircraft");
        }

        private static void CleanUpTravelPreferenceAttributeValuesById(int travelPreferenceAttributeId, string attributeName)
        {
            var attributeValuesDict = new Dictionary<int, string>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["HelloGbyeDev"].ConnectionString))
            {
                var query = string.Format("SELECT [TravelPreferenceAttributeValueId],[Description] FROM [HelloGByeDev].[MasterData].[TravelPreferenceAttributeValues] WHERE [TravelPreferenceAttributeId]={0}", travelPreferenceAttributeId);
                var command = new SqlCommand(query, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        attributeValuesDict.Add(reader.GetInt32(0), reader.GetString(1).ToLowerInvariant().Trim());
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
            }
            

            using (var writer = new StreamWriter(string.Format("../../redundant{0}s.sql",attributeName)))
            {
                var updateTemplate =
                  "SELECT [TravelPreferenceAttributeValueId],[TravelPreferenceAttributeId],[Code],[Description],[AlternateDescription],[Order]" +
                  "FROM [HelloGByeDev].[MasterData].[TravelPreferenceAttributeValues]" +
                  "WHERE [TravelPreferenceAttributeValueId]={0}";
                var reverseDict = new Dictionary<string, int>();
                foreach (var attribute in attributeValuesDict)
                {
                    if (reverseDict.ContainsKey(attribute.Value))
                    {
                        writer.WriteLine(updateTemplate, attribute.Key);
                    }
                    else
                    {
                        reverseDict.Add(attribute.Value, attribute.Key);
                    }
                }
            }

        }

        private static void AirportCityMapUpdateAirportNameScriptGenerator()
        {
            var airportDict = new Dictionary<int, string>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["HelloGbyeDev"].ConnectionString))
            {
                var query = "SELECT [AirportCityMapID], [AirportName] FROM [HelloGByeDev].[MasterData].[AirportCityMap]";
                var command = new SqlCommand(query, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        airportDict.Add(reader.GetInt32(0), reader.GetString(1));
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
            }

            using(var writer = new StreamWriter("../../UPDATE_MasterData.AirportCityMap.sql"))
            {
                var updateTemplate =
                  "UPDATE [MasterData].[AirportCityMap] SET [AirportName]='{0}' WHERE [AirportCityMapID]={1}";
                var updateDict = airportDict.Where(airport => airport.Value.EndsWith(" ")).ToList();
                foreach (var airport in updateDict)
                {
                    writer.WriteLine(updateTemplate, airport.Value.Trim().Replace("'", "''"), airport.Key);
                }
            }

        }

        private static void FirstOrDefaultTest()
        {
            List<string> emptyLs = new List<string>();
            List<string> nullLs = null;

            var y = emptyLs.FirstOrDefault();
            Console.WriteLine(y);

            var x = nullLs.FirstOrDefault();
            Console.WriteLine(x);
        }

        private static void TimeSpanTest()
        {
            var ts0120 = new TimeSpan(0, 1, 2, 0);
            var ts_1120 = new TimeSpan(-1, 1, 2, 0);
            var ts2120 = ts_1120;
        }

        private static void DataSetTest()
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable();
            var dataView = dataTable.AsDataView();
            dataSet.Tables.Add(dataTable);
        }

        private static void SumTest()
        {
            string s = "11:00 am";
            //var opts = s.Split('-');

            DateTime parsedDate;
            if (DateTime.TryParseExact(s, "h:mm tt", null, DateTimeStyles.None, out parsedDate))
            {
                var diff = parsedDate.Hour;
                if (diff > 0)
                {
                    var j = 2;
                    j++;
                }
            }

            int i = 10;
            i = 20;
            Console.WriteLine(s.Trim());
            var l = new SumTest();
            var x = l.NightlyRates.Sum();
        }

        private static void GetRangeTest()
        {
            var itemList = new List<string>() { "a", "b", "c", "d", "e" };
            var newList = itemList.GetRange(0, 1);
            newList = itemList.GetRange(0, -1);
        }

        private static void DecimalToStringTest()
        {
            decimal d = 1234.5676m;
            Console.WriteLine(d.ToString("F0"));
            Console.WriteLine(d.ToString("F1"));
            Console.WriteLine(d.ToString("F2"));
            Console.WriteLine(d.ToString());
            Console.WriteLine(d);
        }

        private static void TimeZoneTest()
        {

            var dateNow = DateTime.Now;
            Console.WriteLine(dateNow);
            var string1 = "Asia/Kabul";
            var string2 = "Europe/Tirane";
            var tz1 = TimeZone.CurrentTimeZone;
            var offset = tz1.GetUtcOffset(dateNow);
            var zone1 = DateTimeZoneProviders.Tzdb[string1];
            var zone2 = DateTimeZoneProviders.Tzdb[string2];
            var zone3 = DateTimeZoneProviders.Tzdb.GetZoneOrNull("xxx");

            var os1 = zone1.GetUtcOffset(new Instant());
            var os2 = zone2.GetUtcOffset(new Instant());
            var os3 = zone3.GetUtcOffset(new Instant());

            Console.WriteLine(offset);
        }

        private static void PhoneNumberValidationTest()
        {
            var stringArray = new[] { "6478643246", "(647)8623246", "(647)862-3246", "(647)-8623246", "(647)-862-3246", "1(647)-862-3246", "+1(647)-862-3246", "+16478643246" };
            var invalidArray = new[] { "0120120000", "(012)0120000", "(012)012-0000", "(012)-0120000", "(012)-012-0000", "1(012)-012-0000", "+1(012)-012-0000", "+10120120000" };
            foreach (var s in stringArray)
            {
                Console.WriteLine("{0} is {1}", s, IsValidPhoneNumber(s));
            }
            foreach (var s in invalidArray)
            {
                Console.WriteLine("{0} is {1}", s, IsValidPhoneNumber(s));
            }

            var s1 = "0000000000";
            for (int i = 0; i < s1.Length; i++)
            {
                var c = s1[i];
                if (char.IsDigit(c))
                {
                    Console.WriteLine("{0} at {1} is a digit", c, i);
                }
                else
                {
                    Console.WriteLine("{0} at {1} is not a digit", c, i);
                }
            }
            Console.WriteLine(IsDigitOnly(s1));
        }

        private static bool IsDigitOnly(string arg)
        {
            return arg.Any(c => !char.IsDigit(c));
        }
        private static bool IsValidPhoneNumber(string arg)
        {
            var numericOnly = new string((from c in arg
                                          where char.IsDigit(c)
                                          select c
                                         ).ToArray());
            var phoneNumberPattern = @"(?<AreaCode>[2-9][0-9][0-9])(?<Number>[2-9][0-9][0-9]\d{4})";
            return Regex.IsMatch(numericOnly, phoneNumberPattern);
        }

        private static void DateTimeConstrutorTest()
        {
            var dt = DateTime.Now;
            var newDt0 = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            var newDt24 = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            Console.WriteLine(dt);
            Console.WriteLine(newDt0);
            Console.WriteLine(newDt24);
        }

        private static void SwitchScopeTest()
        {
            var s = "b";
            switch (s)
            {
                case "a":
                    var dt = "a";
                    Console.WriteLine(dt);
                    break;
                case "b":
                    dt = "b";
                    Console.WriteLine(dt);
                    break;
            }
        }

        private static void DefaultDecimalTest()
        {
            var deci = 3.1415926m;
            var dd = default(decimal);
            var d0 = 0m;
            Console.WriteLine(deci);
            Console.WriteLine(dd);
            Console.WriteLine(d0);
            Console.WriteLine("{0} == {1} is {2}", dd, d0, dd == d0);
        }

        private static void TakeTest()
        {
            var lst = new List<string>() { "0", "1", "2", "3", "4", "5" };
            var lst2 = lst.Take(100);
            Console.WriteLine(lst.Count);
        }

        private async static void SearchOneWayResponseTest()
        {
            var oneWayRequest = new SearchOneWayRequest
            {
                client_ref = "amginetech",
                Adult = "1",
                Child = "0",
                Infant = "0",
                currency = "CAD",
                dep_from = "YYZ",
                dep_to = "PEK",
                departure_date = (DateTime.Now.AddDays(20).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)),
                ret_from = string.Empty,
                ret_to = string.Empty,
                return_date = string.Empty,
                submit = "submit",
                trip_class = "Economy",
                trip_type = "oneway"
            };
            var searchOneWayResponse = await HttpClientBase.Current.PostJson<SearchOneWayRequest, SearchOneWayResponse>(oneWayRequest, "http://fnxml.flightnetwork.com/cgi-bin/b2c-v3/FNJSONCaller.cgi");
            //var searchOneWayResponse = new SearchOneWayResponse();
            Console.Read();
            int i = 0;
            i++;
            Console.WriteLine(i);
        }

        private static void Regex1H01MTest()
        {
            var regex = new Regex(@"(?<hour>\d)H(?<minute>\d{2})M");
            var match = regex.Match("asdfasdfasdfasdf");
            if (match.Success)
            {
                var groups = match.Groups;
                foreach (var groupName in regex.GetGroupNames())
                {
                    Console.WriteLine("Group: {0}, Value: {1}", groupName, groups[groupName].Value);
                }
            }
        }

        private static void DateTimeParseTest()
        {
            var dtString = "asdfasdfasdfasdfasdfa";
            DateTime dt;
            DateTime.TryParse(dtString, out dt);
            Console.WriteLine(dt);
        }

        private static void TaskTest()
        {
            Task task0 = new Task(DateTimeParseTest);
            Task task1 = new Task(new Action(DateTimeParseTest));
            Task task2 = new Task(delegate { DateTimeParseTest(); Regex1H01MTest(); });
            Task task3 = new Task(() => DateTimeParseTest());
            Task task4 = new Task(() => { DateTimeParseTest(); });

            task0.Start();
            task1.Start();
            task2.Start();
            task3.Start();
            task4.Start();
        }

        private static void SharedConfigTest()
        {
            PlayGroundConfigSection x = (PlayGroundConfigSection)ConfigurationManager.GetSection("playGroundGroup/playGround");
            Console.WriteLine("Is in Use: {0}", x.InUse);
            Console.WriteLine("Name : {0}, Age :{1}", x.Author.Name, x.Author.Age);
        }

        private static void SkipTakeTest()
        {
            IEnumerable<string> set = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7" };
            var subset0N = set.Skip(0).ToList();
            var subsetN0 = set.Take(0).ToList();
            var subset00 = set.Skip(0).Take(0).ToList();
            var subset01 = set.Skip(0).Take(1).ToList();
            var subset10 = set.Skip(1).Take(0).ToList();
            var subset11 = set.Skip(1).Take(1).ToList();

            var subset08 = set.Skip(0).Take(8).ToList();
            var subset09 = set.Skip(0).Take(9).ToList();

            var subset70 = set.Skip(7).Take(0).ToList();
            var subset71 = set.Skip(7).Take(1).ToList();
            var subset72 = set.Skip(7).Take(2).ToList();

            var subset80 = set.Skip(8).Take(0).ToList();
            var subset81 = set.Skip(8).Take(1).ToList();
            var subset82 = set.Skip(8).Take(2).ToList();

        }

        private static void ConvertAllTest()
        {
            var set = new List<object> { "0", "1", "2", "3", "4", "5", "6", "7" };
            var subsetN0 = set.Take(0).ToList().ConvertAll<string>(i =>
            {
                var hotel = i as string;
                if (hotel == null) return null;
                //hotel.Rooms.Clear();
                return hotel;
            });
        }

        private static void DictValuesTest()
        {
            var dict = new Dictionary<int, List<string>>();
            dict.Add(1, new List<string> { "1" });
            dict.Add(2, new List<string> { "2" });
            dict.Add(3, new List<string> { "1", "2" });

            var v = dict.Values;
            foreach (var value in v)
            {
                Console.WriteLine(value);
            }

            int w = 1;
            while (true)
            {
                dict[w++].Clear();
                if (dict.Values.All(ls => !ls.Any())) break;
            }
        }
    }
}
