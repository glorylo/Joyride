namespace Joyride.Platforms
{
    public interface IModalDialog : IDetectable
    {
        string Body { get; }
        string Title { get; }
        Screen Accept();
        Screen Dismiss();
        Screen RespondWith(string response);
    }
}