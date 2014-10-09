
namespace Joyride.Platforms.Ios
{

 abstract public class IosModalDialog : Component, IModalDialog
    {
        protected static ScreenFactory ScreenFactory = new IosScreenFactory();
        public abstract string Body { get; }
        public abstract string Title { get; }
        public abstract Screen Accept();
        public abstract Screen Dismiss();
        public abstract Screen RespondWith(string response);

    }
}
