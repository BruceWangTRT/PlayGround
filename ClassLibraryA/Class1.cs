//using System;
//using System.ComponentModel;
//using ClassLibraryA.Aspect;
//using Newtonsoft.Json;
//using PostSharp.Patterns.Model;
//namespace ClassLibraryA
//{
//    //[NotifyPropertyChanged]
//    [UpdateAware]
//    public class Class1
//    {

//        //[JsonProperty]
//        //private bool _isUpdated;
//        [KeepAnEyeOn]
//        public string FirstName { get; set; }

//        [IgnoreAutoChangeNotification, KeepAnEyeOn]
//        public string LastName { get; set; }
//        [IgnoreAutoChangeNotification]
//        public Level2Class1 Level2Class1 { get; set; }
//        public string Level2NameInLevel1 { get { return Level2Class1.Level2Name; } }
//        //public Class1()
//        //{
//        //    ((INotifyPropertyChanged)this).PropertyChanged += HandlePropertyChanged;
//        //    //((INotifyChildPropertyChanged)this).ChildPropertyChanged += HandleChildPropertyChanged;
//        //}
//        public Class1(string firstName)// : this()
//        {
//            FirstName = firstName;
//        }

//        public Class1()
//        {
                
//        }
//        //[JsonConstructor]
//        //public Class1(string firstName
//        //    ,string lastName
//        //    ,Level2Class1 level2Class1
//        //    ,bool isUpdated) //: this()
//        //{
//        //    FirstName = firstName;
//        //    LastName = lastName;
//        //    Level2Class1 = level2Class1;
//        //    _isUpdated = isUpdated;
//        //}

//        //public bool IsUpdated() { return _isUpdated; }

//        //private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
//        //{
//        //    Console.WriteLine("{0} has changed", e.PropertyName);
//        //    _isUpdated = true;
//        //}

//        //private void HandleChildPropertyChanged(object sender, ChildPropertyChangedEventArgs e)
//        //{
//        //    //Console.WriteLine("{0} has changed", e.);
//        //    _isUpdated = true;
//        //}
//    }
//}
