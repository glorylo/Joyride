namespace Joyride.Platforms
{
    public interface IWebview
    {
        Screen SelectOption(string elementName, string value);
        string GetSelectedOption(string elementName);
    }
}