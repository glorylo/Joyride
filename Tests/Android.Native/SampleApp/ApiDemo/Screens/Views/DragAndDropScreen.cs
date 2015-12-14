using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Tests.Android.Native.SampleApp.ApiDemo.Screens.Views
{
    public class DragAndDropScreen : ApiDemoScreen
    {
        [FindsBy(How = How.XPath, Using = "//android.widget.TextView[@text='Views/Drag and Drop']")]
        private IWebElement DragAndDropTitle;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='io.appium.android.apis:id/drag_dot_1']")]
        private IWebElement UpperLeftDot;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='io.appium.android.apis:id/drag_dot_2']")]
        private IWebElement UpperRightDot;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='io.appium.android.apis:id/drag_dot_3']")]
        private IWebElement LowerLeftDot;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='io.appium.android.apis:id/drag_result_text']")]
        private IWebElement DropResult;        
        
        public override string Name { get { return "Drag and Drop"; } }

        public override bool IsOnScreen(int timeOutSecs)
        {
            return Exists("Drag And Drop Title", timeOutSecs);
        }

        
    }
}
