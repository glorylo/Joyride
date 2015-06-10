using Joyride.Extensions;
using Joyride.Interfaces;
using Joyride.Platforms;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Joyride.Specflow.Steps
{
    [Binding]
    public class ModalDialogSteps
    {
        #region Given/Whens
        [Given(@"I (accept|dismiss) the ""([^""]*)"" modal dialog")]
        [When(@"I (accept|dismiss) the ""([^""]*)"" modal dialog")]
        public void GivenIAcceptOrDismissSpecificModalDialog(string acceptOrDismiss, string modalDialogName)
        {
            var accept = (acceptOrDismiss == "accept");
            Context.MobileApp.Do<IDetectModalDialog>(i => i.AcceptModalDialog(accept, modalDialogName, true));
        }

        [Given(@"I (accept|dismiss) any modal dialog")]
        [When(@"I (accept|dismiss) any modal dialog")]
        public void GivenIAcceptOrDismissModalDialog(string acceptOrDismiss)
        {
            Context.MobileApp.Do<IDetectModalDialog>(i => i.AcceptModalDialog(acceptOrDismiss == "accept"));            
        }

        [Given(@"I respond to the ""([^""]*)"" modal dialog with ""([^""]*)""")]
        [When(@"I respond to the ""([^""]*)"" modal dialog with ""([^""]*)""")]
        public void GivenIRespondWithModalDialog(string modalDialogName, string response)
        {
            Context.MobileApp.Do<IDetectModalDialog>(i => i.RespondModalDialog(response, modalDialogName, true));
        }

        #endregion

        #region Thens

        [Then(@"I (should|should not) see any modal dialog")]
        public void ThenIShouldSeeModalDialog(string shouldOrShouldNot)
        {
            IModalDialog dialog = null;
            Context.MobileApp.Do<IDetectModalDialog>(i => dialog = i.DetectModalDialog());

            if (shouldOrShouldNot == "should")
              Assert.IsTrue(dialog != null);
            else
              Assert.IsFalse(dialog != null);
        }

        [Then(@"I (should|should not) see the ""([^""]*)"" modal dialog")]
        public void ThenIShouldSeeTheSpecificModalDialog(string shouldOrShouldNot, string modalDialogName)
        {
            IModalDialog dialog = null;
            Context.MobileApp.Do<IDetectModalDialog>(i => dialog = i.DetectModalDialog(modalDialogName));

            if (shouldOrShouldNot == "should")
                Assert.IsTrue(dialog != null);
            else
                Assert.IsFalse(dialog != null);

        }

        [Then(@"I should see the ""([^""]*)"" modal dialog (equals|ends with|starts with|containing|matching) (body|title) text ""([^""]*)""")]
        public void ThenIShouldSeeTheSpecificModalDialogWithText(string modalDialogName, string compareType, string bodyOrTitle, string text)
        {
            IModalDialog dialog = null;
            Context.MobileApp.Do<IDetectModalDialog>(i => dialog = i.DetectModalDialog(modalDialogName));

            if (bodyOrTitle == "body")
                Assert.IsTrue(dialog != null && dialog.Body.CompareWith(text, compareType.ToCompareType()));
            else
                Assert.IsTrue(dialog != null && dialog.Title.CompareWith(text, compareType.ToCompareType()));
        }

        #endregion
    }
}
