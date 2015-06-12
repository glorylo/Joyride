using Joyride.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Tests.Android.Native.SampleApp.ApiDemo.Screens.Views
{
    public class LongScreen : ApiDemoScreen
    {
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