using System.Configuration;
using HandyConfig.Configuration;


namespace Joyride.Specflow.Configuration
{
    public class JoyrideSection : ConfigurationSection
    {
        public static JoyrideSection Settings { get { return ConfigurationManager.GetSection("joyride") as JoyrideSection;  }}

        [ConfigurationProperty("capabilities", IsRequired = true)]
        public CapabilitiesElement Capabilities { get { return (CapabilitiesElement) base["capabilities"]; } }
        
        [ConfigurationProperty("android")]
        public AndroidElement Android { get { return (AndroidElement) base["android"]; } }
        
        [ConfigurationProperty("ios")]
        public IosElement Ios { get { return (IosElement) base["ios"]; } }        

        [ConfigurationProperty("servers", IsRequired = true)]
        public ServersElement Servers { get { return (ServersElement) base["servers"]; } } 


    }

    public class ServersElement : ConfigurationElement
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public NameValueTypeElementCollection Settings { get { return (NameValueTypeElementCollection) base[""]; } }

    }

    public class IosElement : ConfigurationElement
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public NameValueTypeElementCollection Settings { get { return (NameValueTypeElementCollection) base[""]; } }
        
    }

    public class AndroidElement : ConfigurationElement
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public NameValueTypeElementCollection Settings { get { return (NameValueTypeElementCollection) base[""]; } }
    }

    public class CapabilitiesElement : ConfigurationElement
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public NameValueTypeElementCollection Settings { get { return (NameValueTypeElementCollection) base[""]; } }
    }


}
