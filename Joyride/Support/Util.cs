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

        public static T GetMemberCustomAttribute<T>(object obj, string memberName, BindingFlags flags = BindingFlags.Public) where T : Attribute
        {
            var member = GetMemberInfo(obj, memberName, BindingFlags.NonPublic);
            return member == null ? null : member.GetCustomAttribute<T>();
        }

        public static MemberInfo GetMemberInfo(object obj, string memberName, BindingFlags flags = BindingFlags.Public)
        {
            var searchFlags = BindingFlags.Instance | BindingFlags.IgnoreCase | flags;
            var memberInfo = obj.GetType().GetMember(memberName, searchFlags).FirstOrDefault();
            return memberInfo;
        }

        public static object GetMemberValue(object obj, string memberName, bool includeTrace=true, BindingFlags flags = BindingFlags.Public)
        {
            var member = GetMemberInfo(obj, memberName, flags);

            var property = member as PropertyInfo;

            if (property != null)
            {
                if (includeTrace) Trace.WriteLine("Found property with name:  " + memberName);
                return property.GetValue(obj);
            }

            var field = member as FieldInfo;

            if (field != null)
            {
                if (includeTrace) Trace.WriteLine("Found field with name:  " + memberName);
                return field.GetValue(obj);
            }

            if (includeTrace) Trace.WriteLine("Unable to find member with name:  " + memberName);
            return null;
        }

        public static object GetDynamicMemberValue(dynamic obj, string memberName)
        {
            object memberValue;

            var dictionary = obj as IDictionary<string, object>;

            if (dictionary == null)
            {
                Trace.WriteLine("Unable to cast dynamic type to retrieve member:  " + memberName);
                return null;
            }

            dictionary.TryGetValue(memberName, out memberValue);

            if (memberValue == null)
                Trace.WriteLine("Unable to the find member (" + memberName + ") for dynamic type");

            Trace.WriteLine("Found dynamic member (" + memberName + "): " + memberValue);
            return memberValue;
        }

        public static bool HasMember(dynamic obj, string memberName)
        {
            var dictionary = obj as IDictionary<string, object>;
            if (dictionary == null)
                return false;

            object memberValue;
            dictionary.TryGetValue(memberName, out memberValue);
            var foundMember = memberValue != null;
            if (!foundMember)
                Trace.WriteLine("Unable to find dynamic member: " + memberName);
            return foundMember;
        }
    }
}
