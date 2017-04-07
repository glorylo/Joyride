using TechTalk.SpecFlow;

namespace Joyride.Specflow.Steps
{
    [Binding]
    public class MobileAppSteps : TechTalk.SpecFlow.Steps
    {
        #region Given/Whens
        [Given(@"I launch the ""([^""]*)"" mobile application")]
        [When(@"I launch the ""([^""]*)"" mobile application")]
        public void GivenILaunchMobileApp(string mobileAppName)
        {
            Context.MobileApp.Launch();
        }

        [Given(@"I reset the ""([^""]*)"" mobile application")]
        [When(@"I reset the ""([^""]*)"" mobile application")]
        public void GivenIResetMobileApp(string mobileAppName)
        {
            Context.MobileApp.Reset();
        }


        [Given(@"I close the mobile application")]
        [When(@"I close the mobile application")]
        public void GivenICloseMobileApp()
        {
            Context.MobileApp.Close();
        }

        #endregion
    }
}
