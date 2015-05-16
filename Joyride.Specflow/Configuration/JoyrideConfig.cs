using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Runtime.InteropServices.ComTypes;


namespace Joyride.Specflow.Configuration
{
    public class JoyrideConfig : ConfigurationSection
    {
        private static readonly JoyrideConfig _settings = ConfigurationManager.GetSection("joyride") as JoyrideConfig;

        public static JoyrideConfig Settings { get { return _settings;  }}

        [ConfigurationProperty("capabilities", IsRequired = true)]
        public CapabilityElement Capabilities { get { return (CapabilityElement)base["capabilities"]; } }
        

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

    public class CapabilityElement : ConfigurationElement
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public NameValueConfigurationCollection Settings { get { return (NameValueConfigurationCollection)base[""]; } }
    }
}
