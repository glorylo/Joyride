using System.IO;
using Joyride.Specflow;
using Joyride.Specflow.Configuration;
using Joyride.Specflow.Support;
using TechTalk.SpecFlow;
using Joyride.Android.Tests.SampleApp;

namespace Joyride.Android.Tests.Steps
{
    [Binding]
    public class SpecFlowHooks
    {
        public const Platform TargetPlatform = Platform.Android;  // update either Platform.Android or Platform.Ios

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            var projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            JoyrideConfiguration.SetWorkingDirectory(projectDir);
            var capabilities = JoyrideConfiguration.BundleCapabilities(TargetPlatform, "nexus5"); // change the device
            var server = JoyrideConfiguration.GetServer(); // change the server.  default is "dev"
            RemoteMobileDriver.Initialize(server, TargetPlatform, capabilities);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            RemoteMobileDriver.CleanUp();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            Context.Driver = RemoteMobileDriver.GetInstance();
            Context.MobileApp = new SampleApp.ApiDemoApp();
            // Add your test app here
            // Context.MobileApp = new ApiDemoApp.ApiDemoApp();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (Context.HasError)
                ScreenCapturer.Capture();

            Context.MobileApp.Close();
        }
    }
}