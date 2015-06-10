using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Joyride.Android.Tests.SampleApp.ApiDemo.Screens
{
    public class AppScreen : ApiDemoScreen
    {

        [FindsBy(How = How.XPath, Using = "//[@resource-id='android:id/text1 and @text='Activity']")]
        private IWebElement Activity;

        public override bool IsOnScreen(int timeOutSecs)
        {
            return ElementExists("Activity", timeOutSecs);
        }

        public override string Name
        {
            get { return "App"; }
        }

        public override Screen GoBack()
        {
            Driver.Navigate().Back();
            return ScreenFactory.CreateScreen<MainScreen>();
        }
    }
}