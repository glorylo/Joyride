
using System;
using System.Collections.ObjectModel;
using Joyride.Extensions;
using OpenQA.Selenium;

namespace Joyride.Platforms.Android
{
    abstract public class AndroidScreen : Screen
    {
        protected static ScreenFactory ScreenFactory = new AndroidScreenFactory();

        public override Screen EnterText(string elementName, string text)
        {
            var element = FindElement(elementName);

            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            //Appium clears it regardless of this being called or not.
            //element.Clear();
            element.Click();
            element.SendKeys(text);
            Driver.HideKeyboard();
            //Driver.Navigate().Back();
            return this;
        }

        public virtual bool HasLabel(string label, TextCompare compareType, int timeoutSecs = DefaultWaitSeconds)
        {
            return HasText(label, compareType, timeoutSecs) || HasContentDesc(label, compareType, timeoutSecs);
        }

        protected bool HasContentDesc(string label, TextCompare compareType, int timeoutSecs=DefaultWaitSeconds)
        {
            ReadOnlyCollection<IWebElement> texts = null;
            switch (compareType)
            {
                case TextCompare.Equals:
                    texts = Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().description(""" + label + @""")", timeoutSecs);
                    break;

                case TextCompare.StartsWith:
                    texts =
                        Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().descriptionStartsWith(""" + label + @""")", timeoutSecs);
                    break;

                case TextCompare.Matching:
                    texts = Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().descriptionMatches(""" + label + @""")", timeoutSecs);
                    break;

                case TextCompare.Containing:
                    texts = Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().descriptionContains(""" + label + @""")", timeoutSecs);
                    break;
                default:
                    throw new NotImplementedException("Not implemented compare type: " + compareType);
            }

            if (texts != null && texts.Count != 0)
                return true;

            return false;
        }

        protected bool HasText(string label, TextCompare compareType, int timeoutSecs = DefaultWaitSeconds)
        {
            ReadOnlyCollection<IWebElement> texts = null;
            switch (compareType)
            {
                case TextCompare.Equals:
                    texts = Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().text(""" + label + @""")", timeoutSecs);
                    break;

                case TextCompare.StartsWith:
                    texts =
                        Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().textStartsWith(""" + label + @""")", timeoutSecs);
                    break;

                case TextCompare.Matching:
                    texts = Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().textMatches(""" + label + @""")", timeoutSecs);
                    break;

                case TextCompare.Containing:
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
