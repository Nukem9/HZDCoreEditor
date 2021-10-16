using HZDCoreEditorUI.Util;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace HZDCoreEditorUI.UI
{
    public class TreeDataClassMemberNode : TreeDataNode
    {
        public override object Value { get { return ParentFieldEntry.GetValue(ParentObject); } }

        public override bool HasChildren => Children?.Count > 0;
        public override List<TreeDataNode> Children { get; }
        public override bool IsEditable => AllowEdits;

        private readonly FieldOrProperty ParentFieldEntry;
        private readonly bool AllowEdits;

        public TreeDataClassMemberNode(object parent, FieldOrProperty member, NodeAttributes attributes)
            : base(parent)
        {
            Name = member.GetName();
            TypeName = member.GetMemberType().GetFriendlyName();

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
