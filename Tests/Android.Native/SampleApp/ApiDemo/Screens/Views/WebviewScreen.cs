using Joyride.Extensions;
using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Tests.Android.Native.SampleApp.ApiDemo.Screens.Views
{
    public class WebviewScreen : ApiDemoScreen
    {
        [Webview] [FindsBy(How = How.XPath, Using = "//a[text()='Hello World! - 1']")]
        private IWebElement HelloWorld;

        public override string Name { get { return "Webview"; } }

        public override bool IsOnScreen(int timeOutSecs)
        {
            var xpath =
                "//*[@resource-id='android:id/action_bar_container']//android.widget.TextView[@text='Views/WebView']";
            var element = Driver.FindElement(By.XPath(xpath), timeOutSecs);
            return element != null; 
        }

        
    }
}