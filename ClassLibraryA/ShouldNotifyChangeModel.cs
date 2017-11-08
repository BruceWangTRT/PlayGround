//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using ClassLibraryA.Annotations;

//namespace ClassLibraryA
//{
//    public class ShouldNotifyChangeModel : INotifyPropertyChanged
//    {
//        private string _nameProperty;
//        private int _numberProperty;
//        public event PropertyChangedEventHandler PropertyChanged;

//        [NotifyPropertyChangedInvocator]
//        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }

//        public string NameProperty
//        {
//            get { return _nameProperty; }
//            set
//            {
//                if (value == _nameProperty) return;
//                _nameProperty = value;
//                OnPropertyChanged();
//            }
//        }

//        public int NumberProperty
//        {
//            get { return _numberProperty; }
//            set
//            {
//                if (value == _numberProperty) return;
//                _numberProperty = value;
//                OnPropertyChanged();
//            }
//        }

//        public Class1 ClassProperty { get; set; }
//    }
//}
