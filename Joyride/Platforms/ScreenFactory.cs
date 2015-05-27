using System;

namespace Joyride.Platforms
{
    abstract public class ScreenFactory
    {
        abstract public T CreateScreen<T>() where T : Screen, new();
        abstract public Screen CreateScreen(Type t);

        public abstract IModalDialog CreateModalDialog<T>() where T : IModalDialog, new();
        public abstract IModalDialog CreateModalDialog(Type t);

    }
}
