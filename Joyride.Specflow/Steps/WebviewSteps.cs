using Joyride.Extensions;
using Joyride.Platforms;
using Joyride.Specflow.Support;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Joyride.Specflow.Steps
{
    [Binding]
    public class WebviewSteps : TechTalk.SpecFlow.Steps
    {
        #region Given/Whens

        [Given(@"I select (""(.*?)""|{(.*?)}) for ""([^""]*)""")]
        [When(@"I select (""(.*?)""|{(.*?)}) for ""([^""]*)""")]
        public void WhenISelectOptionForDropdown(string wholeValue, string value, string valueWithCurly, string dropDownList)
        {
            var extractedValue = StepsHelper.ExtractValue(value, valueWithCurly);
            Context.MobileApp.Do<Screen>(s => s.SelectOption(dropDownList, extractedValue));
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
