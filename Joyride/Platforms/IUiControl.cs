namespace Joyride.Platforms
{
    public interface IUiControl
    {
        Screen EnterText(string elementName, string text);
        Screen ClearText(string elementName);
        Screen SetCheckbox(string elementName, bool enabled = true);
    }
}