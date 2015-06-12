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

            if (shouldOrShouldNot == "should")
                Assert.IsTrue(selectedText != null && selectedText.CompareWith(text, compare.ToCompareType()));
            else
                Assert.IsFalse(selectedText != null && selectedText.CompareWith(text, compare.ToCompareType()));
        }
        #endregion
    }
}
