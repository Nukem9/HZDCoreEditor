namespace HZDCoreEditorUI.UI;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using HZDCoreEditorUI.Util;

public partial class TreeDataListNode
{
    public class IndexNode : TreeDataNode
    {
        private readonly TreeDataListNode _parentNode;
        private readonly int _arrayIndex;
        private readonly TreeDataNode _objectWrapperNode;

        public IndexNode(TreeDataListNode parentNode, int index, Type listElementType)
            : base(parentNode.GetList(), listElementType)
        {
            Name = $"[{index}]";

            _parentNode = parentNode;
            _arrayIndex = index;
            _objectWrapperNode = CreateNode(this, new FieldOrProperty(GetType(), nameof(Value)), listElementType);
        }

        public override object Value
        {
            get => ((IList)ParentObject)[_arrayIndex];
            set => ((IList)ParentObject)[_arrayIndex] = value;
        }

        public override bool IsEditable => _objectWrapperNode.IsEditable;

        public override bool HasChildren => _objectWrapperNode.HasChildren;

        public override List<TreeDataNode> Children => _objectWrapperNode.Children;

        public override void CreateContextMenuItems(ContextMenuStrip contextMenu, Action refreshTreeCallback)
        {
            base.CreateContextMenuItems(contextMenu, refreshTreeCallback);

            if (contextMenu.Items.Count > 0)
                contextMenu.Items.Add(new ToolStripSeparator());

            var insertItem = new ToolStripMenuItem();
            insertItem.Text = $"Insert Element at {Name}";
            insertItem.Click += (o, e) =>
            {
                _parentNode.InsertListElement(_arrayIndex);
                refreshTreeCallback();
            };
            contextMenu.Items.Add(insertItem);

            var removeItem = new ToolStripMenuItem();
            removeItem.Text = $"Remove Element at {Name}";
            removeItem.Click += (o, e) =>
            {
                _parentNode.RemoveListElement(_arrayIndex);
                refreshTreeCallback();
            };
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
}
