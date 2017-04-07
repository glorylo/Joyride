using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using TechTalk.SpecFlow;
using Joyride.Platforms;


namespace Joyride.Specflow
{
    public static class Context
    {
        private const string MobileAppKey = "_MobileApp_";
        private const string DriverKey = "_Driver_";
        private const string CurrentUserKey = "_CurrentUser_";
          
        public static void SetValue(string key, object value)
        {
            ScenarioContext.Current[key] = value;
        }

        public static object GetValue(string key)
        {
            if (HasKey(key))
              return ScenarioContext.Current[key];
            return null;
        }

        public static bool HasKey(string key)
        {
            return ScenarioContext.Current.ContainsKey(key);
        }

        public static IMobileApplication MobileApp
        {
            get
            {
                return (IMobileApplication) GetValue(MobileAppKey);
            }
            set { SetValue(MobileAppKey, value); }
        }

        public static AppiumDriver<IWebElement> Driver
        {
            get { return (AppiumDriver<IWebElement>) GetValue(DriverKey); }
            set { SetValue(DriverKey, value); }
        }

        public static bool HasError
        {
            get { return ScenarioContext.Current.TestError != null; }            
        }
        
        public static object CurrentUser
        {
            get { return GetValue(CurrentUserKey); }
            set { SetValue(CurrentUserKey, value); }
        }

    }
}
