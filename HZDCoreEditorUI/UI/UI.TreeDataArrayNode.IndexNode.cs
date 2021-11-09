namespace HZDCoreEditorUI.UI;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using HZDCoreEditorUI.Util;

public partial class TreeDataArrayNode
{
    public class IndexNode : TreeDataNode
    {
        private readonly TreeDataArrayNode _parentNode;
        private readonly int _arrayIndex;
        private readonly TreeDataNode _objectWrapperNode;

        public IndexNode(TreeDataArrayNode parentNode, int index, Type arrayElementType)
            : base(parentNode.GetArray(), arrayElementType)
        {
            Name = $"[{index}]";

            _parentNode = parentNode;
            _arrayIndex = index;
            _objectWrapperNode = CreateNode(this, new FieldOrProperty(GetType(), nameof(Value)), arrayElementType);
        }

        public override object Value
        {
            get => ((Array)ParentObject).GetValue(_arrayIndex);
            set => ((Array)ParentObject).SetValue(value, _arrayIndex);
        }

        public override bool IsEditable => _objectWrapperNode.IsEditable;

        public override bool HasChildren => _objectWrapperNode.HasChildren;

        public override List<TreeDataNode> Children => _objectWrapperNode.Children;

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
            insertItem.Click += (o, e) =>
            {
                _parentNode.InsertArrayElement(_arrayIndex);
                refreshTreeCallback();
            };
            contextMenu.Items.Add(insertItem);

            var removeItem = new ToolStripMenuItem();
            removeItem.Text = $"Remove Element at {Name}";
            removeItem.Click += (o, e) =>
            {
                _parentNode.RemoveArrayElement(_arrayIndex);
                refreshTreeCallback();
            };
            contextMenu.Items.Add(removeItem);
        }

        public override bool FinishEditControl(Control control, Action refreshTreeCallback)
        {
            return _objectWrapperNode.FinishEditControl(control, refreshTreeCallback);
        }
    }
}
