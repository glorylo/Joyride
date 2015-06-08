namespace Joyride.Platforms
{
    public interface IFormAction
    {
        Screen EnterText(string elementName, string text);
        Screen SetCheckbox(string elementName, bool enabled = true);
    }
}