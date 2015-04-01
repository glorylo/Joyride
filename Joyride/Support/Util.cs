using System;

namespace Joyride.Support
{
    static public class Util
    {
        public static object InvokeMethod(Delegate method, params object[] args)
        {
            return method.DynamicInvoke(args);
        }

    }
}
