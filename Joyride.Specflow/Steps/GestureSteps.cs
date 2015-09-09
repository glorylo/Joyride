using System;
using System.Linq;
using Joyride.Platforms;
using Joyride.Specflow.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace Joyride.Specflow.Steps
{
    [Binding]
    public class GestureSteps : TechTalk.SpecFlow.Steps
    {
        public static int ScrollUntilTimeoutSecs = JoyrideConfiguration.QuickInspectTimeoutSecs;
        public static int MaxRetries = JoyrideConfiguration.MaxRetries;

        [Given(@"I tap the ""([^""]*)"" (?:button|field|label|element|link)")]
        [When(@"I tap the ""([^""]*)"" (?:button|field|label|element|link)")]
        public void WhenITapTheButton(string elementName)
        {
            Context.MobileApp.Do<IGesture>(i => i.Tap(elementName));
        }

        [Given(@"I double tap the ""([^""]*)"" (?:button|field|label|element|link)")]
        [When(@"I double tap the ""([^""]*)"" (?:button|field|label|element|link)")]
        public void WhenIDoubleTap(string elementName)
        {
            Context.MobileApp.Do<IGesture>(i => i.DoubleTap(elementName));
        }

        [Given(@"I tap the ""([^""]*)"" (?:button|field|label|element|link) and hold for ""(\d+)"" seconds")]
        [When(@"I tap the ""([^""]*)"" (?:button|field|label|element|link) and hold for ""(\d+)"" seconds")]
        public void WhenITapAndHoldTheButton(string elementName, int seconds)
        {
            Context.MobileApp.Do<IGesture>(i => i.TapAndHold(elementName, seconds));
        }

        private static double GetScale(string speed)
        {
            var scale = 1.0;
            // slowly is 0.75; anythign else (i.e. moderately is 0.85)
            if (speed != String.Empty)
                scale = (speed == "slowly") ? 0.75 : 0.85;
            return scale;
        }

        private static long GetDurationMilliSeconds(string speed)
        {
            var durationMillSecs = 500;
            // slowly is 3000; anythign else (i.e. moderately is 1500)
            if (speed != String.Empty)
               durationMillSecs = (speed == "slowly") ? 3000 : 1500;
            return durationMillSecs;
        }

        [Given(@"I (?:(slowly|moderately) )?scroll the screen (left|right|up|down)")]
        [When(@"I (?:(slowly|moderately) )?scroll the screen (left|right|up|down)")]
        public void GivenIScrollScreenInDirection(string speed, string direction)
        {
            var directionToScroll = (Direction) Enum.Parse(typeof(Direction), direction, true);
            var durationMillSecs = GetDurationMilliSeconds(speed);
            var scale = GetScale(speed);
            Context.MobileApp.Do<IGesture>(i => i.Scroll(directionToScroll, scale, durationMillSecs));
        }

        [Given(@"I (?:(slowly|moderately) )?scroll the screen (left|right|up|down) ""(\d+)"" times")]
        [When(@"I (?:(slowly|moderately) )?scroll the screen (left|right|up|down) ""(\d+)"" times")]
        public void GivenIScrollScreenInDirectionXTimes(string speed, string direction, int times)
        {
            if (times < 2)
                throw new ArgumentException("times have to be greater than 1");

            var directionToScroll = (Direction)Enum.Parse(typeof(Direction), direction, true);
            var durationMillSecs = GetDurationMilliSeconds(speed);
            var scale = GetScale(speed);

            Context.MobileApp.Do<IGesture>(i =>
            {
                for (var t = 0; t < times; t++)
                    i.Scroll(directionToScroll, scale, durationMillSecs);                
            });
        }

        [Given(@"I swipe the ""([^""]*)"" (left|right|up|down)")]
        [When(@"I swipe the ""([^""]*)"" (left|right|up|down)")]
        public void GivenIScrollElementinDirection(string elementName, string direction)
        {
            var directionToScroll = (Direction) Enum.Parse(typeof(Direction), direction, true);
            Context.MobileApp.Do<IGesture>(i => i.Swipe(elementName, directionToScroll));
        }

        [Given(@"I swipe the screen (left|right|up|down)")]
        [When(@"I swipe the screen (left|right|up|down)")]
        public void GivenISwipeScreenInDirection(string direction)
        {
            var directionToSwipe = (Direction) Enum.Parse(typeof(Direction), direction, true);
            Context.MobileApp.Do<IGesture>(i => i.Swipe(directionToSwipe));
        }

        [Given(@"I (?:(slowly|moderately) )?scroll the screen (left|right|up|down) until I see element ""([^""]*)""")]
        [When(@"I (?:(slowly|moderately) )?scroll the screen (left|right|up|down) until I see element ""([^""]*)""")]
        public void GivenIScrollUntil(string speed, string direction, string elementName)
        {
            var directionToScroll = (Direction) Enum.Parse(typeof(Direction), direction, true);
            var durationMillSecs = GetDurationMilliSeconds(speed);
            var scale = GetScale(speed);

            Context.MobileApp.Do<IGesture>(i => i.ScrollUntil(elementName, directionToScroll, MaxRetries, ScrollUntilTimeoutSecs, scale, durationMillSecs));
        }

        [Given(@"I pinch the screen to zoom (out|in)")]
        [When(@"I pinch the screen to zoom (out|in)")]
        public void GivenIPinchTheScreen(string zoom)
        {
            var direction = (Direction) Enum.Parse(typeof(Direction), zoom, true);
            Context.MobileApp.Do<IGesture>(i => i.PinchToZoom(direction));
        }

        [Given(@"I pull the screen (up|down)")]
        [When(@"I pull the screen (up|down)")]
        public void WhenIPullTheScreen(string direction)
        {
            var dir = (Direction) Enum.Parse(typeof (Direction), direction, true);
            Context.MobileApp.Do<IGesture>(i => i.Pull(dir));
        }

        #region Thens
        [Then(@"I (?:(slowly|moderately) )?scroll the screen (left|right|up|down) until I see the following elements")]
        public void ThenIScrollUntil(string speed, string direction, Table table)
        {
            var directionToScroll = (Direction)Enum.Parse(typeof(Direction), direction, true);
            var durationMillSecs = GetDurationMilliSeconds(speed);
            var scale = GetScale(speed);

            var header = table.Header.First();
            var elements = table.Rows.Select(r => r[header]).ToList();

            foreach (var e in elements)
            {
                var elementName = e;
                var scrollUntil = new TestDelegate(() => Context.MobileApp.Do<IGesture>(i => i.ScrollUntil(elementName, 
                                  directionToScroll, MaxRetries, ScrollUntilTimeoutSecs,
                                  scale, durationMillSecs)));
                Assert.DoesNotThrow(scrollUntil, "Unexpected element not found: " + elementName);
            }
        }

        #endregion
    }
}
