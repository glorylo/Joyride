using System;
using System.Collections.Generic;
using HandyConfig.Configuration;
using OpenQA.Selenium.Remote;

namespace Joyride.Specflow.Configuration
{
    public static class JoyrideConfiguration
    {
        public static Uri GetServer(string serverName = "dev")
        {
            var bundler = new ConfigBundler(new Dictionary<string, object>()).Bundle(ServersElement.Settings);
            var serverValue = bundler.Get<string>(serverName);
            return new Uri(serverValue);
        }

        public static DesiredCapabilities BundleCapabilities(Platform platform, string targetName)
        {
            IDictionary<string, object> configs = new Dictionary<string, object>();
            var configBundler = new ConfigBundler(configs);
            configBundler.Bundle(CapabilitiesElement.Settings);
            configBundler.Bundle(platform == Platform.Android ? AndroidElement.Settings : IosElement.Settings);

            configs = configBundler.GetConfigs();
            var capabilities = new DesiredCapabilities(configs as Dictionary<string, object>);
            return capabilities;

        }
    }
}
