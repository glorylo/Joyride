using System;

namespace Joyride.Platforms
{

    /// <summary>
    ///  Detect the modal dialog order from 0 - 100, lower being higher priority.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class DetectAttribute : Attribute
    {
        public int Priority { get; set; }

        public DetectAttribute()
        {
            Priority = 100;
        }
    }
}
