using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedComponent.Configurations
{
    public class AuthorElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "Arial", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        public String Name
        {
            get
            {
                return (String)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("age", DefaultValue = "18", IsRequired = false)]
        [IntegerValidator(ExcludeRange = false, MaxValue = 130, MinValue = 6)]
        public int Age
        {
            get
            { return (int)this["age"]; }
            set
            { this["age"] = value; }
        }
    }
}
