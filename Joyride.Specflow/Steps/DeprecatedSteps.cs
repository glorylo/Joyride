using System;
using System.Diagnostics;
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

        [Then(@"I (should|should not) see the label ""([^""]*)"" with text (equals|starts with|containing) ""([^""]*)""")]
        public void ThenIShouldSeeLabelWithText(string shouldOrShouldNot, string elementName, string compareType, string text)
        {
            string actualLabel = null;
            Context.MobileApp.Do<Screen>(s => actualLabel = s.GetText(elementName));
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




    }
}
