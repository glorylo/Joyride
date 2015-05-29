using System;
using System.Collections.Generic;

namespace Joyride.Platforms
{
    public interface IDetector<out T> where T: IDetectable
    {
        T Detect(Type type);
        T Detect(IEnumerable<Type> types);
        T Detect();
        T Detect(string[] names);
        T Detect(string name);
    }
}
