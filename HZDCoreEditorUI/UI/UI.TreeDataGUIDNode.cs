using Utility;

namespace HZDCoreEditorUI.UI
{
    public class TreeDataGUIDNode : TreeDataClassMemberNode
    {
        public TreeDataGUIDNode(object parent, FieldOrProperty member, NodeAttributes attributes) : base(parent, member, attributes | NodeAttributes.HideChildren)
        {
        }
    }
}
