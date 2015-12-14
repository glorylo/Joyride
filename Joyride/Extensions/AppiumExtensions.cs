using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using Joyride.Support;
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

        public static Size ScreenSize<T>(this AppiumDriver<T> driver) where T: IWebElement
        {
            var size = _screenSize ?? driver.Manage().Window.Size;
            _screenSize = size;
            return (Size) _screenSize;
        }

        public static Point ScreenCenterPoint<T>(this AppiumDriver<T> driver) where T: IWebElement
        {
            var centerX = driver.ScreenSize().Width/2;
            var centerY = driver.ScreenSize().Height/2;
            return new Point(centerX, centerY);
        }

        public static bool IsNative<T>(this AppiumDriver<T> driver) where T: IWebElement
        {
            return driver.Context == NativeAppContext;
        }

        public static void SwitchTo<T>(this AppiumDriver<T> driver, string context, int maxRetries) where T: IWebElement
        {
            var retries = 0;
            do
            {
                try
                {
                    driver.Context = context;
                    Trace.WriteLine("Switched to context: " + context);
                    return;
                }
                catch (Exception)
                {
                    retries++;
                    if (retries == maxRetries) throw;
                }
            } while (retries < maxRetries);
        }

        public static string SwitchToNative<T>(this AppiumDriver<T> driver, int maxRetries = 3) where T : IWebElement
        {
            driver.SwitchTo(NativeAppContext, maxRetries);
            return NativeAppContext;
        }

        public static string SwitchToWebview<T>(this AppiumDriver<T> driver, int maxRetries = 3) where T: IWebElement
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
            driver.SwitchTo(webViewContext, maxRetries);
            return webViewContext;
        }

        public static void DoActionInWebView<T>(this AppiumDriver<T> driver, Action action, int maxRetries = 3) where T: IWebElement
        {
            if (driver.Context != NativeAppContext)
                action();
            else
            {
                driver.SwitchToWebview(maxRetries);
                action();
                driver.SwitchToNative(maxRetries);  
            }              
        }

        public static void EnsureInOutDirection(Direction direction)
        {
            if (direction != Direction.In && direction != Direction.Out)
                throw new ArgumentException("Unexpected direction: " + direction);
        }

        public static void EnsureNotInOutDirection(Direction direction)
        {
            if (direction == Direction.In || direction == Direction.Out)
                throw new ArgumentException("Unexpected direction: " + direction);
        }

        public static void EnsureScaleRange(double scale)
        {
            if (scale > 1.0 || scale < 0)
                throw new ArgumentOutOfRangeException("Zoom only scales to 0.0 - 1.0.  Scale of " + scale + " is out of range.");
        }

        public static void Tap<T>(this AppiumDriver<T> driver, Point location) where T: IWebElement
        {
            new TouchAction(driver).Press(location.X, location.Y).Perform();
        }

        public static void PreciseTap<T>(this AppiumDriver<T> driver, IWebElement element) where T: IWebElement
        {
            driver.Tap(element.GetCenter());
        }

        public static void DoubleTap<T>(this AppiumDriver<T> driver, IWebElement element) where T: IWebElement
        {
           var center = element.GetCenter();
            if (RemotingServices.IsTransparentProxy(element))
            {
                Trace.WriteLine("Detected proxy");
//                var proxy = RemotingServices.GetRealProxy(element);
//                var cachedElement = (IWebElement) Util.GetMemberValue(proxy, "cachedElement", BindingFlags.NonPublic);
//                new TouchAction(driver).
//                    Tap(cachedElement, center.X, center.Y, 2)
//                    .Perform();            

            }
                  

           new TouchAction(driver).
               Tap(element, center.X, center.Y, 2)
               .Perform();            
        }

        public static void TapAndHold<T>(this AppiumDriver<T> driver, IWebElement element, int seconds, bool precise = false)
            where T : IWebElement
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

        public static void Swipe<T>(this AppiumDriver<T> driver, Direction direction, Size dimension,
             double scale=1.0, long durationMilliSecs = 500, int originX=0, int originY=0) 
            where T: IWebElement
        {
            var center = new Point(originX + dimension.Width / 2, originY + dimension.Height / 2);
            var startDeltaX = dimension.Width / 3;
            var endDeltaX = dimension.Width / 2;
            // need to take into account when keyboard is present.
            var startDeltaY = dimension.Height / 7;
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

        public static void Swipe<T>(this AppiumDriver<T> driver, Direction direction, double scale = 1.0, long durationMilliSecs = 500)
            where T: IWebElement
        {
            EnsureScaleRange(scale);
            EnsureNotInOutDirection(direction);
            Swipe(driver, direction, driver.ScreenSize(), scale, durationMilliSecs);
        }

        public static void Swipe<T>(this AppiumDriver<T> driver, IWebElement element, Direction direction, double scale = 1.0, long durationMilliSecs = 500)
            where T : IWebElement
        {
            EnsureNotInOutDirection(direction);
            EnsureScaleRange(scale);

            var upperLeft = element.Location;
            var size = element.Size;
            var offsetX = size.Width/3.0;
            var offsetY = size.Height/3.0;

            double startX = upperLeft.X + size.Width / 2, startY = upperLeft.Y + size.Height / 2;
            double endX = 1.0, endY = 1.0;

            switch (direction)
            {
                case Direction.Down:
                    startY -= offsetY;
                    endY = (driver.ScreenSize().Height * scale) - 1.0;
                    endX = startX;
                    break;
                case Direction.Up:
                    startY += offsetY;
                    endX = startX;
                    endY = startY - (scale*(startY)) + 1.0; 
                    break;

                case Direction.Left:
                    startX += offsetX;
                    endX = startX - (scale*(startX)) + 1.0;
                    endY = startY;
                    break;
                case Direction.Right:
                    startX -= offsetX;
                    endY = startY;
                    endX = (driver.ScreenSize().Width * scale) - 1.0;
                    break;
            }

            new TouchAction(driver)
                .Press(startX, startY)
                .Wait(durationMilliSecs)
                .MoveTo(endX, endY)
                .Release()
                .Perform();
        }

        private static Size CalculateVisibleElementSize<T>(AppiumDriver<T> driver, IWebElement element)
            where T : IWebElement
        {
            var size = element.Size;
            var lowerRight = new Point(element.Location.X + size.Width, element.Location.Y + size.Height);
            var windowSize = driver.ScreenSize();

            if (lowerRight.X > windowSize.Width || lowerRight.Y > windowSize.Height)
                size = new Size(windowSize.Width - element.Location.X, windowSize.Height - element.Location.Y);
            return size;
        }

        public static void PinchToZoom<T>(this AppiumDriver<T> driver, Direction direction, double scale = 1.0) 
            where T: IWebElement
        {
            EnsureScaleRange(scale);          
            EnsureInOutDirection(direction);
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

        public static void Scroll<T>(this AppiumDriver<T> driver, Direction direction, double scale = 1.0, long durationMilliSecs = 2000)
            where T : IWebElement
        {
            EnsureNotInOutDirection(direction);
            EnsureScaleRange(scale);            
            var directionToSwipe = ConvertDirectionToSwipe(direction);
            driver.Swipe(directionToSwipe, scale, durationMilliSecs);
        }

        public static bool ElementWithinBounds<T>(this AppiumDriver<T> driver, IWebElement element) where T: IWebElement
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

        public static void CaptureScreenshot<T>(this AppiumDriver<T> driver, string pathAndFilename, ImageFormat format)
            where T : IWebElement
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

        public static void SwipeFromEdge<T>(this AppiumDriver<T> driver, Direction direction, long durationMillsecs = 1000,
            double scale = 1.0, double offset = 0) where T : IWebElement
        {
            EnsureNotInOutDirection(direction);
            double startX= 1.0, endX = 1.0, startY = 1.0, endY = 1.0;

            var size = driver.ScreenSize();
            switch (direction)
            {
                case Direction.Down:
                    startX = size.Width/2;
                    endX = startX;
                    startY += offset;
                    endY = scale*size.Height - 1.0;
                    break;
                case Direction.Up:
                    startX = size.Width/2;
                    endX = startX;
                    startY = size.Height - 1.0 - offset;
                    endY = (size.Height - (scale*size.Height)) + 1.0;
                    break;
                case Direction.Left:
                    startX = size.Width - 1.0 - offset;
                    endX = (size.Width - (scale * size.Width)) + 1.0;
                    startY = size.Height/2 ;
                    endY = startY;
                    break;            
                case Direction.Right:
                    startX += offset;
                    endX = (size.Width * scale) - 1.0;
                    startY = size.Height/2;
                    endY = startY;
                    break;
            }
            

            new TouchAction(driver)
                .Press(startX, startY)
                .Wait(durationMillsecs)
                .MoveTo(endX, endY)
                .Release()
                .Perform();

        }

        public static void PullScreen<T>(this AppiumDriver<T> driver, Direction direction, long durationMillsecs = 1000, double scale = 1.0, double offset = 0)
            where T : IWebElement
        {

            EnsureNotInOutDirection(direction);
            if ((direction == Direction.Left) || (direction == Direction.Right))
              throw new ArgumentException("Unexpected direction: " + direction);
            SwipeFromEdge(driver, direction, durationMillsecs, scale, offset);
        }

        public static void DragAndDrop<T>(this AppiumDriver<T> driver, IWebElement fromElement, IWebElement toElement, long durationMilliSecs = 1000)
              where T : IWebElement
        {
            if (fromElement == null || toElement == null)
                throw new NoSuchElementException("Unable to perform drag action due to missing elements");

             new TouchAction(driver)
                 .Press(fromElement, 0.5, 0.5)
                 .Wait(durationMilliSecs)
                 .MoveTo(toElement, 0.5, 0.5)
                 .Wait(500)
                 .Release()
                 .Perform();                

        }

    }
}
