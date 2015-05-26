using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Joyride.Platforms.Android
{
    public class ModalDialogDetector 
    {

        public Type BaseModalDialogType { get; set; }

        public Assembly TargetAssembly { get; set; }

        public int DefaultTimoutSecs = 30;

        protected static ModalDialogDetector Detector;

        public static void Register(Assembly assembly, Type baseModalType)
        {
            if (Detector == null)
                Detector = new ModalDialogDetector(assembly, baseModalType);
        }

        public static ModalDialogDetector GetInstance()
        {
            return Detector;
        }

        protected ScreenFactory ScreenFactory = new AndroidScreenFactory();
        protected Dictionary<string, Type> ModalDialogs = new Dictionary<string, Type>();
        protected IEnumerable<Type> DialogTypes;  

        private ModalDialogDetector(Assembly assembly, Type baseModalDialogType)
        {
            TargetAssembly = assembly;
            BaseModalDialogType = baseModalDialogType;
            DialogTypes = GetDialogTypes();
            BuildModalDialogLookupTable();
        }

        protected IEnumerable<Type> GetDialogTypes()
        {
            var list = TargetAssembly.GetTypes().Where(t => t.BaseType == BaseModalDialogType)
                .Select(t =>
                {
                    var attrib = t.GetCustomAttribute(typeof (DetectOrderAttribute), false) as DetectOrderAttribute;
                    var order = (attrib == null) ? 100 : attrib.Order;
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
            return dialog.IsOnScreen(DefaultTimoutSecs);
        }

        public IModalDialog Detect(Type type, int timeoutSecs)
        {
            return (IsOnScreen(type, timeoutSecs)) ? ScreenFactory.CreateModalDialog(type) : null;
        }

        public IModalDialog Detect(int timeoutSecs)
        {
            return (from t in DialogTypes where IsOnScreen(t, timeoutSecs) select ScreenFactory.CreateModalDialog(t)).FirstOrDefault();
        }


        public IModalDialog Detect(string[] modalDialogNames, int timeoutSecs)
        {
            IModalDialog dialog = null;
            var index = 0;

            while ((dialog == null) && index < modalDialogNames.Length)
            {
                dialog = Detect(modalDialogNames[index], timeoutSecs);
                index++;
            }
            return dialog;
        }

        public IModalDialog Detect(string modalDialogName, int timeoutSecs)
        {
            if (!ModalDialogs.ContainsKey(modalDialogName))
                return null;
            var dialogType = ModalDialogs[modalDialogName];
            return IsOnScreen(dialogType , timeoutSecs) ? ScreenFactory.CreateModalDialog(dialogType) : null;
        }

    }
}
