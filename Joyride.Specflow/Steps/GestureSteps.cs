using System;
using Joyride.Platforms;
using Joyride.Specflow.Configuration;
using TechTalk.SpecFlow;

namespace Joyride.Specflow.Steps
{
    [Binding]
    public class GestureSteps : TechTalk.SpecFlow.Steps
    {
        public static int ScrollUntilTimeoutSecs = JoyrideConfiguration.ScrollUntilTimeoutSecs;
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

        [Given(@"I tap the ""([^""]*)"" button and hold for ""(\d+)"" seconds")]
        [When(@"I tap the ""([^""]*)"" button and hold for ""(\d+)"" seconds")]
        public void WhenITapAndHoldTheButton(string elementName, int seconds)
        {
            Context.MobileApp.Do<IGesture>(i => i.TapAndHold(elementName, seconds));
        }

        [Given(@"I scroll the screen (left|right|up|down)")]
        [When(@"I scroll the screen (left|right|up|down)")]
        public void GivenIScrollScreenInDirection(string direction)
        {
            var directionToScroll = (Direction) Enum.Parse(typeof(Direction), direction, true);
            Context.MobileApp.Do<IGesture>(i => i.Scroll(directionToScroll));
        }

        [Given(@"I do a (slight|moderate) scroll (left|right|up|down)")]
        [When(@"I do a (slight|moderate) scroll (left|right|up|down)")]
        public void GivenIScrollScreenInDirectionWithScale(string slightOrModerate, string direction)
        {
            var directionToScroll = (Direction) Enum.Parse(typeof(Direction), direction, true);
            if (slightOrModerate == "slight")
                Context.MobileApp.Do<IGesture>(i => i.Scroll(directionToScroll, 0.5));
            else
                Context.MobileApp.Do<IGesture>(i => i.Scroll(directionToScroll, 0.75));
        }

        [Given(@"I scroll the ""([^""]*)"" (left|right|up|down)")]
        [When(@"I scroll the ""([^""]*)"" (left|right|up|down)")]
        public void GivenIScrollElementinDirection(string elementName, string direction)
        {
            var directionToScroll = (Direction) Enum.Parse(typeof(Direction), direction, true);
            Context.MobileApp.Do<IGesture>(i => i.Scroll(elementName, directionToScroll));
        }

        [Given(@"I swipe the screen (left|right|up|down)")]
        [When(@"I swipe the screen (left|right|up|down)")]
        public void GivenISwipeScreenInDirection(string direction)
        {
            var directionToSwipe = (Direction) Enum.Parse(typeof(Direction), direction, true);
            Context.MobileApp.Do<IGesture>(i => i.Swipe(directionToSwipe));
        }

        [Given(@"I scroll the screen (left|right|up|down) until I see element ""([^""]*)""")]
        [When(@"I scroll the screen (left|right|up|down) until I see element ""([^""]*)""")]
        public void GivenIScrollUntil(string direction, string elementName)
        {
            var directionToScroll = (Direction) Enum.Parse(typeof(Direction), direction, true);
            Context.MobileApp.Do<IGesture>(i => i.ScrollUntil(elementName, directionToScroll, MaxRetries, ScrollUntilTimeoutSecs));
        }

        [Given(@"I pinch the screen to zoom (out|in)")]
        [When(@"I pinch the screen to zoom (out|in)")]
        public void GivenIPinchTheScreen(string zoom)
        {
            var direction = (Direction) Enum.Parse(typeof(Direction), zoom, true);
            Context.MobileApp.Do<IGesture>(i => i.PinchToZoom(direction));
        }

    }
}
