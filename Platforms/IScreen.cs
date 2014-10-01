namespace Joyride.Platforms
{

    // not solidified yet.  to be used later.
    public interface IScreen : ITouchGestures, IWebViewActions, IFormActions
    {
        // would like to get rid of this time out..
        bool IsOnScreen(int timeOutSecs = Component.DefaultWaitSeconds);
        string Name { get; }
    }
}