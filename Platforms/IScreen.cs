namespace Joyride.Platforms
{

    // not solidified yet.  to be used later.
    public interface IScreen : ITouchGestures, IWebViewActions, IFormActions
    {
        bool IsOnScreen(int timeOutSecs = Component.DefaultWaitSeconds);
        string Name { get; }
    }
}