using System.Collections.Generic;
using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Tests.Android.Native.SampleApp.ApiDemo.Screens.App
{
    public class ContextMenuScreen : ApiDemoScreen
    {
        [FindsBy(How = How.Id, Using = "io.appium.android.apis:id/long_press")] 
        private IWebElement LongPress;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/select_dialog_listview']//*[@resource-id='android:id/title']")]
        private IList<IWebElement> Menu;
 
        public override bool IsOnScreen(int timeOutSecs)
        {
            return IsPresent("Long Press", timeOutSecs);
        }

        public override string Name
        {
            get { return "Context Menu"; }
        }

        public override Screen GoBack()
        {
            Driver.Navigate().Back();
            return ScreenFactory.CreateScreen<FragmentScreen>();
        }
    }
}