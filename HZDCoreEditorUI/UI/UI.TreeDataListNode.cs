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
        private readonly NodeAttributes _attributes;
        private Lazy<List<TreeDataNode>> _children;

        #region Indexer node handler
        public class IndexNode : TreeDataNode
        {
            public override object Value
            {
                get => ((IList)ParentObject)[_arrayIndex];
                set => ((IList)ParentObject)[_arrayIndex] = value;
            }

            public override bool IsEditable => _objectWrapperNode.IsEditable;

            public override bool HasChildren => _objectWrapperNode.HasChildren;
            public override List<TreeDataNode> Children => _objectWrapperNode.Children;

            private readonly TreeDataListNode _parentNode;
            private readonly int _arrayIndex;
            private readonly TreeDataNode _objectWrapperNode;

            public IndexNode(TreeDataListNode parentNode, int index, Type listElementType) : base(parentNode.GetList(), listElementType)
            {
                Name = $"[{index}]";

                _parentNode = parentNode;
                _arrayIndex = index;
                _objectWrapperNode = CreateNode(this, new FieldOrProperty(GetType(), nameof(Value)), listElementType);
            }

            public override void CreateContextMenuItems(ContextMenuStrip contextMenu, Action refreshTreeCallback)
            {
                base.CreateContextMenuItems(contextMenu, refreshTreeCallback);

                if (contextMenu.Items.Count > 0)
                    contextMenu.Items.Add(new ToolStripSeparator());

                var insertItem = new ToolStripMenuItem();
                insertItem.Text = $"Insert Element at {Name}";
                insertItem.Click += (o, e) => { _parentNode.InsertListElement(_arrayIndex); refreshTreeCallback(); };
                contextMenu.Items.Add(insertItem);

                var removeItem = new ToolStripMenuItem();
                removeItem.Text = $"Remove Element at {Name}";
                removeItem.Click += (o, e) => { _parentNode.RemoveListElement(_arrayIndex); refreshTreeCallback(); };
                contextMenu.Items.Add(removeItem);
            }

            public override Control CreateEditControl(Rectangle bounds)
            {
                return _objectWrapperNode.CreateEditControl(bounds);
            }

            public override bool FinishEditControl(Control control, Action refreshTreeCallback)
            {
                return _objectWrapperNode.FinishEditControl(control, refreshTreeCallback);
            }
        }
        #endregion

        public TreeDataListNode(object parent, FieldOrProperty member, NodeAttributes attributes) : base(parent, member)
        {
            _memberFieldHandle = member;
            _attributes = attributes;

            RebuildListChildren();
        }

        public override void CreateContextMenuItems(ContextMenuStrip contextMenu, Action refreshTreeCallback)
        {
            base.CreateContextMenuItems(contextMenu, refreshTreeCallback);

            if (contextMenu.Items.Count > 0)
                contextMenu.Items.Add(new ToolStripSeparator());

            var addItem = new ToolStripMenuItem();
            addItem.Text = $"Add New Element";
            addItem.Click += (o, e) => { InsertListElement(GetListLength()); rebuildAll(); };
            contextMenu.Items.Add(addItem);

            var removeAllItem = new ToolStripMenuItem();
            removeAllItem.Text = $"Remove All Elements";
            removeAllItem.Click += (o, e) => { GetList().Clear(); rebuildAll(); };
            contextMenu.Items.Add(removeAllItem);

            void rebuildAll()
            {
                RebuildListChildren();
                refreshTreeCallback();
            }
        }

        public IList GetList()
        {
            return _memberFieldHandle.GetValue<IList>(ParentObject);
        }

        public int GetListLength()
        {
            return GetList()?.Count ?? 0;
        }

        private void InsertListElement(int index)
        {
            GetList().Insert(index, Activator.CreateInstance(GetContainedType()));
            RebuildListChildren();
        }

        private void RemoveListElement(int index)
        {
            GetList().RemoveAt(index);
            RebuildListChildren();
        }

        private void RebuildListChildren()
        {
            _children = new Lazy<List<TreeDataNode>>(() =>
            {
                var list = new List<TreeDataNode>();
                var asList = GetList();

                if (!_attributes.HasFlag(NodeAttributes.HideChildren) && asList != null)
                {
                    var enumerableTemplateType = GetContainedType();

                    // Array elements act as children
                    for (int i = 0; i < asList.Count; i++)
                        list.Add(new IndexNode(this, i, enumerableTemplateType));
                }

                return list;
            });
        }

        private Type GetContainedType()
        {
            // Fetch the type of T from IList<T>
            return GetList()
                .GetType()
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GenericTypeArguments.Length == 1)
                .Single(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .GenericTypeArguments[0];
        }
    }
}
