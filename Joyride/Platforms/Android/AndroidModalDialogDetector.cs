using System;
using System.Reflection;

namespace Joyride.Platforms.Android
{
    public class AndroidModalDialogDetector : ModalDialogDetectorBase
    {

        public AndroidModalDialogDetector(Assembly assembly, Type baseModalDialogType, int defaultTimeoutSecs = DefaultTimoutSecs)
            :base(assembly, baseModalDialogType, defaultTimeoutSecs)
        {
            ScreenFactory = new AndroidScreenFactory();
            BuildModalDialogLookupTable();
        }


    }
}
