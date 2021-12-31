using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Decima
{
    public static partial class RTTI
    {
        /// <summary>
        /// Helper for mapping save game (SaveState) types to game RTTI types.
        /// </summary>
        public sealed class VirtualRTTIList
        {
            public readonly Type ClassType;
            public IReadOnlyList<OrderedFieldInfo.Entry> ResolvedMembers { get => _resolvedMembers.AsReadOnly(); }

            private readonly List<Entry> _members;
            private readonly List<OrderedFieldInfo.Entry> _resolvedMembers;

            private class Entry
            {
                public string Type;
                public string Category;
                public string Name;
            }

            public VirtualRTTIList(string className, int capacity = 0)
            {
                ClassType = GetTypeByName(className);
                _members = new List<Entry>(capacity);
                _resolvedMembers = new List<OrderedFieldInfo.Entry>();
            }

            public void Add(string type, string category, string name)
            {
                _members.Add(new Entry
                {
                    Type = type,
                    Category = category,
                    Name = name,
                });
            }

            public void ResolveMembersToFieldInfo()
            {
                var info = GetOrderedFieldsForClass(ClassType, true);

                foreach (var virtualMember in _members)
                {
                    var resolvedMember = info.Members
                        .Where(x => MatchField(x.Field, virtualMember.Type, virtualMember.Category, virtualMember.Name))
                        .Single();

                    _resolvedMembers.Add(resolvedMember);
                }
            }

            private static bool MatchField(RttiField field, string type, string category, string name)
            {
                if (GetFieldCategory(field) != category)
                    return false;

                if (GetFieldName(field) != name)
                    return false;

                string ftn = GetTypeNameString(field.Type);

                // TODO: Custom int32 type - C# doesn't support typedefs. I can pretend this isn't a problem until I need
                // to write fields.
                if (ftn != "int" && type != "int32")
                {
                    if (!ftn.Equals(type, StringComparison.OrdinalIgnoreCase))
                        return false;
                }

                return true;
            }
        }
    }
}
