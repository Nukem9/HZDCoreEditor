namespace HZDCoreEditorUI.UI;

using System;
using System.Drawing;
using System.Windows.Forms;
using HZDCoreEditorUI.Util;

public class TreeDataGUIDNode : TreeDataClassMemberNode
{
    public TreeDataGUIDNode(object parent, FieldOrProperty member, NodeAttributes attributes)
        : base(parent, member, attributes | NodeAttributes.HideChildren)
    {
    }

    public override Control CreateEditControl(Rectangle bounds)
    {
        // Always use a text box
        return new TextBox
        {
            Bounds = bounds,
            Text = Value?.ToString() ?? string.Empty,
        };
    }

    public override bool FinishEditControl(Control control, Action refreshTreeCallback)
    {
        string textValue = ((TextBox)control).Text;

        try
        {
            var guid = Decima.BaseGGUUID.Empty;

            if (!string.IsNullOrEmpty(textValue))
                guid = Decima.BaseGGUUID.FromString(textValue);

            // Call the BaseGGUUID copy constructor
            Value = Activator.CreateInstance(GetContainedValueType(), guid);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, $"Failed to parse GUID for '{Name}'");
            return false;
        }
        finally
        {
            control.Dispose();
        }

        return true;
    }
}
