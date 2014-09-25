using System;

namespace Joyride.Platforms.Ios
{


    public class IosScreenFactory : ScreenFactory
    {
        public override T CreateScreen<T>()
        {
            if (!typeof(T).IsSubclassOf(typeof(IosScreen)))          
                throw new Exception("Unable to create screen of type:  "  + typeof(T));
            return new T();
        }
    }

}
