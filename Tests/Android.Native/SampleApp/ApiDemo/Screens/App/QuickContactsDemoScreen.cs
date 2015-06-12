using Joyride.Extensions;
using Joyride.Interfaces;
using Joyride.Support;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;

namespace Tests.Android.Native.SampleApp.ApiDemo.Screens.App
{
    public class QuickContactsDemoScreen : ApiDemoScreen, IEntryEnumerable
    {

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/list']//android.widget.TextView")]
        private IList<IWebElement> FirstNames;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/list']/android.widget.RelativeLayout")]
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

        public IEnumerable<dynamic> GetEntries(string collectionName)
        {
            var size = SizeOf("Contacts");

            var properties = new Dictionary<string, string>
            {
                { "Badge", "//*[@resource-id='io.appium.android.apis:id/badge']" },
                { "FirstName", "//*[@resource-id='io.appium.android.apis:id/name']" }
            };

            var creator = new EntryCreator(this, "Contacts", properties);
            for (int i = 1; i <= size; i++)
            {
                yield return creator.GetNextEntry(i);
            }

        }
    }
}
