
using System;
using System.Collections.ObjectModel;
using Joyride.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.MultiTouch;

namespace Joyride.Platforms.Android
{
    abstract public class AndroidScreen : Screen
    {
        protected static ScreenFactory ScreenFactory = new AndroidScreenFactory();
        protected static new AndroidDriver Driver = (AndroidDriver) RemoteMobileDriver.GetInstance();

        public void HideKeyboard()
        {
            // suppress any odd appium errors
            try { Driver.HideKeyboard(); }
            catch { } 
        }

        public override Screen TapAndHold(string elementName, int seconds)
        {
            IWebElement element = FindElement(elementName);

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
            // allow time to render the other half of the screen
            Driver.WaitFor(TimeSpan.FromMilliseconds(500));
            return this;
        }

        public virtual bool HasLabel(string label, CompareType compareType, int timeoutSecs = DefaultWaitSeconds)
        {
            return HasText(label, compareType, timeoutSecs) || HasContentDesc(label, compareType, timeoutSecs);
        }

        protected bool HasContentDesc(string label, CompareType compareType, int timeoutSecs=DefaultWaitSeconds)
        {
            ReadOnlyCollection<IWebElement> texts = null;
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

            if (texts != null && texts.Count != 0)
                return true;

            return false;
        }

        protected bool HasText(string label, CompareType compareType, int timeoutSecs = DefaultWaitSeconds)
        {
            ReadOnlyCollection<IWebElement> texts = null;
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

            if (texts != null && texts.Count != 0)
                return true;

            return false;
        }

        public abstract Screen GoBack();

    }
}
