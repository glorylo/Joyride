
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

            var tryParse = PredicateParser<IDictionary<string, object>>.TryParse(propCondition.Condition);
            if (!tryParse)
            {
                Trace.WriteLine("Failed to Parse Condition on Property (" + propCondition.PropertyName + "):  " + propCondition.Condition);
                return false;
            }

            var propValue = entry[propCondition.PropertyName];
            Trace.WriteLine("Evaluating Condition (" + propCondition.Condition + ") on Property (" + propCondition.PropertyName + ") with value: " + propValue);
            var expression = PredicateParser<IDictionary<string, object>>.Parse(propCondition.Condition);
            var predicate = expression.Compile();
            var meetCondition = predicate(entry);
            if (!meetCondition)
                Trace.WriteLine("Failed to Meet Condition on Property (" + propCondition.PropertyName + "):  " + propCondition.Condition);

            return meetCondition;
        }
    }
}
