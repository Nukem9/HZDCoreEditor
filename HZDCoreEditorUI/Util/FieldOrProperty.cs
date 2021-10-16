using System;
using System.Reflection;

namespace HZDCoreEditorUI.Util
{
    public struct FieldOrProperty
    {
        private readonly MemberInfo _info;

        public FieldOrProperty(FieldInfo info)
        {
            _info = info;
        }

        public FieldOrProperty(PropertyInfo info)
        {
            _info = info;
        }

        public FieldOrProperty(Type type, string memberName)
        {
            _info = type.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (_info == null)
                _info = type.GetField(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (_info == null || (_info.MemberType != MemberTypes.Property && _info.MemberType != MemberTypes.Field))
                throw new ArgumentException("Invalid class member type", nameof(memberName));
        }

        public void SetValue(object obj, object value)
        {
            switch (_info.MemberType)
            {
                case MemberTypes.Property: ((PropertyInfo)_info).SetValue(obj, value); break;
                case MemberTypes.Field: ((FieldInfo)_info).SetValue(obj, value); break;
            }
        }

        public object GetValue(object obj)
        {
            return _info.MemberType switch
            {
                MemberTypes.Property => ((PropertyInfo)_info).GetValue(obj),
                MemberTypes.Field => ((FieldInfo)_info).GetValue(obj),
                _ => null,
            };
        }

        public T GetValue<T>(object obj)
        {
            return (T)GetValue(obj);
        }

        public Type GetMemberType()
        {
            return _info.MemberType switch
            {
                MemberTypes.Property => ((PropertyInfo)_info).PropertyType,
                MemberTypes.Field => ((FieldInfo)_info).FieldType,
                _ => null,
            };
        }

        public string GetName()
        {
            return _info.Name;
        }
    }
}