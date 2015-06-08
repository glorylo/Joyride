using System.Configuration;
using System.Linq;

namespace Joyride.Specflow.Configuration
{
   
    public class DeviceElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DeviceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DeviceElement)element).Name;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "device"; }
        }

        public DeviceElement this[int index]
        {
            get { return (DeviceElement) BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public DeviceElement this[string key]
        {
            get { return (DeviceElement)BaseGet(key); }
        }

        public bool ContainsKey(string key)
        {
            var keys = BaseGetAllKeys();
            return keys.Any(obj => (string) obj == key);
        }
    }
 
}
