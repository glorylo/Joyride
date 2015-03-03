using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;

namespace Joyride.Extensions
{
    public static class AndroidDriverExtension
    {
        public static IWebElement FindElementByAndroidUIAutomator(this AndroidDriver driver, string selector, int timeoutSecs)
        {
            RemoteMobileDriver.SetTimeout(timeoutSecs);
            var element = driver.FindElementWithMethod(new Func<string, IWebElement>(driver.FindElementByAndroidUIAutomator), selector);
            RemoteMobileDriver.SetDefaultWait();
            return element;
        }

        public static ReadOnlyCollection<IWebElement> FindElementsByAndroidUIAutomator(this AndroidDriver driver, string selector, int timeoutSecs)
        {
            RemoteMobileDriver.SetTimeout(timeoutSecs);
            var elements = driver.FindElementsWithMethod(new Func<string, ReadOnlyCollection<IWebElement>>(driver.FindElementsByAndroidUIAutomator), selector);
            RemoteMobileDriver.SetDefaultWait();
            return elements;
        }
    }
}
