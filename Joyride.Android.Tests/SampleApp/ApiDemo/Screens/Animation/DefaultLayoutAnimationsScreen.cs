using System.Collections.Generic;
using Joyride.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Joyride.Android.Tests.SampleApp.ApiDemo.Screens.Animation
{
    public class DefaultLayoutAnimationsScreen : ApiDemoScreen
    {
        [FindsBy(How = How.Id, Using = "io.appium.android.apis:id/addNewButton")][CacheLookup]
        private IWebElement AddButton;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='io.appium.android.apis:id/gridContainer']/android.widget.Button")]
        private IList<IWebElement> Buttons;

        public override bool IsOnScreen(int timeOutSecs)
        {
            var headerXpath = "//android.widget.TextView[@text='Animation/Default Layout Animations']";
            var element = Driver.FindElement(By.XPath(headerXpath), timeOutSecs);
            return element != null;
        }

        public override string Name
        {
            get { return "Default Layout Animations"; }
        }
    }
}