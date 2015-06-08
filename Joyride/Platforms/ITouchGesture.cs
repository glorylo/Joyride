
namespace Joyride.Platforms
{

    public interface ITouchGesture
    {
        Screen Tap(string elementName, bool precise = false);    
        Screen DoubleTap(string elementName);
        Screen TapAndHold(string elementName, int seconds);
        Screen TapInCollection(string collectionName, int oridinal = 1, bool last = false, bool precise = false);
        Screen PinchToZoom(Direction direction, double scale = 1.0);
        Screen Scroll(Direction direction, double scale=1.0, long durationMilliSecs = 500);
        Screen Scroll(string elementName, Direction direction, double scale=1.0, long durationMilliSecs = 500);
        Screen Swipe(Direction direction, double scale=1.0, long durationMilliSecs = 500);
        Screen Swipe(string elementName, Direction direction, double scale=1.0, long durationMilliSecs = 500);
        Screen ScrollUntil(string elementName, Direction direction, double scale=1.0, long durationMilliSecs = 500, int maxRetries = 60);
    }
}