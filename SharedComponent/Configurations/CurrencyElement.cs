using System.Configuration;

namespace SharedComponent.Configurations
{
    public class CurrencyElement : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("travelPort")]
        public TravelPortElement TravelPort
        {
            get { return (TravelPortElement)this["travelPort"]; }
            set { this["travelPort"] = value; }
        }
    }
}
