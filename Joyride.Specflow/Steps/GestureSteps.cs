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

        [Given(@"I (?:(slowly|moderately) )?scroll the screen (left|right|up|down)")]
        [When(@"I (?:(slowly|moderately) )?scroll the screen (left|right|up|down)")]
        public void GivenIScrollScreenInDirection(string speed, string direction)
        {
            var directionToScroll = (Direction) Enum.Parse(typeof(Direction), direction, true);
            var durationMillSecs = 500;
            var scale = 1.0;
            if (speed != String.Empty)
            {
                durationMillSecs = (speed == "slowly") ? 3000 : 1500;
                scale = (speed == "slowly") ? 0.75 : 0.85;
            }

            Context.MobileApp.Do<IGesture>(i => i.Scroll(directionToScroll, scale, durationMillSecs));
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
            var durationMillSecs = 500;
            if (speed != String.Empty)
                durationMillSecs = (speed == "slowly") ? 3000 : 1500;

            Context.MobileApp.Do<IGesture>(i => i.ScrollUntil(elementName, directionToScroll, MaxRetries, ScrollUntilTimeoutSecs, 1.0, durationMillSecs));
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
            var durationMillSecs = 500;
            if (speed != String.Empty)
                durationMillSecs = (speed == "slowly") ? 3000 : 750;

            var header = table.Header.First();
            var elements = table.Rows.Select(r => r[header]).ToList();

            foreach (var e in elements)
            {
                var elementName = e;
                var scrollUntil = new TestDelegate(() => Context.MobileApp.Do<IGesture>(i => i.ScrollUntil(elementName, 
                                  directionToScroll, MaxRetries, ScrollUntilTimeoutSecs,
                                  1.0, durationMillSecs)));
                Assert.DoesNotThrow(scrollUntil, "Unexpected element not found: " + elementName);
            }
        }

        #endregion
    }
}
