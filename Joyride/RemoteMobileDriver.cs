using System;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Remote;


namespace Joyride
{
    public static class RemoteMobileDriver
    {

        //TODO: Consider abstracting driver 
        private static AppiumDriver _driver;

        public const int DefaultWaitSeconds = 30;
        public static int CommandTimeOutSeconds { get; set; }       
        public static bool EnableCustomWaits { get; set; }
        public static Platform Platform { get; set; }

        static RemoteMobileDriver()
        {
            CommandTimeOutSeconds = DefaultWaitSeconds;
            EnableCustomWaits = true;
        }
        
        //TODO: unlikely to fix but not thread safe
        public static void Initialize(Uri hostUri, Platform platform, DesiredCapabilities capabilities)
        {
            if (_driver != null)
                throw new Exception("Unable to create multiple instances of appium driver");

            Platform = platform;
            switch (platform)
            {
                case Platform.Android:
                    _driver = new AndroidDriver(hostUri, capabilities);
                    break;
                case Platform.Ios:
                    _driver = new IOSDriver(hostUri, capabilities);
                    break;
                default:
                    throw new Exception("Unsupported driver for platform:  " + platform);
            }

            SetImplicitWait(TimeSpan.FromSeconds(DefaultWaitSeconds));
        }

        private static void SetImplicitWait(TimeSpan span)
        {
            try
            {
                _driver.Manage().Timeouts().ImplicitlyWait(span);
            }
            catch { } // suppress errors for now
            
        }

        public static void SetTimeout(int seconds)
        {
            if (EnableCustomWaits && seconds != CommandTimeOutSeconds)
            {
                CommandTimeOutSeconds = seconds;
                SetImplicitWait(TimeSpan.FromSeconds(seconds));
            }
        }
        
        public static void SetDefaultWait()
        {
            SetTimeout(DefaultWaitSeconds);
        }

        static public AppiumDriver GetInstance()
        {
            return _driver;
        }
        
        public static void CleanUp()
        {
            _driver.Quit();
            _driver = null;
        }

    }
}
