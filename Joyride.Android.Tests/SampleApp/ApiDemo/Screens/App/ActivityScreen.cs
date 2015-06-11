using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Joyride.Android.Tests.SampleApp.ApiDemo.Screens.App
{
    public class ActivityScreen : ApiDemoScreen
    {
        [FindsBy(How = How.XPath, Using = "//android.widget.TextView[@text='Custom Title']")]
        private IWebElement CustomTitle;

        [FindsBy(How = How.XPath, Using = "//android.widget.TextView[@text='Presentation']")]
        private IWebElement Presentation;

        public override bool IsOnScreen(int timeOutSecs)
        {
            // not reliable 
            return ElementExists("Custom Title", timeOutSecs);;
        }

        public override Screen Tap(string elementName, bool precise = false)
        {
            var screen = base.Tap(elementName, precise);

            if (elementName == "Custom Title")
                return ScreenFactory.CreateScreen<CustomTitleScreen>();

            if (elementName == "Presentation")
                return ScreenFactory.CreateScreen<PresentationScreen>();

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
