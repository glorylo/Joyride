namespace Joyride.Platforms
{
    public interface IFormActions
    {
        Screen EnterText(string elementName, string text);
        Screen SetCheckbox(string elementName, bool enabled = true);
    }
}