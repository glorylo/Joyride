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
