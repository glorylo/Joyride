using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;

namespace Joyride.Extensions
{
    public static class AndroidDriverExtension
    {
        public static IWebElement FindElementByAndroidUIAutomator(this AndroidDriver driver, string selector, int timeoutSecs)
        {
            driver.SetTimeout(timeoutSecs);
            var element = driver.FindElementWithMethod(new Func<string, IWebElement>(driver.FindElementByAndroidUIAutomator), selector);
            driver.SetDefaultWait();
            return element;
        }

        public static IList<IWebElement> FindElementsByAndroidUIAutomator(this AndroidDriver driver, string selector, int timeoutSecs)
        {
            driver.SetTimeout(timeoutSecs);
            var elements = driver.FindElementsWithMethod(new Func<string, IList<IWebElement>>(driver.FindElementsByAndroidUIAutomator), selector);
            driver.SetDefaultWait();
            return elements;
        }
    }
}
