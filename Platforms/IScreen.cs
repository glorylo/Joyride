namespace Joyride.Platforms
{

    // not solidified yet. 
    public interface IScreen : ITouchGestures, IWebViewActions, IFormActions
    {
        // would like to get rid of this time out..
        bool IsOnScreen(int timeOutSecs = Component.DefaultWaitSeconds);
        string Name { get; }
    }
}