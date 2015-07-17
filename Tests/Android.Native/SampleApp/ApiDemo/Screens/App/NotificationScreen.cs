using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Tests.Android.Native.SampleApp.ApiDemo.Screens.App
{
    public class NotificationScreen : ApiDemoScreen
    {
        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/text1' and @text='IncomingMessage']")]
        private IWebElement IncomingMessage;

        public override Screen Tap(string elementName, bool precise = false)
        {
            var screen = base.Tap(elementName, precise);

            if (elementName == "Incoming Message")
                return ScreenFactory.CreateScreen<IncomingMessageScreen>();
            return screen;
        }
        public override bool IsOnScreen(int timeOutSecs)
        {
            return ElementExists("Incoming Message", timeOutSecs);
        }

        public override string Name
        {
            get { return "Notification"; }
        }
    }
}