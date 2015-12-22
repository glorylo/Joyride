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

        static Context()
        {
            ScenarioContext.Current[MobileAppKey] = null;
            ScenarioContext.Current[DriverKey] = null;
            ScenarioContext.Current[CurrentUserKey] = null;
        }
          
        public static void SetValue(string key, object value)
        {
            ScenarioContext.Current[key] = value;
        }

        public static object GetValue(string key)
        {
            return ScenarioContext.Current[key];
        }

        public static IMobileApplication MobileApp
        {
            get {
                return (IMobileApplication)ScenarioContext.Current[MobileAppKey];
            }
            set { ScenarioContext.Current[MobileAppKey] = value; }
        }

        public static AppiumDriver<IWebElement> Driver
        {
            get { return (AppiumDriver<IWebElement>) ScenarioContext.Current[DriverKey]; }
            set { ScenarioContext.Current[DriverKey] = value; }
        }

        public static bool HasError
        {
            get { return ScenarioContext.Current.TestError != null; }            
        }
        
        public static object CurrentUser
        {
            get { return ScenarioContext.Current[CurrentUserKey]; }
            set { ScenarioContext.Current[CurrentUserKey] = value; }
        }

    }
}
