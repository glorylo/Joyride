
using Joyride.Platforms;

namespace Joyride
{
    abstract public class ScreenFactory
    {
        abstract public T CreateScreen<T>() where T : Screen, new();
    }
}
