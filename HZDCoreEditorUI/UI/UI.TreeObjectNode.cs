namespace HZDCoreEditorUI.UI
{
    using System.Collections.Generic;
    using System.Reflection;
    using HZDCoreEditorUI.Util;

    public class TreeObjectNode
    {
        private readonly FieldInfo _nameFieldInfo;
        private readonly FieldInfo _uuidFieldInfo;

        public TreeObjectNode(string category, List<object> childObjects)
        {
            TypeName = category;
            UnderlyingObject = null;
            Children = new List<TreeObjectNode>();

            _nameFieldInfo = null;
            _uuidFieldInfo = null;

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
            _uuidFieldInfo = objectType.GetField("ObjectUUID");
        }

        public string Name => _nameFieldInfo?.GetValue(UnderlyingObject).ToString();

        public string UUID => _uuidFieldInfo?.GetValue(UnderlyingObject)?.ToString();

        public string TypeName { get; private set; }

        public object UnderlyingObject { get; private set; }

        public List<TreeObjectNode> Children { get; private set; }
    }
}
