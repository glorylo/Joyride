namespace Joyride.Platforms
{
    public interface IModalDialog
    {
        string Name { get; }
        string Body { get; }
        string Title { get; }
        Screen Accept();
        Screen Dismiss();
        Screen RespondWith(string response);
        bool IsOnScreen(int timeoutSecs);
    }
}