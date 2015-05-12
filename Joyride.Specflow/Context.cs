using OpenQA.Selenium.Appium;
using TechTalk.SpecFlow;
using Joyride.Platforms;


namespace Joyride.Specflow
{
    public static class Context
    {
        static Context()
        {
            ScenarioContext.Current["MobileApp"] = null;
            ScenarioContext.Current["Driver"] = null;
            ScenarioContext.Current["LogPath"] = @".\";
            ScenarioContext.Current["ScreenshotPath"] = @".\";
            ScenarioContext.Current["CurrentUser"] = null;

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
                return (IMobileApplication) ScenarioContext.Current["MobileApp"];
            }
            set { ScenarioContext.Current["MobileApp"] = value; }
        }

        public static AppiumDriver Driver
        {
            get { return (AppiumDriver) ScenarioContext.Current["Driver"]; }
            set { ScenarioContext.Current["Driver"] = value;  }
        }

        public static string LogPath
        {
            get { return (string) ScenarioContext.Current["LogPath"]; }
            set { ScenarioContext.Current["LogPath"] = value; }
        }

        public static string ScreenshotPath
        {
            get { return (string)ScenarioContext.Current["ScreenshotPath"]; }
            set { ScenarioContext.Current["ScreenshotPath"] = value; }
        }

        public static object CurrentUser
        {
            get { return ScenarioContext.Current["CurrentUser"];  }
            set { ScenarioContext.Current["CurrentUser"] = value;  }
        }

    }
}
