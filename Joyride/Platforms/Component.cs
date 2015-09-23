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
        protected static AppiumDriver Driver { get { return RemoteMobileDriver.GetInstance(); } }

        internal protected View GetCurrentView()
        {
            return Driver.IsNative() ? View.Native : View.Webview;
        }

        internal protected void SetCurrentView(View view)
        {
            if (GetCurrentView() == view) return;
            if (view == View.Native)
                Driver.SwitchToNative();
            else
                Driver.SwitchToWebview();
        }

        internal protected bool IsNativeView()
        {
            return GetCurrentView() == View.Native;
        }

        protected internal bool IsWebview(string elementOrCollectionName)
        {
            var attribute = Util.GetMemberCustomAttribute<WebviewAttribute>(this, elementOrCollectionName.Dehumanize(),
                BindingFlags.NonPublic);
            return attribute != null;
        }

        internal protected IWebElement RetrieveElement(string elementName)
        {
            var element = (IWebElement) Util.GetMemberValue(this, elementName.Dehumanize(), true, BindingFlags.NonPublic);

            if (element == null)
                return null;

//            if (IsWebview(elementName))
//            {
//                Trace.WriteLine("Require switching to webview for element: " + elementName);
//                Driver.DoActionInWebView(() =>
//                {
//                    element = (element.IsPresent())
//                        ? Util.GetMemberValue(element, "WrappedElement") as IWebElement
//                        : null;
//                });
//            }
//            else
//            {
//                element = (element.IsPresent())
//                    ? Util.GetMemberValue(element, "WrappedElement") as IWebElement
//                    : null;
//            }

            element = (element.IsPresent())
                ? Util.GetMemberValue(element, "WrappedElement", false) as IWebElement
                : null;

            return element;
        }


        internal protected IList<IWebElement> RetrieveElements(string collectionName)
        {
            var elements =
                (IList<IWebElement>) Util.GetMemberValue(this, collectionName.Dehumanize(), true, BindingFlags.NonPublic);

            if (elements == null)
                return null;

            //
//            if (IsWebview(collectionName))
//            {
//                Driver.DoActionInWebView(() =>
//                {
//                    isPresent = elements.GetEnumerator().MoveNext();
//                    if (isPresent)
//                        elements = Util.GetMemberValue(elements, "ElementList", BindingFlags.NonPublic) as IList<IWebElement>;
//                });
//            }
//            else
//            {
//                isPresent = elements.GetEnumerator().MoveNext(); // determine if the element is present by retrieving it.
//                if (isPresent)
//                    elements = Util.GetMemberValue(elements, "ElementList", BindingFlags.NonPublic) as IList<IWebElement>;                   
//            }

            var isPresent = elements.GetEnumerator().MoveNext();
            return (isPresent ? elements : null);
        } 

        internal protected IWebElement FindElement(string elementName)
        {
            var element = RetrieveElement(elementName);
            if (element == null)
              Trace.WriteLine("Unable to access element:  " + elementName);  
            return element;
        }

        internal protected IList<IWebElement> FindElements(string collectionName)
        {
            var elements = RetrieveElements(collectionName);

            if (elements == null)
              Trace.WriteLine("Unable to access the collection: " + collectionName);

            return elements;
        }

        internal protected IList<IWebElement> FindElements(string collectionName, int timeoutSecs)
        {
            return Driver.FindElementsWithMethod(timeoutSecs, new Func<string, IList<IWebElement>>(FindElements), collectionName);
        }

        internal protected IWebElement FindElement(string elementName, int timeoutSecs)
        {
            return Driver.FindElementWithMethod(timeoutSecs, new Func<string, IWebElement>(FindElement), elementName);
        }

        public virtual int SizeOf(string collectionName, int timeoutSecs=DefaultWaitSeconds)
        {
            var collection = FindElements(collectionName, timeoutSecs);

            if (collection == null)
                return 0;
            var size = collection.Count;
            Trace.WriteLine("Found collection '" + collectionName + "' with number of items: " + size);
            return size;            
        }

        public virtual bool IsEmpty(string collectionName, int timeoutSecs = DefaultWaitSeconds)
        {
            return (SizeOf(collectionName, timeoutSecs) == 0);
        }

        public virtual bool IsSelected(string elementName)
        {
            var element = FindElement(elementName);
            if (element == null)
                throw new NoSuchElementException("Unable to find element: " + elementName);

            var selected = element.Selected;
            Trace.WriteLine("Element (" + elementName + ") selected:  " + selected);            
            return selected;
        }

        public virtual bool IsEnabled(string elementName)
        {
            var element = FindElement(elementName);
            if (element == null)
                throw new NoSuchElementException("Unable to find element: " + elementName);

            var enabled = element.Enabled;
            Trace.WriteLine("Element (" + elementName + ") enabled:  " + enabled);
            return enabled;
        }

        public virtual string GetElementAttribute(string elementName, string attributeName)
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

        public virtual bool HasTextInCollection(string collectionName, string text, CompareType compareType)
        {
            var theElement = GetElementInCollection(collectionName, text, compareType);
            return (theElement != null);
        }
        
        public virtual string GetElementAttribute(string collectionName, int index, string attributeName)
        {
            IWebElement element;
            try
            {
                element = GetElementInCollection(collectionName, index);
            }
            catch (NoSuchElementException)
            {
                Trace.WriteLine("Unable to get element in collection " + collectionName + " at index " + index);
                return null;
            }
            return element.GetAttribute(attributeName).Trim();
        }

        public virtual bool ElementIsVisible(string elementName, int timeoutSecs)
        {
            var element = FindElement(elementName, timeoutSecs);

            if (element == null)
                return false;
            
            var visible = element.Displayed;
            Trace.WriteLine("Element (" + elementName + ") visibility:  " + visible);
            return visible;
        }

        public virtual bool ElementIsPresent(string elementName, int timeoutSecs)
        {
            var element = FindElement(elementName, timeoutSecs);
            return (element != null);
        }

        internal protected string GetElementFindBySelector(string elementOrCollectionName)
        {
            var attribute = Util.GetMemberCustomAttribute<FindsByAttribute>(this, elementOrCollectionName.Dehumanize(), BindingFlags.NonPublic);
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
            var attribute = Util.GetMemberCustomAttribute<FindsByAttribute>(this, collectionName.Dehumanize(), BindingFlags.NonPublic);

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

        public virtual string GetElementText(string elementName)
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

        //TODO: may remove
        protected void DoActionInWebView(Action action, int maxRetries = 3)
        {
            if (IsNativeView())
            {                
                SetCurrentView(View.Webview);
                action();
                SetCurrentView(View.Native);
            }
            else
            {
                action();
            }
        }
    }
}
