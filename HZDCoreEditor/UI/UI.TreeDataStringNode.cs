using Utility;

namespace HZDCoreEditor.UI
{
    public class TreeDataStringNode : TreeDataClassMemberNode
    {
        public TreeDataStringNode(object parent, FieldOrProperty member, NodeAttributes attributes) : base(parent, member, attributes | NodeAttributes.HideChildren)
        {
        }
    }
}
