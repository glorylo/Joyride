using System;
using System.Linq;
using System.Reflection;
using Joyride.Platforms;
using Joyride.Platforms.Android;
using Tests.Android.Native.SampleApp.ApiDemo.ModalDialogs;

namespace Tests.Android.Native.SampleApp.ApiDemo.Screens
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
