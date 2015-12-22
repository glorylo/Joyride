using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Tests.Android.Native.SampleApp.ApiDemo.Screens.App
{
    public class CustomTitleScreen : ApiDemoScreen
    {
        [FindsBy(How = How.Id, Using = "io.appium.android.apis:id/left_text_button")]
        private IWebElement ChangeLeft;

        [FindsBy(How = How.Id, Using = "io.appium.android.apis:id/right_text_button")]
        private IWebElement ChangeRight;

        [FindsBy(How = How.Id, Using = "io.appium.android.apis:id/left_text_edit")]
        private IWebElement Left;

        [FindsBy(How = How.Id, Using = "io.appium.android.apis:id/right_text_edit")] 
        private IWebElement Right;

        [FindsBy(How = How.Id, Using = "io.appium.android.apis:id/left_text")] 
        private IWebElement LeftTitle;

        [FindsBy(How = How.Id, Using = "io.appium.android.apis:id/right_text")]
        private IWebElement RightTitle;

        public override bool IsOnScreen(int timeOutSecs)
        {
            return IsPresent("Change Left", timeOutSecs) && IsPresent("Change Right", timeOutSecs);
        }

        public override string Name
        {
            get { return "Custom Title"; }
        }

        public override Screen GoBack()
        {
            Driver.Navigate().Back();
            return ScreenFactory.CreateScreen<ActivityScreen>();
        }
    }
}
