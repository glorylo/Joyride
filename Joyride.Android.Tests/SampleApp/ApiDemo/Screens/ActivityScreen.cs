using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Joyride.Android.Tests.SampleApp.ApiDemo.Screens
{
    public class ActivityScreen : ApiDemoScreen
    {
        [FindsBy(How = How.XPath, Using = "//android.widget.TextView[@text='Custom Title']")]
        private IWebElement CustomTitle;

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

            return screen;
        }
        public override string Name
        {
            get { return "Activity"; }
        }

        public override Screen GoBack()
        {
            return ScreenFactory.CreateScreen<MainScreen>();
        }
    }
}
