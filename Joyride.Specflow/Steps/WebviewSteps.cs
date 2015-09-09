using System;
using Joyride.Extensions;
using Joyride.Platforms;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Joyride.Specflow.Steps
{
    [Binding]
    public class WebviewSteps : TechTalk.SpecFlow.Steps
    {
        #region Given/Whens

        [Given(@"I select (?:""(.*?)""|{(.*?)}) for ""([^""]*)""")]
        [When(@"I select (?:""(.*?)""|{(.*?)}) for ""([^""]*)""")]
        public void WhenISelectOptionForDropdown(string quotedValue, string curlyValue, string dropDownList)
        {
            var value = (quotedValue != String.Empty) ? quotedValue : curlyValue;
            Context.MobileApp.Do<Screen>(s => s.SelectOption(dropDownList, value));
        }

        #endregion


        #region Thens

        [Then(@"I (should|should not) see selected drop down ""([^""]*)"" (equals|containing) ""([^""]*)""")]
        public void ThenSelectedDropdownValue(string shouldOrShouldNot, string elementName, string compare, string text)
        {
            string selectedText = null;
            Context.MobileApp.Do<IWebview>(i => selectedText = i.GetSelectedOption(elementName));
            var compareText = selectedText != null && selectedText.CompareWith(text, compare.ToCompareType());
            if (shouldOrShouldNot == "should")
                Assert.IsTrue(compareText, "Unexpected actual text '" + selectedText + "' does not equal '" + text + "'");
            else
                Assert.IsFalse(compareText, "Unexpected actual text '" + selectedText + "' equals '" + text + "'");
        }
        #endregion
    }
}
