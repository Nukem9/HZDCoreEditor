using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HZDCoreEditor.UI
{
    public class TreeDataListNode : TreeDataNode
    {
        public override object Value { get { return $"{TypeName} ({GetListLength()})"; } }

        public override bool HasChildren => GetListLength() > 0;
        public override List<TreeDataNode> Children { get; }
        public override bool IsEditable => false;

        private readonly object ParentObject;
        private readonly FieldOrProperty ParentFieldEntry;

        public TreeDataListNode(object parent, FieldOrProperty member, NodeAttributes attributes)
        {
            Name = member.GetName();
            TypeName = Decima.RTTI.GetTypeNameString(member.GetMemberType()).Replace("_", "<");

            // Pad template brackets on the right hand side
            for (int i = 0; i < TypeName.Where(x => x == '<').Count(); i++)
                TypeName += '>';

            Children = new List<TreeDataNode>();
            ParentObject = parent;
            ParentFieldEntry = member;

            if (!attributes.HasFlag(NodeAttributes.HideChildren))
            {
                Children = new List<TreeDataNode>();
                AddListChildren();
            }
        }

        private IList GetList()
        {
            return ParentFieldEntry.GetValue<IList>(ParentObject);
        }

        private int GetListLength()
        {
            return GetList()?.Count ?? 0;
        }

        private void AddListChildren()
        {
            var asList = GetList();

            if (asList != null)
            {
                // Fetch the type of T from IList<T>
                var enumerableTemplateType = asList.GetType()
                    .GetInterfaces()
                    .Where(i => i.IsGenericType && i.GenericTypeArguments.Length == 1)
                    .Single(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    .GenericTypeArguments[0];

                // Array elements act as children
                for (int i = 0; i < asList.Count; i++)
                    Children.Add(new TreeDataListIndexNode(asList, i, enumerableTemplateType));
            }
        }
    }

    public class TreeDataListIndexNode : TreeDataNode
    {
        public override object Value { get { return ObjectWrapper?.ToString(); } }

        public override bool HasChildren => ObjectWrapperNode.HasChildren;
        public override List<TreeDataNode> Children => ObjectWrapperNode.Children;
        public override bool IsEditable => ObjectWrapperNode.IsEditable;

        private readonly IList ParentObject;
        private readonly int ParentArrayIndex;
        private TreeDataNode ObjectWrapperNode;

        // Property is needed in order to get a FieldOrProperty handle
        private object ObjectWrapper
        {
            get { return ParentObject[ParentArrayIndex]; }
            set { ParentObject[ParentArrayIndex] = value; }
        }

        public TreeDataListIndexNode(IList parent, int index, Type elementType)
        {
            Name = $"[{index}]";
            TypeName = elementType.Name;

            ParentObject = parent;
            ParentArrayIndex = index;

            AddObjectChildren(elementType);
        }

        private void AddObjectChildren(Type elementType)
        {
            ObjectWrapperNode = CreateNode(this, new FieldOrProperty(GetType(), nameof(ObjectWrapper)), elementType);
        }
    }
}
