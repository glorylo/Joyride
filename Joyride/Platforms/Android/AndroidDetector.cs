using System;
using System.Reflection;

namespace Joyride.Platforms.Android
{
    public class AndroidDetector<T> : Detector<T> where T: IDetectable
    {
        public const int DefaultTimoutSecs = 2;

        public AndroidDetector(Assembly assembly, Type baseDetectableType, Func<Type, T> factoryMethod, int defaultTimeoutSecs = DefaultTimoutSecs)
            : base(assembly, baseDetectableType, factoryMethod, defaultTimeoutSecs)
        {
        }
    }
}
