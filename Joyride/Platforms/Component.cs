using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Humanizer;
using Joyride.Extensions;
using Joyride.Support;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.PageObjects;

namespace Joyride.Platforms
{
    abstract public class Component
    {
        public const int DefaultWaitSeconds = RemoteMobileDriver.DefaultWaitSeconds;
        abstract public string Name { get; }
        static protected AppiumDriver Driver { get { return RemoteMobileDriver.GetInstance(); } }

        internal protected IWebElement FindElement(string elementName)
        {
            var element = (IWebElement) Util.GetMemberValue(this, elementName.Dehumanize(), BindingFlags.NonPublic);

            if (element != null && element.IsPresent()) 
                return element;

            Trace.WriteLine("Unable to access element:  " + elementName);
            return null;
        }

        internal protected IWebElement FindCachedElement(string elementName)
        {
            var element = FindElement(elementName);

            if (element == null)
              return null;

            return Util.GetMemberValue(element, "WrappedElement") as IWebElement;
        }

        internal protected IList<IWebElement> FindElements(string collectionName)
        {
            var elements =
                (IList<IWebElement>) Util.GetMemberValue(this, collectionName.Dehumanize(), BindingFlags.NonPublic);

            if (elements != null && elements.GetEnumerator().MoveNext()) 
                return elements;

            Trace.WriteLine("Unable to access the collection: " + collectionName);
            return null;
        }

        internal protected IList<IWebElement> FindElements(string collectionName, int timeoutSecs)
        {
            return Driver.FindElementsWithMethod(timeoutSecs, new Func<string, IList<IWebElement>>(FindElements), collectionName);
        }

        internal protected IWebElement FindElement(string elementName, int timeoutSecs)
        {
            return Driver.FindElementWithMethod(timeoutSecs, new Func<string, IWebElement>(FindElement), elementName);
        }

        public int SizeOf(string collectionName, int timeoutSecs=DefaultWaitSeconds)
        {
            var collection = FindElements(collectionName, timeoutSecs);

            if (collection == null)
                return 0;   
            return collection.Count;            
        }

        public bool IsEmpty(string collectionName, int timeoutSecs = DefaultWaitSeconds)
        {
            return (SizeOf(collectionName, timeoutSecs) == 0);
        }

        public bool IsSelected(string elementName)
        {
            var element = FindElement(elementName);
            if (element == null)
                throw new NoSuchElementException("Unable to find element: " + elementName);

            return element.Selected;
        }

        public string GetElementAttribute(string elementName, string attributeName)
        {
            var element = FindElement(elementName);

            if (element == null)
                 throw new NoSuchElementException("Unable to find element:  " + elementName);                

            var attribValue = element.GetAttribute(attributeName);
            return (attribValue == null) ? null : attribValue.Trim();
        }

        internal protected IWebElement GetElementInCollection(IList<IWebElement> collection, int index, bool last = false)
        {
            var zeroBasedIndex = index - 1;

            if (!last)
            {
                if ((zeroBasedIndex < 0) || (zeroBasedIndex >= collection.Count))
                    throw new NoSuchElementException("Unable to get collection on index " + zeroBasedIndex);
                return collection.ElementAt(zeroBasedIndex);
            }
            return collection.Last();            
        }

        internal protected IWebElement GetElementInCollection(string collectionName, int index, bool last = false)
        {
            var collection = FindElements(collectionName);

            if (collection == null)
                throw new NoSuchElementException("Cannot find collection:  " + collectionName);

            if (collection.Count == 0)
                throw new NoSuchElementException("Collection (" + collectionName + ") contains no items");

            IWebElement element;
            try  {
                element = GetElementInCollection(collection, index, last);
            }
            catch (NoSuchElementException) {
                throw new NoSuchElementException("Unable to get collection (" + collectionName + ") on index " + index);
            }
            return element;
        }

        internal protected IWebElement GetElementInCollection(string collectionName, string text, CompareType compareType)
        {
            var collection = FindElements(collectionName);
            if (collection == null)
                return null;

            return collection.FirstOrDefault(e => e.Text.CompareWith(text, compareType));
        }

        public bool HasTextInCollection(string collectionName, string text, CompareType compareType)
        {
            var theElement = GetElementInCollection(collectionName, text, compareType);
            return (theElement != null);
        }
        
        public string GetElementAttribute(string collectionName, int index, string attributeName)
        {
            IWebElement element;
            try
            {
                element = GetElementInCollection(collectionName, index);
            }
            catch (NoSuchElementException e)
            {
                Trace.WriteLine("Unable to get element in collection " + collectionName + " at index " + index);
                return null;
            }
            return element.GetAttribute(attributeName).Trim();
        }

        public bool ElementIsVisible(string elementName, int timeoutSecs)
        {
            var element = FindElement(elementName, timeoutSecs);

            if (element == null)
                return false;

            return element.Displayed;
        }

        public bool ElementIsPresent(string elementName, int timeoutSecs)
        {
            var element = FindElement(elementName, timeoutSecs);
            return (element != null);
        }

        internal protected FindsByAttribute GetElementFindByAttribute(string elementOrCollectionName)
        {
            var member = Util.GetMemberInfo(this, elementOrCollectionName.Dehumanize(), BindingFlags.NonPublic);
            return member == null ? null : member.GetCustomAttribute<FindsByAttribute>();
        }

        internal protected string GetElementFindBySelector(string elementOrCollectionName)
        {
            var attribute = GetElementFindByAttribute(elementOrCollectionName);
            if (attribute == null)
                return null;
            return attribute.Using;
        }
        
        internal protected Tuple<IWebElement, int, string, string> FindElementWithinCollection(string collectionName, string relativeXpath, int timeoutSecs)
        {           
            var size = SizeOf(collectionName);
            // xpath is 1-based index
            for (var i = 1; i <= size; i++)
            {
                var tuple = FindElementWithinCollection(collectionName, i, relativeXpath, timeoutSecs);
                if (tuple != null)
                    return tuple;
            }
            return null;
        }

        internal protected Tuple<IWebElement, int, string, string> FindElementWithinCollection(string collectionName, int index, string relativeXpath, int timeoutSecs)
        {
            var attribute = GetElementFindByAttribute(collectionName);

            if (attribute == null || attribute.How != How.XPath)
                throw new ArgumentException("Only collection with find by xpath selector is supported.  Ensure the collection, " + collectionName + ", is using xpath");

            var parentXpath = attribute.Using;
            var parentElementXpath = "(" + parentXpath + ")[" + index + "]";
            var xpath = parentElementXpath + relativeXpath;
            var element = Driver.FindElement(By.XPath(xpath), timeoutSecs);
            return (element == null) ? null : Tuple.Create(element, index, element.Text, parentElementXpath);
        }

        internal protected string GetTextFromElementWithinCollection(string collectionName, int index, string relativeXpath,
            int timeoutSecs = 5)
        {
            var tuple = FindElementWithinCollection(collectionName, index, relativeXpath, timeoutSecs);
            return tuple == null ? null : tuple.Item3;
        }

        public string GetElementText(string elementName)
        {
            var element = FindElement(elementName);
            return element == null ? null : element.Text;
        }

        internal protected bool ElementExists(string elementName, int timeoutSecs)
        {            
            return Driver.ElementExists(timeoutSecs, new Func<string, IWebElement>(FindElement), elementName);
        }

        protected Component()
        {
            PageFactory.InitElements(Driver, this);
        }


    }
}
