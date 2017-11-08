using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ClassLibraryA.Utility
{
    public static class Converter
    {
        public static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings
        {
            //DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static string SerializeToStringWithDefaultSettings<T>(this T data)
        {
            return JsonConvert.SerializeObject(data, DefaultJsonSerializerSettings);
        }

        public static T DeserializeToTypeWithDefaultSettings<T>(this string dataString)
        {
            return JsonConvert.DeserializeObject<T>(dataString, DefaultJsonSerializerSettings);
        }
    }
}
