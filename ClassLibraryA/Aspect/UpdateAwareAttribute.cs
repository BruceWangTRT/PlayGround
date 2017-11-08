//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ClassLibraryA.Interface;
//using PostSharp.Aspects;
//using PostSharp.Aspects.Advices;
//using PostSharp.Extensibility;
//using PostSharp.Serialization;
//using System.Collections;
//using System.Reflection;
//using System.Runtime.CompilerServices;

//namespace ClassLibraryA.Aspect
//{
//    [PSerializable]
//    [IntroduceInterface(typeof(IUpdateAware))]
//    public class UpdateAwareAttribute : InstanceLevelAspect, IUpdateAware
//    {
//        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail)]
//        public bool PrivateIsUpdate { get; private set; }

//        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail)]
//        public bool IsUpdated()
//        {
//            return PrivateIsUpdate;
//        }
//        public IEnumerable<PropertyInfo> FindTargetProperties(Type target)
//        {
//            return target.GetProperties().Where(p => p.GetCustomAttributes(typeof(KeepAnEyeOnAttribute), false).Length > 0);
//        }

//        [OnLocationSetValueAdvice, MethodPointcut("FindTargetProperties")]
//        public void OnPropertySet(LocationInterceptionArgs args)
//        {
//            Console.WriteLine("property:{0}",args.LocationFullName);

//            // Don't go further if the new value is equal to the old one.
//            // (Possibly use object.Equals here).
//            if (args.Value == args.GetCurrentValue()) return;

//            // Actually sets the value.
//            args.ProceedSetValue();

//            PrivateIsUpdate = true;
//            ////this.OnPropertyChangedMethod.Invoke(args.Location.Name);
//        }

        
//    }
//}
