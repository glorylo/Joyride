using Joyride.Platforms;

namespace Tests.Android.Native.SampleApp.ApiDemo.Screens
{
    public class NullApiDemoScreen : ApiDemoScreen
    {
        public override string Name
        {
            get { return "Null"; }
        }

        public override bool IsOnScreen(int timeOutSecs)
        {
            return false;
        }

        public override Screen GoBack()
        {
            Driver.Navigate().Back();            
            return this;
        }
    }
}
