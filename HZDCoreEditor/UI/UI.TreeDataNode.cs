using System;
using System.Collections.Generic;
using Utility;

namespace HZDCoreEditor.UI
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
        public virtual object Value { get { return "UNTYPED MEMBER FIELD"; } }
        public virtual string TypeName { get; protected set; }

        public virtual bool HasChildren { get { return false; } }
        public virtual List<TreeDataNode> Children { get { return null; } }

        public virtual bool IsEditable { get { return true; } }

        protected TreeDataNode()
        {
        }

        private static bool TypeInherits(Type objectType, Type baseType)
        {
            while (objectType != null)
            {
                if (objectType == baseType)
                    return true;

                objectType = objectType.BaseType;
            }

            return false;
        }

        private static bool TypeInheritsGeneric(Type objectType, Type genericType)
        {
            while (objectType != null)
            {
                if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == genericType)
                    return true;

                objectType = objectType.BaseType;
            }

            return false;
        }

        public static TreeDataNode CreateNode(object parent, FieldOrProperty member, NodeAttributes attributes = NodeAttributes.None)
        {
            return CreateNode(parent, member, member.GetMemberType(), attributes);
        }

        public static TreeDataNode CreateNode(object parent, FieldOrProperty member, Type overrideType, NodeAttributes attributes = NodeAttributes.None)
        {
            if (overrideType.IsGenericType)
            {
                var generic = overrideType.GetGenericTypeDefinition();

                if (TypeInheritsGeneric(generic, typeof(Decima.BaseArray<>)) ||
                    TypeInheritsGeneric(generic, typeof(List<>)))
                    return new TreeDataListNode(parent, member, attributes);

                if (TypeInheritsGeneric(generic, typeof(Decima.BaseRef<>)) ||
                    TypeInheritsGeneric(generic, typeof(Decima.BaseStreamingRef<>)) ||
                    TypeInheritsGeneric(generic, typeof(Decima.BaseUUIDRef<>)) ||
                    TypeInheritsGeneric(generic, typeof(Decima.BaseCPtr<>)) ||
                    TypeInheritsGeneric(generic, typeof(Decima.BaseWeakPtr<>)))
                    return new TreeDataRefNode(parent, member, attributes);
            }

            if (overrideType.IsArray)
                return new TreeDataArrayNode(parent, member, attributes);

            if (TypeInherits(overrideType, typeof(Decima.BaseGGUUID)))
                return new TreeDataGUIDNode(parent, member, attributes);

            if (TypeInherits(overrideType, typeof(Decima.BaseString)) || TypeInherits(overrideType, typeof(Decima.BaseWString)))
                return new TreeDataStringNode(parent, member, attributes);

            return new TreeDataClassMemberNode(parent, member, attributes);
        }

        public static void SetupTree(BrightIdeasSoftware.TreeListView treeListView, object dataObject)
        {
            treeListView.CanExpandGetter = CanExpandGetter;
            treeListView.ChildrenGetter = ChildrenGetter;
            treeListView.CellEditStarting += CellEditStartingHandler;

            // Create columns
            var nameCol = new BrightIdeasSoftware.OLVColumn("Name", nameof(Name))
            {
                Width = 300,
                IsEditable = false,
            };
            treeListView.Columns.Add(nameCol);

            var valueCol = new BrightIdeasSoftware.OLVColumn("Value", nameof(Value))
            {
                Width = 500,
            };
            treeListView.Columns.Add(valueCol);

            var typeCol = new BrightIdeasSoftware.OLVColumn("Type", nameof(TypeName))
            {
                Width = 200,
                IsEditable = false,
            };
            treeListView.Columns.Add(typeCol);

            // Create a dummy root object to hold the top-level class members
            var rootObject = new TreeDataObjectNode(dataObject, "unnamed_dummy_root_node", NodeAttributes.None);
            treeListView.Roots = rootObject.Children;
        }

        private static bool CanExpandGetter(object model)
        {
            return (model as TreeDataNode).HasChildren;
        }

        private static IEnumerable<TreeDataNode> ChildrenGetter(object model)
        {
            return (model as TreeDataNode).Children;
        }

        private static void CellEditStartingHandler(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            var m = e.RowObject as TreeDataNode;

            if (!m.IsEditable)
                e.Cancel = true;
        }
    }
}
