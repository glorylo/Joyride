using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joyride.Platforms.Android
{
    abstract public class AndroidScreen : Screen
    {
        protected ScreenFactory ScreeFactory = new AndroidScreenFactory();

    }
}
