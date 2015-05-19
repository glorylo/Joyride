using System;
using Joyride.Extensions;
using Joyride.Platforms.Ios;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Joyride.Specflow.Steps
{
    [Binding, Scope(Tag = "ios")]
    public class IosSteps
    {

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
            Context.MobileApp.Do<IosScreen>(s => hasLabel = s.HasLabelInCollection(collectionName, label, compare.ToCompareType()));
            if (shouldOrShouldNot == "should")
                Assert.IsTrue(hasLabel, "Expecting to have a label that " + compare + " text: " + label + " within collection:  " + collectionName);
            else
                Assert.IsFalse(hasLabel, "Expecting not to have a label that " + compare + " text: " + label + " within collection:  " + collectionName);
        }

        [Then(@"I should see the ""(\d+)"" in collection ""([^""]*)"" with (name|value) (equals|starts with|containing|matching) ""([^""]*)""")]
        public void ThenIShouldSeeElementAttributeValueInCollectionCompareWithText(int index, string collectionName, string nameOrValue, string compareType, string text)
        {
            var attributeValue = Context.MobileApp.Screen.GetElementAttribute(collectionName, index, nameOrValue);

            if (attributeValue == null)
                Assert.Fail("Unable to find attribute '" + nameOrValue + "' for '" + index + "' item in " + collectionName);

            Assert.That(attributeValue.CompareWith(text, compareType.ToCompareType()), Is.True,
                "Unexpected text compare with '" + attributeValue + "' is not + " + compareType + " '" + text + "'");
        }

        [Then(@"I (should|should not) see the navigation bar title ""([^""]*)""")]
        public void ThenIShouldScreenWithTitle(string shouldOrShouldNot, string title)
        {
            string screenTitlebar = null;            
            Context.MobileApp.Do<IosScreen>(s => screenTitlebar = s.TitleFromNavigationBar());
            if (shouldOrShouldNot == "should")
              Assert.IsTrue(screenTitlebar == title, "Expected title with '" + title + "' but actual title is '" + screenTitlebar + "'");
            else
              Assert.IsFalse(screenTitlebar == title, "Expected title to be not equal to '" + title + "' but actual title is '" + screenTitlebar + "'");
        }

        [Obsolete("This step is deprecated and will be replaced with a more generic step")]
        [Then(@"I (should|should not) see label with text ""([^""]*)""")]
        public void ThenIShouldSeeLabelWithText(string shouldOrShouldNot, string labelText)
        {
            bool hasLabel = false;
            Context.MobileApp.Do<IosScreen>(s => hasLabel = s.HasLabelContainingText(labelText));
            if (shouldOrShouldNot == "should")                
                Assert.IsTrue(hasLabel);
            else
                Assert.IsFalse(hasLabel);
        }

        [Obsolete("This step is deprecated and will be replaced with a more generic step")]
        [Then(@"I (should|should not) see label in the ""(\d+)"" item in collection ""([^""]*)"" with text ""([^""]*)""")]
        public void ThenIShouldSeeLabelInCollectionWithText(string shouldOrShouldNot, int index, string collectionName, string labelText)
        {
            bool hasLabel = false;
            Context.MobileApp.Do<IosScreen>(s => hasLabel = s.HasLabelContainingText(collectionName, index, labelText));

            if (shouldOrShouldNot == "should")
                Assert.IsTrue(hasLabel);
            else
                Assert.IsFalse(hasLabel);
        }

        #endregion
    }
}
