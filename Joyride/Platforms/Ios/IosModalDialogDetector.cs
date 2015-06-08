using System;
using System.Collections.Generic;
using System.Reflection;

namespace Joyride.Platforms.Ios
{
    public class IosModalDialogDetector : IDetector<IModalDialog>
    {
        public const int DefaultTimeoutSecs = 5;
        protected ScreenFactory ScreenFactory = new IosScreenFactory();

        protected readonly IDetector<IModalDialog> Detector; 

        public IosModalDialogDetector(Assembly assembly, Type baseModalDialogType, int defaultTimeoutSecs=DefaultTimeoutSecs)             
        {
            Detector = new Detector<IModalDialog>(assembly, baseModalDialogType, ScreenFactory.CreateModalDialog, defaultTimeoutSecs);
        }

        public virtual IModalDialog Detect(Type type)
        {
            return Detector.Detect(type);
        }

        public virtual IModalDialog Detect(IEnumerable<Type> types)
        {
            return Detector.Detect(types);
        }

        public virtual IModalDialog Detect()
        {
            return Detector.Detect();
        }

        public virtual IModalDialog Detect(string[] modalDialogNames)
        {
            return Detector.Detect(modalDialogNames);
        }

        public virtual IModalDialog Detect(string modalDialogName)
        {
            return Detector.Detect(modalDialogName);
        }
    }
}
