using HZDCoreEditorUI.Util;
using System;
using System.Collections.Generic;

namespace HZDCoreEditorUI.UI
{
    public class TreeDataNode
    {
        [Flags]
        public enum NodeAttributes
        {
            None = 0,
            HideChildren = 1,
            DisableEditing = 2,
        }

        public virtual string Name { get; protected set; }
        public virtual object Value => "UNTYPED MEMBER FIELD";
        public virtual string Category { get; protected set; }
        public virtual string TypeName { get; protected set; }

        public virtual bool HasChildren => false;
        public virtual List<TreeDataNode> Children => null;

        public object ParentObject { get; protected set; }

        public virtual bool IsEditable => true;

        protected TreeDataNode(object parent)
        {
            ParentObject = parent;
        }

        protected TreeDataNode(object parent, FieldOrProperty member) : this(parent)
        {
            Name = member.GetName();
            Category = member.GetCategory();
            TypeName = member.GetMemberType().GetFriendlyName();
        }

        public static TreeDataNode CreateNode(object parent, FieldOrProperty member, NodeAttributes attributes = NodeAttributes.None)
        {
            return CreateNode(parent, member, member.GetMemberType(), attributes);
        }

        public static TreeDataNode CreateNode(object parent, FieldOrProperty member, Type overrideType, NodeAttributes attributes = NodeAttributes.None)
        {
            if (overrideType.IsGenericType)
            {
                var type = overrideType.GetGenericTypeDefinition();

                if (type.InheritsGeneric(typeof(Decima.BaseArray<>)) ||
                    type.InheritsGeneric(typeof(List<>)))
                    return new TreeDataListNode(parent, member, attributes);

                if (type.Inherits(typeof(Decima.BaseRef)) ||
                    type.InheritsGeneric(typeof(Decima.BaseStreamingRef<>)) ||
                    type.InheritsGeneric(typeof(Decima.BaseUUIDRef<>)) ||
                    type.InheritsGeneric(typeof(Decima.BaseCPtr<>)) ||
                    type.InheritsGeneric(typeof(Decima.BaseWeakPtr<>)))
                    return new TreeDataRefNode(parent, member, attributes);
            }

            if (overrideType.IsArray)
                return new TreeDataArrayNode(parent, member, attributes);

            if (overrideType.Inherits(typeof(Decima.BaseGGUUID)))
                return new TreeDataGUIDNode(parent, member, attributes);

            if (overrideType.Inherits(typeof(Decima.BaseString)) || overrideType.Inherits(typeof(Decima.BaseWString)))
                return new TreeDataStringNode(parent, member, attributes);

            return new TreeDataClassMemberNode(parent, member, attributes);
        }
    }
}
