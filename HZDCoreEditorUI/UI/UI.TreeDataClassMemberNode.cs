using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HZDCoreEditorUI.Util;

namespace HZDCoreEditorUI.UI
{
    public class TreeDataClassMemberNode : TreeDataNode
    {
        public override string Value { get { return ParentFieldEntry.GetValue(ParentObject)?.ToString(); } }

        public override bool HasChildren => Children.Count > 0;
        public override List<TreeDataNode> Children { get; }

        private readonly object ParentObject;
        private readonly FieldInfo ParentFieldEntry;

        public TreeDataClassMemberNode(object parent, FieldInfo field)
        {
            Name = field.Name;
            TypeName = field.FieldType.GetFriendlyName();

            Children = new List<TreeDataNode>();
            ParentObject = parent;
            ParentFieldEntry = field;

            AddObjectChildren();
        }

        private void AddObjectChildren()
        {
            var objectInstance = ParentFieldEntry.GetValue(ParentObject);
            var objectType = objectInstance?.GetType();

            if (Type.GetTypeCode(objectType) != TypeCode.Object)
                return;

            // Class member variales act as children
            var fields = objectType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
                Children.Add(CreateNode(objectInstance, field));
        }
    }
}
