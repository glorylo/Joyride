using System.Collections.Generic;
using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Tests.Android.Native.SampleApp.ApiDemo.Screens.App
{
    public class IncomingMessageScreen : ApiDemoScreen
    {
        [FindsBy(How = How.XPath, Using = "//*[@resource-id='io.appium.android.apis:id/notify_app']")]
        private IWebElement ShowAppNotification;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='com.android.systemui:id/notification_stack_scroller']/android.widget.FrameLayout/android.view.View[@resource-id='com.android.systemui:id/backgroundNormal']")]        
        private IList<IWebElement> Notifications;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='com.android.systemui:id/notification_stack_scroller']/android.widget.FrameLayout[1]")]
        private IWebElement JoeNotification;
        
        public override Screen Tap(string elementName, bool precise = false)
        {
            var screen = base.Tap(elementName, precise);
            return screen;
        }

        public override bool IsOnScreen(int timeOutSecs)
        {
            return ElementExists("Show App Notification", timeOutSecs);
        }

        public override string Name
        {
            get { return "Incoming Message"; }
        }


    }
}