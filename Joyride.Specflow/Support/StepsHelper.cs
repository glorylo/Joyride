
namespace Joyride.Specflow.Support
{
    public static class StepsHelper
    {
        public static string ExtractValue(string value, string valueWithCurly)
        {
             return (value == "") ? valueWithCurly : value;
        }
    }
}
