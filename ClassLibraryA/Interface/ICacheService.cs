using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryA.Interface
{
    public interface ICacheService
    {
        bool SetValue<T>(string userKey, T obj, string nameSpace, string set);
        T GetValue<T>(string userKey, string nameSpace, string set);
    }
}
