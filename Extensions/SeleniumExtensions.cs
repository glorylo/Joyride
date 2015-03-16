using System;
using System.Collections.Generic;
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
        private static readonly Object ThisLock = new object();
        private static IWebElement FindElementWithTimeout(Func<IWebElement> func, int timeoutSecs)
        {
            IWebElement element = null;
            lock (ThisLock)
            {
                RemoteMobileDriver.SetTimeout(timeoutSecs);
                element = func();
                RemoteMobileDriver.SetDefaultWait();                
            }
            return element;
        }

        private static IList<IWebElement> FindElementsWithTimeout(Func<IList<IWebElement>> func, int timeoutSecs)
        {
            IList<IWebElement> elements = null;
            lock (ThisLock)
            {
                RemoteMobileDriver.SetTimeout(timeoutSecs);
                elements = func();
                RemoteMobileDriver.SetDefaultWait();
            }
            return elements;
        }

        public static void DoActionWithTimeout(this RemoteWebDriver driver, int timeoutSecs, Action action)
        {
            RemoteMobileDriver.SetTimeout(timeoutSecs);
            action();
            RemoteMobileDriver.SetDefaultWait();
        }

        public static IWebElement FindElement(this IWebElement parentElement, By by, int timeoutSecs)
        {
            return FindElementWithTimeout(() =>
            {                
                IWebElement element = null;            
                try {
                    element = parentElement.FindElement(by);    
                }
                catch (Exception) { return null; }                        
                return element;
            }, timeoutSecs);
        }

        public static IList<IWebElement> FindElements(this IWebElement parentElement, By by, int timeoutSecs)
        {
            return FindElementsWithTimeout(() =>
            {
                IList<IWebElement> elements = null;
                try  {
                    elements = parentElement.FindElements(by);
                }
                catch (Exception) { return null; }
                return elements;
            }, timeoutSecs);
        }

        public static IWebElement FindElement(this RemoteWebDriver driver, By by, int timeoutSecs)
        {
            return FindElementWithTimeout(() => driver.FindElementWithMethod(new Func<By, IWebElement>(driver.FindElement), by), timeoutSecs);
        }

        public static IList<IWebElement> FindElements(this RemoteWebDriver driver, By by, int timeoutSecs)
        {
            return FindElementsWithTimeout(() => driver.FindElementsWithMethod(new Func<By, IList<IWebElement>>(driver.FindElements), by), timeoutSecs);
        }

        public static IWebElement FindElementWithMethod(this RemoteWebDriver driver, Delegate findMethod,
            params object[] arguments)
        {
            IWebElement element = null;
            try {
                element = ScreenHelper.InvokeMethod(findMethod, arguments) as IWebElement;
            }
            catch (Exception) { return null; }
            return element;
        }

        public static IWebElement FindElementWithMethod(this RemoteWebDriver driver, int timeoutSecs,
            Delegate findMethod, params object[] args)
        {
            return FindElementWithTimeout(() => driver.FindElementWithMethod(findMethod, args), timeoutSecs);
        }

        public static IList<IWebElement> FindElementsWithMethod(this RemoteWebDriver driver, int timeoutSecs,
            Delegate findMethod, params object[] args)
        {
            return FindElementsWithTimeout(() => driver.FindElementsWithMethod(findMethod, args), timeoutSecs);
        }

        public static IList<IWebElement> FindElementsWithMethod(this RemoteWebDriver driver, Delegate findMethod,
            params object[] arguments)
        {
            IList<IWebElement> elements = null;
            try {
                elements = ScreenHelper.InvokeMethod(findMethod, arguments) as IList<IWebElement>;
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
            var foundElement = FindElementWithTimeout(() => driver.FindElementWithMethod(findMethod, arguments), timeoutSecs);
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
