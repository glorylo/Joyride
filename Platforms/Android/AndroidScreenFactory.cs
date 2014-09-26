using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joyride.Platforms.Android
{
    public class AndroidScreenFactory : ScreenFactory
    {
        public override T CreateScreen<T>()
        {
            if (!typeof(T).IsSubclassOf(typeof(AndroidScreen)))
                throw new Exception("Unable to create screen of type:  " + typeof(T));
            return new T();
        }
    }
}
