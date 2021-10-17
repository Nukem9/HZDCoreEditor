using System.Reflection;

namespace Decima
{
    public static partial class RTTI
    {
        /// <summary>
        /// Helper for assigning values to class members when reading binary data. This automatically handles members
        /// defined with multiple inheritance.
        /// </summary>
        public sealed class OrderedFieldInfo
        {
            public sealed class Entry
            {
                public readonly FieldInfo MIBase;
                public readonly FieldInfo Field;
                public readonly bool SaveStateOnly;

                public Entry(FieldInfo miBase, FieldInfo field, bool saveStateOnly)
                {
                    MIBase = miBase;
                    Field = field;
                    SaveStateOnly = saveStateOnly;
                }

                public void SetValue(object parent, object value)
                {
                    // If using emulated MI: fetch the base class pointer first, then write the field
                    Field.SetValue(MIBase?.GetValue(parent) ?? parent, value);
                }

                public object GetValue(object parent)
                {
                    return Field.GetValue(MIBase?.GetValue(parent) ?? parent);
                }
            }

            public readonly FieldInfo[] MIBases;
            public readonly Entry[] Members;

            public OrderedFieldInfo(FieldInfo[] bases, Entry[] members)
            {
                MIBases = bases;
                Members = members;
            }
        }
    }
}
