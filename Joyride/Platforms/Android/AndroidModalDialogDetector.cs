using System;
using System.Reflection;

namespace Joyride.Platforms.Android
{
    public class AndroidModalDialogDetector : ModalDialogDetector
    {
        protected ScreenFactory ScreenFactor = new AndroidScreenFactory();
        public AndroidModalDialogDetector(Assembly assembly, Type baseModalDialogType, int defaultTimeoutSecs = DefaultTimoutSecs) 
            : base(assembly, baseModalDialogType, defaultTimeoutSecs)
        {
        }
    }
}
