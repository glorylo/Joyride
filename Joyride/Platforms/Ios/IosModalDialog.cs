
using System;
using System.Collections.Generic;
using Joyride.Extensions;
using OpenQA.Selenium;

namespace Joyride.Platforms.Ios
{

    abstract public class IosModalDialog : Component, IModalDialog
    {
        protected static readonly ScreenFactory ScreenFactory = new IosScreenFactory();
        protected static readonly Func<Screen> NoTransition = () => null;

        public virtual string Title
        {
            get
            {
                var element = Driver.FindElement(By.XPath("//UIAAlert//UIAStaticText[1]"), DefaultWaitSeconds);
                return (element == null) ? null : element.Text;
            }
        }
        
        public virtual string Body
        {
            get
            {
                var element = Driver.FindElement(By.XPath("//UIAAlert//UIAStaticText[2]"), DefaultWaitSeconds);
                return (element == null) ? null : element.Text;
            }
        }
        
        public abstract Screen Accept();
        public abstract Screen Dismiss();
        public abstract Screen RespondWith(string response);
        public abstract bool IsOnScreen(int timeoutSecs);

        protected void SetTransition(string response, Func<Screen> func) { TransitionMap[response] = func; }
        protected Screen TransitionFromResponse(string response) { return TransitionMap[response](); }
        protected Dictionary<string, Func<Screen>> TransitionMap = new Dictionary<string, Func<Screen>>();
    }
}
