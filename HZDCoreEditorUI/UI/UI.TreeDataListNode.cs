namespace HZDCoreEditorUI.UI;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HZDCoreEditorUI.Util;

public partial class TreeDataListNode : TreeDataNode
{
    private readonly FieldOrProperty _memberFieldHandle;
    private readonly NodeAttributes _attributes;
    private Lazy<List<TreeDataNode>> _children;

    public TreeDataListNode(object parent, FieldOrProperty member, NodeAttributes attributes)
        : base(parent, member)
    {
        _memberFieldHandle = member;
        _attributes = attributes;

        RebuildListChildren();
    }

    public override object Value => $"{TypeName} ({GetListLength()})";

    public override bool HasChildren => GetListLength() > 0;

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
            InsertListElement(GetListLength());
            RebuildAll();
        };
        contextMenu.Items.Add(addItem);

        var removeAllItem = new ToolStripMenuItem();
        removeAllItem.Text = $"Remove All Elements";
        removeAllItem.Click += (o, e) =>
        {
            GetList().Clear();
            RebuildAll();
        };
        contextMenu.Items.Add(removeAllItem);

        void RebuildAll()
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
