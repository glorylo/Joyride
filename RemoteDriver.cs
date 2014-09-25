using System;
using System.Drawing;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Remote;


namespace Joyride
{
    public static class RemoteDriver
    {
        private static RemoteWebDriver _driver;
        public const int DefaultWaitSeconds = 30;
        public static int CommandTimeOutSeconds { get; set; }
        private static Size? _screenSize = null;
        public static bool EnableCustomWaits { get; set; }
        public static Size ScreenSize
        {
            get
            {
                if (_screenSize != null)
                    return (Size) _screenSize;
                _screenSize = _driver.Manage().Window.Size;
                return (Size) _screenSize;
            }
        }

        public static Point CenterLocation
        {
            get
            {
                var centerX = ScreenSize.Width/2;
                var centerY = ScreenSize.Height/2;
                return new Point(centerX, centerY);
            }
        }

        static RemoteDriver()
        {
            CommandTimeOutSeconds = DefaultWaitSeconds;
            EnableCustomWaits = true;
        }
        
        static public void Initialize(Uri hostUri, DesiredCapabilities capabilities)
        {
            _driver = new AppiumDriver(hostUri, capabilities);
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(DefaultWaitSeconds));
        }

        static public void SetTimeout(int seconds)
        {
            if (EnableCustomWaits && seconds != CommandTimeOutSeconds)
            {
                CommandTimeOutSeconds = seconds;
                _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(seconds));  
            }
        }
        
        static public void SetDefaultWait()
        {
            SetTimeout(DefaultWaitSeconds);
        }

        static public RemoteWebDriver GetInstance()
        {
            return _driver;
        }
        
        static public void CleanUp()
        {
            _driver.Quit();
        }

    }
}
