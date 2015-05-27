using System;
using System.Reflection;

namespace Joyride.Platforms.Android
{
    public class AndroidModalDialogDetector : ModalDialogDetectorBase
    {
        public const int DefaultTimoutSecs = 2;

        public AndroidModalDialogDetector(Assembly assembly, Type baseModalDialogType, int defaultTimeoutSecs = DefaultTimoutSecs)
            :base(assembly, baseModalDialogType, defaultTimeoutSecs)
        {
            ScreenFactory = new AndroidScreenFactory();
            BuildModalDialogLookupTable();
        }
    }
}
