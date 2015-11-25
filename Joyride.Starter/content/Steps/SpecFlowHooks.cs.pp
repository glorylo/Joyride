using System.IO;
using Joyride;
using Joyride.Specflow;
using Joyride.Specflow.Configuration;
using Platform = Joyride.Platform;
using Joyride.Specflow.Support;
using TechTalk.SpecFlow;

namespace $rootnamespace$.Steps
{
    [Binding]
    public class SpecFlowHooks
    {	
	    [BeforeTestRun]
        public static void BeforeTestRun()
        {
            var workingDir = Directory.GetCurrentDirectory();
            JoyrideConfiguration.SetLogPaths(workingDir);
            var capabilities = JoyrideConfiguration.BundleCapabilities(); 
            var server = JoyrideConfiguration.GetServerUri(); 
            var targetPlatform = JoyrideConfiguration.TargetPlatform;
            RemoteMobileDriver.Initialize(server, targetPlatform, capabilities);
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
			// Add your test app here
            // Context.MobileApp = new TestApp();  
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