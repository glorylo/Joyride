using System;
using System.Linq;
using Joyride.Extensions;
using Joyride.Platforms;
using Joyride.Specflow.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace Joyride.Specflow.Steps
{
    [Binding]
    public class ScreenSteps : TechTalk.SpecFlow.Steps
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

        #endregion

        #region Thens


        [Then(@"I (should|should not) see the text of (?:button|field|label|element|link) ""([^""]*)"" (equals|starts with|containing|matching) (?:""(.*?)""|{(.*?)})")]
        public void ThenIShouldSeeElementValueCompareWithText(string shouldOrShouldNot, string elementName, string compareType, string quotedText, string curlyText)
        {
            var text = (quotedText != String.Empty) ? quotedText : curlyText;
            string actualText = null;
            Context.MobileApp.Do<Screen>(s => actualText = s.GetElementText(elementName));

            if (shouldOrShouldNot == "should")
                Assert.That(actualText != null && actualText.CompareWith(text, compareType.ToCompareType()), Is.True,
                    "Unexpected text compare for with actual text '" + actualText + "' is not " + compareType + " '" + text + "'");
            else
                Assert.That(actualText != null && actualText.CompareWith(text, compareType.ToCompareType()), Is.False,
                    "Unexpected text compare for with actual text " + actualText + "' is " + compareType + " '" + text + "'");
        }

        [Then(@"I (should|should not) see the text of (?:field|element) ""([^""]*)"" cleared")]
        public void ThenIShouldSeeElementCleared(string shouldOrShouldNot, string elementName)
        {
            string actualText = null;
            Context.MobileApp.Do<Screen>(s => actualText = s.GetElementText(elementName));

            if (shouldOrShouldNot == "should")
                Assert.That(String.IsNullOrEmpty(actualText), Is.True,
                    "Unexpected text should be cleared:  " + actualText);
            else
                Assert.That(String.IsNullOrEmpty(actualText), Is.False,
                    "Unexpected text of '" + elementName + "' should not be cleared");
        }

        [Then(@"the (?:button|field|label|element|link|checkbox|switch) ""([^""]*)"" (should|should not) be present")]
        public void ThenIShouldSeeElementIsPresent(string elementName, string shouldOrShouldNot)
        {
            var elementPresent = false;
            if (shouldOrShouldNot == "should")
            {
                Context.MobileApp.Do<Screen>(s => elementPresent = s.ElementIsPresent(elementName, TimeoutSecs));
                Assert.IsTrue(elementPresent);
            }
            else
            {
                Context.MobileApp.Do<Screen>(s => elementPresent = s.ElementIsPresent(elementName, NonExistenceTimeoutSecs));
                Assert.IsFalse(elementPresent); 
            }               
        }

        [Then(@"I (should|should not) see the (?:button|field|label|element|link|checkbox|switch) ""([^""]*)""")]
        public void ThenIShouldSeeElement(string shouldOrShouldNot, string elementName)
        {
            var elementVisible = false;
            if (shouldOrShouldNot == "should")
            {
                Context.MobileApp.Do<Screen>(s => elementVisible = s.ElementIsVisible(elementName, TimeoutSecs));
                Assert.IsTrue(elementVisible);
            }
            else
            {
                Context.MobileApp.Do<Screen>(s => elementVisible = s.ElementIsVisible(elementName, NonExistenceTimeoutSecs));
                Assert.IsFalse(elementVisible);
            }                
        }

        [Then(@"I (should|should not) be on the ""([^""]*)"" screen")]
        public void ThenIShouldBeOnSomeScreen(string shouldOrShouldNot, string screen)
        {
            var onScreen = false;
            Context.MobileApp.Do<Screen>(s => onScreen = s.Name.Equals(screen) && s.IsOnScreen(TimeoutSecs));

            if (shouldOrShouldNot == "should")
               Assert.IsTrue(onScreen, "Incorrectly on screen: " + Context.MobileApp.Screen.Name);
            else
               Assert.IsFalse(onScreen, "Unexpected to be on a screen other than: " + Context.MobileApp.Screen.Name);
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

        [Then(@"I (should|should not) see the following elements")]
        public void ThenIShouldSeeAllTheFollowingElements(string shouldOrShouldNot, Table table)
        {
            var header = table.Header.First();
            var elements = table.Rows.Select(r => r[header]).ToList();
            var timeoutSecs = (shouldOrShouldNot == "should")
                ? TimeoutSecs
                : NonExistenceTimeoutSecs;

            foreach (var e in elements)
            {
                var foundElement = false;
                var elementName = e;
                Context.MobileApp.Do<Screen>(s => foundElement = s.ElementIsVisible(elementName, timeoutSecs));

                if (shouldOrShouldNot == "should")
                    Assert.IsTrue(foundElement, "Unexpected element not visible: " + elementName);
                else
                    Assert.IsFalse(foundElement, "Unexpected element is visible: " + elementName);
            }
        }

        [Then(@"the following elements (should|should not) be present")]
        public void ThenIShouldSeeAllTheFollowingElementsPresent(string shouldOrShouldNot, Table table)
        {
            var header = table.Header.First();
            var elements = table.Rows.Select(r => r[header]).ToList();
            var timeoutSecs = (shouldOrShouldNot == "should")
                ? TimeoutSecs
                : NonExistenceTimeoutSecs;

            foreach (var e in elements)
            {
                var foundElement = false;
                var elementName = e;
                Context.MobileApp.Do<Screen>(s => foundElement = s.ElementIsPresent(elementName, timeoutSecs));

                if (shouldOrShouldNot == "should")
                    Assert.IsTrue(foundElement, "Unexpected element not present: " + elementName);
                else
                    Assert.IsFalse(foundElement, "Unexpected element is present: " + elementName);
            }
        }

        #endregion

    }
}
