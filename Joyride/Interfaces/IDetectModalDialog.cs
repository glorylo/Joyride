using Joyride.Platforms;

namespace Joyride.Interfaces
{
    public interface IDetectModalDialog
    {
        IModalDialog DetectModalDialog();
        IModalDialog DetectModalDialog(string modalDialogName);
        Screen AcceptModalDialog(bool accept, string modalDialogName, bool throwException = false);
        Screen AcceptModalDialog(bool accept);
        Screen RespondModalDialog(string response, string modalDialogName, bool throwException = false);
    }
}