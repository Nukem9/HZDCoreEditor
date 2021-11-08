namespace HZDCoreEditorUI.Util
{
    using System;

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
}