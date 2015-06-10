using Joyride.Extensions;
using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Joyride.Android.Tests.SampleApp.ApiDemo.Screens
{
    public class MainScreen : ApiDemoScreen
    {
        [FindsBy(How = How.XPath, Using = "//[@resource-id='android:id/text1 and @text='App']")] 
        private IWebElement App;


        public override Screen Tap(string elementName, bool precise = false)
        {
            var screen = base.Tap(elementName, precise);
            switch (elementName)
            {
                case "App":
                    return ScreenFactory.CreateScreen<AppScreen>();

                default:
                    return screen;
            }
        }

        public override bool IsOnScreen(int timeOutSecs)
        {
            const string headerXpath = "//*[@resource-id='android:id/action_bar']/android.widget.TextView[@text='API Demos']";
            var header = Driver.FindElement(By.XPath(headerXpath), timeOutSecs);
            return header != null;
        }

        public override string Name
        {
            get { return "Main"; }
        }

        public override Screen GoBack()
        {
            Driver.Navigate().Back();
            return ScreenFactory.CreateScreen<NullApiDemoScreen>();
        }
    }
}
