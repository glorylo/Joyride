using System.Configuration;
using HandyConfig.Configuration;


namespace Joyride.Specflow.Configuration
{
    public class JoyrideSectionHandler : ConfigurationSection
    {
        public static JoyrideSectionHandler Settings { get { return ConfigurationManager.GetSection("joyride") as JoyrideSectionHandler;  }}

        [ConfigurationProperty("capabilities", IsRequired = true)]
        public CapabilitiesElement Capabilities { get { return (CapabilitiesElement) base["capabilities"]; } }
        
        [ConfigurationProperty("servers", IsRequired = true)]
        public ServersElement Servers { get { return (ServersElement) base["servers"]; } }
    }


    public class TargetElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TargetElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TargetElement) element).Name;
        }

    }

    public class TargetElement : HandyConfigElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
    }


    public class IosElement : HandyConfigElement
    {
        public static NameValueTypeElementCollection Settings = JoyrideSectionHandler.Settings.Capabilities.Ios.NameValueTypes;


        
    }

    public class AndroidElement : HandyConfigElement
    {
        public static NameValueTypeElementCollection Settings = JoyrideSectionHandler.Settings.Capabilities.Android.NameValueTypes;

    }
}
