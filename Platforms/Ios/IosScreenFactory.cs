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

        public override Screen CreateScreen(Type t)
        {
            if (!t.IsSubclassOf(typeof(IosScreen)))
                throw new Exception("Unable to create screen of type:  " + t);
            return (Screen)Activator.CreateInstance(t);
        }

    }

}
