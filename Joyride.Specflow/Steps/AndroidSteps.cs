using Joyride.Extensions;
using Joyride.Platforms.Android;
using Joyride.Specflow.Configuration;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Joyride.Specflow.Steps
{
    [Binding, Scope(Tag = "android")]
    public class AndroidSteps : TechTalk.SpecFlow.Steps
    {
        public static int TimeoutSecs = JoyrideConfiguration.TimeoutSecs;
        public static int QuickInspectTimeoutSecs = JoyrideConfiguration.QuickInspectTimeoutSecs;

        #region Given/Whens

        [Given(@"I go back")]
        [When(@"I go back")]
        public void GivenITapOnTheAndroidDevicesBackButton()
        {
            Context.MobileApp.Do<AndroidScreen>(s => s.GoBack());
        }

        [Given(@"I hide the keyboard")]
        [When(@"I hide the keyboard")]
        public void GivenIHideKeyboard()
        {
            Context.MobileApp.Do<AndroidScreen>(s => s.HideKeyboard());
        }

        #endregion
        #region Thens


        #region Collections 

        // only xpath is supported so the single quote character is not allowed.
        [Then(@"I (should|should not) see the label (equals|starts with|containing) text ""([^""']*)"" within the ""([^""]*)"" collection")]
        public void ThenIShouldSeeLabelWithinCollection(string shouldOrShouldNot, string compare, string label, string collectionName)
        {
            var hasLabel = false;
            Context.MobileApp.Do<AndroidScreen>(s => hasLabel = s.HasLabelInCollection(collectionName, label, compare.ToCompareType(), QuickInspectTimeoutSecs));
            if (shouldOrShouldNot == "should")
                Assert.IsTrue(hasLabel, "Expecting to have a label that " + compare + " text: " + label + " within collection:  " + collectionName);
            else
                Assert.IsFalse(hasLabel, "Expecting not to have a label that " + compare + " text: " + label + " within collection:  " + collectionName);
        }

        #endregion

        [Then(@"I (should|should not) see a label (equals|starts with|containing|matching) text ""([^""]*)""")]
        public void ThenIShouldSeeLabel(string shouldOrShouldNot, string compare, string label)
        {
            var hasLabel = false;
            Context.MobileApp.Do<AndroidScreen>(s => hasLabel = s.HasLabel(label, compare.ToCompareType(), TimeoutSecs));

            if (shouldOrShouldNot == "should")
              Assert.IsTrue(hasLabel, "Expecting to have a label that " + compare + " text: " + label);
            else
              Assert.IsFalse(hasLabel, "Expecting not to have a label that " + compare + " text: " + label);
        }

        # region UI Controls 

        [Then(@"I (should|should not) see the ""([^""]*)"" (?:element|checkbox|switch) (checked|unchecked|enabled|disabled)")]
        public void ThenIShouldSeeElementEnabledOrDisabled(string shouldOrShouldNot, string elementName, string checkedOrUnchecked)
        {
            var value = (checkedOrUnchecked == "checked" || checkedOrUnchecked == "enabled");
            
            var actualValue = false;
            Context.MobileApp.Do<AndroidScreen>( s => actualValue = s.IsChecked(elementName));

            if (shouldOrShouldNot == "should")
                Assert.IsTrue(actualValue == value);
            else
                Assert.IsFalse(actualValue == value);
        }

        #endregion

        #endregion
    }
}
