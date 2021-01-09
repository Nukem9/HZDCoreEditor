using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HZDCoreEditorUI.Util;

namespace HZDCoreEditorUI.UI
{
    class TreeObjectNode
    {
        public string Name => NameFieldInfo?.GetValue(UnderlyingObject).ToString();
        public string UUID => UUIDFieldInfo?.GetValue(UnderlyingObject)?.ToString();
        public string TypeName { get; private set; }
        public object UnderlyingObject { get; private set; }
        public List<TreeObjectNode> Children { get; private set; }

        private readonly FieldInfo NameFieldInfo;
        private readonly FieldInfo UUIDFieldInfo;

        private TreeObjectNode(string category, List<object> childObjects)
        {
            TypeName = category;
            UnderlyingObject = null;
            Children = new List<TreeObjectNode>();
            NameFieldInfo = null;
            UUIDFieldInfo = null;

            foreach (var obj in childObjects)
                Children.Add(new TreeObjectNode(obj));
        }

        private TreeObjectNode(object containedObject)
        {
            var objectType = containedObject.GetType();

            TypeName = objectType.GetFriendlyName();
            UnderlyingObject = containedObject;
            Children = null;

            NameFieldInfo = objectType.GetField("Name");
            UUIDFieldInfo = objectType.GetField("ObjectUUID");
        }

        public static void SetupTree(BrightIdeasSoftware.TreeListView treeListView, List<object> childObjects)
        {
            treeListView.CanExpandGetter = CanExpandGetter;
            treeListView.ChildrenGetter = ChildrenGetter;

            // Create columns
            var typeCol = new BrightIdeasSoftware.OLVColumn("Object", nameof(TypeName))
            {
                Width = 200,
                IsEditable = false,
            };
            treeListView.Columns.Add(typeCol);

            var nameCol = new BrightIdeasSoftware.OLVColumn("Name", nameof(Name))
            {
                Width = 200,
                IsEditable = false,
            };
            treeListView.Columns.Add(nameCol);

            var uuidCol = new BrightIdeasSoftware.OLVColumn("UUID", nameof(UUID))
            {
                Width = 300,
                IsEditable = false,
            };
            treeListView.Columns.Add(uuidCol);

            // Sort object list into each category based on the type name
            var categorizedObjects = new Dictionary<string, List<object>>();

            foreach (var obj in childObjects)
            {
                string typeString = obj.GetType().Name;

                if (!categorizedObjects.TryGetValue(typeString, out List<object> categoryList))
                {
                    categoryList = new List<object>();
                    categorizedObjects.Add(typeString, categoryList);
                }

                categoryList.Add(obj);
            }

            // Register list view categories
            var treeViewRoots = new List<TreeObjectNode>();

            foreach (string key in categorizedObjects.Keys.OrderBy(x => x))
                treeViewRoots.Add(new TreeObjectNode(key, categorizedObjects[key]));

            treeListView.Roots = treeViewRoots;
        }

        private static bool CanExpandGetter(object model)
        {
            return (model as TreeObjectNode).Children != null;
        }

        private static IEnumerable<TreeObjectNode> ChildrenGetter(object model)
        {
            return (model as TreeObjectNode).Children;
        }
    }
}
