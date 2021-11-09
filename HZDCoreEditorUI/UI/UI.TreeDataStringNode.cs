namespace HZDCoreEditorUI.UI;

using System;
using System.Drawing;
using System.Windows.Forms;
using HZDCoreEditorUI.Util;

public class TreeDataStringNode : TreeDataClassMemberNode
{
    public TreeDataStringNode(object parent, FieldOrProperty member, NodeAttributes attributes)
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
            // Call the 'string' constructor for this type
            Value = Activator.CreateInstance(GetContainedValueType(), textValue);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, $"Failed to update a string?! A constructor is probably missing.");
            return false;
        }
        finally
        {
            control.Dispose();
        }

        return true;
    }
}
