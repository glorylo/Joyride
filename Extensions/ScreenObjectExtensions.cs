using System.Globalization;

namespace Joyride.Extensions
{
    static public class ScreenObjectExtensions
    {
        static public string ToPascalCase(this string text)
        {
            var yourString = text.ToLower();
            var info = CultureInfo.CurrentCulture.TextInfo;
            return info.ToTitleCase(yourString).Replace(" ", string.Empty);            
        }

        static public string ToTitleCase(this string text)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(text);            
        }
    }
}
