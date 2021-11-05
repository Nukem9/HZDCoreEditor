using HZDCoreEditorUI.Util;
using System.Collections.Generic;
using System.Reflection;

namespace HZDCoreEditorUI.UI
{
    public class TreeDataRefNode : TreeDataNode
    {
        public override object Value => _memberFieldHandle.GetValue(ParentObject);

        public override bool HasChildren => Children.Count > 0;
        public override List<TreeDataNode> Children { get; }

        private readonly FieldOrProperty _memberFieldHandle;

        public TreeDataRefNode(object parent, FieldOrProperty member, NodeAttributes attributes) : base(parent, member)
        {
            _memberFieldHandle = member;

            if (!attributes.HasFlag(NodeAttributes.HideChildren))
            {
                Children = new List<TreeDataNode>();
                AddObjectChildren();
            }
        }

        private void AddObjectChildren()
        {
            // Ref objects may be null while loading SaveState classes
            var objectInstance = Value;

            if (objectInstance != null)
            {
                var fields = objectInstance.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var field in fields)
                    Children.Add(CreateNode(objectInstance, new FieldOrProperty(field), NodeAttributes.HideChildren));
            }
        }
    }
}
