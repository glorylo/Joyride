using System;
using Joyride.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Remote;


namespace Joyride
{
    public static class RemoteMobileDriver
    {

        //TODO: Consider abstracting driver 
        private static AppiumDriver<IWebElement> _driver;

        public const int DefaultWaitSeconds = 30;
        public static Platform Platform { get; set; }
        
        //TODO: unlikely to fix but not thread safe
        public static void Initialize(Uri hostUri, Platform platform, DesiredCapabilities capabilities)
        {
            if (_driver != null)
                throw new Exception("Unable to create multiple instances of appium driver");

            Platform = platform;
            switch (platform)
            {
                case Platform.Android:
                    _driver = new AndroidDriver<IWebElement>(hostUri, capabilities);
                    break;
                case Platform.Ios:
                    _driver = new IOSDriver<IWebElement>(hostUri, capabilities);
                    break;
                default:
                    throw new Exception("Unsupported driver for platform:  " + platform);
            }

            _driver.SetImplicitWait(TimeSpan.FromSeconds(DefaultWaitSeconds));
        }

        public static AppiumDriver<IWebElement> GetInstance()
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
