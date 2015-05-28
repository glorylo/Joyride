
namespace Joyride.Platforms
{
    public interface IDetectable
    {        
        bool IsOnScreen(int timeOutSecs);
        string Name { get; }
    }
}
