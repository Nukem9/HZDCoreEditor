namespace HZDCoreEditorUI.UI;

using System;
using System.Collections.Generic;
using System.Reflection;
using HZDCoreEditorUI.Util;

public class TreeDataClassMemberNode : TreeDataNode
{
    private readonly FieldOrProperty _memberFieldHandle;
    private readonly bool _allowEditing;

    public TreeDataClassMemberNode(object parent, FieldOrProperty member, NodeAttributes attributes)
        : base(parent, member)
    {
        _memberFieldHandle = member;
        _allowEditing = !attributes.HasFlag(NodeAttributes.DisableEditing);

        if (!attributes.HasFlag(NodeAttributes.HideChildren))
        {
            Children = new List<TreeDataNode>();
            AddObjectChildren();
        }
    }

    public override object Value
    {
        get => _memberFieldHandle.GetValue(ParentObject);
        set => _memberFieldHandle.SetValue(ParentObject, value);
    }

    public override bool IsEditable => _allowEditing;

    public override bool HasChildren => Children?.Count > 0;

    public override List<TreeDataNode> Children { get; }

    private void AddObjectChildren()
    {
        var objectInstance = _memberFieldHandle.GetValue(ParentObject);
        var objectType = objectInstance?.GetType();

        if (Type.GetTypeCode(objectType) != TypeCode.Object)
            return;

        // Class member variables act as children
        var fields = objectType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var field in fields)
            Children.Add(CreateNode(objectInstance, new FieldOrProperty(field)));
    }
}
