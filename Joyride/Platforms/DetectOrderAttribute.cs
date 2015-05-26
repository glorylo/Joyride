using System;

namespace Joyride.Platforms
{

    /// <summary>
    ///  Detect the modal dialog order from 0 - 100, lower being higher priority.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DetectOrderAttribute : Attribute
    {
        public int Order { get { return order; } set { order = value; } }
        protected int order;

        public DetectOrderAttribute()
        {
            order = 100;
        }
    }
}
