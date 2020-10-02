using System.Collections.Generic;
using System.Reflection;

namespace HZDCoreEditor.UI
{
    public class TreeDataNode
    {
        public virtual string Name { get; protected set; }
        public virtual string Value { get { return "UNTYPED MEMBER FIELD"; } }
        public virtual string TypeName { get; protected set; }

        public virtual bool HasChildren { get { return false; } }
        public virtual List<TreeDataNode> Children { get { return null; } }

        public static TreeDataNode CreateNode(object parent, FieldInfo field)
        {
            if (field.FieldType.IsGenericType)
            {
                var genericType = field.FieldType.GetGenericTypeDefinition();

                if (genericType == typeof(Decima.DS.Array<>) || genericType == typeof(Decima.HZD.Array<>) || genericType == typeof(List<>))
                    return new TreeDataListNode(parent, field);
            }

            if (field.FieldType.IsArray)
                return new TreeDataArrayNode(parent, field);

            return new TreeDataClassMemberNode(parent, field);
        }

        public static void SetupTree(BrightIdeasSoftware.TreeListView treeListView, object dataObject)
        {
            treeListView.CanExpandGetter = CanExpandGetter;
            treeListView.ChildrenGetter = ChildrenGetter;

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
            var rootObject = new TreeDataObjectNode(dataObject, "unnamed_dummy_root_node");
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
    }
}
