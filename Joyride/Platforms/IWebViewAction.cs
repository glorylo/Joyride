namespace Joyride.Platforms
{
    public interface IWebViewAction
    {
        Screen SelectOption(string elementName, string value);
        string GetSelectedOption(string elementName);
    }
}