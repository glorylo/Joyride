namespace Joyride.Platforms
{
    public interface IWebViewActions
    {
        void SelectOption(string elementName, string value);
        string GetSelectedOption(string elementName);
        string GetSourceWebView();
    }
}