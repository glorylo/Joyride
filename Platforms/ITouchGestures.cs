using System;
using OpenQA.Selenium;

namespace Joyride.Platforms
{
    //not used
    public interface ITappable
    {
        Screen Tap(string elementName, bool precise = false);
    }

    public interface ITouchGestures : ITappable
    {
        Screen DoubleTap(string elementName);
        Screen TapAndHold(string elementName, int seconds);
        Screen TapInCollection(string collectionName, int oridinal = 1, bool last = false, bool precise = false);
        Screen TapInCollection(string collectionName, Predicate<IWebElement> predicate);
        Screen PinchToZoom(Direction direction, double scale = 1.0);
        Screen Scroll(Direction direction, double scale=1.0, long durationMilliSecs = 500);
        Screen Scroll(string elementName, Direction direction, double scale=1.0, long durationMilliSecs = 500);
        Screen Swipe(Direction direction, double scale=1.0, long durationMilliSecs = 500);
        Screen Swipe(string elementName, Direction direction, double scale=1.0, long durationMilliSecs = 500);
        Screen ScrollUntil(string elementName, Direction direction, double scale=1.0, long durationMilliSecs = 500, int maxRetries = 30);
    }
}