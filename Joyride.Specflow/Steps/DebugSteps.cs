using System;
using System.IO;
using Joyride.Extensions;
using Joyride.Specflow.Configuration;
using Joyride.Specflow.Support;
using TechTalk.SpecFlow;

namespace Joyride.Specflow.Steps
{
    [Binding]
    public class DebugSteps : TechTalk.SpecFlow.Steps
    {
        public static string LogPath = JoyrideConfiguration.LogPath;

        [Given(@"I dump DOM trace")]
        [When(@"I dump DOM trace")]
        [Then(@"I dump DOM trace")]
        public void DumpDomTrace()
        {
            using (var writer = new StreamWriter(LogPath + Context.MobileApp.Screen.Name +".xml"))
            {
                writer.WriteLine(Context.MobileApp.Screen.GetSource());
            }            
        }

        [Given(@"I wait for ""(\d+)"" seconds")]
        [When(@"I wait for ""(\d+)"" seconds")]
        [Then(@"I wait for ""(\d+)""seconds")]
        public void GiveIWaitFor(int timeoutSecs)
        {
            Context.Driver.WaitFor(TimeSpan.FromSeconds(timeoutSecs));
        }

        [Given(@"I dump DOM trace in webview")]
        [When(@"I dump DOM trace in webview")]
        [Then(@"I dump DOM trace in webview")]
        public void DumpDomTraceWebview()
        {
            using (var writer = new StreamWriter(LogPath + Context.MobileApp.Screen.Name + ".html"))
            {
                writer.WriteLine(Context.MobileApp.Screen.GetSourceWebView());
            }            
        }

        [Given(@"I take a screenshot")]
        [When(@"I take a screenshot")]
        [Then(@"I take a screenshot")]
        public void TakeScreenshot()
        {
            ScreenCapturer.Capture();
        }
    }
}
