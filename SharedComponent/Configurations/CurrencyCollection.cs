using System.Configuration;

namespace SharedComponent.Configurations
{
    public class CurrencyCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CurrencyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CurrencyElement)element).Name;
        }

        public CurrencyElement GetByName(string name)
        {
            return (CurrencyElement)BaseGet(name);
        }
    }
}
