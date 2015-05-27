
namespace Joyride.Platforms.Ios
{
    abstract public class IosMobileApplication : MobileApplication
    {
        protected static readonly ScreenFactory ScreenFactory = new IosScreenFactory();
    }
}
