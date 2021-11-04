using HZDCoreEditorUI.Util;
using System;
using System.Collections.Generic;

namespace HZDCoreEditorUI.UI
{
    public class TreeDataArrayNode : TreeDataNode
    {
        public override object Value => $"Array<{TypeName}> ({GetArrayLength()})";

        public override bool HasChildren => GetArrayLength() > 0;
        public override List<TreeDataNode> Children => _children.Value;
        public override bool IsEditable => false;

        private readonly FieldOrProperty _parentFieldEntry;
        private readonly Lazy<List<TreeDataNode>> _children;

        public TreeDataArrayNode(object parent, FieldOrProperty member, NodeAttributes attributes) : base(parent, member)
        {
            _parentFieldEntry = member;
            _children = new Lazy<List<TreeDataNode>>(() => AddArrayChildren(attributes));
        }

        private Array GetArray()
        {
            return _parentFieldEntry.GetValue<Array>(ParentObject);
        }

        private int GetArrayLength()
        {
            return GetArray()?.Length ?? 0;
        }

        private List<TreeDataNode> AddArrayChildren(NodeAttributes attributes)
        {
            var nodes = new List<TreeDataNode>();
            var array = GetArray();

            if (!attributes.HasFlag(NodeAttributes.HideChildren) && array != null)
            {
                var elementType = array.GetType().GetElementType();

                // Array entries act as children
                for (int i = 0; i < array.Length; i++)
                    nodes.Add(new TreeDataArrayIndexNode(array, i, elementType));
            }

            return nodes;
        }
    }

    public class TreeDataArrayIndexNode : TreeDataNode
    {
        public override object Value => ObjectWrapper;

        public override bool HasChildren => _objectWrapperNode.HasChildren;
        public override List<TreeDataNode> Children => _objectWrapperNode.Children;
        public override bool IsEditable => _objectWrapperNode.IsEditable;

        private readonly int _arrayIndex;
        private TreeDataNode _objectWrapperNode;

        // A "fake" property is needed in order to get a FieldOrProperty handle
        private object ObjectWrapper
        {
            get => ((Array)ParentObject).GetValue(_arrayIndex);
            set => ((Array)ParentObject).SetValue(value, _arrayIndex);
        }

        public TreeDataArrayIndexNode(Array parent, int index, Type arrayElementType) : base(parent)
        {
            Name = $"[{index}]";
            TypeName = arrayElementType.GetFriendlyName();

            _arrayIndex = index;

            AddObjectChildren();
        }

        private void AddObjectChildren()
        {
            _objectWrapperNode = CreateNode(this, new FieldOrProperty(GetType(), nameof(ObjectWrapper)));
        }
    }
}
