using Joyride.Platforms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Joyride.Android.Tests.SampleApp.ApiDemo.ModalDialogs
{
    public class LongMessageModalDialog : ApiDemoModalDialog
    {
        [FindsBy(How = How.XPath, Using = "//android.widget.Button[@text='OK']")]
        private IWebElement Ok;

        [FindsBy(How = How.XPath, Using = "//android.widget.Button[@text='Cancel']")]
        private IWebElement Cancel;

        [FindsBy(How = How.XPath, Using = "//android.widget.Button[@text='Something']")]
        private IWebElement Something;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/message']")]
        private IWebElement BodyMessage;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/alertTitle']")]
        private IWebElement AlertTitle;


        public LongMessageModalDialog()
        {
           TransitionMap.Add("Ok", NoTransition);
           TransitionMap.Add("Cancel", NoTransition);
           TransitionMap.Add("Something", NoTransition);
        }

        public override bool IsOnScreen(int timeOutSecs)
        {
            return ElementExists("Something", timeOutSecs) &&
                   BodyMessage.Text.StartsWith("Plloaso mako nuto siwuf cakso dodtos anr koop");
        }

        public override string Body
        {
            get { return BodyMessage.Text; }
        }

        public override string Title
        {
            get { return AlertTitle.Text; }
        }

        public override Screen Accept()
        {
            Ok.Click();
            return TransitionFromResponse("Ok");
        }

        public override Screen Dismiss()
        {
            Cancel.Click();
            return TransitionFromResponse("Cancel");
        }

        public override Screen RespondWith(string response)
        {
            switch (response)
            {
                case "Ok":
                    return Accept();
                case "Cancel":
                    return Dismiss();
                case "Something":
                    Something.Click();
                    return TransitionFromResponse("Something");
                default:
                    throw new UndefinedResponseException("Undefined response of: " + response);
            }
        }

        public override string Name
        {
            get { return "Long Message"; }
        }
    }
}
