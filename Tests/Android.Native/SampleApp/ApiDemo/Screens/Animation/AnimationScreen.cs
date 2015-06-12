using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Tests.Android.Native.SampleApp.ApiDemo.Screens.Animation
{
    public class AnimationScreen : ApiDemoScreen
    {
        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/text1' and @text='Default Layout Animations']")]
        private IWebElement DefaultLayoutAnimations;

        public override bool IsOnScreen(int timeOutSecs)
        {
            return ElementExists("Default Layout Animations", timeOutSecs);
        }

        public override Screen Tap(string elementName, bool precise = false)
        {
            var screen = base.Tap(elementName, precise);

            switch (elementName)
            {
                case "Default Layout Animations":
                    return ScreenFactory.CreateScreen<DefaultLayoutAnimationsScreen>();
                default:
                    return screen;
            }
        }

        public override string Name
        {
            get { return "Animation"; }
        }
    }
}
