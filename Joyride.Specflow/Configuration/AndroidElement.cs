using System.Configuration;
using HandyConfig.Configuration;

namespace Joyride.Specflow.Configuration
{
    public class AndroidElement : HandyConfigElement
    {
        [ConfigurationProperty("devices", IsRequired = true)]
        public DeviceElementCollection Devices
        {
            get { return this["devices"] as DeviceElementCollection; }
        }
    }
}