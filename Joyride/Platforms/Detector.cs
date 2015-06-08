using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public T DefaultValue { get; set; }
        public Assembly TargetAssembly { get; set; }
        public int TimeoutSecs { get; set; }
        public int SingleDetectTimeoutSecs = RemoteMobileDriver.DefaultWaitSeconds;

        public Detector(Assembly assembly, Type baseDetectableType, Func<Type, T> factoryMethod, int defaultTimeoutSecs) 
        {
            TargetAssembly = assembly;
            TimeoutSecs = defaultTimeoutSecs;
            DetectableTypes = GetDetectableTypes();
            FactoryMethod = factoryMethod;
            SetBaseDetectableType(baseDetectableType);
            BuildLookupTable();
            DefaultValue = default(T);
        }

        public Detector(Assembly assembly, Type baseDetectableType, Func<Type, T> factoryMethod, T defaultValue, int defaultTimeoutSecs)
            : this(assembly, baseDetectableType, factoryMethod, defaultTimeoutSecs)
        {
            DefaultValue = defaultValue;
        }

        protected void SetBaseDetectableType(Type baseDetectableType)
        {
            if (typeof(IDetectable).IsAssignableFrom(baseDetectableType))
                BaseDetectableType = baseDetectableType;
            else
                throw new ArgumentException("Unexpected type of: " + baseDetectableType);
        }

        protected void BuildLookupTable()
        {
            foreach (var t in DetectableTypes)
            {
                var detectable = FactoryMethod(t);
                LookupTable.Add(detectable.Name, t);
            }    
        }

        public bool IsOnScreen(Type type, int timeoutSecs)
        {
            var detectable = FactoryMethod(type);
            return detectable.IsOnScreen(timeoutSecs);
        }

        public IEnumerable<Type> GetDetectableTypes()
        {
            var list = TargetAssembly.GetTypes().Where(t => t.BaseType == BaseDetectableType)
                .Select(t =>
                {
                    var attrib = t.GetCustomAttribute(typeof(DetectAttribute), false) as DetectAttribute;
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

        public T Detect(Type type)
        {
            return (IsOnScreen(type, SingleDetectTimeoutSecs)) ? FactoryMethod(type) : DefaultValue;
        }

        public T Detect()
        {
            return Detect(DetectableTypes);
        }

        public T Detect(IEnumerable<Type> dialogTypes)
        {
            return(from t in dialogTypes 
                   where IsOnScreen(t, TimeoutSecs) 
                   select FactoryMethod(t))
                   .DefaultIfEmpty(DefaultValue)
                   .First();
        }

        public T Detect(string[] detectableNames)
        {
            var detectable = DefaultValue;
            var index = 0;

            while (detectable.Equals(DefaultValue) && index < detectableNames.Length)
            {
                var name = detectableNames[index];
                if (!LookupTable.ContainsKey(name))
                    Trace.WriteLine("Unable to find detectable:  " + name);
                else
                {
                    var detectableType = LookupTable[name];
                    detectable = IsOnScreen(detectableType, TimeoutSecs) ? FactoryMethod(detectableType) : DefaultValue;
                }
                index++;
            }
            return detectable;
        }

        public T Detect(string name)
        {
            if (!LookupTable.ContainsKey(name))
            {
                Trace.WriteLine("Unable to find detectable:  " + name);
                return DefaultValue;
            }
                
            var detectableType = LookupTable[name];
            return IsOnScreen(detectableType, SingleDetectTimeoutSecs) ? FactoryMethod(detectableType) : DefaultValue;
        }

    }
}