using Joyride.Extensions;
using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Joyride.Android.Tests.SampleApp.ApiDemo.Screens
{
    public class PresentationScreen : ApiDemoScreen
    {
        [FindsBy(How = How.Id, Using = "io.appium.android.apis:id/show_all_displays")]
        private IWebElement ShowAllDisplays;
        
        public override bool IsOnScreen(int timeOutSecs)
        {
            var title = Driver.FindElementWithImplicitWait(By.XPath("//android.widget.TextView[@text='App/Activity/Presentation']"));
            return title != null;
        }

        public override string Name
        {
            get { return "Presentation"; }
        }

        public override Screen GoBack()
        {
            Driver.Navigate().Back();
            return ScreenFactory.CreateScreen<ActivityScreen>();
        }
    }
}
