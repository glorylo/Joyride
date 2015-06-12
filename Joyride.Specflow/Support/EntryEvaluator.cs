
using Humanizer;
using PredicateParser;
using System.Collections.Generic;
using System.Diagnostics;

namespace Joyride.Specflow.Support
{
    public static class EntryEvaluator
    {

        public static bool MeetsCondition(PropertyCondition propCondition, IDictionary<string, object> entry)
        {

            var propertyName = propCondition.PropertyName.Dehumanize();
            var condition = propCondition.Condition;
            var tryParse = PredicateParser<dynamic>.TryParse(condition);
            if (!tryParse)
            {
                Trace.WriteLine("Failed to Parse Condition on Property (" + propertyName + "):  " + condition);
                return false;
            }
            
            var propValue = entry[propertyName];
            Trace.WriteLine("Evaluating Condition (" + condition + ") on Property (" + propertyName + ") with value: " + propValue);
            var expression = PredicateParser<dynamic>.Parse(condition);
            var predicate = expression.Compile();
            var meetsCondition = predicate(entry);
            if (!meetsCondition)
                Trace.WriteLine("Failed to Meet Condition on Property (" + propertyName + ") with value (" + propValue + "):  " + condition);

            return meetsCondition;
        }
    }
}
