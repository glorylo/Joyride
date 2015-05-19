using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyConfig.Configuration;
using OpenQA.Selenium.Remote;

namespace Joyride.Specflow.Configuration
{
    public class CapabilityBundler
    {
        private DesiredCapabilities _capabilities;
        private readonly Platform _platform;
        public CapabilityBundler(Platform platform)
        {
            _platform = platform;
        }

        public DesiredCapabilities Bundle(string targetName)
        {
            IDictionary<string, object> configs = new Dictionary<string, object>();
            var configBundler = new ConfigBundler(configs);
            configBundler.Bundle(CapabilitiesElement.Settings);
            if (_platform == Platform.Android)
                configBundler.Bundle(AndroidElement.Settings);
            else
                configBundler.Bundle(IosElement.Settings);
            
            configs = configBundler.GetConfigs();
            _capabilities = new DesiredCapabilities(configs as Dictionary<string, object>);
            return _capabilities;
        }



    }
}
