
using System;
using System.Collections.Generic;
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
        
        public virtual void HideKeyboard()
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

         public override Screen SetCheckbox(string elementName, bool enabled = true)
         {
             var element = FindElement(elementName);

             if (element == null)
                 throw new NoSuchElementException("Cannot find element:  " + elementName);

             var isChecked = element.GetAttribute("checked");
             var selected = (isChecked != null && isChecked == "true");             
             if ((enabled && !selected) || (!enabled && selected))
                 element.Click();

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

        public virtual bool HasLabel(string label, CompareType compareType, int timeoutSecs)
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

        internal protected bool HasContentDesc(string label, CompareType compareType, int timeoutSecs)
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

        internal protected bool HasText(string label, CompareType compareType, int timeoutSecs)
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

        public override Screen Rotate(ScreenOrientation orientation)
        {
            base.Rotate(orientation);
            // allow time to render
            Driver.WaitFor(TimeSpan.FromMilliseconds(500));
            return this;
        }

    }
}
