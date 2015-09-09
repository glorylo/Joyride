using Joyride.Platforms;
using TechTalk.SpecFlow;

namespace Joyride.Specflow.Steps
{
    [Binding]
    public class UiControlSteps : TechTalk.SpecFlow.Steps
    {
        [Given(@"I enter ""([^""]*)"" in the ""([^""]*)"" field")]
        [When(@"I enter ""([^""]*)"" in the ""([^""]*)"" field")]
        public void GivenICanEnterInTheField(string fieldValue, string fieldName)
        {
            Context.MobileApp.Do<IUiControl>(i => i.EnterText(fieldName, fieldValue));
        }

        [Given(@"I clear the text for field ""([^""]*)""")]
        [When(@"I clear the text for field ""([^""]*)""")]
        public void GivenIClearTextInTheField(string fieldName)
        {
            Context.MobileApp.Do<IUiControl>(i => i.ClearText(fieldName));
        }
        
        [Given(@"I (uncheck|check) the ""([^""]*)"" checkbox")]
        [When(@"I (uncheck|check) the ""([^""]*)"" checkbox")]
        public void WhenICheckSomeCheckbox(string checkOrUnchecked, string checkboxName)
        {
            if (checkOrUnchecked == "check")
                Context.MobileApp.Do<IUiControl>(i => i.SetCheckbox(checkboxName));
            else
                Context.MobileApp.Do<IUiControl>(i => i.SetCheckbox(checkboxName, false));
        }
    }
}
