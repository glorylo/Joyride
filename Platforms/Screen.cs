using System;
using System.Linq;
using Joyride.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;

namespace Joyride.Platforms
{
    abstract public class Screen : Component, IScreen
    {
        abstract public bool IsOnScreen(int timeOutSecs = DefaultWaitSeconds);
        public virtual Screen Tap(string elementName, bool precise = false)
        {
            IWebElement element = FindElement(elementName);

            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            if (!precise)
                element.Click();
            else
                Driver.PreciseTap(element);

            return this;
        }

        public virtual Screen DoubleTap(string elementName)
        {
            IWebElement element = FindElement(elementName);

            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            Driver.DoubleTap(element);
            return this;
        }

        public virtual Screen TapAndHold(string elementName, int seconds)
        {
            IWebElement element = FindElement(elementName);

            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            Driver.TapAndHold(element, seconds);
            return this;

        }

        public virtual Screen TapInCollection(string collectionName, int oridinal = 1, bool last = false)
        {
            var element = GetElementInCollection(collectionName, oridinal, last);
            element.Click();
            return this;
        }

        public virtual Screen TapInCollection(string collectionName, Predicate<IWebElement> predicate)
        {
            var collection = FindElements(collectionName, DefaultWaitSeconds);

            if (collection == null)
                throw new NoSuchElementException("Cannot find collection:  " + collectionName);


            foreach (var item in collection.Where(item => predicate(item)))
            {
                Driver.PreciseTap(item);
                return this;
            }
            throw new NoSuchElementException("item not found in collection " + collectionName);
        }

        public virtual Screen PinchToZoom(Direction direction, double scale = 1.0)
        {
            Driver.PinchToZoom(direction, scale);
            return this;
        }

      
        public virtual Screen EnterText(string elementName, string text)
        {
            var element = FindElement(elementName);

            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            element.Clear();
            element.SendKeys(text);
            return this;
        }

        public virtual Screen SetCheckbox(string elementName, bool enabled = true)
        {
            var element = FindElement(elementName);

            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            var selected = element.Selected;
            if ((enabled && !selected) || (!enabled && selected))
                element.Click();

            return this;
        }

        // only works in webview currently
        public virtual Screen SelectOption(string elementName, string value)
        {
            Driver.DoActionInWebView(() =>
            {
                var selectElement = FindElement(elementName);
                if (selectElement == null)
                    throw new NoSuchElementException("Cannot find element:  " + elementName);

                var selector = new SelectElement(selectElement);
                selector.SelectByText(value);
            });
            return this;
        }

        // only works in webview currently
        public virtual string GetSelectedOption(string elementName)
        {
            string selected = null;
            Driver.DoActionInWebView(() =>
            {
                var selectElement = FindElement(elementName);
                if (selectElement == null)
                    throw new NoSuchElementException("Cannot find element:  " + elementName);

                var selector = new SelectElement(selectElement);
                selected = selector.SelectedOption.Text;
            });
            return selected;
        }

        public virtual Screen Scroll(Direction direction, double scale=1.0, long durationMilliSecs = 500)
        {
            Driver.Scroll(direction, scale, durationMilliSecs);
            return this;
        }

        public virtual Screen Scroll(string elementName, Direction direction, double scale=1.0, long durationMilliSecs = 500)
        {
            var element = FindElement(elementName);
            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);
            Driver.Scroll(element, direction, scale, durationMilliSecs);
            return this;
        }

        public virtual Screen Swipe(Direction direction, double scale=1.0, long durationMilliSecs = 500)
        {
            Driver.Swipe(direction, scale, durationMilliSecs);
            return this;
        }

        public virtual Screen Swipe(string elementName, Direction direction, double scale=1.0, long durationMilliSecs = 500)
        {
            var element = FindElement(elementName);
            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);
            Driver.Swipe(element, direction, scale, durationMilliSecs);
            return this;
        }

        public virtual Screen ScrollUntil(string elementName, Direction direction, double scale=1.0, long durationMilliSecs = 500, int maxRetries = 30)
        {
            var element = FindElement(elementName, 3);

            if (element.IsPresent() && element.Displayed)
                return this;
            var numRetries = 0;

            while (numRetries <= maxRetries)
            {
                Driver.Scroll(direction, scale, durationMilliSecs);
                element = FindElement(elementName, 3);
                if (element.IsPresent() && element.Displayed)
                    return this;

                numRetries++;
            }
            throw new NoSuchElementException("Unable to find visible element: " + elementName);
        }

        public string GetSourceWebView()
        {
            string source = null;
            Driver.DoActionInWebView(() => { source = GetSource(); });
            return source;
        }

        public string GetSource() { return Driver.PageSource; }


    }

}
