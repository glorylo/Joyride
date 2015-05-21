using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using HandyConfig.Configuration;

namespace Joyride.Specflow.Configuration
{
    public class IosElement : HandyConfigElement
    {
        [ConfigurationProperty("devices")]
        public DeviceElementCollection Devices
        {
            get { return this["devices"] as DeviceElementCollection; }
        }
    }
}
