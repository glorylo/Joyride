using System;
using System.Collections.Generic;
using System.Configuration;
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

        public static int TimeoutSecs = 30;
        public static int NonexistenceTimeoutSecs = 15;

        public static string ScreenshotPath { get; private set; }
        public static string LogPath { get; private set; }
        private static string _projectDir;

        public static void SetWorkingDirectory(string projectDir)
        {
            _projectDir = projectDir;
            LogPath = _projectDir + @"\Logs\";
            ScreenshotPath = _projectDir + @"\Screenshots\";
        }

        public static Uri GetServer(string serverName = "dev")
        {
            var bundler = new ConfigBundler().Bundle(Servers);
            var serverValue = bundler.Get<string>(serverName);
            return new Uri(serverValue);
        }

        private static NameValueTypeElementCollection GetDeviceCapabilities(Platform platform, string deviceKey)
        {
            var devices = (platform == Platform.Android) ? Config.Capabilities.Android.Devices : Config.Capabilities.Ios.Devices;
        
            if (devices.ContainsKey(deviceKey))
                return devices[deviceKey].NameValueTypes;
            
            throw new KeyNotFoundException("Unable to find device on " + platform + " with key: " + deviceKey);
        }

        public static DesiredCapabilities BundleCapabilities(Platform platform, string deviceKey)
        {            
            var configBundler = new ConfigBundler();
            configBundler.Bundle(Capabilities)
              .Bundle(platform == Platform.Android ? AndroidCapabilities : IosCapabilities)
              .Bundle(GetDeviceCapabilities(platform, deviceKey));
            var configs = configBundler.GetConfigs();
            var capabilities = new DesiredCapabilities(configs as Dictionary<string, object>);
            return capabilities;

        }
    }
}
