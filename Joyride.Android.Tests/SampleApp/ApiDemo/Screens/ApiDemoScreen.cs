using System.Reflection;
using Joyride.Android.Tests.SampleApp.ApiDemo.ModalDialogs;
using Joyride.Platforms.Android;

namespace Joyride.Android.Tests.SampleApp.ApiDemo.Screens
{
    public abstract class ApiDemoScreen : AndroidScreen
    {
        static ApiDemoScreen()
        {
            var assembly = Assembly.GetExecutingAssembly();
            ModalDialogDetector = new AndroidModalDialogDetector(assembly, typeof(ApiDemoModalDialog));
        }
    }
}
