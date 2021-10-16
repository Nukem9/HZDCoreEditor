using System;

namespace HZDCoreEditor.Util
{
    internal static class ReflectionUtil
    {
        public static bool Inherits(this Type objectType, Type baseType)
        {
            while (objectType != null)
            {
                if (objectType == baseType)
                    return true;

                objectType = objectType.BaseType;
            }

            return false;
        }

        public static bool InheritsGeneric(this Type objectType, Type genericType)
        {
            while (objectType != null)
            {
                if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == genericType)
                    return true;

                objectType = objectType.BaseType;
            }

            return false;
        }
    }

    public struct FieldOrProperty
    {
        private readonly MemberInfo Info;

        public FieldOrProperty(FieldInfo info)
        {
            Info = info;
        }

        public FieldOrProperty(PropertyInfo info)
        {
            Info = info;
        }

        public FieldOrProperty(Type type, string memberName)
        {
            Info = type.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (Info == null)
                Info = type.GetField(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (Info == null || (Info.MemberType != MemberTypes.Property && Info.MemberType != MemberTypes.Field))
                throw new ArgumentException("Invalid class member type", nameof(memberName));
        }

        public void SetValue(object obj, object value)
        {
            switch (Info.MemberType)
            {
                case MemberTypes.Property: ((PropertyInfo)Info).SetValue(obj, value); break;
                case MemberTypes.Field: ((FieldInfo)Info).SetValue(obj, value); break;
            }
        }

        public object GetValue(object obj)
        {
            return Info.MemberType switch
            {
                MemberTypes.Property => ((PropertyInfo)Info).GetValue(obj),
                MemberTypes.Field => ((FieldInfo)Info).GetValue(obj),
                _ => null,
            };
        }

        public T GetValue<T>(object obj)
        {
            return (T)GetValue(obj);
        }

        public Type GetMemberType()
        {
            return Info.MemberType switch
            {
                MemberTypes.Property => ((PropertyInfo)Info).PropertyType,
                MemberTypes.Field => ((FieldInfo)Info).FieldType,
                _ => null,
            };
        }

        public string GetName()
        {
            return Info.Name;
        }
    }
}