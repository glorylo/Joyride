﻿
namespace Joyride.Platforms
{

    public interface IGesture
    {
        Screen Pull(Direction direction, long durationMillSecs = 1000);
        Screen Tap(string elementName, bool precise = false);    
        Screen DoubleTap(string elementName);
        Screen TapAndHold(string elementName, int seconds);
        Screen PinchToZoom(Direction direction, double scale = 1.0);
        Screen Scroll(Direction direction, double scale=1.0, long durationMilliSecs = 500);
        Screen Swipe(Direction direction, double scale=1.0, long durationMilliSecs = 500);
        Screen Swipe(string elementName, Direction direction, double scale=1.0, long durationMilliSecs = 500);
        Screen ScrollUntil(string elementName, Direction direction, int maxRetries, int timeoutSecs, double scale = 1.0, long durationMilliSecs = 500);
        Screen DragAndDrop(string fromElementName, string toElementName);
    }
}