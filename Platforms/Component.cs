using System;
using System.Collections.ObjectModel;
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
        protected IWebElement FindElement(string elementName)
        {
            IWebElement element = null;
            MemberInfo member = GetType()
                .GetMember(elementName.ToPascalCase(), BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault();
            if (member == null)
                throw new NoSuchElementException(elementName + " is not found");

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

        protected ReadOnlyCollection<IWebElement> FindElements(string elementName)
        {
            ReadOnlyCollection<IWebElement> elements = null;
            MemberInfo member = GetType()
                .GetMember(elementName.ToPascalCase(), BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault();
            if (member == null)
                throw new NoSuchElementException(elementName + " is not found");

            var property = member as PropertyInfo;
            if (property != null)
            {
                elements = (ReadOnlyCollection<IWebElement>)property.GetValue(this, null);
                Trace.WriteLine("Found property with elementName:  " + elementName);
                return elements;
            }

            var field = member as FieldInfo;
            if (field != null)
            {
                elements = (ReadOnlyCollection<IWebElement>)field.GetValue(this);
                Trace.WriteLine("Found field with elementName:  " + elementName);
                return elements;
            }
            Trace.WriteLine("Unable to find collection with name:  " + elementName);
            return null;
        }

        protected ReadOnlyCollection<IWebElement> FindElements(string elementName, int timeoutSecs)
        {
            return Driver.FindElementsWithMethod(timeoutSecs, new Func<string, ReadOnlyCollection<IWebElement>>(FindElements), elementName);
        }

        protected IWebElement FindElement(string elementName, int timeoutSecs)
        {
            return Driver.FindElementWithMethod(timeoutSecs, new Func<string, IWebElement>(FindElement), elementName);
        }

        public int SizeOf(string collectionName)
        {
            var collection = FindElements(collectionName, DefaultWaitSeconds);

            if (collection == null)
                throw new NoSuchElementException("Cannot find collection:  " + collectionName);
            return collection.Count;
        }

        public bool IsEmpty(string collectionName)
        {
            return (SizeOf(collectionName) == 0);
        }

        public string GetElementAttribute(string elementName, string attributeName)
        {
            var element = FindElement(elementName);

            if (element == null)
                return null;

            var attribValue = element.GetAttribute(attributeName);
            return (attribValue == null) ? null : attribValue.Trim();
        }

        protected IWebElement GetElementInCollectionAt(string collectionName, int index, bool last=false)
        {
            var collection = FindElements(collectionName, DefaultWaitSeconds);

            if (collection == null)
                throw new NoSuchElementException("Cannot find collection:  " + collectionName);

            var zeroBasedIndex = index - 1;

            if (collection.Count == 0)
                throw new NoSuchElementException("Collection (" + collectionName + ") contains no items");

            if (!last)
            {
                if ((zeroBasedIndex < 0) || (zeroBasedIndex >= collection.Count))
                    throw new NoSuchElementException("Unable to get collection (" + collectionName + ") on index " +
                                                     zeroBasedIndex);
                return collection.ElementAt(zeroBasedIndex);
            }
            return collection.Last();
        }
        
        public string GetElementAttribute(string collectionName, int index, string attributeName)
        {
            IWebElement element;
            try
            {
                element = GetElementInCollectionAt(collectionName, index);
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
