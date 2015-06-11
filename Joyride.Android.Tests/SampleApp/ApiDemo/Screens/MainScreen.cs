using Joyride.Extensions;
using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Joyride.Android.Tests.SampleApp.ApiDemo.Screens
{
    public class MainScreen : ApiDemoScreen
    {
        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/text1' and @text='App']")] 
        private IWebElement App;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/text1' and @text='Animation']")]
        private IWebElement Animation;

        public override Screen Tap(string elementName, bool precise = false)
        {
            var screen = base.Tap(elementName, precise);
            switch (elementName)
            {
                case "App":
                    return ScreenFactory.CreateScreen<AppScreen>();

                case "Animation":
                    return ScreenFactory.CreateScreen<AnimationScreen>();

                default:
                    return screen;
            }
        }

        public override bool IsOnScreen(int timeOutSecs)
        {
            return ElementExists("App", timeOutSecs);
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
