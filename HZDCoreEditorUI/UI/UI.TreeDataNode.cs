namespace HZDCoreEditorUI.UI;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using HZDCoreEditorUI.Util;

public class TreeDataNode
{
    protected TreeDataNode(object parent, Type memberType)
    {
        TypeName = memberType.GetFriendlyName();
        ParentObject = parent;
    }

    protected TreeDataNode(object parent, FieldOrProperty member)
        : this(parent, member.GetMemberType())
    {
        Name = member.GetName();
        Category = member.GetCategory();
    }

    [Flags]
    public enum NodeAttributes
    {
        None = 0,
        HideChildren = 1,
        DisableEditing = 2,
    }

    public virtual string Name { get; protected set; }

    public virtual object Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public virtual string Category { get; protected set; }

    public virtual string TypeName { get; protected set; }

    public virtual bool IsEditable => false;

    public virtual bool HasChildren => false;

    public virtual List<TreeDataNode> Children => null;

    public object ParentObject { get; protected set; }

    public static TreeDataNode CreateNode(object parent, FieldOrProperty member, NodeAttributes attributes = NodeAttributes.None)
    {
        return CreateNode(parent, member, member.GetMemberType(), attributes);
    }

    public static TreeDataNode CreateNode(object parent, FieldOrProperty member, Type overrideType, NodeAttributes attributes = NodeAttributes.None)
    {
        if (overrideType.IsGenericType)
        {
            var type = overrideType.GetGenericTypeDefinition();

            if (type.InheritsGeneric(typeof(Decima.BaseArray<>)) ||
                type.InheritsGeneric(typeof(List<>)))
                return new TreeDataListNode(parent, member, attributes);

            if (type.Inherits(typeof(Decima.BaseRef)) ||
                type.InheritsGeneric(typeof(Decima.BaseStreamingRef<>)) ||
                type.InheritsGeneric(typeof(Decima.BaseUUIDRef<>)) ||
                type.InheritsGeneric(typeof(Decima.BaseCPtr<>)) ||
                type.InheritsGeneric(typeof(Decima.BaseWeakPtr<>)))
                return new TreeDataRefNode(parent, member, attributes);
        }

        if (overrideType.IsArray)
            return new TreeDataArrayNode(parent, member, attributes);

        if (overrideType.Inherits(typeof(Decima.BaseGGUUID)))
            return new TreeDataGUIDNode(parent, member, attributes);

        if (overrideType.Inherits(typeof(Decima.BaseString)) || overrideType.Inherits(typeof(Decima.BaseWString)))
            return new TreeDataStringNode(parent, member, attributes);

        return new TreeDataClassMemberNode(parent, member, attributes);
    }

    public virtual void CreateContextMenuItems(ContextMenuStrip contextMenu, Action refreshTreeCallback)
    {
    }

    public virtual Control CreateEditControl(System.Drawing.Rectangle bounds)
    {
        var targetType = GetContainedValueType();

        if (targetType.IsEnum)
        {
            // Enums converted to dropdown boxes
            var comboBox = new ComboBox
            {
                Bounds = bounds,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DisplayMember = nameof(KeyValuePair<int, int>.Key),
                ValueMember = nameof(KeyValuePair<int, int>.Value),
            };

            foreach (string name in Enum.GetNames(targetType))
            {
                var item = new KeyValuePair<string, object>(name, Enum.Parse(targetType, name));
                comboBox.Items.Add(item);

                if (item.Value.Equals(Value))
                    comboBox.SelectedItem = item;
            }

            return comboBox;
        }
        else if (targetType == typeof(bool))
        {
            // Bools converted to check boxes
            return new CheckBox
            {
                Bounds = bounds,
                Checked = (bool)Value,
                BackColor = System.Drawing.Color.FromArgb(240, 240, 240),
            };
        }
        else
        {
            // Everything else defaults to text
            return new TextBox
            {
                Bounds = bounds,
                Text = Value?.ToString() ?? string.Empty,
            };
        }
    }

    public virtual bool FinishEditControl(Control control, Action refreshTreeCallback)
    {
        var targetType = GetContainedValueType();

        // Save the values from any controls created in CreateEditControl
        try
        {
            if (targetType.IsEnum)
            {
                var kvp = (KeyValuePair<string, object>)((ComboBox)control).SelectedItem;

                Value = Enum.Parse(targetType, kvp.Key);
            }
            else if (targetType == typeof(bool))
            {
                Value = ((CheckBox)control).Checked;
            }
            else
            {
                var textValue = ((TextBox)control).Text;

                Value = TypeDescriptor.GetConverter(targetType).ConvertFromString(textValue);
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, $"Error setting '{Name}'");
            return false;
        }
        finally
        {
            control.Dispose();
        }

        return true;
    }

    protected Type GetContainedValueType()
    {
        // Even though typeof(Value) is 'object', we need the actual instance type
        return Value.GetType();
    }
}
