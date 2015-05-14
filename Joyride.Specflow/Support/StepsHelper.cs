
using Humanizer;
using PredicateParser;
using System.Collections.Generic;
using System.Diagnostics;

namespace Joyride.Specflow.Support
{
    public static class StepsHelper
    {
        public static string ExtractValue(string value, string valueWithCurly)
        {
             return (value == "") ? valueWithCurly : value;
        }

        public static bool EvaluateCondition(PropertyCondition propCondition, IDictionary<string, object> entry)
        {

            var propertyName = propCondition.PropertyName.Dehumanize();
            var tryParse = PredicateParser<dynamic>.TryParse(propCondition.Condition);
            if (!tryParse)
            {
                Trace.WriteLine("Failed to Parse Condition on Property (" + propertyName + "):  " + propCondition.Condition);
                return false;
            }
            
            var propValue = entry[propertyName];
            Trace.WriteLine("Evaluating Condition (" + propCondition.Condition + ") on Property (" + propertyName + ") with value: " + propValue);
            var expression = PredicateParser<dynamic>.Parse(propCondition.Condition);
            var predicate = expression.Compile();
            var meetCondition = predicate(entry);
            if (!meetCondition)
                Trace.WriteLine("Failed to Meet Condition on Property (" + propertyName + "):  " + propCondition.Condition);

            return meetCondition;
        }
    }
}
