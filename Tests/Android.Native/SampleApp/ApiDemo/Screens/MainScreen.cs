using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Tests.Android.Native.SampleApp.ApiDemo.Screens.Animation;
using Tests.Android.Native.SampleApp.ApiDemo.Screens.App;
using Tests.Android.Native.SampleApp.ApiDemo.Screens.Views;

namespace Tests.Android.Native.SampleApp.ApiDemo.Screens
{
    public class MainScreen : ApiDemoScreen
    {
        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/text1' and @text='App']")] 
        private IWebElement App;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/text1' and @text='Animation']")]
        private IWebElement Animation;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/text1' and @text='Views']")]
        private IWebElement Views;

        public override Screen Tap(string elementName, bool precise = false)
        {
            var screen = base.Tap(elementName, precise);
            switch (elementName)
            {
                case "App":
                    return ScreenFactory.CreateScreen<AppScreen>();

                case "Animation":
                    return ScreenFactory.CreateScreen<AnimationScreen>();

                case "Views":
                    return ScreenFactory.CreateScreen<ViewsScreen>();

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
