using System;
using System.Collections.Generic;
using Utility;

namespace HZDCoreEditor.UI
{
    public class TreeDataArrayNode : TreeDataNode
    {
        public override object Value { get { return $"Array<{TypeName}> ({GetArrayLength()})"; } }

        public override bool HasChildren => GetArrayLength() > 0;
        public override List<TreeDataNode> Children { get; }
        public override bool IsEditable => false;

        private readonly object ParentObject;
        private readonly FieldOrProperty ParentFieldEntry;

        public TreeDataArrayNode(object parent, FieldOrProperty member, NodeAttributes attributes)
        {
            Name = member.GetName();
            TypeName = member.GetMemberType().Name;

            ParentObject = parent;
            ParentFieldEntry = member;

            if (!attributes.HasFlag(NodeAttributes.HideChildren))
            {
                Children = new List<TreeDataNode>();
                AddArrayChildren();
            }
        }

        private Array GetArray()
        {
            return ParentFieldEntry.GetValue<Array>(ParentObject);
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
        public override object Value { get { return ObjectWrapper?.ToString(); } }

        public override bool HasChildren => ObjectWrapperNode.HasChildren;
        public override List<TreeDataNode> Children => ObjectWrapperNode.Children;
        public override bool IsEditable => ObjectWrapperNode.IsEditable;

        private readonly Array ParentObject;
        private readonly int ParentArrayIndex;
        private TreeDataNode ObjectWrapperNode;

        // Property is needed in order to get a FieldOrProperty handle
        private object ObjectWrapper
        {
            get { return ParentObject.GetValue(ParentArrayIndex); }
            set { ParentObject.SetValue(value, ParentArrayIndex); }
        }

        public TreeDataArrayIndexNode(Array parent, int index)
        {
            Name = $"[{index}]";
            TypeName = parent.GetType().GetElementType().Name;

            ParentObject = parent;
            ParentArrayIndex = index;

            AddObjectChildren();
        }

        private void AddObjectChildren()
        {
            ObjectWrapperNode = CreateNode(this, new FieldOrProperty(GetType(), nameof(ObjectWrapper)));
        }
    }
}
