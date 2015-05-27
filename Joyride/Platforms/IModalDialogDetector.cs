using System;
using System.Collections.Generic;

namespace Joyride.Platforms
{
    public interface IModalDialogDetector
    {
        IModalDialog Detect(Type type);
        IModalDialog Detect(IEnumerable<Type> types);
        IModalDialog Detect();
        IModalDialog Detect(string[] modalDialogNames);
        IModalDialog Detect(string modalDialogName);
    }
}