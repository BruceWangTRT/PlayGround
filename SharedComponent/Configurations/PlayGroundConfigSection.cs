using System.Configuration;

namespace SharedComponent.Configurations
{
    public class PlayGroundConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("inUse", DefaultValue = "false", IsRequired = true)]
        public bool InUse
        {
            get { return (bool)this["inUse"]; }
            set { this["inUse"] = value; }
        }

        [ConfigurationProperty("author")]
        public AuthorElement Author
        {
            get { return (AuthorElement)this["author"]; }
            set { this["author"] = value; }
        }

        [ConfigurationProperty("currencies")]
        [ConfigurationCollection(typeof(CurrencyCollection), AddItemName = "currency")]
        public CurrencyCollection Currencies
        {
            get
            {
                return (CurrencyCollection)base["currencies"];
            }
        }
    }
}
