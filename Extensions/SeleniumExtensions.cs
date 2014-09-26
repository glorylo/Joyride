using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Reflection;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Joyride.Support;

namespace Joyride.Extensions
{
    public static class SeleniumExtensions
    {
        public static void DoActionWithTimeout(this RemoteWebDriver driver, int timeoutSecs, Action action)
        {
            RemoteDriver.SetTimeout(timeoutSecs);
            action();
            RemoteDriver.SetDefaultWait();
        }

        public static IWebElement FindElement(this IWebElement parentElement, By by, int timeoutSecs)
        {
            IWebElement element = null;
            RemoteDriver.SetTimeout(timeoutSecs);
            try {
                element = parentElement.FindElement(by);    
            }
            catch (Exception) { return null; }
            
            RemoteDriver.SetDefaultWait();
            return element;
        }

        public static ReadOnlyCollection<IWebElement> FindElements(this IWebElement parentElement, By by, int timeoutSecs)
        {
            ReadOnlyCollection<IWebElement> elements = null;
            RemoteDriver.SetTimeout(timeoutSecs);
            try
            {
                elements = parentElement.FindElements(by);
            }
            catch (Exception) { return null; }

            RemoteDriver.SetDefaultWait();
            return elements;
        }

        public static IWebElement FindElement(this RemoteWebDriver driver, By by, int timeoutSecs)
        {
            RemoteDriver.SetTimeout(timeoutSecs);
            var element = driver.FindElementWithMethod(new Func<By, IWebElement>(driver.FindElement), by);
            RemoteDriver.SetDefaultWait();
            return element;
        }

        public static ReadOnlyCollection<IWebElement> FindElements(this RemoteWebDriver driver, By by, int timeoutSecs)
        {
            RemoteDriver.SetTimeout(timeoutSecs);
            var elements = driver.FindElementsWithMethod(new Func<By, ReadOnlyCollection<IWebElement>>(driver.FindElements), by);
            RemoteDriver.SetDefaultWait();
            return elements;
        }

        public static IWebElement FindElementWithMethod(this RemoteWebDriver driver, Delegate findMethod,
            params object[] arguments)
        {
            IWebElement element = null;
            try
            {
                element = ScreenHelper.InvokeMethod(findMethod, arguments) as IWebElement;
            }
            catch (Exception) { return null; }
            return element;
        }

        public static IWebElement FindElementWithMethod(this RemoteWebDriver driver, int timeoutSecs,
            Delegate findMethod, params object[] args)
        {
            RemoteDriver.SetTimeout(timeoutSecs);
            var element = driver.FindElementWithMethod(findMethod, args);
            RemoteDriver.SetDefaultWait();
            return element;
        }

        public static ReadOnlyCollection<IWebElement> FindElementsWithMethod(this RemoteWebDriver driver, int timeoutSecs,
            Delegate findMethod, params object[] args)
        {
            RemoteDriver.SetTimeout(timeoutSecs);
            var elements = driver.FindElementsWithMethod(findMethod, args);
            RemoteDriver.SetDefaultWait();
            return elements;
        }

        public static ReadOnlyCollection<IWebElement> FindElementsWithMethod(this RemoteWebDriver driver, Delegate findMethod,
            params object[] arguments)
        {
            ReadOnlyCollection<IWebElement> elements = null;
            try {
                elements = ScreenHelper.InvokeMethod(findMethod, arguments) as ReadOnlyCollection<IWebElement>;
            }
            catch (Exception) { return null; }
            return elements;
        }

        public static bool ElementExists(this RemoteWebDriver driver, Delegate findMethod, params object[] arguments)
        {
            var element = driver.FindElementWithMethod(findMethod, arguments);
            return (element != null);
        }

        public static bool ElementExists(this RemoteWebDriver driver, int timeoutSecs, Delegate findMethod,
            params object[] arguments)
        {
            RemoteDriver.SetTimeout(timeoutSecs);
            var foundElement = driver.FindElementWithMethod(findMethod, arguments);
            RemoteDriver.SetDefaultWait();
            return (foundElement != null);
        }

        public static bool IsPresent(this IWebElement element)
        {
            bool foundElement;
            try {
                foundElement = (element != null && !element.Location.IsEmpty);
            }
            catch (Exception) {
                return false;
            }
            return foundElement;
        }

        public static void WaitFor(this RemoteWebDriver driver, TimeSpan timeSpan)
        {
            Thread.Sleep(timeSpan);
        }

        public static Point GetCenter(this IWebElement element)
        {
            Point upperLeft = element.Location;
            var size = element.Size;
            int x = upperLeft.X + size.Width/2;
            int y = upperLeft.Y + size.Height/2;
            return new Point(x, y);
        }

        public static string GetIdForElement(this IWebElement element)
        {
            var fieldInfo = typeof (RemoteWebElement).GetField("elementId",
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
                return (string) fieldInfo.GetValue(element);
            return null;
        }
    }
}
