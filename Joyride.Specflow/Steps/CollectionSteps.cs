using System;
using System.Data;
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
        #region Given/Whens

        #endregion

        #region #Thens
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

        [Then(@"I should see all the following properties within the collection ""([^""]*)""")]
        public void ThenIShouldSeeTheFollowingPropertiesWithinCollection(string collectionName, Table table)
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
                
                Assert.IsTrue(conditionsMeet);
            });
        }

        [Then(@"I should see all the following properties within the collection ""([^""]*)"" meeting the conditions")]
        public void ThenIShouldSeeTheFollowingPropertiesInCollectionMeetingCondition(string collectionName, Table table)
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

                            if (string.IsNullOrEmpty(c.Condition))
                                continue;

                            if (!StepsHelper.EvaluateCondition(c, e))
                                conditionsMeet = false;

                        }

                        if (c.Mandatory && !foundProperty)
                            conditionsMeet = false;
                    }
                }

                Assert.IsTrue(conditionsMeet);
            });
        }



        #endregion

    }
}
