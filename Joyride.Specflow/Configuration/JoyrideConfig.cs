using System.Configuration;


namespace Joyride.Specflow.Configuration
{
    public class JoyrideConfig : ConfigurationSection
    {
        private static readonly JoyrideConfig _settings = ConfigurationManager.GetSection("joyride") as JoyrideConfig;

        public static JoyrideConfig Settings { get { return _settings;  }}

        [ConfigurationProperty("capabilities", IsRequired = true)]
        public GlobalCapabilityElement Capabilities { get { return (GlobalCapabilityElement) base["capabilities"]; } }
        
        [ConfigurationProperty("android")]
        public AndroidElement Android { get { return (AndroidElement) base["android"]; } }
        
        [ConfigurationProperty("ios")]
        public IosElement Ios { get { return (IosElement) base["ios"]; } }        

        [ConfigurationProperty("endpoints", IsRequired = true)]
        public EndPointElement Endpoints { get { return (EndPointElement) base["endpoints"]; } } 


    }

    public class EndPointElement : ConfigurationElement
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public NameValueConfigurationCollection Settings { get {  return (NameValueConfigurationCollection) base[""]; }}

    }

    public class IosElement : ConfigurationElement
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public NameValueConfigurationCollection Settings { get { return (NameValueConfigurationCollection)base[""]; } }
        
    }

    public class AndroidElement : ConfigurationElement
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public NameValueConfigurationCollection Settings { get { return (NameValueConfigurationCollection)base[""]; } }
    }

    public class GlobalCapabilityElement : ConfigurationElement
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public NameValueConfigurationCollection Settings { get { return (NameValueConfigurationCollection)base[""]; } }
    }

    /*
    public class CapabilityElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string) this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public object Value
        {
            get { return this["value"]; }
            set { this["value"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return (string) this["type"]; }
            set { this["type"] = value; }
        }

        public class CapabilityElementCollection : ConfigurationElementCollection
        {
            protected override ConfigurationElement CreateNewElement()
            {
                return new CapabilityElement();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((CapabilityElement) element).Name;
            }
        }
     
    }
      
     */
}
