using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Tests.Android.Native.SampleApp.ApiDemo.Screens.App
{
    public class ActivityScreen : ApiDemoScreen
    {
        [FindsBy(How = How.XPath, Using = "//android.widget.TextView[@text='Custom Title']")]
        private IWebElement CustomTitle;

        [FindsBy(How = How.XPath, Using = "//android.widget.TextView[@text='Presentation']")]
        private IWebElement Presentation;

        [FindsBy(How = How.XPath, Using = "//android.widget.TextView[@text='QuickContactsDemo']")]
        private IWebElement QuickContactsDemo;

        public override bool IsOnScreen(int timeOutSecs)
        {
            // not reliable 
            return IsPresent("Custom Title", timeOutSecs); ;
        }

        public override Screen Tap(string elementName, bool precise = false)
        {
            var screen = base.Tap(elementName, precise);

            if (elementName == "Custom Title")
                return ScreenFactory.CreateScreen<CustomTitleScreen>();

            if (elementName == "Presentation")
                return ScreenFactory.CreateScreen<PresentationScreen>();

            if (elementName == "Quick Contacts Demo")
                return ScreenFactory.CreateScreen<QuickContactsDemoScreen>();

            return screen;
        }

        public override string Name
        {
            get { return "Activity"; }
        }

        public override Screen GoBack()
        {
            Driver.Navigate().Back();
            return ScreenFactory.CreateScreen<MainScreen>();
        }
    }
}
