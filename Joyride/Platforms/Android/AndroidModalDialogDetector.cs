using System;
using System.Reflection;

namespace Joyride.Platforms.Android
{
    public class AndroidModalDialogDetector : ModalDialogDetectorBase
    {
        public AndroidModalDialogDetector(Assembly assembly, Type baseModalDialogType, ScreenFactory factory, int defaultTimeoutSecs = DefaultTimoutSecs)
            :base(assembly, baseModalDialogType, factory, defaultTimeoutSecs)
        {
            
        }


    }
}
