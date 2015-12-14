using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.iOS;

namespace Joyride.Extensions
{
    public static class IosDriverExtension
    {
        public static T FindElementByIosUIAutomation<T>(this IOSDriver<T> driver, string selector, int timeoutSecs) where T : IWebElement
        {
            driver.SetTimeout(timeoutSecs);
            var element = driver.FindElementWithMethod(new Func<string, T>(driver.FindElementByIosUIAutomation), selector);
            driver.SetDefaultWait();
            return (T) element;

        }

        public static IList<T> FindElementsByIosUIAutomation<T>(this IOSDriver<T> driver, string selector, int timeoutSecs) where T : IWebElement
        {
            driver.SetTimeout(timeoutSecs);
            var elements = driver.FindElementsWithMethod(new Func<string, IList<T>>(driver.FindElementsByIosUIAutomation), selector);
            driver.SetDefaultWait();
            return (IList<T>) elements;
        }
    }
}
