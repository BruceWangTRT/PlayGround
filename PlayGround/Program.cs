using NodaTime;
using PlayGround.Model;
using SharedComponent.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using AerospikeCacheService;
using ClassLibraryA;
using ClassLibraryA.Mapping;
using ClassLibraryA.Model;
using ClassLibraryA.Model.Exception;
using ClassLibraryA.Utility;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace PlayGround
{
    class Program
    {
        public static void Main(string[] args)
        {
            #region Past Tests
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
            //NestedForTest();
            //TaskDelayTest();
            //TaskResultTest();
            //TaskDelayTest2();
            //ExpediaImageFileLoadTest();
            //ConfigElementCollectionTest();
            //MsgPackTest();
            //XmlReaderTest();
            //LoadAmenities();
            //FluentNhibernateTest();
            //NofityChangeTest();
            //AerospikeTest();
            //AggregateExceptionTest();
            //DecimalToStringTest();
            //GenerateScriptForMessageId();
            //UsingTest();
            //StringSplitTest();
            //ForeachTest();
            //GenerateHexKey(args);
            //SystemConvertTest();
            #endregion
            FluentNhibernateCreatDbTest();
            Console.ReadLine();
        }

        private static void SystemConvertTest()
        {
            var unknown = "asdfasdfasdfsdafs";
            var date = Convert.ToDateTime(unknown);
            Console.WriteLine(date);
        }

        private static void GenerateHexKey(string[] argv)
        {
            int len = 64;
            if (argv.Length > 0)
                len = int.Parse(argv[0]);
            byte[] buff = new byte[len / 2];
            RNGCryptoServiceProvider rng = new
                                    RNGCryptoServiceProvider();
            rng.GetBytes(buff);
            StringBuilder sb = new StringBuilder(len);
            for (int i = 0; i < buff.Length; i++)
                sb.Append(string.Format("{0:X2}", buff[i]));
            Console.WriteLine(sb);
        }

        private static void ForeachTest()
        {
            List<string> l = null;
            foreach (var x in l)
            {
                Console.WriteLine("hi");
            }
            Console.WriteLine("good");
        }

        private static void StringSplitTest()
        {
            var originalEmail = "bookings@hellogbye.com";
            var splitEmail = originalEmail.Split('@');
            Console.WriteLine(splitEmail);
        }

        private static void UsingTest()
        {
            SqlConnection shouldBeDisposed;
            using (shouldBeDisposed = new SqlConnection(""))
            {
                Console.WriteLine(shouldBeDisposed.State.ToString());
            }    
        }

        private static void GenerateScriptForMessageId()
        {
            var names = typeof(MessageId).GetAllPublicConstantNames<string>();
            var englishTemplate =
                "INSERT INTO [MasterData].[LocalizedMessage]([MessageId],[LanguageCode],[CultureCode],[PlatformId],[MessageText],[AddedDateTime],[ModifiedDateTime],[MessageTitle])"
                +
                "VALUES('{0}', 'en', 'US', 'Web', '{0}' , GETDATE(), GETDATE(), '{0}')";

            var defaultTemplate = 
                "INSERT INTO[MasterData].[LocalizedMessage]([MessageId],[LanguageCode],[CultureCode],[PlatformId],[MessageText],[AddedDateTime],[ModifiedDateTime],[MessageTitle])"
                +
                " VALUES('{0}',null,null,null,'{0}' , GETDATE(),GETDATE(),'{0}')";

            File.WriteAllLines("NewMessageIds.sql", names.Select(n=>string.Format(englishTemplate,n)).ToList().Union(names.Select(n=>string.Format(defaultTemplate,n))));


            Console.WriteLine(names.Count);
        }

        public class Classx
        {
            public Classx()
            {
                    
            }

            public List<string> TextList { get; set; }

            public void Test()
            {
                foreach (var text in TextList)
                {
                    
                }
            }
        }

        private static void AggregateExceptionTest()
        {
            Task<string[]>[] tasks = new Task<string[]>[1];
            tasks[0] = Task<string[]>.Factory.StartNew(() => {throw new PlayGroundException(false);});
            //tasks[1] = Task<string[]>.Factory.StartNew(() => { throw new PlayGroundException(true); });
            //tasks[2] = Task<string[]>.Factory.StartNew(() => {throw new NotImplementedException();});

            try
            {
                Task.WaitAll(tasks);
            }
            catch (PlayGroundException pge)
            {
                throw pge;
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void AerospikeTest()
        {
            var cacheService = new BaseCacheService();
            var obj = new HotelImage()
            {
                Caption = "c",
                DefaultImage = true,
                Height = 1,
                Image = "I",
                Width = 2
            };
            if (cacheService.SetValue("myKey1", obj,ttlSec:3))
            {
                var objShouldNotExpire = new HotelImage()
                {
                    Caption = "c",
                    DefaultImage = true,
                    Height = 1,
                    Image = "I",
                    Width = 2
                };
                cacheService.SetValue("myKey2", objShouldNotExpire);
                var obj2 = cacheService.GetValue<HotelImage>("myKey1");
                var objShouldNotExpire2 = cacheService.GetValue<HotelImage>("myKey2");
                Console.WriteLine(obj2.SerializeToStringWithDefaultSettings());
                Console.WriteLine(objShouldNotExpire2.SerializeToStringWithDefaultSettings());
            }
        }

        private static void NofityChangeTest()
        {
            //var c1 = new ShouldNotifyChangeModel()
            //{
            //    ClassProperty = new Class1(),
            //    NameProperty = "name1",
            //    NumberProperty = 1
            //};
            ////c1.PropertyChanged += HandlePropertyChanged;

            //c1.NameProperty = "name 1 changed";
            //c1.NumberProperty = 2;
            //c1.ClassProperty = new Class1();

            //var c2 = new ShouldNotifyChangeModel()
            //{
            //    ClassProperty = new Class1(),
            //    NameProperty = "name2",
            //    NumberProperty = 2
            //};
            //c1 = c2;//this do not raise any event

            //var p1 = new Class1() {FirstName = "f1",LastName = "l1"};
            ////((INotifyPropertyChanged)p1).PropertyChanged += HandlePropertyChanged;
            //p1.FirstName = "f2";
            //p1.LastName = "l2";
            //p1= new Class1();//this do not raise any event

            //var level2 = new Level2Class1();
            ////var p2 = new Class1("f", "l", level2, false);
            //var p2 = new Class1();
            //p2.FirstName = "new first name";
            //p2.LastName = "new last name";
            //p2.Level2Class1 = level2;
            //p2.Level2Class1.Level2Name = "new name";
            //p2.Level2Class1.Level2Phone = "new phone";
            ////p2.FirstName = "l2";

            //var stringP2 = JsonConvert.SerializeObject(p2);
            //var p3 = JsonConvert.DeserializeObject<Class1>(stringP2);


        }

        private static void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine("{0} has changed",e.PropertyName);
        }

        private static void FluentNhibernateCreatDbTest()
        {
            var sessionFactory = CreateSessionFactory();
            
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var product = new Product()
                    {
                        Id = Guid.NewGuid(),
                        Name = "a",
                        Category = "1",
                        Discontinued = false
                    };
                    session.SaveOrUpdate(product);
                    transaction.Commit();
                }
            }
        }

        private static void FluentNhibernateTest()
        {
            var sessionFactory = CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    // create a couple of AirlinePointsProgram
                    var p1 = new AirlinePointsProgram
                    {
                        Id = 1,
                        ProgramName = "Program 1",
                        AirlineIATACode = "AA",
                        Country = "American"
                    };

                    var p2 = new AirlinePointsProgram
                    {
                        Id = 2,
                        ProgramName = "Program 2",
                        AirlineIATACode = "BB",
                        Country = "Brazil"
                    };

                    var p3 = new AirlinePointsProgram
                    {
                        Id = 3,
                        ProgramName = "Program 3",
                        AirlineIATACode = "CC",
                        Country = "Canada"
                    };

                    var p4 = new AirlinePointsProgram
                    {
                        Id = 4,
                        ProgramName = "Program 4",
                        AirlineIATACode = "DD",
                        Country = "Dutch"
                    };

                    // save all AirlinePointsProgram
                    session.SaveOrUpdate(p1);
                    session.SaveOrUpdate(p2);
                    session.SaveOrUpdate(p3);
                    session.SaveOrUpdate(p4);

                    transaction.Commit();
                }

                using (var transaction = session.BeginTransaction())
                {
                    var p1 = new AirlinePointsProgram
                    {
                        Id = 1,
                        ProgramName = "Program 1",
                        AirlineIATACode = "AA",
                        Country = "American"
                    };

                    var p0 = new AirlinePointsProgram
                    {
                        Id = 0,
                        ProgramName = "Program 0",
                        AirlineIATACode = "oo",
                        Country = "o"
                    };

                    //Create a UserAirlinePointsProgram
                    var userProgram1 = new UserAirlinePointsProgram
                    {
                        Id = 11,
                        Program = p1,
                        ProgramNumber = "0000-1111-2222-3333",
                        UserProfileId = Guid.Parse("54325A10-A672-48FE-B428-0000D74F781C")
                    };

                    var userProgram2 = new UserAirlinePointsProgram
                    {
                        Id = 22,
                        Program = p0,
                        ProgramNumber = "some number",
                        UserProfileId = Guid.Parse("54325A10-A672-48FE-B428-0000D74F781C")
                    };

                    session.SaveOrUpdate(userProgram1);
                    session.SaveOrUpdate(userProgram2);

                    transaction.Commit();
                }

                Console.ReadKey();
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            var connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=NhibernateTest;Integrated Security=true;MultipleActiveResultSets=True;";
            return Fluently.Configure() 
              .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString).ShowSql())
              .Mappings(m =>
                //m.FluentMappings.AddFromAssemblyOf<AirlinePointsProgram>())
                m.FluentMappings.Add<ProductMap>())
              .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true,true))
              //.ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
              .BuildConfiguration()
              .BuildSessionFactory();
        }

        private static void LoadAmenities()
        {
            //Read spreadsheet
            DataTable sheetData = new DataTable();
            using (OleDbConnection conn = ReturnConnection("../../Amenities Mapping (Expedia).xlsx"))
            {
                conn.Open();
                // retrieve the data using data adapter
                OleDbDataAdapter sheetAdapter = new OleDbDataAdapter("select * from [Expedia Mapping to Icons$]", conn);
                sheetAdapter.Fill(sheetData);
            }

            //Read all Expedia Amenities from DB
            var expediaPropertyAmenityDict = new Dictionary<string, int>();//Amenity 
            var expediaRoomAmenityDict = new Dictionary<string, int>();//Amenity description to expedia attribute Id
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["HelloGbyeDev"].ConnectionString))
            {
                var query = "SELECT [AttributeID],[AttributeDesc] FROM [HelloGByeDev].[dbo].[AttributeList] where [Type] = 'PropertyAmenity'";
                var command = new SqlCommand(query, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        expediaPropertyAmenityDict.Add(reader.GetString(1), reader.GetInt32(0));
                    }
                    reader.Dispose();
                    query = "SELECT [AttributeID],[AttributeDesc] FROM [HelloGByeDev].[dbo].[AttributeList] where [Type] = 'RoomAmenity'";
                    command = new SqlCommand(query, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        expediaRoomAmenityDict.Add(reader.GetString(1), reader.GetInt32(0));
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
            }

            //For each entry in spreadsheet, find Id by name in Expedia Amenities
            //And write Id and iconUrl to .sql file
            using (var writer = new StreamWriter("../../PopulateAmenityToIcon.sql"))
            {
                var insertTemplate =
                    "INSERT INTO [MasterData].[ExpediaAmenityToIcon]"+
                    "([AmenityId],[IconUrl])"+
                    "VALUES ('{0}', '{1}')";
                foreach (DataRow row in sheetData.Rows)
                {
                    var description = Convert.ToString(row[1]);
                    var iconUrl = Convert.ToString(row[2]);
                    if (string.IsNullOrEmpty(iconUrl)) continue;
                    if (expediaPropertyAmenityDict.ContainsKey(description))
                    {
                        writer.WriteLine(insertTemplate, expediaPropertyAmenityDict[description], iconUrl);
                    }
                    if (expediaRoomAmenityDict.ContainsKey(description))
                    {
                        writer.WriteLine(insertTemplate, expediaRoomAmenityDict[description], iconUrl);
                    }
                }
            }
        }



        #region Test Methods
        private static void XmlReaderTest()
        {
            var string1 = File.ReadAllText("N38XNQ.xml");
            var x = GetVenderRecordLocators(string1);
            foreach (var item in x)
            {
                Console.WriteLine(item);
            }
            string1 = File.ReadAllText("ZQJNDG.xml");
            x = GetVenderRecordLocators(string1);
            foreach (var item in x)
            {
                Console.WriteLine(item);
            }
        }
        private static List<string> GetVenderRecordLocators(string pnrResponseXml)
        {
            var venderRecordLocators = new List<string>();
            using (XmlReader reader = XmlReader.Create(new StringReader(pnrResponseXml)))
            {
                while (reader.ReadToFollowing("RecLocInfo"))
                {
                    reader.ReadToFollowing("RecLoc");
                    reader.Read();
                    venderRecordLocators.Add(reader.Value);
                }
            }
            return venderRecordLocators;
        }
        private static void MsgPackTest()
        {
            //var baseAirport = new AirportBase
            //{
            //    AirportCityMapID = 1,
            //    AirportCode = "YYZ",
            //    AirportName = "Pearson",
            //    CategoryCountryCode = "Category_CA",
            //    CityCode = "CA_01",
            //    CityName = "Toronto",
            //    Country = "Canada",
            //    CountryCode = "CA",
            //    DefaultDisplay = false,
            //    DestinationCategory = "Destination_1",
            //    HilightCountry = false,
            //    IsMetroCode = false,
            //    Latitude = 0,
            //    Longitude = 0,
            //    RegionCode = "ON",
            //    RegionName = "Ontario",
            //    SelectionOrder = 1,
            //    SkipRow = 0,
            //    State = "state",
            //    //TimeZone="EST"
            //};
            var msgSerilizer = new MsgSerilizer();
            //byte[] stream = msgSerilizer.Serialize<AirportBase>(baseAirport);
            //var fileStream = File.OpenWrite(@"c:\msg.txt");
            //fileStream.Write(stream,0,stream.Length);
            //fileStream.Close();
            //var advancedAirport = msgSerilizer.Deserialize<AirportBase>(File.ReadAllBytes(@"c:\msg.txt"));
            //Console.WriteLine(advancedAirport.Landscape);

            var publicModel1 = new MsgPackPublicModel();
            publicModel1.Mpublic = 11;
            publicModel1.SetMprotected(12);
            publicModel1.SetMprivate(13);
            var publicModel2 = new MsgPackPublicModel();
            publicModel2.Mpublic = 21;
            publicModel2.SetMprotected(22);
            publicModel2.SetMprivate(23);
            var publicModel3 = new MsgPackPublicModel();
            publicModel3.Mpublic = 31;
            publicModel3.SetMprotected(32);
            publicModel3.SetMprivate(33);
            var testModel = new MsgTestModel()
            {
                Xpub = 1,
                PublicModel = publicModel1,
            };
            testModel.SetList(new List<MsgPackPublicModel>() { publicModel2, publicModel3 });
            //Serialize
            byte[] stream = msgSerilizer.Serialize<MsgTestModel>(testModel);
            var fileStream = File.OpenWrite(@"c:\msg.txt");
            fileStream.Write(stream, 0, stream.Length);
            fileStream.Close();
            //Deserialize
            var advancedTestModel = msgSerilizer.Deserialize<MsgTestModel>(File.ReadAllBytes(@"c:\msg.txt"));
            Console.WriteLine(advancedTestModel.Ypub);
        }

        private static void ConfigElementCollectionTest()
        {
            PlayGroundConfigSection x = (PlayGroundConfigSection)ConfigurationManager.GetSection("playGroundGroup/playGround");
            var collection = x.Currencies;
            var usdElement = collection.GetByName("USD");
            Console.WriteLine(usdElement.Name);
            //Console.WriteLine("Is in Use: {0}", x.InUse);
            //Console.WriteLine("Name : {0}, Age :{1}", x.Author.Name, x.Author.Age);
        }
        private static void ExpediaImageFileLoadTest()
        {
            int prevHotelId = -1;
            List<LocalImages> localImgs = new List<LocalImages>();

            foreach (var imgRow in DelimitedFile.Read<LocalImages>(@"C:\Expedia Dump\HotelImageList.txt"))
            {
                if (prevHotelId == -1)
                {
                    localImgs.Add(imgRow);
                }
                else if (prevHotelId != -1 && imgRow.EANHotelID == prevHotelId)
                {
                    localImgs.Add(imgRow);
                }
                else if (prevHotelId != -1 && imgRow.EANHotelID != prevHotelId)
                {
                    if (localImgs.Count > 0)
                    {
                        //TODO:dump localImgs into hotel
                        Console.WriteLine($"{imgRow.EANHotelID} : {localImgs.Count} pics");
                        //Hotel obj = hotelList.FirstOrDefault(t => t.HotelCode == prevHotelId);
                        foreach (var localImg in localImgs)
                        {
                            CreateImage(localImg);
                        }
                        localImgs = new List<LocalImages>();
                    }
                    localImgs.Add(imgRow);
                }
                prevHotelId = imgRow.EANHotelID;
            }
        }
        private static HotelImage CreateImage(LocalImages img)
        {
            HotelImage image = new HotelImage();
            image.Caption = img.Caption;
            image.DefaultImage = img.DefaultImage;
            image.Image = img.URL;
            image.Height = img.Height;
            image.Width = img.Width;
            return image;
        }
        private static void PdfTest()
        {
            //EO.Pdf.Runtime.AddLicense(
            //    "DFmXpM0X6Jzc8gQQyJ21vMTitWuoucjbt3Wm8PoO5Kfq6doPvUaBpLHLn3Xj" +
            //    "7fQQ7azc6c/nrqXg5/YZ8p7cwp61n1mXpM0M66Xm+8+4iVmXpLHLn1mXwPIP" +
            //    "41nr/QEQvFu807/745+ZpAcQ8azg8//ooWqtssHNn2i1kZvLn1mXwMAM66Xm" +
            //    "+8+4iVmXpLHn7qvb6QP07Z/mpPUM8560psbas2iptMLhoVnq+fPw96ng9vYe" +
            //    "wK20psbas2iptMLioVnt6QMe6KjlwbPctVuXs8+4iVmXpLHn8qLe8vIf9Kvc" +
            //    "wgUc1JPF2MjfsHLp6QPc8obw7egh9qC0wc3a8qLe8vIf9Kvcwp61u2jj7fQQ" +
            //    "7azcwp61dePt9BDtrNzCnrWf");
            //var html = File.ReadAllText("eticket_template.html");
            //var stopWatch = Stopwatch.StartNew();
            ////HtmlToPdf.Options.VisibleElementIds = "xxx";
            //HtmlToPdf.Options.PageSize = PdfPageSizes.A5;
            //HtmlToPdf.Options.OutputArea = new RectangleF(0f, 0f, 5.8f, 8.3f);
            //HtmlToPdf.ConvertHtml(html, "eticket_template.pdf");
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
            //stopWatch.Stop();
            //Console.WriteLine(stopWatch.ElapsedMilliseconds);
        }
        private static void TaskDelayTest2()
        {
            Stopwatch sw = Stopwatch.StartNew();
            var delay = Task.Delay(1000).ContinueWith(_ =>
            {
                sw.Stop();
                return sw.ElapsedMilliseconds;
            });

            Console.WriteLine("Elapsed milliseconds: {0}", delay.Result);
        }
        private static void TaskResultTest()
        {
            var t1 = new Task<int>(() => 9 + 1);
            var t2 = t1.ContinueWith((x) => x.Result + 1);
            t1.Wait();
            Console.WriteLine(t2.Result);
        }
        private static void TaskDelayTest()
        {
            var source = new CancellationTokenSource();
            var t = Task.Run(async delegate
            {
                await Task.Delay(TimeSpan.FromSeconds(1.5), source.Token);
                return 42;
            });
            source.Cancel();
            try
            {
                t.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                    Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.Write("Task t Status: {0}", t.Status);
            if (t.Status == TaskStatus.RanToCompletion)
                Console.Write(", Result: {0}", t.Result);
            source.Dispose();
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
            float f = 1234.5676f;
            double doub = 1234.5678d;
            Console.WriteLine(d.ToString("F0"));
            Console.WriteLine(d.ToString("F1"));
            Console.WriteLine(d.ToString("F20"));
            Console.WriteLine(d.ToString());

            Console.WriteLine(f.ToString("F20"));
            Console.WriteLine(doub.ToString("F20"));
            Console.WriteLine(d);


            Console.WriteLine(((decimal)(121/1.3)).ToString("F20"));
            Console.WriteLine(((double)(121/1.3)).ToString("F20"));
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
            var invalidArray = new[] { "0120120000", "(012)0120000", "(012)012-0000", "(012)-0120000", "(012)-012-0000", "1(012)-012-0000", "+1(012)-012-0000", "+10120120000" };
            //foreach (var s in stringArray)
            //{
            //    Console.WriteLine("{0} is {1}", s, IsValidPhoneNumber(s));
            //}
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
        #endregion
    }
}
