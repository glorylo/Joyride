using Joyride.Extensions;
using Joyride.Platforms.Ios;
using Joyride.Specflow.Configuration;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Joyride.Specflow.Steps
{
    [Binding, Scope(Tag = "ios")]
    public class IosSteps : TechTalk.SpecFlow.Steps
    {
        public static int TimeoutSecs = JoyrideConfiguration.TimeoutSecs;
        public static int InspectTimeoutSecs = JoyrideConfiguration.QuickInspectTimeoutSecs;

        #region Given/Whens

        [Given(@"I hide the keyboard")]
        [When(@"I hide the keyboard")]
        public void GivenIHideKeyboard()
        {
            Context.MobileApp.Do<IosScreen>(s => s.HideKeyboard());
        }

        [Given(@"I drag the ""([^""]*)"" slider with value ""(\d+)%""")]
        [When(@"I drag the ""([^""]*)"" slider with value ""(\d+)%""")]
        public void WhenIDragSlider(string elementName, int value)
        {
            Context.MobileApp.Do<IosScreen>(s => s.DragSlider(elementName, value));            
        }
        #endregion


        #region Thens

        //TODO: needs to be tested
        // only xpath is supported so the single quote character is not allowed.
        [Then(@"I (should|should not) see the label (equals|starts with|containing) text ""([^""']*)"" within the ""([^""]*)"" collection")]
        public void ThenIShouldSeeLabelWithinCollection(string shouldOrShouldNot, string compare, string label, string collectionName)
        {
            var hasLabel = false;
            Context.MobileApp.Do<IosScreen>(s => hasLabel = s.HasLabelInCollection(collectionName, label, compare.ToCompareType(), InspectTimeoutSecs));
            if (shouldOrShouldNot == "should")
                Assert.IsTrue(hasLabel, "Expecting to have a label that " + compare + " text: " + label + " within collection:  " + collectionName);
            else
                Assert.IsFalse(hasLabel, "Expecting not to have a label that " + compare + " text: " + label + " within collection:  " + collectionName);
        }

        [Then(@"I (should|should not) see the navigation bar title ""([^""]*)""")]
        public void ThenIShouldScreenWithTitle(string shouldOrShouldNot, string title)
        {
            string screenTitlebar = null;
            Context.MobileApp.Do<IosScreen>(s => screenTitlebar = s.TitleFromNavigationBar(TimeoutSecs));
            if (shouldOrShouldNot == "should")
              Assert.IsTrue(screenTitlebar == title, "Expected title with '" + title + "' but actual title is '" + screenTitlebar + "'");
            else
              Assert.IsFalse(screenTitlebar == title, "Expected title to be not equal to '" + title + "' but actual title is '" + screenTitlebar + "'");
        }

        [Then(@"I (should|should not) see the current page of ""(\d+)"" for element ""([^""]*)""")]
        public void ThenIShouldSeePageIndicator(string shouldOrShouldNot, int page, string elementName)
        {

            var actualPageIndicator = 0;
            Context.MobileApp.Do<IosScreen>(i => actualPageIndicator = i.CurrentPageOnIndictator(elementName));

            if (shouldOrShouldNot == "should")
                Assert.IsTrue(actualPageIndicator == page, "Unexpected page indicator of "  + actualPageIndicator + ".  Expecting:  " + page);
            else
                Assert.IsFalse(actualPageIndicator == page, "Unexpected page indicator of " + actualPageIndicator + " equals " + page);

        }
        #endregion
    }
}
