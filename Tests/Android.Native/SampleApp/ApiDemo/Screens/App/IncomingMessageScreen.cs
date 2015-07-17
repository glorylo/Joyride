using System;
using System.Collections.Generic;
using Joyride;
using Joyride.Extensions;
using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Support.PageObjects;

namespace Tests.Android.Native.SampleApp.ApiDemo.Screens.App
{
    public class IncomingMessageScreen : ApiDemoScreen
    {
        [FindsBy(How = How.XPath, Using = "//*[@resource-id='io.appium.android.apis:id/notify_app']")]
        private IWebElement ShowAppNotification;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='com.android.systemui:id/notification_stack_scroller']/android.widget.FrameLayout")]
        
        private IList<IWebElement> Notifications;

        public Screen SwipeInCollection(string collectionName, Direction direction, int oridinal = 1, bool last = false)
        {
            var elements = FindElements("Notifications");
            var element = GetElementInCollection(elements, oridinal, last);


            var center = element.GetCenter();
            new TouchAction(Driver).Press(center.X, center.Y).Wait(500).MoveTo(0,center.Y).Release().Perform();
            return this;
        }


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