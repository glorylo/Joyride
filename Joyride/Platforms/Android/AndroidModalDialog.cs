namespace Joyride.Platforms.Android
{
    // testing... this approach
    abstract public class AndroidModalDialog : Component, IModalDialog
    {
        protected static ScreenFactory ScreenFactory = new AndroidScreenFactory();
        public abstract string Body { get; }
        public abstract string Title { get; }
        public abstract Screen Accept();
        public abstract Screen Dismiss();
        public abstract Screen RespondWith(string response);

    }

}
