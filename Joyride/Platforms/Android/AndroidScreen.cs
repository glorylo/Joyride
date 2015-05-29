
using System;
using System.Collections.Generic;
using System.Linq;
using Joyride.Extensions;
using Joyride.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.MultiTouch;

namespace Joyride.Platforms.Android
{
    abstract public class AndroidScreen : Screen, IDetectModalDialog
    {
        protected static ScreenFactory ScreenFactory = new AndroidScreenFactory();
        protected static IDetector<IModalDialog> ModalDialogDetector;
        protected static new AndroidDriver Driver = (AndroidDriver) RemoteMobileDriver.GetInstance();

        public virtual IModalDialog DetectModalDialog()
        {
            return ModalDialogDetector.Detect();
        }

        public virtual IModalDialog DetectModalDialog(Type dialogType)
        {
            return ModalDialogDetector.Detect(dialogType);
        }

        public virtual IModalDialog DetectModalDialog(string modalDialogName)
        {
            return ModalDialogDetector.Detect(modalDialogName);
        }

        public virtual Screen AcceptModalDialog(bool accept, string modalDialogName)
        {
            return AcceptModalDialogs(accept, new [] { modalDialogName });
        }

        public virtual Screen AcceptModalDialog(bool accept)
        {
            return AcceptModalDialogs(accept);
        }

        protected Screen AcceptModalDialogs(bool accept, string[] dialogs)
        {
            var dialog = !dialogs.Any() ? ModalDialogDetector.Detect() : ModalDialogDetector.Detect(dialogs);

            if (dialog == null)
                return this;

            var screen =  accept ? dialog.Accept() : dialog.Dismiss();
            return screen ?? this;
        }

        protected Screen AcceptModalDialogs(bool accept, params Type[] dialogTypes)
        {
            var dialog = !dialogTypes.Any() ? ModalDialogDetector.Detect() : ModalDialogDetector.Detect(dialogTypes);

            if (dialog == null)
                return this;

            var screen = accept ? dialog.Accept() : dialog.Dismiss();
            return screen ?? this;
        }

        public void HideKeyboard()
        {
            // suppress any odd appium errors
            try { Driver.HideKeyboard(); }
            catch { }

            // allow time to render the other half of the screen
            Driver.WaitFor(TimeSpan.FromMilliseconds(500));
        }

        public override Screen TapAndHold(string elementName, int seconds)
        {
            var element = FindElement(elementName);

            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            Driver.TapAndHold(element, seconds, true);
            return this;

        }

         public override Screen DoubleTap(string elementName)
        {
            var element = FindElement(elementName);

            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);


            var point = element.GetCenter();
            //new TouchAction(Driver).Tap(point.X, point.Y, 2).Perform();           
            new TouchAction(Driver)
                 .Press(point.X, point.Y)
                 .Wait(50)
                 .Release()
                 .Wait(100)
                 .Press(point.X, point.X)
                 .Wait(50)
                 .Release().
                 Perform();
            return this;
        }

        public override Screen EnterText(string elementName, string text)
        {
            var element = FindElement(elementName);

            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            element.Click();
            element.Clear();
            element.SendKeys(text);
            HideKeyboard();
            return this;
        }

        public virtual bool HasLabel(string label, CompareType compareType, int timeoutSecs = DefaultWaitSeconds)
        {
            return HasText(label, compareType, timeoutSecs) || HasContentDesc(label, compareType, timeoutSecs);
        }

        public virtual bool HasLabelInCollection(string collectionName, string label, CompareType compareType)
        {
            var xpath = "//*";
            switch (compareType)
            {
                case CompareType.StartsWith:
                    xpath += "[starts-with(@text, '" + label + "')]";
                    break;

                case CompareType.Containing:
                    xpath += "[contains(@text, '" + label + "')]";
                    break;
                    
                case CompareType.Equals:
                    xpath += "[@text='" + label + "']";
                    break;

                default:
                    throw new NotImplementedException("Other text compares are not implemented");
            }
            var tuple = FindElementWithinCollection(collectionName, xpath);
            return (tuple != null);
        }

        internal protected bool HasContentDesc(string label, CompareType compareType, int timeoutSecs=DefaultWaitSeconds)
        {
            IList<IWebElement> texts;
            switch (compareType)
            {
                case CompareType.Equals:
                    texts = Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().description(""" + label + @""")", timeoutSecs);
                    break;

                case CompareType.StartsWith:
                    texts =
                        Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().descriptionStartsWith(""" + label + @""")", timeoutSecs);
                    break;

                case CompareType.Matching:
                    texts = Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().descriptionMatches(""" + label + @""")", timeoutSecs);
                    break;

                case CompareType.Containing:
                    texts = Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().descriptionContains(""" + label + @""")", timeoutSecs);
                    break;
                default:
                    throw new NotImplementedException("Not implemented compare type: " + compareType);
            }

            return texts != null && texts.Count != 0;
        }

        internal protected bool HasText(string label, CompareType compareType, int timeoutSecs = DefaultWaitSeconds)
        {
            IList<IWebElement> texts = null;
            switch (compareType)
            {
                case CompareType.Equals:
                    texts = Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().text(""" + label + @""")", timeoutSecs);
                    break;

                case CompareType.StartsWith:
                    texts =
                        Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().textStartsWith(""" + label + @""")", timeoutSecs);
                    break;

                case CompareType.Matching:
                    texts = Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().textMatches(""" + label + @""")", timeoutSecs);
                    break;

                case CompareType.Containing:
                    texts = Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().textContains(""" + label + @""")", timeoutSecs);
                    break;
                default:
                    throw new NotImplementedException("Not implemented compare type: " + compareType);
            }

            return texts != null && texts.Count != 0;
        }

        public abstract Screen GoBack();

        public static bool IsOnScreen<T>(int timeoutSecs) where T : Screen, new()
        {
            return ScreenFactory.CreateScreen<T>().IsOnScreen(timeoutSecs);
        }

        public static bool IsOnScreen(Type t, int timeoutSecs)
        {
            return ScreenFactory.CreateScreen(t).IsOnScreen(timeoutSecs);
        }
    }
}
