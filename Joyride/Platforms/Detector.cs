using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Joyride.Platforms
{
    public class Detector<T> : IDetector<T> where T : IDetectable
    {
        protected IEnumerable<Type> DetectableTypes;
        protected Func<Type, T> FactoryMethod { get; set; }
        protected Dictionary<string, Type> LookupTable = new Dictionary<string, Type>();
        public Type BaseDetectableType { get; set; }
        public Assembly TargetAssembly { get; set; }
        public int TimeoutSecs { get; set; }

        public Detector(Assembly assembly, Type baseDetectableType, Func<Type, T> factoryMethod, int defaultTimeoutSecs) 
        {
            TargetAssembly = assembly;
            TimeoutSecs = defaultTimeoutSecs;
            DetectableTypes = GetDetectableTypes();
            FactoryMethod = factoryMethod;

            if (typeof(IDetectable).IsAssignableFrom(baseDetectableType))
              BaseDetectableType = baseDetectableType;
            else
                throw new ArgumentException("Unexpected type of: " + baseDetectableType);
            BuildLookupTable();
        }

        public IEnumerable<Type> GetDetectableTypes()
        {
            var list = TargetAssembly.GetTypes().Where(t => t.BaseType == BaseDetectableType)
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

        protected void BuildLookupTable()
        {
            foreach (var t in DetectableTypes)
            {
                var detectable = FactoryMethod(t);
                LookupTable.Add(detectable.Name, t);
            }    
        }

        protected bool IsOnScreen(Type type, int timeoutSecs)
        {
            var detectable = FactoryMethod(type);
            return detectable.IsOnScreen(TimeoutSecs);
        }

        public T Detect(Type type)
        {
            return (IsOnScreen(type, TimeoutSecs)) ? FactoryMethod(type) : default(T);
        }

        public T Detect()
        {
            return Detect(DetectableTypes);
        }

        public T Detect(IEnumerable<Type> dialogTypes)
        {
            return(from t in dialogTypes where IsOnScreen(t, TimeoutSecs) select FactoryMethod(t)).FirstOrDefault();
        }

        public T Detect(string[] detectableTypes)
        {
            var detectable = default(T);
            var index = 0;

            while ((detectable == null) && index < detectableTypes.Length)
            {
                detectable = Detect(detectableTypes[index]);
                index++;
            }
            return detectable;
        }

        public T Detect(string modalDialogName)
        {
            if (!LookupTable.ContainsKey(modalDialogName))
                return default(T);
            var detectableType = LookupTable[modalDialogName];
            return IsOnScreen(detectableType, TimeoutSecs) ? FactoryMethod(detectableType) : default(T);
        }

    }
}