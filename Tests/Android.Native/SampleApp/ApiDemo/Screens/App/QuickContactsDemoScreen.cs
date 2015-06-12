using Joyride.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;

namespace Tests.Android.Native.SampleApp.ApiDemo.Screens.App
{
    public class QuickContactsDemoScreen : ApiDemoScreen
    {

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/list']//android.widget.TextView")]
        private IList<IWebElement> Contacts;

        public override bool IsOnScreen(int timeOutSecs)
        {
            var xpath = @"//android.widget.TextView[@text='App/Activity/QuickContactsDemo']";
            var header = Driver.FindElement(By.XPath(xpath), timeOutSecs);
            return header != null;
        }

        public override string Name
        {
            get { return "Quick Contacts Demo"; }
        }
    }
}
