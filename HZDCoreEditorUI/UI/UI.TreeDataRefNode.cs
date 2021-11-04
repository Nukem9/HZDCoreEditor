using HZDCoreEditorUI.Util;
using System.Collections.Generic;
using System.Reflection;

namespace HZDCoreEditorUI.UI
{
    public class TreeDataRefNode : TreeDataNode
    {
        public override object Value => _parentFieldEntry.GetValue(ParentObject);

        public override bool HasChildren => Children.Count > 0;
        public override List<TreeDataNode> Children { get; }
        public override bool IsEditable => false;

        private readonly FieldOrProperty _parentFieldEntry;

        public TreeDataRefNode(object parent, FieldOrProperty member, NodeAttributes attributes) : base(parent, member)
        {
            _parentFieldEntry = member;

            if (!attributes.HasFlag(NodeAttributes.HideChildren))
            {
                Children = new List<TreeDataNode>();
                AddObjectChildren();
            }
        }

        private void AddObjectChildren()
        {
            // Ref objects may be null while loading SaveState classes
            var objectInstance = _parentFieldEntry.GetValue(ParentObject);

            if (objectInstance != null)
            {
                var fields = objectInstance.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var field in fields)
                    Children.Add(CreateNode(objectInstance, new FieldOrProperty(field), NodeAttributes.HideChildren));
            }
        }
    }
}
