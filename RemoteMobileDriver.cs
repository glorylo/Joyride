using System;
using System.Drawing;
using Joyride.Extensions;
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
        public static bool EnableCustomWaits { get; set; }

        static RemoteMobileDriver()
        {
            CommandTimeOutSeconds = DefaultWaitSeconds;
            EnableCustomWaits = true;
        }
        
        //TODO: unlikely to fix but not thread safe
        static public void Initialize(Uri hostUri, DesiredCapabilities capabilities)
        {
            if (_driver != null)
                throw new Exception("Unable to create multiple instances of appium driver");

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
