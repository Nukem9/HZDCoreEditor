using HZDCoreEditorUI.Util;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HZDCoreEditorUI.UI
{
    public class TreeDataStringNode : TreeDataClassMemberNode
    {
        public TreeDataStringNode(object parent, FieldOrProperty member, NodeAttributes attributes) : base(parent, member, attributes | NodeAttributes.HideChildren)
        {
        }

        public override Control CreateEditControl(Rectangle bounds)
        {
            // Always use a text box
            return new TextBox
            {
                Bounds = bounds,
                Text = Value?.ToString() ?? "",
            };
        }

        public override bool FinishEditControl(Control control)
        {
            string textValue = ((TextBox)control).Text;

            try
            {
                // Call the BaseString copy constructor
                Value = Activator.CreateInstance(_memberType, textValue);
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
}
