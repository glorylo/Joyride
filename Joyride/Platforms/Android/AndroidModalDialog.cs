using System;
using System.Collections.Generic;

namespace Joyride.Platforms.Android
{
    abstract public class AndroidModalDialog : ModalDialog
    {
        protected static readonly ScreenFactory ScreenFactory = new AndroidScreenFactory();

    }

}
