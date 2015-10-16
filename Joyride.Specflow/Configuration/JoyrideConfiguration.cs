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
        private static readonly NameValueTypeElementCollection Run = Config.Run.NameValueTypes;

        public static int TimeoutSecs = 30;
        public static int NonexistenceTimeoutSecs = 15;
        public static int MaxRetries = 40;
        public static int QuickInspectTimeoutSecs = 2;

        public const string RunsettingsServer = "server";
        public const string RunsettingsPlatform = "platform";
        public const string RunsettingsDevice = "device";

        public static string ScreenshotPath { get; private set; }
        public static string LogPath { get; private set; }
        private static string _projectDir;
        private static ConfigBundler _runBundler;
        private static ConfigBundler RunsBundler
        {
            get
            {
                if (_runBundler != null) return _runBundler;
                _runBundler = new ConfigBundler().Bundle(Run);
                return _runBundler;
            } 
        }

        public static Platform TargetPlatform
        {
            get
            {
                var platform = RunsBundler.Get<string>(RunsettingsPlatform);
                return (Platform) Enum.Parse(typeof (Platform), platform, true);
            }
        }

        public static string TargetDevice { get { return RunsBundler.Get<string>(RunsettingsDevice); } }
        public static string TargetServer { get { return RunsBundler.Get<string>(RunsettingsServer); } }

        public static void SetWorkingDirectory(string projectDir)
        {
            _projectDir = projectDir;
            LogPath = _projectDir + @"\Logs\";
            ScreenshotPath = _projectDir + @"\Screenshots\";
        }

        private static NameValueTypeElementCollection GetDeviceCapabilities(Platform platform, string deviceKey)
        {
            var devices = (platform == Platform.Android) ? Config.Capabilities.Android.Devices : Config.Capabilities.Ios.Devices;
        
            if (devices.ContainsKey(deviceKey))
                return devices[deviceKey].NameValueTypes;
            
            throw new KeyNotFoundException("Unable to find device on " + platform + " with key: " + deviceKey);
        }

        private static DesiredCapabilities BundleCapabilities(Platform platform, string deviceKey)
        {            
            var configBundler = new ConfigBundler();
            configBundler.Bundle(Capabilities)
              .Bundle(platform == Platform.Android ? AndroidCapabilities : IosCapabilities)
              .Bundle(GetDeviceCapabilities(platform, deviceKey));
            var configs = configBundler.GetConfigs();
            var capabilities = new DesiredCapabilities(configs as Dictionary<string, object>);
            return capabilities;

        }

        public static DesiredCapabilities BundleCapabilities()
        {
            return BundleCapabilities(TargetPlatform, TargetDevice);
        }

        public static Uri GetServerUri()
        {            
            var bundler = new ConfigBundler().Bundle(Servers);
            var serverValue = bundler.Get<string>(TargetServer);
            return new Uri(serverValue);
        }

    }
}
