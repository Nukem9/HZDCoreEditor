using HZDCoreEditorUI.Util;
using System.Collections.Generic;
using System.Reflection;

namespace HZDCoreEditorUI.UI
{
    class TreeObjectNode
    {
        public string Name => _nameFieldInfo?.GetValue(UnderlyingObject).ToString();
        public string UUID => _UUIDFieldInfo?.GetValue(UnderlyingObject)?.ToString();
        public string TypeName { get; private set; }
        public object UnderlyingObject { get; private set; }
        public List<TreeObjectNode> Children { get; private set; }

        private readonly FieldInfo _nameFieldInfo;
        private readonly FieldInfo _UUIDFieldInfo;

        public TreeObjectNode(string category, List<object> childObjects)
        {
            TypeName = category;
            UnderlyingObject = null;
            Children = new List<TreeObjectNode>();

            _nameFieldInfo = null;
            _UUIDFieldInfo = null;

            foreach (var obj in childObjects)
                Children.Add(new TreeObjectNode(obj));
        }

        public TreeObjectNode(object containedObject)
        {
            var objectType = containedObject.GetType();

            TypeName = objectType.GetFriendlyName();
            UnderlyingObject = containedObject;
            Children = null;

            _nameFieldInfo = objectType.GetField("Name");
            _UUIDFieldInfo = objectType.GetField("ObjectUUID");
        }
    }
}
