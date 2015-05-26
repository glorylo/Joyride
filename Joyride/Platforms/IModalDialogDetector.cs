using System;

namespace Joyride.Platforms
{
    public interface IModalDialogDetector
    {
        IModalDialog Detect(Type type);
        IModalDialog Detect();
        IModalDialog Detect(string[] modalDialogNames);
        IModalDialog Detect(string modalDialogName);
    }
}