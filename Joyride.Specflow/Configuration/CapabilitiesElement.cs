using System.Configuration;
using HandyConfig.Configuration;

namespace Joyride.Specflow.Configuration
{
    public class CapabilitiesElement : HandyConfigElement
    {
        public static NameValueTypeElementCollection Settings = JoyrideSectionHandler.Settings.Capabilities.NameValueTypes;

        [ConfigurationProperty("android")]
        public AndroidElement Android { get { return (AndroidElement) base["android"]; } }

        [ConfigurationProperty("ios")]
        public IosElement Ios { get { return (IosElement) base["ios"]; } }        

    }
}