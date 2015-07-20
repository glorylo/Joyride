using System;
using System.Diagnostics;
using System.Linq;
using Joyride.Extensions;
using Joyride.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Support.UI;

namespace Joyride.Platforms
{
    abstract public class Screen : Component, IScreen, IDetectModalDialog
    {
        abstract public bool IsOnScreen(int timeOutSecs);
        protected static IDetector<IModalDialog> ModalDialogDetector;

        #region ModalDialog Handling
        public virtual IModalDialog DetectModalDialog()
        {
            return ModalDialogDetector.Detect();
        }

        public virtual IModalDialog DetectModalDialog(string modalDialogName)
        {
            return ModalDialogDetector.Detect(modalDialogName);
        }

        public virtual Screen AcceptModalDialog(bool accept, string modalDialogName, bool throwException = false)
        {
            var dialog = DetectModalDialog(modalDialogName);
            if (dialog == null)
            {
                if (throwException)
                    throw new NoAlertPresentException("Unable to detect the modal dialog: " + modalDialogName);
                Trace.WriteLine("Unable to detect modal dialog: " + modalDialogName);
                return this;
            }
            var screen = (accept) ? dialog.Accept() : dialog.Dismiss();
            return screen ?? this;
        }

        public virtual Screen AcceptModalDialog(bool accept)
        {
            return AcceptModalDialogs(accept);
        }

        public virtual Screen RespondModalDialog(string response, string modalDialogName, bool throwException = false)
        {
            var dialog = DetectModalDialog(modalDialogName);
            if (dialog == null)
            {
                if (throwException)
                    throw new NoAlertPresentException("Unable to detect the modal dialog: " + modalDialogName);
                Trace.WriteLine("Unable to detect modal dialog: " + modalDialogName);
                return this;
            }
            return dialog.RespondWith(response) ?? this;
        }

        internal protected IModalDialog DetectModalDialog(Type dialogType)
        {
            return ModalDialogDetector.Detect(dialogType);
        }

        internal protected IModalDialog DetectModalDialogs(params Type[] dialogs)
        {
            var dialog = !dialogs.Any() ? ModalDialogDetector.Detect() : ModalDialogDetector.Detect(dialogs);
            return dialog;
        }

        internal protected Screen AcceptModalDialog(bool accept, Type dialogType, bool throwException = false)
        {
            var dialog = DetectModalDialog(dialogType);
            if (dialog == null)
            {
                if (throwException)
                    throw new NoAlertPresentException("Unable to detect the modal dialog of type: " + dialogType);
                Trace.WriteLine("Unable to detect modal dialog of type: " + dialogType);
                return this;
            }

            var screen = (accept) ? dialog.Accept() : dialog.Dismiss();
            return screen ?? this;
        }

        internal protected Screen RespondModalDialog(string response, Type dialogType, bool throwException = false)
        {
            var dialog = DetectModalDialog(dialogType);
            if (dialog == null)
            {
                if (throwException)
                    throw new NoAlertPresentException("Unable to detect the modal dialog of type: " + dialogType);
                Trace.WriteLine("Unable to detect modal dialog of type: " + dialogType);
                return this;
            }
            return dialog.RespondWith(response) ?? this;
        }

        internal protected Screen AcceptModalDialogs(bool accept, params Type[] dialogTypes)
        {
            var dialog = !dialogTypes.Any() ? ModalDialogDetector.Detect() : ModalDialogDetector.Detect(dialogTypes);

            if (dialog == null)
                return this;

            var screen = accept ? dialog.Accept() : dialog.Dismiss();
            return screen ?? this;
        }

        #endregion

        #region Gestures

        public virtual Screen Pull(Direction direction, int durationMillSecs = 1000)
        {
            Driver.PullScreen(direction, durationMillSecs);
            Driver.WaitFor(TimeSpan.FromMilliseconds(500));
            return this;
        }

        public Screen SwipeInCollection(string collectionName, Direction direction, int oridinal = 1, bool last = false)
        {
            var elements = FindElements(collectionName);
            var element = GetElementInCollection(elements, oridinal, last);
            Driver.Swipe(element, direction);
            return this;
        }

        internal protected void TapInWebview(string elementName)
        {
            Driver.DoActionInWebView(() =>
            {
                var element = FindElement(elementName);
                if (element == null)
                    throw new NoSuchElementException("Cannot find element:  " + elementName);

                element.Click();
            });
        }

        public virtual Screen Tap(string elementName, bool precise = false)
        {
            var element = FindElement(elementName);

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
            var element = FindCachedElement(elementName);

            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            Driver.DoubleTap(element);
            return this;
        }

        public virtual Screen TapAndHold(string elementName, int seconds)
        {
            var element = FindCachedElement(elementName);

            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            Driver.TapAndHold(element, seconds);
            return this;

        }

        public virtual Screen TapInCollection(string collectionName, int oridinal = 1, bool last = false, bool precise = false)
        {
            var element = GetElementInCollection(collectionName, oridinal, last);
            
            if (!precise)
                element.Click();
            else
                Driver.PreciseTap(element);
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

        public virtual Screen Scroll(Direction direction, double scale=1.0, long durationMilliSecs = 500)
        {
            Driver.Scroll(direction, scale, durationMilliSecs);
            return this;
        }

        public virtual Screen Swipe(Direction direction, double scale=1.0, long durationMilliSecs = 500)
        {
            Driver.Swipe(direction, scale, durationMilliSecs);
            return this;
        }

        public virtual Screen Swipe(string elementName, Direction direction, double scale=1.0, long durationMilliSecs = 500)
        {
            var element = FindCachedElement(elementName);
            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);
            Driver.Swipe(element, direction, scale, durationMilliSecs);
            return this;
        }

        public virtual Screen ScrollUntil(string elementName, Direction direction, int maxRetries, int timeoutSecs, double scale=1.0, long durationMilliSecs = 500)
        {
            var element = FindElement(elementName, timeoutSecs);

            if (element.IsPresent() && element.Displayed)
                return this;
            var numRetries = 0;

            while (numRetries <= maxRetries)
            {
                Driver.Scroll(direction, scale, durationMilliSecs);
                element = FindElement(elementName, timeoutSecs);
                if (element.IsPresent() && element.Displayed)
                    return this;

                numRetries++;
            }
            throw new NoSuchElementException("Unable to find visible element: " + elementName);
        }
        #endregion 

        #region UI Controls

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

        #endregion

        #region WebView

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

        #endregion

        public virtual Screen Rotate(ScreenOrientation orientation)
        {
            Driver.Orientation = orientation;
            return this;
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
