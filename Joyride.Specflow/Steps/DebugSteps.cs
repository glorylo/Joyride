using System;
using System.IO;
using Joyride.Extensions;
using TechTalk.SpecFlow;

namespace Joyride.Specflow.Steps
{
    [Binding]
    public class DebugSteps
    {
        [Given(@"I dump DOM trace")]
        [When(@"I dump DOM trace")]
        [Then(@"I dump DOM trace")]
        public void DumpDomTrace()
        {
            using (var writer = new StreamWriter(Context.LogPath + Context.MobileApp.Screen.Name +".xml"))
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
            using (var writer = new StreamWriter(Context.LogPath + Context.MobileApp.Screen.Name + ".html"))
            {
                writer.WriteLine(Context.MobileApp.Screen.GetSourceWebView());
            }            
        }
    }
}
