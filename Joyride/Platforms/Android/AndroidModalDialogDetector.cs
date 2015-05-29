using System;
using System.Collections.Generic;
using System.Reflection;

namespace Joyride.Platforms.Android
{
    public class AndroidModalDialogDetector : IDetector<IModalDialog>
    {
        protected ScreenFactory ScreenFactory = new AndroidScreenFactory();
        public const int DefaultTimoutSecs = 2;
        protected readonly IDetector<IModalDialog> Detector;

        public AndroidModalDialogDetector(Assembly assembly, Type baseModalDialogType, int defaultTimeoutSecs = DefaultTimoutSecs)
        {
            Detector = new Detector<IModalDialog>(assembly, baseModalDialogType, ScreenFactory.CreateModalDialog, defaultTimeoutSecs);
        }

        public IModalDialog Detect(Type type)
        {
            return Detector.Detect(type);
        }

        public IModalDialog Detect(IEnumerable<Type> types)
        {
            return Detector.Detect(types);
        }

        public IModalDialog Detect()
        {
            return Detector.Detect();
        }

        public IModalDialog Detect(string[] modalDialogNames)
        {
            return Detector.Detect(modalDialogNames);
        }

        public IModalDialog Detect(string modalDialogName)
        {
            return Detector.Detect(modalDialogName);
        }
    }
}
