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

        protected IWebElement FindModalDialog(string text = null, bool title = true, int timeoutSecs = 5)
        {
            const string xpath = "//UIAAlert[@visible='true']";

            if (text == null)
                return Driver.FindElement(By.XPath(xpath), timeoutSecs);

            string xpathContainsText;

            //TODO: For alerts the name attribute is actually populated but not the label.  
            if (title)
                xpathContainsText =
                    "//UIAAlert[@visible='true' and contains(@name, '" + text + "')]";
            else
                xpathContainsText = "//UIAAlert[@visible='true']//UIAStaticText[contains(@label, '" + text + "')]";

            return Driver.FindElement(By.XPath(xpathContainsText), timeoutSecs);
        }

        public virtual Screen AcceptModalDialog(bool accept = true, bool title = true, string alertText = null, int timeoutSecs = 5)
        {
            var dialog = FindModalDialog(alertText, title, timeoutSecs: timeoutSecs);

            if (dialog != null)
            {
                if (accept)
                    Driver.SwitchTo().Alert().Accept();
                else
                    Driver.SwitchTo().Alert().Dismiss();
            }
            return this;
        }

        public String TitleFromNavigationBar(int timeoutSecs = 5)
        {
            var xpath = "//UIANavigationBar[1]/UIAStaticText[1]";
            var element = Driver.FindElement(By.XPath(xpath), timeoutSecs);

            if (element == null)
                throw new NoSuchElementException("Unable to find the navigation bar with static text");

            return element.Text;
        }

        public bool HasModalDialog(string dialogText, bool title = true, int timeOutSecs = 5)
        {
            return (FindModalDialog(dialogText, title, timeOutSecs) != null);
        }


        public void HideKeyboard()
        {
            var windowSize = Driver.ScreenSize();
            var pointBehindKeyboard = new Point(windowSize.Width / 2, windowSize.Height / 3);
            Driver.Tap(pointBehindKeyboard);
        }

        protected string BuildTableCellXpath(string text, TextCompare compare, string parentElement = null)
        {
            string parentXpath = "";
            if (parentElement != null)
            {
                parentXpath = GetElementXPathSelector(parentElement);

                if (parentXpath == null)
                    throw new NoSuchElementException("Unable to find parent element: " + parentElement);
            }

            var xpath = "(" + parentXpath + "//UIATableCell/UIAStaticText[";
            switch (compare)
            {
                case TextCompare.StartsWith:
                    xpath = xpath + "starts-with(@name, '" + text + "')])[1]/..";
                    break;

                case TextCompare.Containing:
                    xpath = xpath + "contains(@name, '" + text + "')])[1]/..";
                    break;
                case TextCompare.Equals:
                    xpath = xpath + "@name='" + text + "'])[1]/..";
                    break;

                default:
                    throw new NotImplementedException("Other text compares are not implemented");
            }
            return xpath;
        }

        public virtual Screen TapTableCellWithText(string text, TextCompare compare, bool precise = true, string parentElement = null, int timeoutSecs = DefaultWaitSeconds)
        {
            var xpath = BuildTableCellXpath(text, compare, parentElement);
            var tableCell = Driver.FindElement(By.XPath(xpath), timeoutSecs);

            if (tableCell == null)
                throw new NotFoundException("Unable to find table cell with :  " + text);

            if (precise)
                Driver.PreciseTap(tableCell);
            else
                tableCell.Click();

            return this;
        }

        public virtual void ScrollToTableCellWithText(Direction direction, string text, TextCompare compare, int maxRetries = 30, string parentElement = null)
        {
            var xpath = BuildTableCellXpath(text, compare, parentElement);

            var element = Driver.FindElement(By.XPath(xpath), 3);


            if (element.IsPresent() && element.Displayed)
                return;

            var numRetries = 0;

            while (numRetries <= maxRetries)
            {
                Driver.Scroll(direction);
                element = Driver.FindElement(By.XPath(xpath), 3);
                if (element.IsPresent() && element.Displayed)
                    return;

                numRetries++;
            }
            throw new NoSuchElementException("Unable to find table cell with xpath: " + xpath);
        }


    }
}
