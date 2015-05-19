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
        private readonly Platform _platform;
        private readonly string _targetName;

        public CapabilityBundler(Platform platform, string targetName)
        {
            _platform = platform;
            _targetName = targetName;
        }

        public DesiredCapabilities Bundle()
        {
            IDictionary<string, object> configs = new Dictionary<string, object>();
            var configBundler = new ConfigBundler(configs);
            configBundler.Bundle(CapabilitiesElement.Settings);
            configBundler.Bundle(_platform == Platform.Android ? AndroidElement.Settings : IosElement.Settings);

            configs = configBundler.GetConfigs();
            var capabilities = new DesiredCapabilities(configs as Dictionary<string, object>);
            return capabilities;
        }



    }
}
