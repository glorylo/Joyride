using System.Configuration;
using HandyConfig.Configuration;


namespace Joyride.Specflow.Configuration
{
    public class JoyrideSectionHandler : ConfigurationSection
    {
        public static JoyrideSectionHandler Settings { get { return ConfigurationManager.GetSection("joyride") as JoyrideSectionHandler;  }}

        [ConfigurationProperty("capabilities", IsRequired = true)]
        public CapabilitiesElement Capabilities { get { return (CapabilitiesElement) base["capabilities"]; } }
        
        [ConfigurationProperty("android")]
        public AndroidElement Android { get { return (AndroidElement) base["android"]; } }
        
        [ConfigurationProperty("ios")]
        public IosElement Ios { get { return (IosElement) base["ios"]; } }        

        [ConfigurationProperty("servers", IsRequired = true)]
        public ServersElement Servers { get { return (ServersElement) base["servers"]; } } 


    }

    public class DesiredCapabilityElement : ConfigurationElement
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(NameValueTypeElementCollection), AddItemName = "add")]
        public NameValueTypeElementCollection NameValueTypes
        {
            get { return (NameValueTypeElementCollection) this[""]; }
            set { this[""] = value; }
        }        
    }

    public class ServersElement : DesiredCapabilityElement
    {
        public static NameValueTypeElementCollection Settings = JoyrideSectionHandler.Settings.Servers.NameValueTypes;

 
    }

    public class IosElement : DesiredCapabilityElement
    {
        public static NameValueTypeElementCollection Settings = JoyrideSectionHandler.Settings.Ios.NameValueTypes;
        
    }

    public class AndroidElement : DesiredCapabilityElement
    {
        public static NameValueTypeElementCollection Settings = JoyrideSectionHandler.Settings.Android.NameValueTypes;

    }

    public class CapabilitiesElement : DesiredCapabilityElement
    {
        public static NameValueTypeElementCollection Settings = JoyrideSectionHandler.Settings.Capabilities.NameValueTypes;

    }


}
