using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryA.Model.Exception
{
    public class PlayGroundException : System.Exception
    {
        public bool ShouldHandle { get; set; }

        public PlayGroundException(bool shouldHandle)
        {
            ShouldHandle = shouldHandle;
        }
    }
}
