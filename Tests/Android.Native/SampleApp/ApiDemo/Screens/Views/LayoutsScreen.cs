using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Tests.Android.Native.SampleApp.ApiDemo.Screens.Views
{
    public class LayoutsScreen : ApiDemoScreen
    {
        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/text1' and @text='ScrollView']")]
        private IWebElement ScrollView;


        public override Screen Tap(string elementName, bool precise = false)
        {
            var screen = base.Tap(elementName, precise);

            switch (elementName)
            {
                case "Scroll View":
                    return ScreenFactory.CreateScreen<ScrollViewScreen>();

                default:
                    return screen;
            }
        }

        public override bool IsOnScreen(int timeOutSecs)
        {
            return ElementExists("Scroll View", timeOutSecs);
        }

        public override string Name
        {
            get { return "Layouts"; }
        }
    }
}
