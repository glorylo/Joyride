using System;
using System.Diagnostics;
using OpenQA.Selenium.Appium;

namespace Joyride.Platforms
{
    public abstract class MobileApplication : IMobileApplication
    {
        protected Screen Screen;
        abstract public string Identifier { get; }
        public Screen CurrentScreen { get { return Screen; }}
        protected AppiumDriver Driver { get { return RemoteMobileDriver.GetInstance(); } }

        public virtual void Launch()
        {
            Driver.LaunchApp();
        }

        public virtual void Close()
        {
            Driver.CloseApp();
        }

        public virtual void Do<T>(Func<T, Screen> func) where T : class
        {
            var anyScreenOrInterface = CastScreen<T>();
            var beforeTransition = Screen;
            Screen = func(anyScreenOrInterface);

            if (Screen != beforeTransition)
                Trace.WriteLine("Current Screen '" + beforeTransition.Name + "' transition to '" + Screen.Name + "'");
        }

        public virtual void Do<T>(Action<T> func) where T : class
        {
            var screen = CastScreen<T>();
            func(screen);
        }

        protected T CastScreen<T>() where T : class
        {
            if (!typeof(T).IsInterface)
                if (!typeof(Screen).IsAssignableFrom(typeof(T)))
                    throw new Exception("Unable to cast screen from type '" + CurrentScreen.GetType() + " to type:  " + typeof(T));

            dynamic anyScreenOrInterface = CurrentScreen;
            return (T) anyScreenOrInterface;
        }
    }
}