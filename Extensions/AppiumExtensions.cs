using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.MultiTouch;

namespace Joyride.Extensions
{
    public static class AppiumExtensions
    {
        public const string NativeAppContext = "NATIVE_APP";

        public static void DoActionInWebView(this AppiumDriver driver, Action action, int maxRetries=3)
        {
            var contextCount = 1;
            var retries = 0;

            var contexts = driver.GetContexts();
            contextCount = contexts.Count;

            while ((contextCount == 1) && (retries <= maxRetries))
            {
                driver.WaitFor(TimeSpan.FromSeconds(2));
                contexts = driver.GetContexts();
                contextCount = contexts.Count;
                retries++;
                Trace.WriteLine("Unable to switch to web context.  Retries:  " + retries);
            }
            
            if (contextCount == 1)                
                throw new Exception("Unable to switch to webview");

            var webViewContext = contexts.Last();
            driver.SetContext(webViewContext);
            action();
            driver.SetContext(NativeAppContext);
        }


        public static void Tap(this AppiumDriver driver, Point location)
        {
            new TouchAction(driver).Press(location.X, location.Y).Perform();
        }

        public static void PreciseTap(this AppiumDriver driver, IWebElement element)
        {
            driver.Tap(element.GetCenter());
        }

        //TODO:  does not work.  Appears to be an appium bug.
        public static void DoubleTap(this AppiumDriver driver, Point point)
        {

/*
            new TouchAction(driver)
                .Press(point.X, point.Y)
                .Wait(200)
                .Release()
                .Wait(300)
                .Press(0, 0)
                .Wait(200)
                .Release()
                .Perform();
 */

            new TouchAction(driver)
                .Tap(point.X, point.Y, count: 2)
                .Perform(); 
         }
        public static void DoubleTap(this AppiumDriver driver, IWebElement element)
        {
            driver.DoubleTap(element.GetCenter());
        }
        
        //TODO: does not work
        public static void ScrollToVisible(this AppiumDriver driver, IWebElement element)
        {
            //var scrollLength = 0;
            //if (element.Location.Y < 0)
            //    scrollLength = RemoteDriver.ScreenSize.Height + (-1*element.Location.Y);

            var args = new Dictionary<string, string>
            {
                {"direction", "up"},
                {"element", element.GetIdForElement()}
            };
            driver.ExecuteScript("mobile: scrollTo", args);   
        }

        public static void TapAndHold(this AppiumDriver driver, IWebElement element, int seconds)
        {
            var center = element.GetCenter();
            new TouchAction(driver)
                .Press(center.X, center.Y)
                .Wait(seconds * 1000)
                .Release()
                .Perform();           
        }

        public static void Swipe(this AppiumDriver driver, Direction direction, Size dimension,
            long durationMilliSecs = 500, int originX=0, int originY=0)
        {
            var center = new Point(originX + dimension.Width / 2, originY + dimension.Height / 2);
            var startDeltaX = dimension.Width / 5;
            var endDeltaX = dimension.Width / 2;
            var startDeltaY = dimension.Height / 8;
            var endDeltaY = dimension.Height / 2;

            switch (direction)
            {
                case Direction.Left:
                    endDeltaX *= -1;
                    startDeltaY = endDeltaY = 0;
                    break;

                case Direction.Right:
                    startDeltaX *= -1;
                    startDeltaY = endDeltaY = 0;
                    break;

                case Direction.Up:
                    endDeltaY *= -1;
                    startDeltaX = endDeltaX = 0;
                    break;

                case Direction.Down:
                    startDeltaY *= -1;
                    startDeltaX = endDeltaX = 0;
                    break;

                default:
                    throw new InvalidEnumArgumentException("Invalid direction:  " + direction);
            }

            new TouchAction(driver)
                .Press(center.X + startDeltaX, center.Y + startDeltaY)
                .Wait(durationMilliSecs)
                .MoveTo(center.X + endDeltaX, center.Y + endDeltaY)
                .Release().Perform();
        }

        public static void Swipe(this AppiumDriver driver, Direction direction, long durationMilliSecs=500)
        {
            Swipe(driver, direction, RemoteMobileDriver.ScreenSize, durationMilliSecs);
        }

        public static void Swipe(this AppiumDriver driver, IWebElement element, Direction direction, long durationMilliSecs = 500)
        {
            var size = CalculateVisibleElementSize(element);
            Swipe(driver, direction, size, durationMilliSecs, element.Location.X, element.Location.Y);
        }

        private static Size CalculateVisibleElementSize(IWebElement element)
        {
            var size = element.Size;
            var lowerRight = new Point(element.Location.X + size.Width, element.Location.Y + size.Height);
            var windowSize = RemoteMobileDriver.ScreenSize;

            if (lowerRight.X > windowSize.Width || lowerRight.Y > windowSize.Height)
                size = new Size(windowSize.Width - element.Location.X, windowSize.Height - element.Location.Y);
            return size;
        }

        public static void PinchToZoom(this AppiumDriver driver, Direction direction, double scale = 1.0)
        {
            if (scale > 1.0 || scale < 0)
                throw new ArgumentOutOfRangeException("Zoom only scales to 0.0 - 1.0.  Scale of " + scale + " is out of range.");
            
            var windowSize = RemoteMobileDriver.ScreenSize;
            var windowCenter = new Point(windowSize.Width / 2, windowSize.Height / 2);
            var upperLeft = new Point(windowCenter.X - windowSize.Width / 4, windowCenter.Y - windowSize.Height / 4);
            var bottomRight = new Point(windowCenter.X + windowSize.Width / 4, windowCenter.Y + windowSize.Height / 4);
            int deltaX, deltaY;
            switch (direction)
            {
                case Direction.In:
                    deltaX = (int) (-1*scale*upperLeft.X);
                    deltaY = (int)(-1*scale*upperLeft.Y);
                    break;
                case Direction.Out:
                    deltaX = (int)(scale * windowCenter.X - upperLeft.X);
                    deltaY = (int)(scale * bottomRight.Y - windowCenter.Y);
                    break;
                default:
                    throw new InvalidEnumArgumentException("Invalid direction:  " + direction);
            }

            var finger1 = new TouchAction(driver)
                .Press(upperLeft.X, upperLeft.Y)
                .Wait(1000)
                .MoveTo(deltaX, deltaY)
                .Wait(1000)
                .Release();

            var finger2 = new TouchAction(driver)
                .Press(bottomRight.X, bottomRight.Y)
                .Wait(1000)
                .MoveTo((-1) * deltaX, (-1) * deltaY)
                .Wait(1000)
                .Release();

            var zoomAction = new MultiAction(driver).Add(finger1).Add(finger2);
            zoomAction.Perform();
        }
        private static Direction ConvertDirectionToSwipe(Direction direction)
        {
            Direction directionToSwipe;
            switch (direction)
            {
                case Direction.Left:
                    directionToSwipe = Direction.Right;
                    break;
                case Direction.Right:
                    directionToSwipe = Direction.Left;
                    break;

                case Direction.Up:
                    directionToSwipe = Direction.Down;
                    break;
                case Direction.Down:
                    directionToSwipe = Direction.Up;
                    break;
                default:
                    throw new InvalidEnumArgumentException("Invalid direction:  " + direction);
            }
            return directionToSwipe;
        }

        public static void Scroll(this AppiumDriver driver, Direction direction, long durationMilliSecs = 500)
        {
            var directionToSwipe = ConvertDirectionToSwipe(direction);
            driver.Swipe(directionToSwipe, durationMilliSecs);
        }

        public static void Scroll(this AppiumDriver driver, IWebElement element, Direction direction, long durationMilliSecs = 500)
        {
            var directionToSwipe = ConvertDirectionToSwipe(direction);
            driver.Swipe(element, directionToSwipe, durationMilliSecs);
        }

    }
}
