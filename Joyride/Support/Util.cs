using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Joyride.Support
{
    static public class Util
    {
        public static object InvokeMethod(Delegate method, params object[] args)
        {
            return method.DynamicInvoke(args);
        }

        public static MemberInfo GetMemberInfo(object obj, string memberName, BindingFlags flags = BindingFlags.Public)
        {
            var searchFlags = BindingFlags.Instance | BindingFlags.IgnoreCase | flags;
            var memberInfo = obj.GetType().GetMember(memberName, searchFlags).FirstOrDefault();
            return memberInfo;
        }

        public static object GetMemberValue(object obj, string memberName, BindingFlags flags = BindingFlags.Public)
        {
            var member = GetMemberInfo(obj, memberName, flags);

            var property = member as PropertyInfo;

            if (property != null)
            {
                Trace.WriteLine("Found property with name:  " + memberName);
                return property.GetValue(obj);
            }

            var field = member as FieldInfo;

            if (field != null)
            {
                Trace.WriteLine("Found field with name:  " + memberName);
                return field.GetValue(obj);
            }

            Trace.WriteLine("Unable to find member with name:  " + memberName);
            return null;
        }

        public static object GetDynamicMemberValue(dynamic obj, string memberName, BindingFlags flags = BindingFlags.Public)
        {
            var memberValue = GetMemberValue((object)obj, memberName, flags);

            if (memberValue != null)
                return memberValue;

            var dictionary = obj as IDictionary<string, object>;

            if (dictionary == null)
            {
                Trace.WriteLine("Unable to cast dynamic type to retrieve member:  " + memberName);
                return null;
            }

            dictionary.TryGetValue(memberName, out memberValue);

            if (memberValue == null)
                Trace.WriteLine("Unable to the find member (" + memberName + ") for dynamic type");

            return memberValue;
        }
    }
}
