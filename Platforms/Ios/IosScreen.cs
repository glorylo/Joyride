using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Joyride.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace Joyride.Platforms.Ios
{
    abstract public class IosScreen : Screen
    {
        protected static ScreenFactory ScreenFactory = new IosScreenFactory();

        public bool HasLabelContainingText(string labelText, int timeoutSecs = DefaultWaitSeconds)
        {
            var xpath = "//UIAStaticText[contains(@label,'" + labelText + "')]";
            var element = Driver.FindElement(By.XPath(xpath), timeoutSecs);
            return (element != null);
        }

        public virtual bool HasLabelContainingText(string collectionName, int index, string labelText, int timeoutSecs = DefaultWaitSeconds)
        {
            var element = GetElementInCollectionAt(collectionName, index);
            var texts = element.FindElements(By.ClassName("UIAStaticText"), timeoutSecs);

            if (texts.Count == 0)
                return false;

            var textWithLabelText = texts.FirstOrDefault(e => e.Text.Contains(labelText));
            return (textWithLabelText != null);
        }

        public void SetSlider(string elementName, int percentage)
        {
            if ((percentage < 0) || (percentage > 100))
                throw new IndexOutOfRangeException("Slider can only accept values 1-100.  Requested: " + percentage);

            var slider = FindElement(elementName);

            if (slider == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            var actualValue = (double)percentage / 100;
            slider.SendKeys(actualValue.ToString());
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


        public String TitleFromNavigationBar(int timeoutSecs = 5)
        {
            var xpath = "//UIANavigationBar[1]/UIAStaticText[1]";
            var element = Driver.FindElement(By.XPath(xpath), timeoutSecs);

            if (element == null)
                throw new NoSuchElementException("Unable to find the navigation bar with static text");

            return element.Text;
        }


        public void HideKeyboard()
        {
            var windowSize = Driver.ScreenSize();
            var pointBehindKeyboard = new Point(windowSize.Width / 2, windowSize.Height / 3);
            Driver.Tap(pointBehindKeyboard);
        }



    }
}
