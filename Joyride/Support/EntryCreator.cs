using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using Joyride.Platforms;

namespace Joyride.Support
{
    public delegate object ProcessValue(string key, string value);

    /// <summary>
    /// Utility class to create entries off from a collection.  Provide the component, the collection and a element map (property, relative xpath pairs)
    /// Optionally can take a delegate to process text values such as extracting something from a known format.  
    /// </summary>
    public class EntryCreator
    {
        private readonly Component _component;        
        private readonly IDictionary<string, string> _elementMap;
        private readonly string _collectionName;
        private readonly ProcessValue _processValue;

        public EntryCreator(Component component, string collectionName, IDictionary<string, string> elementMap)
        {
            _component = component;
            _elementMap = elementMap;
            _collectionName = collectionName;
            _processValue = (key, text) => text;
        }

        public EntryCreator(Component component, string collectionName, IDictionary<string, string> elementMap, ProcessValue processText)
        {
            _component = component;
            _elementMap = elementMap;
            _collectionName = collectionName;
            _processValue = processText;
        }

        private IDictionary<string, object> AddProperty(IDictionary<string, object> dictonary, int index, string xpath, string key)
        {
            var text = _component.GetTextFromElementWithinCollection(_collectionName, index, xpath);
            if (text != null)
            {
                Trace.WriteLine("Found property, " + key + ", with value: " + text);
                var finalValue = _processValue(key, text);
                Trace.WriteLine("Processed property (" + key + ") with value: " + finalValue);
                dictonary.Add(key, finalValue);
            }
            else
                Trace.WriteLine("Unable to find property:  " + key);

            return dictonary;
        }

        public IDictionary<string, object> GetNextEntry(int index)
        {
            var dictionary = new ExpandoObject() as IDictionary<string, object>;
            return _elementMap.Aggregate(dictionary, (current, e) => AddProperty(current, index, e.Value, e.Key));
        }
    }
}
