using System;

namespace Joyride.Platforms
{

    /// <summary>
    ///  Detect the modal dialog order from 0 - 100, lower being higher priority.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DetectAttribute : Attribute
    {
        public int Priority { get { return priority; } set { priority = value; } }
        protected int priority;

        public DetectAttribute()
        {
            priority = 100;
        }
    }
}
