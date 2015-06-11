using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Joyride.Android.Tests.SampleApp.ApiDemo.Screens
{
    public class AppScreen : ApiDemoScreen
    {

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/text1' and @text='Activity']")]
        private IWebElement Activity;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/text1' and @text='Alert Dialogs']")]
        private IWebElement AlertDialogs;


        public override Screen Tap(string elementName, bool precise = false)
        {
            var screen = base.Tap(elementName, precise);

            if (elementName == "Alert Dialogs")
                return ScreenFactory.CreateScreen<AlertDialogsScreen>();

            if (elementName == "Activity")
                return ScreenFactory.CreateScreen<ActivityScreen>();

            return screen;
        }
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