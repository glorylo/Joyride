using Joyride.Platforms.Android;
using Tests.Android.Native.SampleApp.ApiDemo.Screens;

namespace Tests.Android.Native.SampleApp.ApiDemo
{
    public class ApiDemoApp : AndroidMobileApplication
    {
        public override string Identifier
        {
            get { return "io.appium.android.apis"; }
        }

        public override void Launch()
        {
            base.Launch();
            CurrentScreen = ScreenFactory.CreateScreen<MainScreen>();
        }

        public override void Close()
        {
            base.Close();
            CurrentScreen = ScreenFactory.CreateScreen<NullApiDemoScreen>();
        }
    }
}
