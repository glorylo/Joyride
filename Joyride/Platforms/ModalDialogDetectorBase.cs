using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Joyride.Platforms
{
    public class ModalDialogDetectorBase : IModalDialogDetector
    {
        public Type BaseModalDialogType { get; set; }

        public Assembly TargetAssembly { get; set; }

        public const int DefaultTimoutSecs = 2;
        public int TimeoutSecs { get; set; }

        protected ScreenFactory ScreenFactory;
        protected Dictionary<string, Type> ModalDialogs = new Dictionary<string, Type>();
        protected IEnumerable<Type> DialogTypes;

        protected ModalDialogDetectorBase(Assembly assembly, Type baseModalDialogType, int defaultTimeoutSecs = DefaultTimoutSecs)
        {
            TargetAssembly = assembly;
            BaseModalDialogType = baseModalDialogType;
            TimeoutSecs = defaultTimeoutSecs;
            DialogTypes = GetDialogTypes();
        }

        protected IEnumerable<Type> GetDialogTypes()
        {
            var list = TargetAssembly.GetTypes().Where(t => t.BaseType == BaseModalDialogType)
                .Select(t =>
                {
                    var attrib = t.GetCustomAttribute(typeof (DetectAttribute), false) as DetectAttribute;
                    var order = (attrib == null) ? 100 : attrib.Priority;
                    return new
                    {
                        Order = order,
                        Type = t
                    };
                })
                .OrderBy(t => t.Order)
                .Select(t => t.Type);
            return list;
        }

        protected void BuildModalDialogLookupTable()
        {
            foreach (var t in DialogTypes)
            {
                var dialog = ScreenFactory.CreateModalDialog(t);
                ModalDialogs.Add(dialog.Name, t);
            }    
        }

        protected bool IsOnScreen(Type type, int timeoutSecs)
        {
            var dialog = ScreenFactory.CreateModalDialog(type);
            return dialog.IsOnScreen(TimeoutSecs);
        }

        public IModalDialog Detect(Type type)
        {
            return (IsOnScreen(type, TimeoutSecs)) ? ScreenFactory.CreateModalDialog(type) : null;
        }

        public IModalDialog Detect()
        {
            return Detect(DialogTypes);
        }

        public IModalDialog Detect(IEnumerable<Type> dialogTypes)
        {
            return(from t in dialogTypes where IsOnScreen(t, TimeoutSecs) select ScreenFactory.CreateModalDialog(t)).FirstOrDefault();
        }

        public IModalDialog Detect(string[] modalDialogNames)
        {
            IModalDialog dialog = null;
            var index = 0;

            while ((dialog == null) && index < modalDialogNames.Length)
            {
                dialog = Detect(modalDialogNames[index]);
                index++;
            }
            return dialog;
        }

        public IModalDialog Detect(string modalDialogName)
        {
            if (!ModalDialogs.ContainsKey(modalDialogName))
                return null;
            var dialogType = ModalDialogs[modalDialogName];
            return IsOnScreen(dialogType, TimeoutSecs) ? ScreenFactory.CreateModalDialog(dialogType) : null;
        }

    }
}
