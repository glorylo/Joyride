using Joyride.Extensions;
using Joyride.Platforms;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Joyride.Specflow.Steps
{
    // reserving the tag for deprecated steps
    [Binding, Scope(Tag = "deprecated")]
    public class DeprecatedSteps : TechTalk.SpecFlow.Steps
    {
        [Then(@"I (should|should not) see element ""([^""]*)"" with (.*) (equals|starts with|containing|matching) ""([^""]*)""")]
        public void ThenIShouldSeeElementValueCompareWithText(string shouldOrShouldNot, string elementName, string attribute, string compareType, string text)
        {
            string attributeValue = null;
            Context.MobileApp.Do<Screen>(s => attributeValue = s.GetElementAttribute(elementName, attribute));

            if (shouldOrShouldNot == "should")
                Assert.That(attributeValue.CompareWith(text, compareType.ToCompareType()), Is.True,
                    "Unexpected text compare for attribute " + attribute + " with '" + attributeValue + "' is not " + compareType + " '" + text + "'");
            else
                Assert.That(attributeValue.CompareWith(text, compareType.ToCompareType()), Is.False,
                    "Unexpected text compare for attribute " + attribute + " with '" + attributeValue + "' is " + compareType + " '" + text + "'");
        }
    }
}
