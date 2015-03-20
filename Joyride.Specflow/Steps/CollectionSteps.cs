using System;
using System.Linq;
using Joyride.Interfaces;
using Joyride.Specflow.Support;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Joyride.Specflow.Steps
{
    [Binding]
    public class CollectionSteps
    {
        [Then(@"there (should|should not) exists ""([^""]*)"" within the collection ""([^""]*)""")]
        public void ThenIShouldSeeProperty(string shouldOrShouldNot, string property, string collectionName)
        {
            var hasProperty = false;
            Context.MobileApp.Do<IEntryEnumerable>(i =>
            {
                var entries = i.GetEntries(collectionName);
                foreach (var e in entries.Where(e => e.ContainsKey(property)))
                {
                    object value;
                    e.TryGetValue(property, out value);
                    hasProperty = true;
                    break;
                }
            });

            if (shouldOrShouldNot == "should")
                Assert.IsTrue(hasProperty);
            else
                Assert.IsFalse(hasProperty);
        }

        [Then(@"I (should|should not) see all the following properties within the collection ""([^""]*)""")]
        public void ThenIShouldSeeTheFollowingPropertiesWithinCollection(string shouldOrShouldNot, string collectionName, Table table)
        {
            var conditionsMeet = true;
            Context.MobileApp.Do<IEntryEnumerable>(i =>
            {
                var conditions = table.CreateSet<PropertyCondition>();
                if (conditions == null)
                    throw new ArgumentException("Unable to retrieve entry property table.");

                var entries = i.GetEntries(collectionName);
                foreach (var e in entries)
                {
                    if (!conditionsMeet)
                        break;

                    foreach (var c in conditions)
                    {
                        var foundProperty = e.ContainsKey(c.PropertyName);
                        if (foundProperty)
                        {
                            object value;
                            e.TryGetValue(c.PropertyName, out value);                            
                            continue;
                        }

                        if (c.Mandatory)
                            conditionsMeet = false;
                    }
                }
                
                if (shouldOrShouldNot == "should")
                    Assert.IsTrue(conditionsMeet);
                else
                    Assert.IsFalse(conditionsMeet);
            });
        }


    }
}
