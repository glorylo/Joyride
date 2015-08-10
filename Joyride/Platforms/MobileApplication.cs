using System;
using System.Diagnostics;
using Joyride.Extensions;
using OpenQA.Selenium.Appium;

namespace Joyride.Platforms
{
    public abstract class MobileApplication : IMobileApplication
    {
        protected static int MaxLaunchRetries = 5;
        protected static double RetryDelaySecs = 2;
        protected Screen CurrentScreen;
        protected int TransitionDelayMs = 0;
        abstract public string Identifier { get; }
        public Screen Screen { get { return CurrentScreen; }}
        protected AppiumDriver Driver { get { return RemoteMobileDriver.GetInstance(); } }

        public virtual void Launch()
        {
            var retries = 0;
            do
            {
                try
                {
                    Driver.LaunchApp();
                    return;
                }
                catch (Exception)
                {
                    retries++;
                    Trace.WriteLine("Failed to launch app.  Retries: " + retries);
                    Driver.WaitFor(TimeSpan.FromSeconds(RetryDelaySecs));

                    if (retries == MaxLaunchRetries)
                        throw;
                }

            } while (retries < MaxLaunchRetries);
        }

        public virtual void Close()
        {
            Driver.CloseApp();
        }

        public virtual void Do<T>(Func<T, Screen> func) where T : class
        {
            var anyScreenOrInterface = CastScreen<T>();
            var beforeTransition = CurrentScreen;
            CurrentScreen = func(anyScreenOrInterface);

            if (CurrentScreen != beforeTransition)
            {
                Trace.WriteLine("Current Screen '" + beforeTransition.Name + "' transition to '" + CurrentScreen.Name + "'");
                if (TransitionDelayMs > 0)
                    Driver.WaitFor(TimeSpan.FromMilliseconds(TransitionDelayMs));
            }
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
                    throw new Exception("Unable to cast screen from type '" + Screen.GetType() + " to type:  " + typeof(T));

            dynamic anyScreenOrInterface = Screen;
            return (T) anyScreenOrInterface;
        }
    }
}