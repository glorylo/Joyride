using System;
using System.Diagnostics;
using System.IO;
using Joyride;
using Joyride.Specflow;
using Joyride.Specflow.Configuration;
using Joyride.Specflow.Support;
using OpenQA.Selenium.Remote;
using TechTalk.SpecFlow;
using Tests.Android.Native.SampleApp.ApiDemo;

namespace Tests.Android.Native.Steps
{
    [Binding]
    public class SpecFlowHooks
    {
        public static Platform TargetPlatform { get; set; }
        public static Uri Server { get; set; }
        public static DesiredCapabilities Capabilities { get; set; }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            var workingDir = Directory.GetCurrentDirectory();
            JoyrideConfiguration.SetLogPaths(workingDir);
            Capabilities = JoyrideConfiguration.BundleCapabilities(); 
            Server = JoyrideConfiguration.GetServerUri(); 
            TargetPlatform = JoyrideConfiguration.TargetPlatform;
            RemoteMobileDriver.Connect(Server,TargetPlatform, Capabilities);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            RemoteMobileDriver.Disconnect();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            Context.Driver = RemoteMobileDriver.GetInstance();
            Context.MobileApp = new ApiDemoApp();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            try
            {
                if (Context.HasError)
                    ScreenCapturer.Capture();

                Context.MobileApp.Close();
            }
            catch(Exception e)
            {
                Trace.WriteLine("Cleaning up app encountered unexpected error");
                Trace.WriteLine(e);
            }
        }
    }
}