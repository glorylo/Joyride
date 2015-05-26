using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Humanizer;
using OpenQA.Selenium;

namespace Joyride.Platforms.Android
{
    public class ModalDialogDetector
    {

        public Type BaseModalDialogType { get; set; }

        public Assembly TargetAssembly { get; set; }

        public static int DefaultTimoutSecs = 30;

        protected ScreenFactory ScreenFactory = new AndroidScreenFactory();
        protected Dictionary<string, Type> ModalDialogs = new Dictionary<string, Type>();
        protected IEnumerable<Type> DialogTypes;  

        public ModalDialogDetector(Assembly assembly, Type baseModalDialogType)
        {
            TargetAssembly = assembly;
            BaseModalDialogType = baseModalDialogType;
            DialogTypes = GetDialogTypes();
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

        protected bool IsOnScreen(Type type)
        {
            var dialog = ScreenFactory.CreateModalDialog(type);
            if (dialog.IsOnScreen(DefaultTimoutSecs))
                return true;

            return false;
        }

        public IModalDialog Detect()
        {
            foreach (var t in DialogTypes)
            {
                var dialog = ScreenFactory.CreateModalDialog(t);
            }

            return null;
        }

/*
        public static IModalDialog Detect(string[] modalDialogNames, int timeoutSecs = DefaultWaitSeconds)
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

        public static IModalDialog Detect(string modalDialogName, int timeoutSecs = DefaultWaitSeconds)
        {
            var id = Dialogs[modalDialogName];
            var dialog = Driver.FindElement(By.Id(id), timeoutSecs);

            if (dialog == null)
                return null;
            Trace.WriteLine("Detected Modal Dialog:  " + modalDialogName);
            var type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == (modalDialogName.Dehumanize() + "ModalDialog"));
            return ScreenFactory.CreateModalDialog(type);
        }
*/
    }
}
