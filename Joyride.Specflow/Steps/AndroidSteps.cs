using System;
using Joyride.Extensions;
using Joyride.Platforms.Android;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Joyride.Specflow.Steps
{
    [Binding, Scope(Tag = "android")]
    public class AndroidSteps
    {
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
        

        [Then(@"I (should|should not) see a label (equals|starts with|containing|matching) text ""([^""]*)""")]
        public void ThenIShouldSeeLabel(string shouldOrShouldNot, string compare, string label)
        {
            var hasLabel = false;
            Context.MobileApp.Do<AndroidScreen>(s => hasLabel = s.HasLabel(label, compare.ToCompareType(), 5));

            if (shouldOrShouldNot == "should")
              Assert.IsTrue(hasLabel, "Expecting to have a label that " + compare + " text: " + label);
            else
              Assert.IsFalse(hasLabel, "Expecting not to have a label that " + compare + " text: " + label);
        }

        #endregion
    }
}
