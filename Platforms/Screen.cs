using System;
using System.Linq;
using Joyride.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;

namespace Joyride.Platforms
{
    abstract public class Screen : Component
    {
        abstract public bool IsOnScreen(int timeOutSecs = RemoteDriver.DefaultWaitSeconds);
        public virtual Screen Tap(string elementName, bool precise = false)
        {
            IWebElement element = FindElement(elementName);

            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            if (!precise)
                element.Click();
            else
                ((AppiumDriver)Driver).PreciseTap(element);

            return this;
        }

        public virtual Screen DoubleTap(string elementName)
        {
            IWebElement element = FindElement(elementName);

            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            ((AppiumDriver)Driver).DoubleTap(element);
            return this;
        }

        public virtual Screen TapAndHold(string elementName, int seconds)
        {
            IWebElement element = FindElement(elementName);

            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            ((AppiumDriver)Driver).TapAndHold(element, seconds);
            return this;

        }

        public virtual Screen TapInCollection(string collectionName, int oridinal = 1, bool last = false)
        {
            var element = GetElementInCollectionAt(collectionName, oridinal, last);
            element.Click();
            return this;
        }

        public virtual Screen TapInCollection(string collectionName, Predicate<IWebElement> predicate)
        {
            var collection = FindElements(collectionName, RemoteDriver.DefaultWaitSeconds);

            if (collection == null)
                throw new NoSuchElementException("Cannot find collection:  " + collectionName);


            foreach (var item in collection.Where(item => predicate(item)))
            {
                ((AppiumDriver)Driver).PreciseTap(item);
                return this;
            }
            throw new NoSuchElementException("item not found in collection " + collectionName);
        }

        public void PinchToZoom(Direction direction, double scale = 1.0)
        {
            ((AppiumDriver)Driver).PinchToZoom(direction, scale);
        }
        
        protected void EnterTextWebView(string elementName, string text, string doneButton = "Done")
        {

            //TODO:  need to refix this.  should first find element before accessing tap and should handle exceptions better
            var driver = (AppiumDriver)Driver;

            var element = FindElement(elementName);
            if (element == null)
                throw new NoSuchElementException("Unable to find element " + elementName);

            element.Click();
            element.Clear();

            int repeat = element.Text.Count();
            string backspaces = "";

            for (int i = 0; i < repeat; i++)
            {
                backspaces += Keys.Backspace;
            }

            if (repeat > 0)
                driver.Keyboard.SendKeys(backspaces);

            driver.Keyboard.SendKeys(text);
            driver.ClickDoneToHideKeyboard(doneButton);
        }

        /*
                protected string ClearElementWithBackspaces(IWebElement element)
                {
                    var backspaces = new string(Convert.ToChar(Keys.Backspace), element.GetAttribute("value").Count());

                    return backspaces.Length > 0 ? backspaces : "";
                }
        */ 

        public virtual void EnterText(string elementName, string text)
        {
            IWebElement element = FindElement(elementName);

            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            element.Click();
            element.Clear();
            element.SendKeys(text);
        }

        public virtual void SetCheckbox(string elementName, bool enabled = true)
        {
            var element = FindElement(elementName);

            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            var selected = element.Selected;
            if ((enabled && !selected) || (!enabled && selected))
                element.Click();
        }


        // only works in webview currently
        public virtual void SelectOption(string elementName, string value)
        {
            ((AppiumDriver)Driver).DoActionInWebView(() =>
            {
                var selectElement = FindElement(elementName);
                if (selectElement == null)
                    throw new NoSuchElementException("Cannot find element:  " + elementName);

                var selector = new SelectElement(selectElement);
                selector.SelectByText(value);
            });
        }

        // only works in webview currently
        public virtual string GetSelectedOption(string elementName)
        {
            string selected = null;
            ((AppiumDriver)Driver).DoActionInWebView(() =>
            {
                var selectElement = FindElement(elementName);
                if (selectElement == null)
                    throw new NoSuchElementException("Cannot find element:  " + elementName);

                var selector = new SelectElement(selectElement);
                selected = selector.SelectedOption.Text;
            });
            return selected;
        }

        public virtual Screen Scroll(Direction direction, long durationMilliSecs = 500)
        {
            ((AppiumDriver)Driver).Scroll(direction, durationMilliSecs);
            return this;
        }

        public virtual Screen Scroll(string elementName, Direction direction, long durationMilliSecs = 500)
        {
            var element = FindElement(elementName);
            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);
            ((AppiumDriver)Driver).Scroll(element, direction, durationMilliSecs);
            return this;
        }

        public virtual Screen Swipe(Direction direction, long durationMilliSecs = 500)
        {
            ((AppiumDriver)Driver).Swipe(direction, durationMilliSecs);
            return this;
        }

        public virtual Screen Swipe(string elementName, Direction direction, long durationMilliSecs = 500)
        {
            var element = FindElement(elementName);
            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);
            ((AppiumDriver)Driver).Swipe(element, direction, durationMilliSecs);
            return this;
        }

        public virtual Screen ScrollUntil(string elementName, Direction direction, long durationMilliSecs = 500, int maxRetries = 30)
        {
            var element = FindElement(elementName, 3);

            if (element.IsPresent() && element.Displayed)
                return this;
            var numRetries = 0;

            while (numRetries <= maxRetries)
            {
                ((AppiumDriver)Driver).Scroll(direction, durationMilliSecs);
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
            ((AppiumDriver)Driver).DoActionInWebView(() => { source = GetSource(); });
            return source;
        }

        public string GetSource() { return Driver.PageSource; }


    }

}
