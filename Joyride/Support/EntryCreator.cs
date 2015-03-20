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
        protected Component Component { get; private set; }        
        protected IDictionary<string, string> ElementMap { get;  private set; }
        protected string CollectionName { get;  private set; }
        protected ProcessValue ProcessValue { get; private set; }

        public EntryCreator(Component component, string collectionName, IDictionary<string, string> elementMap)
        {
            Component = component;
            ElementMap = elementMap;
            CollectionName = collectionName;
            ProcessValue = (key, text) => text;
        }

        public EntryCreator(Component component, string collectionName, IDictionary<string, string> elementMap, ProcessValue processText)
        {
            Component = component;
            ElementMap = elementMap;
            CollectionName = collectionName;
            ProcessValue = processText;
        }

        protected string GetElementText(int index, string xpath)
        {
            return Component.GetTextFromElementWithinCollection(CollectionName, index, xpath);
        }

        protected virtual IDictionary<string, object> AddProperty(IDictionary<string, object> dictonary, int index, string xpath, string key)
        {
            var text = GetElementText(index, xpath);
            if (text != null)
            {
                Trace.WriteLine("Found property, " + key + ", with value: " + text);
                var finalValue = ProcessValue(key, text);
                Trace.WriteLine("Processed property (" + key + ") with value: " + finalValue);
                dictonary.Add(key, finalValue);
            }
            else
                Trace.WriteLine("Unable to find property:  " + key);

            return dictonary;
        }

        public virtual IDictionary<string, object> GetNextEntry(int index)
        {
            var dictionary = new ExpandoObject() as IDictionary<string, object>;
            return ElementMap.Aggregate(dictionary, (current, e) => AddProperty(current, index, e.Value, e.Key));
        }
    }
}
