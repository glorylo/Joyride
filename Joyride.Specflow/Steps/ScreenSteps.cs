using System;
using System.Diagnostics;
using Joyride.Extensions;
using Joyride.Platforms;
using Joyride.Specflow.Support;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Joyride.Specflow.Steps
{
    [Binding]
    public class ScreenSteps
    {

        #region Given/Whens

        [Given(@"I enter ""([^""]*)"" in the ""([^""]*)"" field")]
        [When(@"I enter ""([^""]*)"" in the ""([^""]*)"" field")]
        public void GivenICanEnterInTheField(string fieldValue, string fieldName)
        {
            Context.MobileApp.Do<Screen>(s => s.EnterText(fieldName, fieldValue));
        }

        [Given(@"I tap the ""([^""]*)"" (button|field|label|element)")]
        [When(@"I tap the ""([^""]*)"" (button|field|label|element)")]
        public void WhenITapTheButton(string elementName, string elementType)
        {
            Context.MobileApp.Do<Screen>(s => s.Tap(elementName));
        }

        [Given(@"I double tap the ""([^""]*)"" (button|field|label|element)")]
        [When(@"I double tap the ""([^""]*)"" (button|field|label|element)")]
        public void WhenIDoubleTap(string elementName, string elementType)
        {
            Context.MobileApp.Do<Screen>(s => s.DoubleTap(elementName));
        }
        
        [Given(@"I tap the ""([^""]*)"" button and hold for ""(\d+)"" seconds")]
        [When(@"I tap the ""([^""]*)"" button and hold for ""(\d+)"" seconds")]        
        public void WhenITapAndHoldTheButton(string elementName, int seconds)
        {
            Context.MobileApp.Do<Screen>(s => s.TapAndHold(elementName, seconds));
        }
                
        [Given(@"I select (""(.*?)""|{(.*?)}) for ""([^""]*)""")]
        [When(@"I select (""(.*?)""|{(.*?)}) for ""([^""]*)""")]
        public void WhenISelectOptionForDropdown(string wholeValue, string value, string valueWithCurly, string dropDownList)
        {
            var extractedValue = StepsHelper.ExtractValue(value, valueWithCurly);
            Context.MobileApp.Do<Screen>(s => s.SelectOption(dropDownList, extractedValue));
        }
       
        [Given(@"I (uncheck|check) the ""([^""]*)"" checkbox")]
        [When(@"I (uncheck|check) the ""([^""]*)"" checkbox")]
        public void WhenICheckSomeCheckbox(string checkOrUnchecked, string checkboxName)
        {
            if (checkOrUnchecked == "check")
                Context.MobileApp.Do<Screen>(s => s.SetCheckbox(checkboxName));
            else
                Context.MobileApp.Do<Screen>(s => s.SetCheckbox(checkboxName, false));
        }

        [Given(@"I scroll the screen (left|right|up|down)")]
        [When(@"I scroll the screen (left|right|up|down)")]
        public void GivenIScrollScreenInDirection(string direction)
        {
            var directionToScroll = (Direction) Enum.Parse(typeof(Direction), direction, true);
            Context.MobileApp.Do<Screen>(s => s.Scroll(directionToScroll));
        }

        [Given(@"I do a (slight|moderate) scroll (left|right|up|down)")]
        [When(@"I do a (slight|moderate) scroll (left|right|up|down)")]
        public void GivenIScrollScreenInDirectionWithScale(string slightOrModerate, string direction)
        {
            var directionToScroll = (Direction)Enum.Parse(typeof(Direction), direction, true);
            if (slightOrModerate == "slight")
              Context.MobileApp.Do<Screen>(s => s.Scroll(directionToScroll, 0.5));
            else
              Context.MobileApp.Do<Screen>(s => s.Scroll(directionToScroll, 0.75));
        }

        [Given(@"I scroll the ""([^""]*)"" (left|right|up|down)")]
        [When(@"I scroll the ""([^""]*)"" (left|right|up|down)")]
        public void GivenIScrollElementinDirection(string elementName, string direction)
        {
            var directionToScroll = (Direction)Enum.Parse(typeof(Direction), direction, true);
            Context.MobileApp.Do<Screen>(s => s.Scroll(elementName, directionToScroll));
        }

        [Given(@"I swipe the screen (left|right|up|down)")]
        [When(@"I swipe the screen (left|right|up|down)")]
        public void GivenISwipeScreenInDirection(string direction)
        {
            var directionToSwipe = (Direction) Enum.Parse(typeof(Direction), direction, true);
            Context.MobileApp.Do<Screen>(s => s.Swipe(directionToSwipe));
        }

        [Given(@"I scroll the screen (left|right|up|down) until I see element ""([^""]*)""")]
        [When(@"I scroll the screen (left|right|up|down) until I see element ""([^""]*)""")]
        public void GivenIScrollUntil(string direction, string elementName)
        {
            var directionToScroll = (Direction)Enum.Parse(typeof(Direction), direction, true);
            Context.MobileApp.Do<Screen>(s => s.ScrollUntil(elementName, directionToScroll));
        }

        [Given(@"I pinch the screen to zoom (out|in)")]
        [When(@"I pinch the screen to zoom (out|in)")]
        public void GivenIPinchTheScreen(string zoom)
        {
            var direction = (Direction) Enum.Parse(typeof (Direction), zoom, true);
            Context.MobileApp.Do<Screen>(s => s.PinchToZoom(direction));
        }

        #endregion

        #region Thens

        [Then(@"I should see the label ""([^""]*)"" with text (equals|starts with|containing) ""([^""]*)""")]
        public void ThenIShouldSeeLabelWithText(string elementName, string compareType, string text)
        {
            var actualLabel = Context.MobileApp.Screen.GetElementText(elementName);
            Trace.Write("Actual Label is:  " + actualLabel);

            if (actualLabel == null)
                Assert.Fail("Unable to find value for element: " + elementName);

            Assert.That(actualLabel.CompareWith(text, compareType.ToCompareType()), Is.True,
               "Unexpected text compare with '" + actualLabel + "' is not " + compareType + " '" + text + "'");
        }

        [Then(@"the element ""([^""]*)"" (should|should not) be present")]
        public void ThenIShouldSeeElementIsPresent(string elementName, string shouldOrShouldNot)
        {
            if (shouldOrShouldNot == "should")
               Assert.IsTrue(Context.MobileApp.Screen.ElementIsPresent(elementName));
            else
               Assert.IsFalse(Context.MobileApp.Screen.ElementIsPresent(elementName, 10));
        }

        [Then(@"I (should|should not) see the (field|element|label|button) ""([^""]*)""")]
        public void ThenIShouldSeeElement(string shouldOrShouldNot, string elementType, string elementName)
        {
            if (shouldOrShouldNot == "should")
                Assert.IsTrue(Context.MobileApp.Screen.ElementIsVisible(elementName));
            else
                Assert.IsFalse(Context.MobileApp.Screen.ElementIsVisible(elementName, 10));
        }

        [Then(@"I should be on the ""([^""]*)"" screen")]
        public void ThenIShouldBeOnSomeScreen(string screen)
        {
            Assert.IsTrue(Context.MobileApp.Screen.Name.Equals(screen) && Context.MobileApp.Screen.IsOnScreen(),
                "Incorrectly on screen: " + Context.MobileApp.Screen.Name);
        }
        
        [Then(@"I should see element ""([^""]*)"" with (.*) (equals|starts with|containing|matching) ""([^""]*)""")]
        public void ThenIShouldSeeElementValueCompareWithText(string elementName, string attribute, string compareType, string text)
        {
            var attributeValue = Context.MobileApp.Screen.GetElementAttribute(elementName, attribute);

            if (attributeValue == null)
                Assert.Fail("Unable to find attribute " + attribute + " for element: " + elementName);

            Assert.That(attributeValue.CompareWith(text, compareType.ToCompareType()), Is.True,
                "Unexpected text compare for attribute " + attribute + " with '" + attributeValue + "' is not " + compareType + " '" + text + "'");
        }

        [Then(@"I (should|should not) see selected drop down ""([^""]*)"" (equals|containing) ""([^""]*)""")]
        public void ThenSelectedDropdownValue(string shouldOrShouldNot, string elementName, string compare, string text)
        {
            var selectedText = Context.MobileApp.Screen.GetSelectedOption(elementName);
            if (shouldOrShouldNot == "should")
              Assert.IsTrue(selectedText != null && selectedText.CompareWith(text, compare.ToCompareType()));
            else
              Assert.IsFalse(selectedText != null && selectedText.CompareWith(text, compare.ToCompareType()));
        }

        [Then(@"I fail the scenario with reason ""([^""]*)""")]
        public void ThenIFailScenario(string reason)
        {
            Assert.Fail(reason);
        }

        #endregion

    }
}
