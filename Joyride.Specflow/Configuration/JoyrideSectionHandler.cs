using System.Configuration;
using HandyConfig.Configuration;

namespace Joyride.Specflow.Configuration
{
    public class JoyrideSectionHandler : ConfigurationSection
    {
        [ConfigurationProperty("capabilities", IsRequired = true)]
        public CapabilitiesElement Capabilities { get { return (CapabilitiesElement) base["capabilities"]; } }
        
        [ConfigurationProperty("servers", IsRequired = true)]
        public HandyConfigElement Servers { get { return (HandyConfigElement) base["servers"]; } }

        [ConfigurationProperty("run", IsRequired = true)]
        public HandyConfigElement Run { get { return (HandyConfigElement) base["run"]; } }
    }

}
