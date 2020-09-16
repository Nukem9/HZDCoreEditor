using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace HZDCoreEditor
{
    public partial class FormCoreView : Form
    {
        private readonly List<object> CoreObjectList;

        class TreeObjectHolderNode
        {
            public string Name => NameFieldInfo?.GetValue(UnderlyingObject).ToString();
            public string UUID => UUIDFieldInfo?.GetValue(UnderlyingObject).ToString();
            public string TypeName { get; private set; }
            public object UnderlyingObject { get; private set; }
            public List<TreeObjectHolderNode> Children { get; private set; }

            private readonly FieldInfo NameFieldInfo;
            private readonly FieldInfo UUIDFieldInfo;

            private TreeObjectHolderNode(string category, List<object> childObjects)
            {
                TypeName = category;
                UnderlyingObject = null;
                Children = new List<TreeObjectHolderNode>();
                NameFieldInfo = null;
                UUIDFieldInfo = null;

                foreach (var obj in childObjects)
                    Children.Add(new TreeObjectHolderNode(obj));
            }

            private TreeObjectHolderNode(object containedObject)
            {
                var objectType = containedObject.GetType();

                TypeName = objectType.Name;
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
                var typeCol = new BrightIdeasSoftware.OLVColumn("Object", nameof(TypeName));
                typeCol.Width = 300;
                treeListView.Columns.Add(typeCol);

                var nameCol = new BrightIdeasSoftware.OLVColumn("Name", nameof(Name));
                treeListView.Columns.Add(nameCol);

                var uuidCol = new BrightIdeasSoftware.OLVColumn("UUID", nameof(UUID));
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
                var treeViewRoots = new List<TreeObjectHolderNode>();

                foreach (string key in categorizedObjects.Keys.OrderBy(x => x))
                    treeViewRoots.Add(new TreeObjectHolderNode(key, categorizedObjects[key]));

                treeListView.Roots = treeViewRoots;
            }

            private static bool CanExpandGetter(object model)
            {
                return (model as TreeObjectHolderNode).Children != null;
            }

            private static IEnumerable<TreeObjectHolderNode> ChildrenGetter(object model)
            {
                return (model as TreeObjectHolderNode).Children;
            }
        }

        public class TreeDataMemberHolderNode
        {
            public string Name { get; private set; }
            public string Value { get { return UnderlyingField.GetValue(UnderlyingObject)?.ToString(); } }
            public string TypeName { get; private set; }
            public List<TreeDataMemberHolderNode> Children { get; private set; }

            private readonly object UnderlyingObject;
            private readonly FieldInfo UnderlyingField;

            public TreeDataMemberHolderNode(object parent, FieldInfo field)
            {
                Name = field.Name;
                TypeName = field.FieldType.Name;
                Children = new List<TreeDataMemberHolderNode>();

                UnderlyingObject = parent;
                UnderlyingField = field;
            }

            public static bool CanExpandGetter(object model)
            {
                return (model as TreeDataMemberHolderNode).Children.Count > 0;
            }

            public static IEnumerable<TreeDataMemberHolderNode> ChildrenGetter(object model)
            {
                return (model as TreeDataMemberHolderNode).Children;
            }

            public static void SetupTree(BrightIdeasSoftware.TreeListView treeListView, object dataObject)
            {
                treeListView.CanExpandGetter = CanExpandGetter;
                treeListView.ChildrenGetter = ChildrenGetter;
                
                // Create columns
                var nameCol = new BrightIdeasSoftware.OLVColumn("Name", nameof(Name));
                nameCol.Width = 300;
                treeListView.Columns.Add(nameCol);

                var valueCol = new BrightIdeasSoftware.OLVColumn("Value", nameof(Value));
                valueCol.Width = 500;
                treeListView.Columns.Add(valueCol);

                var typeCol = new BrightIdeasSoftware.OLVColumn("Type", nameof(TypeName));
                typeCol.Width = 200;
                treeListView.Columns.Add(typeCol);

                // Children are the class member variables
                var classType = dataObject.GetType();
                var fields = classType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                var treeViewRoots = new List<TreeDataMemberHolderNode>();

                foreach (var field in fields)
                {
                    var entry = new TreeDataMemberHolderNode(dataObject, field);

                    treeViewRoots.Add(entry);
                }

                treeListView.Roots = treeViewRoots;
            }
        }

        private BrightIdeasSoftware.TreeListView TV1;
        private BrightIdeasSoftware.TreeListView TV2;

        public FormCoreView(List<object> coreObjectList)
        {
            CoreObjectList = coreObjectList;
            InitializeComponent();
        }

        private void FormCoreView_Load(object sender, EventArgs e)
        {
            BuildObjectView();
            BuildDataView();
        }

        private void BuildObjectView()
        {
            var treeListView = new BrightIdeasSoftware.TreeListView();
            TV1 = treeListView;
            treeListView.FullRowSelect = true;
            treeListView.Dock = DockStyle.Fill;

            treeListView.ItemSelectionChanged += TreeListView_ItemSelected;

            TreeObjectHolderNode.SetupTree(treeListView, CoreObjectList);

            splitContainer.Panel1.Controls.Add(treeListView);
        }

        private void TreeListView_ItemSelected(object sender, EventArgs e)
        {
            var underlying = (TV1.SelectedObject as TreeObjectHolderNode)?.UnderlyingObject;

            if (underlying != null)
            {
                TV2.Clear();
                TreeDataMemberHolderNode.SetupTree(TV2, underlying);
            }
        }

        private void BuildDataView()
        {
            var treeListView = new BrightIdeasSoftware.TreeListView();
            TV2 = treeListView;
            treeListView.Dock = DockStyle.Fill;

            TreeDataMemberHolderNode.SetupTree(treeListView, CoreObjectList[0]);

            splitContainer.Panel2.Controls.Add(treeListView);
        }
    }
}
