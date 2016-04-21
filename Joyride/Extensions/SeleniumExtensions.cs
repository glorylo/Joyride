using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Remote;
using Joyride.Support;


namespace Joyride.Extensions
{
    public static class SeleniumExtensions
    {
        // not thread safe!
        private static int _commandTimeOutSeconds = RemoteMobileDriver.DefaultWaitSeconds;
        public static int DefaultWaitSeconds = RemoteMobileDriver.DefaultWaitSeconds;

        public static void SetTimeout(this RemoteWebDriver driver, int seconds)
        {
            if (seconds != _commandTimeOutSeconds)
            {
                _commandTimeOutSeconds = seconds;
                driver.SetImplicitWait(TimeSpan.FromSeconds(seconds));
            }
        }

        public static void SetDefaultWait(this RemoteWebDriver driver)
        {
            driver.SetTimeout(DefaultWaitSeconds);
        }

        public static void SetImplicitWait(this IWebDriver driver, TimeSpan span)
        {
            try {
                driver.Manage().Timeouts().ImplicitlyWait(span);
            }
            // suppress errors for now 
            catch {
                Trace.WriteLine("Unable to set timeout to:  " + span);
            } 
        }
                
        private static IWebElement FindElementWithTimeout(RemoteWebDriver driver, Func<IWebElement> func, int timeoutSecs)
        {

            //TODO: Would like to swap out with WebDriverWait
/*            IWebElement element = null;
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSecs));
            try
            {
                element = wait.Until((d) => func());
            }
            catch
            {
                return null;
            }
            return element;
*/            
            driver.SetTimeout(timeoutSecs);
            var element = func();
            driver.SetDefaultWait();                            
            return element;
        }

        private static IList<IWebElement> FindElementsWithTimeout(RemoteWebDriver driver, Func<IList<IWebElement>> func, int timeoutSecs)
        {
            //TODO: Would like to swap out with WebDriverWait
            driver.SetTimeout(timeoutSecs);
            var elements = func();
            driver.SetDefaultWait();            
            return elements;
        }

        public static void DoActionWithTimeout(this RemoteWebDriver driver, int timeoutSecs, Action action)
        {
            //TODO: Would like to swap out with WebDriverWait
            driver.SetTimeout(timeoutSecs);
            action();
            driver.SetDefaultWait();            
        }
        
        public static IWebElement FindElement(this RemoteWebDriver driver, By by, int timeoutSecs)
        {
            return FindElementWithTimeout(driver, () => driver.FindElementWithMethod(new Func<By, IWebElement>(driver.FindElement), by), timeoutSecs);
        }

        public static IList<IWebElement> FindElements(this RemoteWebDriver driver, By by, int timeoutSecs)
        {
            return FindElementsWithTimeout(driver, () => driver.FindElementsWithMethod(new Func<By, IList<IWebElement>>(driver.FindElements), by), timeoutSecs);
        }

        public static IWebElement FindElementWithImplicitWait(this RemoteWebDriver driver, By by)
        {
            return driver.FindElementWithMethod(new Func<By, IWebElement>(driver.FindElement), by);
        }
        public static IList<IWebElement> FindElementsWithImplicitWait(this RemoteWebDriver driver, By by)
        {
            return driver.FindElementsWithMethod(new Func<By, IWebElement>(driver.FindElement), by);
        }

        public static IWebElement FindElementWithMethod(this RemoteWebDriver driver, Delegate findMethod,
            params object[] arguments)
        {
            IWebElement element;
            try {
                element = Util.InvokeMethod(findMethod, arguments) as IWebElement;
            }
            catch (Exception) { return null; }
            return element;
        }

        public static IWebElement FindElementWithMethod(this RemoteWebDriver driver, int timeoutSecs,
            Delegate findMethod, params object[] args)
        {
            return FindElementWithTimeout(driver, () => driver.FindElementWithMethod(findMethod, args), timeoutSecs);
        }

        public static IList<IWebElement> FindElementsWithMethod(this RemoteWebDriver driver, int timeoutSecs,
            Delegate findMethod, params object[] args)
        {
            return FindElementsWithTimeout(driver, () => driver.FindElementsWithMethod(findMethod, args), timeoutSecs);
        }

        public static IList<IWebElement> FindElementsWithMethod(this RemoteWebDriver driver, Delegate findMethod,
            params object[] arguments)
        {
            IList<IWebElement> elements;
            try {
                elements = Util.InvokeMethod(findMethod, arguments) as IList<IWebElement>;
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
            var foundElement = FindElementWithTimeout(driver, () => driver.FindElementWithMethod(findMethod, arguments), timeoutSecs);
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
            var upperLeft = element.Location;
            var size = element.Size;
            int x = upperLeft.X + size.Width/2;
            int y = upperLeft.Y + size.Height/2;
            return new Point(x, y);
        }

        public static IWebElement Unpack(this IWebElement element)
        {
            if (element == null)
                return null;

            var wrapsElement = element as IWrapsElement;
            if (wrapsElement != null)
                return wrapsElement.WrappedElement;

            return element;
        }

    }
}
