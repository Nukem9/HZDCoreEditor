using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HZDCoreEditorUI.Util;

namespace HZDCoreEditorUI.UI
{
    public class TreeDataArrayNode : TreeDataNode
    {
        public override string Value { get { return $"Array<{TypeName}> ({GetArrayLength()})"; } }

        public override bool HasChildren => GetArrayLength() > 0;
        public override List<TreeDataNode> Children { get; }

        private readonly object ParentObject;
        private readonly FieldInfo ParentFieldEntry;

        public TreeDataArrayNode(object parent, FieldInfo field)
        {
            Name = field.Name;
            TypeName = field.FieldType.GetFriendlyName();

            Children = new List<TreeDataNode>();
            ParentObject = parent;
            ParentFieldEntry = field;

            AddArrayChildren();
        }

        private Array GetArray()
        {
            return ParentFieldEntry.GetValue(ParentObject) as Array;
        }

        private int GetArrayLength()
        {
            return GetArray()?.Length ?? 0;
        }

        private void AddArrayChildren()
        {
            var asArray = GetArray();

            // Array entries act as children
            for (int i = 0; i < asArray?.Length; i++)
                Children.Add(new TreeDataArrayIndexNode(asArray, i));
        }
    }

    public class TreeDataArrayIndexNode : TreeDataNode
    {
        public override string Value { get { return $"{ParentObject.GetValue(ParentArrayIndex)?.ToString()}"; } }

        public override bool HasChildren => Children.Count > 0;
        public override List<TreeDataNode> Children { get; }

        private readonly Array ParentObject;
        private readonly int ParentArrayIndex;

        public TreeDataArrayIndexNode(Array parent, int index)
        {
            Name = $"[{index}]";
            TypeName = parent.GetType().GetElementType().GetFriendlyName();

            Children = new List<TreeDataNode>();
            ParentObject = parent;
            ParentArrayIndex = index;

            AddObjectChildren();
        }

        private void AddObjectChildren()
        {
            var objectInstance = ParentObject.GetValue(ParentArrayIndex);
            var objectType = objectInstance?.GetType();

            if (Type.GetTypeCode(objectType) == TypeCode.Object)
            {
                // If it's not a basic type, avoid an extra tree-expando-entry by making child class members part
                // of this node.
                var fields = objectType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var field in fields)
                {
                    var entry = new TreeDataClassMemberNode(objectInstance, field);

                    Children.Add(entry);
                }
            }
        }
    }
}
