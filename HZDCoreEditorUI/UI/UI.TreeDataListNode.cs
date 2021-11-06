using HZDCoreEditorUI.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HZDCoreEditorUI.UI
{
    public class TreeDataListNode : TreeDataNode
    {
        public override object Value => $"{TypeName} ({GetListLength()})";

        public override bool HasChildren => GetListLength() > 0;
        public override List<TreeDataNode> Children => _children.Value;

        private readonly FieldOrProperty _memberFieldHandle;
        private readonly Lazy<List<TreeDataNode>> _children;

        public TreeDataListNode(object parent, FieldOrProperty member, NodeAttributes attributes) : base(parent, member)
        {
            _memberFieldHandle = member;
            _children = new Lazy<List<TreeDataNode>>(() => AddListChildren(attributes));
        }

        private IList GetList()
        {
            return _memberFieldHandle.GetValue<IList>(ParentObject);
        }

        private int GetListLength()
        {
            return GetList()?.Count ?? 0;
        }

        private List<TreeDataNode> AddListChildren(NodeAttributes attributes)
        {
            var list = new List<TreeDataNode>();

            if (!attributes.HasFlag(NodeAttributes.HideChildren))
            {
                var asList = GetList();

                if (asList != null)
                {
                    // Fetch the type of T from IList<T>
                    var enumerableTemplateType = asList.GetType()
                        .GetInterfaces()
                        .Where(i => i.IsGenericType && i.GenericTypeArguments.Length == 1)
                        .Single(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        .GenericTypeArguments[0];

                    // Array elements act as children
                    for (int i = 0; i < asList.Count; i++)
                        list.Add(new TreeDataListIndexNode(asList, i, enumerableTemplateType));
                }
            }

            return list;
        }
    }

    public class TreeDataListIndexNode : TreeDataNode
    {
        public override object Value
        {
            get => ((IList)ParentObject)[_arrayIndex];
            set => ((IList)ParentObject)[_arrayIndex] = value;
        }

        public override bool IsEditable => _objectWrapperNode.IsEditable;

        public override bool HasChildren => _objectWrapperNode.HasChildren;
        public override List<TreeDataNode> Children => _objectWrapperNode.Children;

        private readonly int _arrayIndex;
        private TreeDataNode _objectWrapperNode;

        public TreeDataListIndexNode(IList parent, int index, Type listElementType) : base(parent, listElementType)
        {
            Name = $"[{index}]";
            _arrayIndex = index;

            AddObjectChildren(listElementType);
        }

        public override Control CreateEditControl(Rectangle bounds)
        {
            return _objectWrapperNode.CreateEditControl(bounds);
        }

        public override bool FinishEditControl(Control control)
        {
            return _objectWrapperNode.FinishEditControl(control);
        }

        private void AddObjectChildren(Type elementType)
        {
            _objectWrapperNode = CreateNode(this, new FieldOrProperty(GetType(), nameof(Value)), elementType);
        }
    }
}
