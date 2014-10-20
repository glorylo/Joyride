using System;
using System.Text.RegularExpressions;


namespace Joyride.Extensions
{
    public static class TextCompareExtensions
    {
        public static bool CompareWith(this string yourString, string compareWithString, TextCompare compareType)
        {
            switch (compareType)
            {
                case TextCompare.Equals:
                    return yourString == compareWithString;

                case TextCompare.NotEqual:
                    return yourString != compareWithString;

                case TextCompare.StartsWith:
                    return yourString.StartsWith(compareWithString);

                case TextCompare.EndsWith:
                    return yourString.EndsWith(compareWithString);

                case TextCompare.Containing:
                    return yourString.Contains(compareWithString);

                case TextCompare.Matching:
                    var match = Regex.Match(yourString, compareWithString);
                    return match.Success;

                default:
                    throw new Exception("Unknown compare type:  " + compareType);

            }

        }


    }
}
