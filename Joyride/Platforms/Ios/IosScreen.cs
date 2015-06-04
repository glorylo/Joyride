using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Joyride.Extensions;
using OpenQA.Selenium;
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

        public override Screen EnterText(string elementName, string text)
        {
            var element = FindElement(elementName);
            if (element == null)
                throw new NoSuchElementException("Unable to find element " + elementName);

            element.SendKeys(text);
            return this;
        }

        //TODO: needs to be tested with ios step in joyride.specflow
        public virtual bool HasLabelInCollection(string collectionName, string label, CompareType compareType)
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
            var tuple = FindElementWithinCollection(collectionName, xpath);
            return (tuple != null);
        }


        //TODO: would like to rewrite using text compare
        [Obsolete]
        public bool HasLabelContainingText(string labelText, int timeoutSecs = DefaultWaitSeconds)
        {
            var xpath = "//UIAStaticText[contains(@label,'" + labelText + "')]";
            var element = Driver.FindElement(By.XPath(xpath), timeoutSecs);
            return (element != null);
        }

        //TODO: would like to write this using the collection abilities
        [Obsolete]
        public virtual bool HasLabelContainingText(string collectionName, int index, string labelText, int timeoutSecs = DefaultWaitSeconds)
        {
            var element = GetElementInCollection(collectionName, index);
            var texts = element.FindElements(By.ClassName("UIAStaticText"), timeoutSecs);

            if (texts.Count == 0)
                return false;

            var textWithLabelText = texts.FirstOrDefault(e => e.Text.Contains(labelText));
            return (textWithLabelText != null);
        }

        public Screen DragSlider(string elementName, int percentage)
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

        public int CurrentPageOnIndictator(string elementName)
        {
            var value = GetElementAttribute(elementName, "value");
            if (value == null)
                throw new NoSuchElementException("Unable to get value for element:  " + elementName);

            var match = Regex.Match(value, @"page (?<current>\d+) of (?<max>\d+)");
            if (match.Success)
                return Int32.Parse(match.Groups["current"].Value);

            throw new Exception("Unable to extra current page from:  " + value);
        }

        public bool HasNavigationBarTitled(string title, int timeoutSecs = DefaultWaitSeconds)
        {
            var xpath = "//UIANavigationBar[1]/UIAStaticText[@label='" + title + "']";
            var element = Driver.FindElement(By.XPath(xpath), timeoutSecs);
            return (element != null);
        }


        public String TitleFromNavigationBar(int timeoutSecs)
        {
            const string xpath = "//UIANavigationBar[1]/UIAStaticText[1]";
            var element = Driver.FindElement(By.XPath(xpath), timeoutSecs);

            if (element == null)
                throw new NoSuchElementException("Unable to find the navigation bar with static text");

            return element.Text;
        }


        public virtual void HideKeyboard()
        {
            var windowSize = Driver.ScreenSize();
            var pointBehindKeyboard = new Point(windowSize.Width / 2, windowSize.Height / 3);
            Driver.Tap(pointBehindKeyboard);
        }

    }
}
