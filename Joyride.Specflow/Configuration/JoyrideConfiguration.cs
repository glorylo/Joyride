using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using HandyConfig.Configuration;
using OpenQA.Selenium.Remote;

namespace Joyride.Specflow.Configuration
{
    public static class JoyrideConfiguration
    {
        private static readonly JoyrideSectionHandler Config =
            ConfigurationManager.GetSection("joyride") as JoyrideSectionHandler;

        private static readonly NameValueTypeElementCollection Capabilities = Config.Capabilities.NameValueTypes;
        private static readonly NameValueTypeElementCollection AndroidCapabilities = Config.Capabilities.Android.NameValueTypes;
        private static readonly NameValueTypeElementCollection IosCapabilities = Config.Capabilities.Ios.NameValueTypes;
        private static readonly NameValueTypeElementCollection Servers = Config.Servers.NameValueTypes; 

        public static Uri GetServer(string serverName = "dev")
        {
            var bundler = new ConfigBundler(new Dictionary<string, object>()).Bundle(Servers);
            var serverValue = bundler.Get<string>(serverName);
            return new Uri(serverValue);
        }

        public static DesiredCapabilities BundleCapabilities(Platform platform, string targetName)
        {
            IDictionary<string, object> configs = new Dictionary<string, object>();
            var configBundler = new ConfigBundler(configs);
            configBundler.Bundle(Capabilities);
            configBundler.Bundle(platform == Platform.Android ? AndroidCapabilities : IosCapabilities);

            configs = configBundler.GetConfigs();
            var capabilities = new DesiredCapabilities(configs as Dictionary<string, object>);
            return capabilities;

        }
    }
}
