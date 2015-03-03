using System;
using System.Text.RegularExpressions;


namespace Joyride.Extensions
{
    public static class TextCompareExtensions
    {
        public static bool CompareWith(this string yourString, string compareWithString, CompareType compareType)
        {
            switch (compareType)
            {
                case CompareType.Equals:
                    return yourString == compareWithString;

                case CompareType.NotEqual:
                    return yourString != compareWithString;

                case CompareType.StartsWith:
                    return yourString.StartsWith(compareWithString);

                case CompareType.EndsWith:
                    return yourString.EndsWith(compareWithString);

                case CompareType.Containing:
                    return yourString.Contains(compareWithString);

                case CompareType.Matching:
                    var match = Regex.Match(yourString, compareWithString);
                    return match.Success;

                default:
                    throw new Exception("Unknown compare type:  " + compareType);

            }

        }

        public static CompareType ToCompareType(this string yourString)
        {
            return (CompareType)Enum.Parse(typeof(CompareType), yourString.Replace(" ", string.Empty), true);
        }


    }
}
