using Utility;

namespace HZDCoreEditorUI.UI
{
    public class TreeDataStringNode : TreeDataClassMemberNode
    {
        public TreeDataStringNode(object parent, FieldOrProperty member, NodeAttributes attributes) : base(parent, member, attributes | NodeAttributes.HideChildren)
        {
        }
    }
}
