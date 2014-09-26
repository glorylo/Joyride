using System;
using System.Drawing;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Remote;


namespace Joyride
{
    public static class RemoteMobileDriver
    {

        //TODO: Consider abstracting driver if there are new players in this space out there.  
        //Currently there is no other driver doing remote selenium interface for mobile.  
        //Simplicity of design wins out here.
        private static AppiumDriver _driver;
        public const int DefaultWaitSeconds = 30;
        public static int CommandTimeOutSeconds { get; set; }
        private static Size? _screenSize;
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

        static RemoteMobileDriver()
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

        static public AppiumDriver GetInstance()
        {
            return _driver;
        }
        
        static public void CleanUp()
        {
            _driver.Quit();
        }

    }
}
