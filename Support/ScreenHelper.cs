using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Joyride.Support
{
    static public class ScreenHelper
    {
        public static object InvokeMethod(Delegate method, params object[] args)
        {
            return method.DynamicInvoke(args);
        }

    }
}
