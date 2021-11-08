using HZDCoreEditorUI.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HZDCoreEditorUI.UI
{
    public class TreeDataArrayNode : TreeDataNode
    {
        public override object Value => $"Array<{TypeName}> ({GetArrayLength()})";

        public override bool HasChildren => GetArrayLength() > 0;
        public override List<TreeDataNode> Children => _children.Value;

        private readonly FieldOrProperty _memberFieldHandle;
        private readonly NodeAttributes _attributes;
        private Lazy<List<TreeDataNode>> _children;

        #region Indexer node handler
        public class IndexNode : TreeDataNode
        {
            public override object Value
            {
                get => ((Array)ParentObject).GetValue(_arrayIndex);
                set => ((Array)ParentObject).SetValue(value, _arrayIndex);
            }

            public override bool IsEditable => _objectWrapperNode.IsEditable;

            public override bool HasChildren => _objectWrapperNode.HasChildren;
            public override List<TreeDataNode> Children => _objectWrapperNode.Children;

            private readonly TreeDataArrayNode _parentNode;
            private readonly int _arrayIndex;
            private readonly TreeDataNode _objectWrapperNode;

            public IndexNode(TreeDataArrayNode parentNode, int index, Type arrayElementType) : base(parentNode.GetArray(), arrayElementType)
            {
                Name = $"[{index}]";

                _parentNode = parentNode;
                _arrayIndex = index;
                _objectWrapperNode = CreateNode(this, new FieldOrProperty(GetType(), nameof(Value)), arrayElementType);
            }

            public override Control CreateEditControl(Rectangle bounds)
            {
                return _objectWrapperNode.CreateEditControl(bounds);
            }

            public override void CreateContextMenuItems(ContextMenuStrip contextMenu, Action refreshTreeCallback)
            {
                base.CreateContextMenuItems(contextMenu, refreshTreeCallback);

                if (contextMenu.Items.Count > 0)
                    contextMenu.Items.Add(new ToolStripSeparator());

                var insertItem = new ToolStripMenuItem();
                insertItem.Text = $"Insert Element at {Name}";
                insertItem.Click += (o, e) => { _parentNode.InsertArrayElement(_arrayIndex); refreshTreeCallback(); };
                contextMenu.Items.Add(insertItem);

                var removeItem = new ToolStripMenuItem();
                removeItem.Text = $"Remove Element at {Name}";
                removeItem.Click += (o, e) => { _parentNode.RemoveArrayElement(_arrayIndex); refreshTreeCallback(); };
                contextMenu.Items.Add(removeItem);
            }

            public override bool FinishEditControl(Control control, Action refreshTreeCallback)
            {
                return _objectWrapperNode.FinishEditControl(control, refreshTreeCallback);
            }
        }
        #endregion

        public TreeDataArrayNode(object parent, FieldOrProperty member, NodeAttributes attributes) : base(parent, member)
        {
            _memberFieldHandle = member;
            _attributes = attributes;

            RebuildArrayChildren();
        }

        public override void CreateContextMenuItems(ContextMenuStrip contextMenu, Action refreshTreeCallback)
        {
            base.CreateContextMenuItems(contextMenu, refreshTreeCallback);

            if (contextMenu.Items.Count > 0)
                contextMenu.Items.Add(new ToolStripSeparator());

            var addItem = new ToolStripMenuItem();
            addItem.Text = $"Add New Element";
            addItem.Click += (o, e) => { InsertArrayElement(GetArrayLength()); rebuildAll(); };
            contextMenu.Items.Add(addItem);

            var removeAllItem = new ToolStripMenuItem();
            removeAllItem.Text = $"Remove All Elements";
            removeAllItem.Click += (o, e) => { _memberFieldHandle.SetValue(ParentObject, Array.CreateInstance(GetContainedType(), 0)); rebuildAll(); };
            contextMenu.Items.Add(removeAllItem);

            void rebuildAll()
            {
                RebuildArrayChildren();
                refreshTreeCallback();
            }
        }

        public Array GetArray()
        {
            return _memberFieldHandle.GetValue<Array>(ParentObject);
        }

        public int GetArrayLength()
        {
            return GetArray()?.Length ?? 0;
        }

        private void InsertArrayElement(int index)
        {
            var array = GetArray();
            var newArray = Array.CreateInstance(GetContainedType(), array.Length + 1);

            Array.Copy(array, 0, newArray, 0, index);
            Array.Copy(array, index, newArray, index + 1, array.Length - index);
            newArray.SetValue(Activator.CreateInstance(GetContainedType()), index);

            _memberFieldHandle.SetValue(ParentObject, newArray);
            RebuildArrayChildren();
        }

        private void RemoveArrayElement(int index)
        {
            var array = GetArray();
            var newArray = Array.CreateInstance(GetContainedType(), array.Length - 1);

            Array.Copy(array, 0, newArray, 0, index);
            Array.Copy(array, index + 1, newArray, index, array.Length - index - 1);

            _memberFieldHandle.SetValue(ParentObject, newArray);
            RebuildArrayChildren();
        }

        private void RebuildArrayChildren()
        {
            _children = new Lazy<List<TreeDataNode>>(() =>
            {
                var nodes = new List<TreeDataNode>();
                var array = GetArray();

                if (!_attributes.HasFlag(NodeAttributes.HideChildren) && array != null)
                {
                    var elementType = GetContainedType();

                    // Array entries act as children
                    for (int i = 0; i < array.Length; i++)
                        nodes.Add(new IndexNode(this, i, elementType));
                }

                return nodes;
            });
        }

        private Type GetContainedType()
        {
            return GetArray().GetType().GetElementType();
        }
    }
}
