namespace Joyride.Platforms
{
    public interface IWebViewActions
    {
        Screen SelectOption(string elementName, string value);
        string GetSelectedOption(string elementName);
    }
}