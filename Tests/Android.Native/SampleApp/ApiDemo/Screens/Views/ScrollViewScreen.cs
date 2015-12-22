using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Tests.Android.Native.SampleApp.ApiDemo.Screens.Views
{
    public class ScrollViewScreen : ApiDemoScreen
    {
        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/text1' and @text='2. Long']")]
        private IWebElement Long;

        public override bool IsOnScreen(int timeOutSecs)
        {
            return IsPresent("Long", timeOutSecs);
        }

        public override Joyride.Platforms.Screen Tap(string elementName, bool precise = false)
        {
            var screen = base.Tap(elementName, precise);

            switch (elementName)
            {
                case "Long":
                    return ScreenFactory.CreateScreen<LongScreen>();
                default:
                    return screen;
            }
        }
        public override string Name
        {
            get { return "Scroll View"; }
        }
    }
}
