using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HZDCoreEditorUI.UI
{
    public class TreeDataListNode : TreeDataNode
    {
        public override string Value { get { return $"List<{TypeName}> ({GetListLength()})"; } }

        public override bool HasChildren => GetListLength() > 0;
        public override List<TreeDataNode> Children { get; }

        private readonly object ParentObject;
        private readonly FieldInfo ParentFieldEntry;

        public TreeDataListNode(object parent, FieldInfo field)
        {
            Name = field.Name;
            TypeName = field.FieldType.Name;

            Children = new List<TreeDataNode>();
            ParentObject = parent;
            ParentFieldEntry = field;

            AddListChildren();
        }

        private IList GetList()
        {
            return ParentFieldEntry.GetValue(ParentObject) as IList;
        }

        private int GetListLength()
        {
            return GetList()?.Count ?? 0;
        }

        private void AddListChildren()
        {
            var asList = GetList();

            // Array entries act as children
            for (int i = 0; i < asList?.Count; i++)
                Children.Add(new TreeDataListIndexNode(asList, i));
        }
    }

    public class TreeDataListIndexNode : TreeDataNode
    {
        public override string Value { get { return $"{ParentObject[ParentArrayIndex]?.ToString()}"; } }

        public override bool HasChildren => Children.Count > 0;
        public override List<TreeDataNode> Children { get; }

        private readonly IList ParentObject;
        private readonly int ParentArrayIndex;

        public TreeDataListIndexNode(IList parent, int index)
        {
            Name = $"[{index}]";
            TypeName = "";

            Children = new List<TreeDataNode>();
            ParentObject = parent;
            ParentArrayIndex = index;

            AddObjectChildren();
        }

        private void AddObjectChildren()
        {
            var objectInstance = ParentObject[ParentArrayIndex];
            var objectType = objectInstance?.GetType();

            if (Type.GetTypeCode(objectType) == TypeCode.Object)
            {
                // If it's not a basic type, avoid an extra tree-expando-entry by making child class members part
                // of this node.
                var fields = objectType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var field in fields)
                    Children.Add(CreateNode(objectInstance, field));
            }
        }
    }
}
