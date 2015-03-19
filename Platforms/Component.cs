using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Joyride.Extensions;
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

        protected MemberInfo GetMemberInfo(string elementOrCollectionName)
        {
            MemberInfo member = GetType()
                .GetMember(elementOrCollectionName.ToPascalCase(), BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault();
            if (member == null)
                throw new NoSuchElementException(elementOrCollectionName + " is not defined on: " + Name);
            return member;
        }

        protected IWebElement FindElement(string elementName)
        {
            IWebElement element = null;
            var member = GetMemberInfo(elementName);

            var property = member as PropertyInfo;
            if (property != null)
            {
                element = (IWebElement)property.GetValue(this, null);
                if (element.IsPresent())
                {
                    Trace.WriteLine("Found property with elementName:  " + elementName);
                    return element;
                }
            }

            var field = member as FieldInfo;
            if (field != null)
            {
                element = (IWebElement)field.GetValue(this);
                if (element.IsPresent())
                {
                    Trace.WriteLine("Found field with elementName:  " + elementName);
                    return element;
                }
            }
            Trace.WriteLine("Unable to find element with name:  " + elementName);
            return null;
        }

        protected IList<IWebElement> FindElements(string collectionName)
        {
            IList<IWebElement> elements = null;
            MemberInfo member = GetMemberInfo(collectionName);

            var property = member as PropertyInfo;
            if (property != null)
            {
                elements = (IList<IWebElement>)property.GetValue(this, null);
                Trace.WriteLine("Found property with collection:  " + collectionName);
                return elements;
            }

            var field = member as FieldInfo;
            if (field != null)
            {
                elements = (IList<IWebElement>)field.GetValue(this);
                Trace.WriteLine("Found field with collection:  " + collectionName);

                // able to access elements
                if (elements.GetEnumerator().MoveNext())
                   return elements;
            }            
            return null;
        }

        protected IList<IWebElement> FindElements(string collectionName, int timeoutSecs)
        {
            return Driver.FindElementsWithMethod(timeoutSecs, new Func<string, IList<IWebElement>>(FindElements), collectionName);
        }

        protected IWebElement FindElement(string elementName, int timeoutSecs)
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

        public string GetElementAttribute(string elementName, string attributeName)
        {
            var element = FindElement(elementName);

            if (element == null)
                return null;

            var attribValue = element.GetAttribute(attributeName);
            return (attribValue == null) ? null : attribValue.Trim();
        }

        protected IWebElement GetElementInCollection(IList<IWebElement> collection , int index, bool last = false)
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

        protected IWebElement GetElementInCollection(string collectionName, int index, bool last=false)
        {
            var collection = FindElements(collectionName, DefaultWaitSeconds);

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

        protected IWebElement GetElementInCollection(string collectionName, string text, CompareType compareType)
        {
            var collection = FindElements(collectionName, DefaultWaitSeconds);
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

        public bool ElementIsVisible(string elementName, int timeoutSecs=DefaultWaitSeconds)
        {
            var element = FindElement(elementName, timeoutSecs);

            if (element == null)
                return false;

            return element.Displayed;
        }

        public bool ElementIsPresent(string elementName, int timeoutSecs = DefaultWaitSeconds)
        {
            var element = FindElement(elementName, timeoutSecs);
            return (element != null);
        }

        protected FindsByAttribute GetElementFindByAttribute(string elementOrCollectionName)
        {
            var member = GetMemberInfo(elementOrCollectionName);
            if (member == null)
                return null;
            return member.GetCustomAttribute<FindsByAttribute>();
        }

        protected string GetElementFindBySelector(string elementOrCollectionName)
        {
            var attribute = GetElementFindByAttribute(elementOrCollectionName);
            if (attribute == null)
                return null;
            return attribute.Using;
        }
        
        //TODO: remove this method
        [Obsolete("This method is no longer supported. Use GetElementFindBySelector")]
        protected string GetElementXPathSelector(string elementName)
        {
            var field = GetType().GetField(elementName.ToPascalCase(), BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null)
                return null;

            var attrib = (FindsByAttribute)field.GetCustomAttribute(typeof(FindsByAttribute));
            if (attrib.How != How.XPath)
                return null;
            return attrib.Using;
        }

        protected Tuple<IWebElement, int, string, string> FindElementWithinCollection(string collectionName, string relativeXpath, int timeoutSecs = 5)
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

        protected Tuple<IWebElement, int, string, string> FindElementWithinCollection(string collectionName, int index, string relativeXpath, int timeoutSecs = 5)
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

        protected string GetTextFromElementWithinCollection(string collectionName, int index, string relativeXpath,
            int timeoutSecs = 5)
        {
            var tuple = FindElementWithinCollection(collectionName, index, relativeXpath, timeoutSecs);
            return tuple == null ? null : tuple.Item3;
        }

        public string GetElementText(string elementName)
        {
            var element = FindElement(elementName);

            if (element == null)
                return null;

            return element.Text;
        }

        protected bool ElementExists(string elementName, int timeoutSecs=10)
        {            
            return Driver.ElementExists(timeoutSecs, new Func<string, IWebElement>(FindElement), elementName);
        }

        protected Component()
        {
            PageFactory.InitElements(Driver, this);
        }


    }
}
