using System;
using System.Collections.Generic;
using System.Reflection;

namespace HZDCoreEditor.UI
{
    public class TreeDataClassMemberNode : TreeDataNode
    {
        public override object Value { get { return ParentFieldEntry.GetValue(ParentObject); } }

        public override bool HasChildren => Children?.Count > 0;
        public override List<TreeDataNode> Children { get; }
        public override bool IsEditable => AllowEdits;

        private readonly object ParentObject;
        private readonly FieldOrProperty ParentFieldEntry;
        private readonly bool AllowEdits;

        public TreeDataClassMemberNode(object parent, FieldOrProperty member, NodeAttributes attributes)
        {
            Name = member.GetName();
            TypeName = member.GetMemberType().Name;

            ParentObject = parent;
            ParentFieldEntry = member;
            AllowEdits = !attributes.HasFlag(NodeAttributes.DisableEditing);

            if (!attributes.HasFlag(NodeAttributes.HideChildren))
            {
                Children = new List<TreeDataNode>();
                AddObjectChildren();
            }
        }

        private void AddObjectChildren()
        {
            var objectInstance = ParentFieldEntry.GetValue(ParentObject);
            var objectType = objectInstance?.GetType();

            if (Type.GetTypeCode(objectType) != TypeCode.Object)
                return;

            // Class member variables act as children
            var fields = objectType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
                Children.Add(CreateNode(objectInstance, new FieldOrProperty(field)));
        }
    }
}
