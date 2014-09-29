using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Appium;

namespace Joyride.Platforms.Ios
{
    abstract public class IosMobileApplication : IMobileApplication
    {
        abstract public string BundleId { get; }

        protected Screen Screen;

        public Screen CurrentScreen { get { return Screen; }}

        protected AppiumDriver Driver { get { return RemoteMobileDriver.GetInstance(); } }

        public virtual void Launch()
        {
            //Driver.LaunchApp();
        }

        public virtual void Close()
        {
            Driver.CloseApp();
        }

        public void Do<T>(Func<T, Screen> func) where T : class
        {
            var anyScreenOrInterface = CastScreen<T>();
            Screen = func(anyScreenOrInterface);
        }

        public void Do<T>(Action<T> func) where T : class
        {
            var screen = CastScreen<T>();
            func(screen);
        }

        protected T CastScreen<T>() where T : class
        {
            if (!typeof(T).IsInterface || !typeof(T).IsSubclassOf(typeof(IosScreen)))
                throw new Exception("Unable to cast screen to type:  " + typeof(T));

            dynamic anyScreenOrInterface = CurrentScreen;
            return (T) anyScreenOrInterface;
        }


    }
}
