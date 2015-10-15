using System;
using System.Drawing;
using System.Text.RegularExpressions;
using Joyride.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;

namespace Joyride.Platforms.Ios
{
    abstract public class IosScreen : Screen
    {
        protected static ScreenFactory ScreenFactory = new IosScreenFactory();
        protected static new IOSDriver Driver = (IOSDriver) RemoteMobileDriver.GetInstance();

        public override Screen TapAndHold(string elementName, int seconds)
        {
            var element = FindElement(elementName);

            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            Driver.TapAndHold(element, seconds, true);
            return this;
        }

        protected Screen EnterTextBySendKeys(string elementName, string text)
        {
            var element = FindElement(elementName);
            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            element.Click();
            element.Clear();
            element.SendKeys(text);
            return this;
        }  

        protected Screen EnterTextBySetValue(string elementName, string text)
        {
            var element = FindElement(elementName) as AppiumWebElement;
            if (element == null)
                throw new NoSuchElementException("Unable to find element " + elementName);

            element.SetImmediateValue(text);
            return this;
        }

        // TODO: Not needed but may need to change
        public override Screen EnterText(string elementName, string text)
        {
            base.EnterText(elementName, text);
            return this;
        }
        
        //TODO: needs to be tested with ios step in joyride.specflow
        public virtual bool HasLabelInCollection(string collectionName, string label, CompareType compareType, int timeoutSecs)
        {
            var xpath = "//*";
            switch (compareType)
            {
                case CompareType.StartsWith:
                    xpath += "[starts-with(@label, '" + label + "')]";
                    break;

                case CompareType.Containing:
                    xpath += "[contains(@label, '" + label + "')]";
                    break;

                case CompareType.Equals:
                    xpath += "[@label='" + label + "']";
                    break;

                default:
                    throw new NotImplementedException("Other text compares are not implemented");
            }
            var tuple = FindElementWithinCollection(collectionName, xpath, timeoutSecs);
            return (tuple != null);
        }

        public virtual Screen DragSlider(string elementName, int percentage)
        {
            if ((percentage < 0) || (percentage > 100))
                throw new IndexOutOfRangeException("Slider can only accept values 1-100.  Requested: " + percentage);

            var slider = FindElement(elementName);

            if (slider == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            var actualValue = (double)percentage / 100;
            slider.SendKeys(actualValue.ToString());
            return this;
        }

        public virtual int CurrentPageOnIndictator(string elementName)
        {
            var value = GetElementAttribute(elementName, "value");
            if (value == null)
                throw new NoSuchElementException("Unable to get value for element:  " + elementName);

            var match = Regex.Match(value, @"page (?<current>\d+) of (?<max>\d+)");
            if (match.Success)
                return Int32.Parse(match.Groups["current"].Value);

            throw new Exception("Unable to extra current page from:  " + value);
        }

        public virtual bool HasNavigationBarTitled(string title, int timeoutSecs)
        {
            var xpath = "//UIANavigationBar[1]/UIAStaticText[@label='" + title + "']";
            var element = Driver.FindElement(By.XPath(xpath), timeoutSecs);
            return (element != null);
        }


        public virtual String TitleFromNavigationBar(int timeoutSecs)
        {
            const string xpath = "//UIANavigationBar[1]/UIAStaticText[1]";
            var element = Driver.FindElement(By.XPath(xpath), timeoutSecs);

            if (element == null)
                throw new NoSuchElementException("Unable to find the navigation bar with static text");

            return element.Text;
        }

        // TODO: Not reliable.  
        public virtual void HideKeyboard()
        {
            var windowSize = Driver.ScreenSize();
            var pointBehindKeyboard = new Point(windowSize.Width / 2, windowSize.Height / 3);
            Driver.Tap(pointBehindKeyboard);
        }

        public virtual bool HasLabel(string text, CompareType compareType, int timeoutSecs)
        {
            var compareStr = "";
            switch (compareType)
            {
                case CompareType.StartsWith:
                    compareStr = "BEGINSWITH";
                    break;

                case CompareType.EndsWith:
                    compareStr = "ENDSWITH";
                    break;

                case CompareType.Equals:
                    compareStr = "==";
                    break;

                case CompareType.Matching:
                    compareStr = "MATCHES";
                    break;

                case CompareType.Containing:
                    compareStr = "CONTAINS";
                    break;

                default:
                    throw new ArgumentException("Unsupported compare type: " + compareType);
            }

            var escapedText = text.Replace(@"\", "")   // do not allow backslash chars
                              .Replace(@"'", @"\\'")   // replace single quotes with escaped backslash + single quote
                              .Replace(@"""", @"\"""); // replace with unescaped backslash (due to delimited by single quotes) + double quote
            var selector = String.Format(@".getFirstWithPredicate(""ANY staticTexts.label {0} '{1}'"")", compareStr, escapedText);

            var element = Driver.FindElementByIosUIAutomation(selector, timeoutSecs);
            return element != null;
        }
   

    }
}
