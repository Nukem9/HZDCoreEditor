#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Decima
{
    public static partial class RTTI
    {
        public class RttiField
        {
            public readonly MemberInfo @MemberInfo;

            public Type Type => MemberInfo.MemberType switch
            {
                MemberTypes.Property => ((PropertyInfo)MemberInfo).PropertyType,
                MemberTypes.Field => ((FieldInfo)MemberInfo).FieldType,
            };

            public string Name => MemberInfo.Name;

            public RttiField(FieldInfo info)
            {
                if (info.MemberType != MemberTypes.Field)
                    throw new ArgumentException($"Expecting a Field type only. Got {info.MemberType}", nameof(info));

                MemberInfo = info;
            }

            public RttiField(PropertyInfo info)
            {
                if (info.MemberType != MemberTypes.Property)
                    throw new ArgumentException($"Expecting a Field type only. Got {info.MemberType}", nameof(info));

                MemberInfo = info;
            }

            public void SetValue(object obj, object value)
            {
                switch (MemberInfo.MemberType)
                {
                    case MemberTypes.Property: ((PropertyInfo)MemberInfo).SetValue(obj, value); break;
                    case MemberTypes.Field: ((FieldInfo)MemberInfo).SetValue(obj, value); break;
                }
            }

            public object GetValue(object obj)
            {
                return MemberInfo.MemberType switch
                {
                    MemberTypes.Property => ((PropertyInfo)MemberInfo).GetValue(obj),
                    MemberTypes.Field => ((FieldInfo)MemberInfo).GetValue(obj),
                };
            }

            public T GetValue<T>(object obj)
            {
                return (T)GetValue(obj);
            }

            public T GetCustomAttribute<T>() where T : Attribute
            {
                return MemberInfo.MemberType switch
                {
                    MemberTypes.Property => ((PropertyInfo)MemberInfo).GetCustomAttribute<T>(),
                    MemberTypes.Field => ((FieldInfo)MemberInfo).GetCustomAttribute<T>(),
                };
            }

            public static IEnumerable<RttiField> GetMembers(Type type, BindingFlags flags)
            {
                var fields = type.GetFields(flags);

                foreach (var field in fields)
                    yield return new RttiField(field);

                var properties = type.GetProperties(flags);

                foreach (var property in properties)
                    yield return new RttiField(property);
            }
        }
    }
}
