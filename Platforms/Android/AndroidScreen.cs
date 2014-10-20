
using System;
using System.Collections.ObjectModel;
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

        public virtual bool HasLabel(string label, TextCompare compareType)
        {
            return HasText(label, compareType) || HasContentDesc(label, compareType);
        }

        protected bool HasContentDesc(string label, TextCompare compareType)
        {
            ReadOnlyCollection<IWebElement> texts = null;
            switch (compareType)
            {
                case TextCompare.Equals:
                    texts = Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().description(""" + label + @""")");
                    break;

                case TextCompare.StartsWith:
                    texts =
                        Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().descriptionStartsWith(""" + label + @""")");
                    break;

                case TextCompare.Matching:
                    texts = Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().descriptionMatches(""" + label + @""")");
                    break;

                case TextCompare.Containing:
                    texts = Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().descriptionContains(""" + label + @""")");
                    break;
                default:
                    throw new NotImplementedException("Not implemented compare type: " + compareType);
            }

            if (texts != null && texts.Count != 0)
                return true;

            return false;
        }

        protected bool HasText(string label, TextCompare compareType)
        {
            ReadOnlyCollection<IWebElement> texts = null;
            switch (compareType)
            {
                case TextCompare.Equals:
                    texts = Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().text(""" + label + @""")");
                    break;

                case TextCompare.StartsWith:
                    texts =
                        Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().textStartsWith(""" + label + @""")");
                    break;

                case TextCompare.Matching:
                    texts = Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().textMatches(""" + label + @""")");
                    break;

                case TextCompare.Containing:
                    texts = Driver.FindElementsByAndroidUIAutomator(@"new UiSelector().textContains(""" + label + @""")");
                    break;
                default:
                    throw new NotImplementedException("Not implemented compare type: " + compareType);
            }

            if (texts != null && texts.Count != 0)
                return true;

            return false;
        }

    }
}
