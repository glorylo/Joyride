using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Support.Extensions;

namespace Joyride.Extensions
{
    public static class AppiumExtensions
    {
        public const string NativeAppContext = "NATIVE_APP";        

        //cache screen size for performance
        private static Size? _screenSize;

        public static Size ScreenSize(this AppiumDriver driver)
        {
            var size = _screenSize ?? driver.Manage().Window.Size;
            _screenSize = size;
            return (Size) _screenSize;
        }

        public static Point ScreenCenterPoint(this AppiumDriver driver)
        {
            var centerX = driver.ScreenSize().Width/2;
            var centerY = driver.ScreenSize().Height/2;
            return new Point(centerX, centerY);
        }

        public static void DoActionInWebView(this AppiumDriver driver, Action action, int maxRetries=3)
        {
            var contextCount = 1;
            var retries = 0;

            var contexts = driver.Contexts;
            contextCount = contexts.Count;

            while ((contextCount == 1) && (retries <= maxRetries))
            {
                driver.WaitFor(TimeSpan.FromSeconds(2));
                contexts = driver.Contexts;
                contextCount = contexts.Count;
                retries++;
                Trace.WriteLine("Unable to switch to web context.  Retries:  " + retries);
            }
            
            if (contextCount == 1)                
                throw new Exception("Unable to switch to webview");

            var webViewContext = contexts.Last();
            driver.Context  = webViewContext;
            action();
            driver.Context = NativeAppContext;
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

        public static void TapAndHold(this AppiumDriver driver, IWebElement element, int seconds, bool precise=false)
        {
            if (precise)
            {
                var center = element.GetCenter();
                new TouchAction(driver)
                    .Press(center.X, center.Y)
                    .Wait(seconds*1000)
                    .Release()
                    .Perform();
            }
            else
            {
                new TouchAction(driver)
                    .Press(element)
                    .Wait(seconds*1000)
                    .Release()
                    .Perform();
            }
        }

        public static void Swipe(this AppiumDriver driver, Direction direction, Size dimension,
             double scale=1.0, long durationMilliSecs = 500, int originX=0, int originY=0)
        {
            var center = new Point(originX + dimension.Width / 2, originY + dimension.Height / 2);
            var startDeltaX = dimension.Width / 5;
            var endDeltaX = dimension.Width / 2;
            var startDeltaY = dimension.Height / 8;
            var endDeltaY = dimension.Height / 2;

            switch (direction)
            {
                case Direction.Left:
                    endDeltaX = (int) (endDeltaX  * -1 * scale);
                    startDeltaY = endDeltaY = 0;
                    break;

                case Direction.Right:
                    startDeltaX = (int) (startDeltaX * -1 * scale);
                    startDeltaY = endDeltaY = 0;
                    break;

                case Direction.Up:
                    endDeltaY = (int) (endDeltaY  * -1 * scale);
                    startDeltaX = endDeltaX = 0;
                    break;

                case Direction.Down:
                    startDeltaY = (int) (startDeltaY * -1 * scale);
                    startDeltaX = endDeltaX = 0;
                    break;

                default:
                    throw new InvalidEnumArgumentException("Invalid direction:  " + direction);
            }

            var startX = center.X + startDeltaX;
            var startY = center.Y + startDeltaY;
            var endX = startX + endDeltaX;
            var endY = startY + endDeltaY;
            new TouchAction(driver)
                .Press(startX, startY)
                .Wait(durationMilliSecs)
                .MoveTo(endX, endY)
                .Release().Perform();
        }

        public static void Swipe(this AppiumDriver driver, Direction direction, double scale=1.0, long durationMilliSecs=500)
        {
            Swipe(driver, direction, driver.ScreenSize(), scale, durationMilliSecs);
        }

        public static void Swipe(this AppiumDriver driver, IWebElement element, Direction direction, double scale = 1.0, long durationMilliSecs = 500)
        {
            var size = CalculateVisibleElementSize(driver, element);
            Swipe(driver, direction, size, scale, durationMilliSecs, element.Location.X, element.Location.Y);
        }

        private static Size CalculateVisibleElementSize(AppiumDriver driver, IWebElement element)
        {
            var size = element.Size;
            var lowerRight = new Point(element.Location.X + size.Width, element.Location.Y + size.Height);
            var windowSize = driver.ScreenSize();

            if (lowerRight.X > windowSize.Width || lowerRight.Y > windowSize.Height)
                size = new Size(windowSize.Width - element.Location.X, windowSize.Height - element.Location.Y);
            return size;
        }

        public static void PinchToZoom(this AppiumDriver driver, Direction direction, double scale = 1.0)
        {
            if (scale > 1.0 || scale < 0)
                throw new ArgumentOutOfRangeException("Zoom only scales to 0.0 - 1.0.  Scale of " + scale + " is out of range.");
            
            var windowSize = driver.ScreenSize();
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

        public static void Scroll(this AppiumDriver driver, Direction direction, double scale=1.0, long durationMilliSecs = 2000)
        {
            var directionToSwipe = ConvertDirectionToSwipe(direction);
            driver.Swipe(directionToSwipe, scale, durationMilliSecs);
        }

        public static void Scroll(this AppiumDriver driver, IWebElement element, Direction direction, double scale = 1.0, long durationMilliSecs = 500)
        {
            var directionToSwipe = ConvertDirectionToSwipe(direction);
            driver.Swipe(element, directionToSwipe, scale, durationMilliSecs);
        }

        public static bool ElementWithinBounds(this AppiumDriver driver, IWebElement element)
        {
            if (element == null)
                return false;

            var screenSize = driver.ScreenSize();

            // element out of bounds
            if ((element.Location.X < 0) || (element.Location.X > screenSize.Width)) 
                return false;

            if ((element.Location.Y < 0) || (element.Location.Y > screenSize.Height))
                return false;

            if ((element.Location.X + element.Size.Width  > screenSize.Width) ||
                (element.Location.Y + element.Size.Height > screenSize.Height))
                return false;

            return true;
        }

        public static void CaptureScreenshot(this AppiumDriver driver, string pathAndFilename, ImageFormat format)
        {
            try
            {
                var screenshot = driver.TakeScreenshot();
                screenshot.SaveAsFile(pathAndFilename, format);
            }
            catch (Exception e)
            {
                Trace.WriteLine("Unexpected error in saving screenshot to: " + pathAndFilename);
                Trace.Write("Stacktrace:  " + e.StackTrace);
            }
        }

    }
}
