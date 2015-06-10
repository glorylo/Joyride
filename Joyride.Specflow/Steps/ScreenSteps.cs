using System;
using System.Diagnostics;
using Joyride.Extensions;
using Joyride.Platforms;
using Joyride.Specflow.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace Joyride.Specflow.Steps
{
    [Binding]
    public class ScreenSteps
    {
        public static int TimeoutSecs = JoyrideConfiguration.TimeoutSecs;
        public static int NonExistenceTimeoutSecs = JoyrideConfiguration.NonexistenceTimeoutSecs;

        #region Given/Whens

        [Given(@"I rotate the screen to (landscape|portrait) orientation")]
        [When(@"I rotate the screen to (landscape|portrait) orientation")]
        public void GivenIRotateTheScreen(string orientation)
        {
            var mode = orientation == "landscape" ? ScreenOrientation.Landscape : ScreenOrientation.Portrait;
            Context.MobileApp.Do<Screen>(s => s.Rotate(mode));
        }


        [Given(@"I enter ""([^""]*)"" in the ""([^""]*)"" field")]
        [When(@"I enter ""([^""]*)"" in the ""([^""]*)"" field")]
        public void GivenICanEnterInTheField(string fieldValue, string fieldName)
        {
            Context.MobileApp.Do<Screen>(s => s.EnterText(fieldName, fieldValue));
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


        #endregion

        #region Thens

        [Then(@"I should see the label ""([^""]*)"" with text (equals|starts with|containing) ""([^""]*)""")]
        public void ThenIShouldSeeLabelWithText(string elementName, string compareType, string text)
        {
            string actualLabel = null;
            Context.MobileApp.Do<Screen>(s => actualLabel = s.GetElementText(elementName));
            Trace.Write("Actual Label is:  " + actualLabel);

            if (actualLabel == null)
                Assert.Fail("Unable to find value for element: " + elementName);

            Assert.That(actualLabel.CompareWith(text, compareType.ToCompareType()), Is.True,
               "Unexpected text compare with '" + actualLabel + "' is not " + compareType + " '" + text + "'");
        }

        [Then(@"the element ""([^""]*)"" (should|should not) be present")]
        public void ThenIShouldSeeElementIsPresent(string elementName, string shouldOrShouldNot)
        {
            var elementPresent = false;
            if (shouldOrShouldNot == "should")
            {
                Context.MobileApp.Do<Screen>(s => elementPresent = s.ElementIsPresent(elementName));
                Assert.IsTrue(elementPresent);
            }
            else
            {
                Context.MobileApp.Do<Screen>(s => elementPresent = s.ElementIsPresent(elementName, NonExistenceTimeoutSecs));
                Assert.IsFalse(elementPresent); 
            }
               
        }

        [Then(@"I (should|should not) see the (field|element|label|button) ""([^""]*)""")]
        public void ThenIShouldSeeElement(string shouldOrShouldNot, string elementType, string elementName)
        {
            var elementVisible = false;
            if (shouldOrShouldNot == "should")
            {
                Context.MobileApp.Do<Screen>(s => elementVisible = s.ElementIsVisible(elementName));
                Assert.IsTrue(elementVisible);
            }
            else
            {
                Context.MobileApp.Do<Screen>(s => elementVisible = s.ElementIsVisible(elementName, NonExistenceTimeoutSecs));
                Assert.IsFalse(elementVisible);
            }                
        }

        [Then(@"I should be on the ""([^""]*)"" screen")]
        public void ThenIShouldBeOnSomeScreen(string screen)
        {
            var onScreen = false;
            Context.MobileApp.Do<Screen>(s => onScreen = s.Name.Equals(screen) && s.IsOnScreen(TimeoutSecs));
            Assert.IsTrue(onScreen,
                "Incorrectly on screen: " + Context.MobileApp.Screen.Name);
        }
        
        [Then(@"I should see element ""([^""]*)"" with (.*) (equals|starts with|containing|matching) ""([^""]*)""")]
        public void ThenIShouldSeeElementValueCompareWithText(string elementName, string attribute, string compareType, string text)
        {
            string attributeValue = null;            
            Context.MobileApp.Do<Screen>(s => attributeValue = s.GetElementAttribute(elementName, attribute));

            if (attributeValue == null)
                Assert.Fail("Unable to find attribute " + attribute + " for element: " + elementName);

            Assert.That(attributeValue.CompareWith(text, compareType.ToCompareType()), Is.True,
                "Unexpected text compare for attribute " + attribute + " with '" + attributeValue + "' is not " + compareType + " '" + text + "'");
        }

/*      
        [Then(@"the screen should be on (landscape|portrait) orientation")]
        public void ThenScreenOrientation(string orientation)
        {
            var matchOrientation = false;
            var mode = (orientation == "landscape") ? ScreenOrientation.Landscape : ScreenOrientation.Portrait;
            Context.MobileApp.Do<Screen>(s => matchOrientation = s.IsOrientation(mode));

            Assert.IsTrue(matchOrientation, "Unexpected screen orientation not matching:  " + orientation);
        }
*/

        [Then(@"I fail the scenario with reason ""([^""]*)""")]
        public void ThenIFailScenario(string reason)
        {
            Assert.Fail(reason);
        }

        #endregion

    }
}
