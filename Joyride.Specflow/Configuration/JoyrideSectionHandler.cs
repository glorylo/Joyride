using System.Configuration;
using HandyConfig.Configuration;
using Humanizer;
using System;
using System.Collections.Generic;


namespace Joyride.Specflow.Configuration
{
    public class JoyrideSectionHandler : ConfigurationSection
    {
        [ConfigurationProperty("capabilities", IsRequired = true)]
        public CapabilitiesElement Capabilities { get { return (CapabilitiesElement) base["capabilities"]; } }
        
        [ConfigurationProperty("servers", IsRequired = true)]
        public ServersElement Servers { get { return (ServersElement) base["servers"]; } }
    }

    [ConfigurationCollection(typeof(TargetElement))]
    public class TargetElementCollection : ConfigurationElementCollection
    {
        protected override string ElementName
        {
            get { return "target"; }
        }

        protected override bool IsElementName(string elementName)
        {
            return elementName.Equals("target", StringComparison.InvariantCultureIgnoreCase);
        }

        public TargetElement this[int id]
        {
            get  {  return (TargetElement) BaseGet(id); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TargetElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TargetElement) element).Key;
        }
        /*
        public bool ContainsKey(string key)
        {
            var result = false;
            object[] keys = BaseGetAllKeys();

            foreach (object obj in keys)
            {
                if ((string)obj == key)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }  */

    }

    
    public class TargetsElement : ConfigurationElement
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(TargetElementCollection))]
        public TargetElementCollection Targets {
            get { return this[""] as TargetElementCollection;  }
            set { this[""] = value; }
        } 
                                                    
        /*                                         }
        public IEnumerable<TargetElement> TargetElements
        {
            get
            {
                foreach (TargetElement target in Targets)
                {
                    if (target != null)
                        yield return target;
                }
            }
        }
         */ 
        
      
    }
     

    public class TargetElement : ConfigurationElement
    {
        
        [ConfigurationProperty("key", IsKey = true, IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }
        
        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(NameValueTypeElementCollection), AddItemName = "add")]
        public NameValueTypeElementCollection NameValueTypes
        {
            get { return (NameValueTypeElementCollection)this[""]; }
            set { this[""] = value; }
        }
        

    }


    public class IosElement : HandyConfigElement
    {

        


        
    }

    public class AndroidElement : HandyConfigElement
    {
        [ConfigurationProperty("targets", IsRequired = true)]
        
        public TargetsElement Targets
        {
            get
            {
                return (TargetsElement) base["targets"];
            }
        }




   

    }
}
