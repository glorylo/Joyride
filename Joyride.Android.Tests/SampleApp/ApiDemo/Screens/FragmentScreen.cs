using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Joyride.Android.Tests.SampleApp.ApiDemo.Screens
{
    public class FragmentScreen : ApiDemoScreen
    {
        [FindsBy(How = How.XPath, Using = "//android.widget.TextView[@text='Context Menu']")]
        private IWebElement ContextMenu;

        [FindsBy(How = How.XPath, Using = "//android.widget.TextView[@text='Presentation']")]
        private IWebElement Presentation;

        public override bool IsOnScreen(int timeOutSecs)
        {
            // not reliable 
            return ElementExists("Context Menu", timeOutSecs); ;
        }

        public override Screen Tap(string elementName, bool precise = false)
        {
            var screen = base.Tap(elementName, precise);

            if (elementName == "Context Menu")
                return ScreenFactory.CreateScreen<ContextMenuScreen>();

            return screen;
        }
        public override string Name
        {
            get { return "Fragment"; }
        }

        public override Screen GoBack()
        {
            return ScreenFactory.CreateScreen<AppScreen>();
        }
    }
}
