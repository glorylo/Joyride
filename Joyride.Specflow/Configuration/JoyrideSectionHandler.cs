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


    public class IosElement : HandyConfigElement
    {
        public static NameValueTypeElementCollection Settings = JoyrideSectionHandler.Settings.Ios.NameValueTypes;
        
    }

    public class AndroidElement : HandyConfigElement
    {
        public static NameValueTypeElementCollection Settings = JoyrideSectionHandler.Settings.Android.NameValueTypes;

    }
}
