using System;
using System.Reflection;

namespace Joyride.Platforms.Ios
{
    public class IosModalDialogDetector : ModalDialogDetectorBase
    {
        public const int DefaultTimeoutSecs = 5;

        public IosModalDialogDetector(Assembly assembly, Type baseModalDialogType, int defaultTimeoutSecs=DefaultTimeoutSecs) 
            : base(assembly, baseModalDialogType, defaultTimeoutSecs)
        {
            ScreenFactory = new IosScreenFactory();
            BuildModalDialogLookupTable();
        }
    }
}
