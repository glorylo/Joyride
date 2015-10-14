using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.iOS;

namespace Joyride.Extensions
{
    public static class IosDriverExtension
    {
        public static IWebElement FindElementByIosUIAutomation(this IOSDriver driver, string selector, int timeoutSecs)
        {
            driver.SetTimeout(timeoutSecs);
            var element = driver.FindElementWithMethod(new Func<string, IWebElement>(driver.FindElementByIosUIAutomation), selector);
            driver.SetDefaultWait();
            return element;

        }

        public static IList<IWebElement> FindElementsByIosUIAutomation(this IOSDriver driver, string selector, int timeoutSecs)
        {
            driver.SetTimeout(timeoutSecs);
            var elements = driver.FindElementsWithMethod(new Func<string, IList<IWebElement>>(driver.FindElementsByIosUIAutomation), selector);
            driver.SetDefaultWait();
            return elements;
        }
    }
}
