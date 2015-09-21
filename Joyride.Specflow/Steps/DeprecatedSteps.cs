using System;
using System.Diagnostics;
using Joyride.Extensions;
using Joyride.Platforms;
using Joyride.Platforms.Ios;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Joyride.Specflow.Steps
{
    // reserving the tag for deprecated steps
    [Binding, Scope(Tag = "deprecated")]
    public class DeprecatedSteps : TechTalk.SpecFlow.Steps
    {

        [Then(@"I (should|should not) see the label ""([^""]*)"" with text (equals|starts with|containing) ""([^""]*)""")]
        public void ThenIShouldSeeLabelWithText(string shouldOrShouldNot, string elementName, string compareType, string text)
        {
            string actualLabel = null;
            Context.MobileApp.Do<Screen>(s => actualLabel = s.GetElementText(elementName));
            Trace.Write("Actual Label is:  " + actualLabel);

            if (actualLabel == null)
                Assert.Fail("Unable to find value for element: " + elementName);

            if (shouldOrShouldNot == "should")
                Assert.That(actualLabel.CompareWith(text, compareType.ToCompareType()), Is.True,
                   "Unexpected text compare with '" + actualLabel + "' is not " + compareType + " '" + text + "'");
            else
                Assert.That(actualLabel.CompareWith(text, compareType.ToCompareType()), Is.False,
                 "Unexpected text compare with '" + actualLabel + "' is " + compareType + " '" + text + "'");
        }

        [Then(@"I should see the ""(\d+)"" in collection ""([^""]*)"" with (name|value) (equals|starts with|containing|matching) ""([^""]*)""")]
        public void ThenIShouldSeeElementAttributeValueInCollectionCompareWithText(int index, string collectionName, string nameOrValue, string compareType, string text)
        {
            string attributeValue = null;
            Context.MobileApp.Do<IosScreen>(s => attributeValue = s.GetElementAttribute(collectionName, index, nameOrValue));

            if (attributeValue == null)
                Assert.Fail("Unable to find attribute '" + nameOrValue + "' for '" + index + "' item in " + collectionName);

            Assert.That(attributeValue.CompareWith(text, compareType.ToCompareType()), Is.True,
                "Unexpected text compare with '" + attributeValue + "' is not + " + compareType + " '" + text + "'");
        }


    }
}
