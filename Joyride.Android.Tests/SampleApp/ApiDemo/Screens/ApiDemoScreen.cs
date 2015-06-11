using System;
using System.Linq;
using System.Reflection;
using Joyride.Android.Tests.SampleApp.ApiDemo.ModalDialogs;
using Joyride.Platforms;
using Joyride.Platforms.Android;

namespace Joyride.Android.Tests.SampleApp.ApiDemo.Screens
{
    public abstract class ApiDemoScreen : AndroidScreen
    {
        protected static IDetector<Screen> ScreenDetector;

        static ApiDemoScreen()
        {
            var assembly = Assembly.GetExecutingAssembly();
            ModalDialogDetector = new AndroidModalDialogDetector(assembly, typeof(ApiDemoModalDialog));

            var unknownScreen = ScreenFactory.CreateScreen<NullApiDemoScreen>();
            ScreenDetector = new AndroidScreenDetector(assembly, typeof(ApiDemoScreen), unknownScreen);
        }

        protected static Screen Detect(params Type[] screenTypes)
        {
            return (!screenTypes.Any()) ? ScreenDetector.Detect() : ScreenDetector.Detect(screenTypes);
        }

        public override Screen GoBack()
        {
            Driver.Navigate().Back();
            return Detect();
        }
    }
}
