using Joyride.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Tests.Android.Native.SampleApp.ApiDemo.Screens.Views
{
    public class LongScreen : ApiDemoScreen
    {
        [FindsBy(How = How.XPath, Using = "//android.widget.Button[@text='Button 1']")]
        private IWebElement Button1;

        [FindsBy(How = How.XPath, Using = "//android.widget.Button[@text='Button 15']")]
        private IWebElement Button15;

        [FindsBy(How = How.XPath, Using = "//android.widget.Button[@text='Button 25']")]
        private IWebElement Button25;

        [FindsBy(How = How.XPath, Using = "//android.widget.Button[@text='Button 35']")]
        private IWebElement Button35;

        [FindsBy(How = How.XPath, Using = "//android.widget.Button[@text='Button 45']")]
        private IWebElement Button45;

        [FindsBy(How = How.XPath, Using = "//android.widget.Button[@text='Button 55']")]
        private IWebElement Button55;

        [FindsBy(How = How.XPath, Using = "//android.widget.Button[@text='Button 63']")] 
        private IWebElement Button63;

        public override bool IsOnScreen(int timeOutSecs)
        {
            var xpath = "//android.widget.TextView[@text='Views/Layouts/ScrollView/2. Long']";
            var element = Driver.FindElement(By.XPath(xpath), timeOutSecs);
            return element != null;
        }

        public override string Name
        {
            get { return "Long"; }
        }
    }
}