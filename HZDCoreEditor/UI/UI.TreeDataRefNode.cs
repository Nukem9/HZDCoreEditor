using System.Collections.Generic;
using System.Reflection;

namespace HZDCoreEditor.UI
{
    public class TreeDataRefNode : TreeDataNode
	{
		public override object Value { get { return ParentFieldEntry.GetValue(ParentObject); } }

		public override bool HasChildren => Children.Count > 0;
		public override List<TreeDataNode> Children { get; }
        public override bool IsEditable => false;

        private readonly object ParentObject;
		private readonly FieldOrProperty ParentFieldEntry;

		public TreeDataRefNode(object parent, FieldOrProperty member, NodeAttributes attributes)
		{
			Name = member.GetName();
			TypeName = member.GetMemberType().Name;

			ParentObject = parent;
			ParentFieldEntry = member;

            if (!attributes.HasFlag(NodeAttributes.HideChildren))
            {
                Children = new List<TreeDataNode>();
                AddObjectChildren();
            }
        }

		private void AddObjectChildren()
		{
			// Ref object shouldn't be null, skip checks for it
			var objectInstance = ParentFieldEntry.GetValue(ParentObject);
			var fields = objectInstance.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			foreach (var field in fields)
				Children.Add(CreateNode(objectInstance, new FieldOrProperty(field), NodeAttributes.HideChildren | NodeAttributes.DisableEditing));
		}
	}
}
