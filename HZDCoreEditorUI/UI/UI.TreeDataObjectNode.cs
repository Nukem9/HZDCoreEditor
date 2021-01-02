using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HZDCoreEditorUI.Util;

namespace HZDCoreEditorUI.UI
{
    public class TreeDataObjectNode : TreeDataNode
    {
        public override string Value { get { return ObjectInstance.ToString(); } }

        public override bool HasChildren => Children.Count > 0;
        public override List<TreeDataNode> Children { get; }

        private readonly object ObjectInstance;

        public TreeDataObjectNode(object instance, string name)
        {
            if (instance == null)
                throw new ArgumentNullException("Not supported. Use TreeDataClassMemberHolder instead if objects are potentially null.", nameof(instance));

            Name = name;
            TypeName = instance.GetType().GetFriendlyName();

            Children = new List<TreeDataNode>();
            ObjectInstance = instance;

            AddObjectChildren();
        }

        private void AddObjectChildren()
        {
            var objectType = ObjectInstance.GetType();

            if (Type.GetTypeCode(objectType) != TypeCode.Object)
                return;

            // Class member variales act as children
            var fields = objectType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
                Children.Add(CreateNode(ObjectInstance, field));
        }
    }
}
