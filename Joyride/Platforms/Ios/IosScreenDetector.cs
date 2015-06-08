using System;
using System.Collections.Generic;
using System.Reflection;

namespace Joyride.Platforms.Ios
{
    public class IosScreenDetector : IDetector<Screen>
    {
        protected static ScreenFactory ScreenFactory = new IosScreenFactory();
        public const int DefaultTimoutSecs = 4;
        protected readonly IDetector<Screen> Detector;

        public IosScreenDetector(Assembly assembly, Type baseDetectableType, int defaultTimeoutSecs=DefaultTimoutSecs)
        {
              Detector = new Detector<Screen>(assembly, baseDetectableType, ScreenFactory.CreateScreen, defaultTimeoutSecs);
        }

        public IosScreenDetector(Assembly assembly, Type baseDetectableType, Screen defaultValue, int defaultTimeoutSecs = DefaultTimoutSecs)
        {
            Detector = new Detector<Screen>(assembly, baseDetectableType, ScreenFactory.CreateScreen, defaultValue, defaultTimeoutSecs);
        }

        public virtual Screen Detect(Type type)
        {
            return Detector.Detect(type);
        }

        public virtual Screen Detect(IEnumerable<Type> types)
        {
            return Detector.Detect(types);
        }

        public virtual Screen Detect()
        {
            return Detector.Detect();
        }

        public virtual Screen Detect(string[] names)
        {
            return Detector.Detect(names);
        }

        public virtual Screen Detect(string name)
        {
            return Detector.Detect(name);
        }
    }
}
