using System.Configuration;

namespace SharedComponent.Configurations
{
    public class TravelPortElement : ConfigurationElement
    {
        [ConfigurationProperty("hostAccessProfile")]
        public string HostAccessProfile
        {
            get { return (string)this["hostAccessProfile"]; }
            set { this["hostAccessProfile"] = value; }
        }

        [ConfigurationProperty("userName")]
        public string UserName
        {
            get { return (string)this["userName"]; }
            set { this["userName"] = value; }
        }

        [ConfigurationProperty("password")]
        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }

        [ConfigurationProperty("bsp")]
        public string Bsp
        {
            get { return (string)this["bsp"]; }
            set { this["bsp"] = value; }
        }
    }
}
