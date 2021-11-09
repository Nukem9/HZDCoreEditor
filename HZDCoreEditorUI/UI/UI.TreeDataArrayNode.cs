namespace HZDCoreEditorUI.UI;

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HZDCoreEditorUI.Util;

public partial class TreeDataArrayNode : TreeDataNode
{
    private readonly FieldOrProperty _memberFieldHandle;
    private readonly NodeAttributes _attributes;
    private Lazy<List<TreeDataNode>> _children;

    public TreeDataArrayNode(object parent, FieldOrProperty member, NodeAttributes attributes)
        : base(parent, member)
    {
        _memberFieldHandle = member;
        _attributes = attributes;

        RebuildArrayChildren();
    }

    public override object Value => $"Array<{TypeName}> ({GetArrayLength()})";

    public override bool HasChildren => GetArrayLength() > 0;

    public override List<TreeDataNode> Children => _children.Value;

    public override void CreateContextMenuItems(ContextMenuStrip contextMenu, Action refreshTreeCallback)
    {
        base.CreateContextMenuItems(contextMenu, refreshTreeCallback);

        if (contextMenu.Items.Count > 0)
            contextMenu.Items.Add(new ToolStripSeparator());

        var addItem = new ToolStripMenuItem();
        addItem.Text = $"Add New Element";
        addItem.Click += (o, e) =>
        {
            InsertArrayElement(GetArrayLength());
            RebuildAll();
        };
        contextMenu.Items.Add(addItem);

        var removeAllItem = new ToolStripMenuItem();
        removeAllItem.Text = $"Remove All Elements";
        removeAllItem.Click += (o, e) =>
        {
            _memberFieldHandle.SetValue(ParentObject, Array.CreateInstance(GetContainedType(), 0));
            RebuildAll();
        };
        contextMenu.Items.Add(removeAllItem);

        void RebuildAll()
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
