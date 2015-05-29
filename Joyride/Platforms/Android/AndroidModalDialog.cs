using System;
using System.Collections.Generic;

namespace Joyride.Platforms.Android
{
    abstract public class AndroidModalDialog : Component, IModalDialog
    {
        protected static readonly ScreenFactory ScreenFactory = new AndroidScreenFactory();
        protected static readonly Func<Screen> NoTransition = () => null;
        public abstract string Body { get; }
        public abstract string Title { get; }
        public abstract Screen Accept();
        public abstract Screen Dismiss();
        public abstract Screen RespondWith(string response);
        public abstract bool IsOnScreen(int timoutSecs);

        protected void SetTransition(string response, Func<Screen> func) { TransitionMap[response] = func; }
        protected Screen TransitionFromResponse(string response) { return TransitionMap[response](); }
        protected Dictionary<string, Func<Screen>> TransitionMap = new Dictionary<string, Func<Screen>>();

    }

}
