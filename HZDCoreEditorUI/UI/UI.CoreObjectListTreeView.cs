namespace HZDCoreEditorUI.UI
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using BrightIdeasSoftware;
    using HZDCoreEditorUI.Util;

    public class CoreObjectListTreeView : TreeListView
    {
        private readonly OLVColumn[] _defaultColumns;

        public CoreObjectListTreeView()
        {
            CanExpandGetter = CanExpandGetterHandler;
            ChildrenGetter = ChildrenGetterHandler;
            CellRightClick += CellRightClickHandler;

            // Columns are hardcoded. Keep them cached in case the view needs to be reset.
            _defaultColumns = new OLVColumn[3];

            _defaultColumns[0] = new OLVColumn("Object", nameof(TreeObjectNode.TypeName))
            {
                Width = 200,
                IsEditable = false,
            };

            _defaultColumns[1] = new OLVColumn("Name", nameof(TreeObjectNode.Name))
            {
                Width = 200,
                IsEditable = false,
            };

            _defaultColumns[2] = new OLVColumn("UUID", nameof(TreeObjectNode.UUID))
            {
                Width = 300,
                IsEditable = false,
            };

            CreateColumns();
        }

        public void RebuildTreeFromObjects(List<object> baseObjects)
        {
            // Sort object list into each category based on the type name
            var categorizedObjects = new Dictionary<string, List<object>>();

            foreach (var obj in baseObjects)
            {
                string typeString = obj.GetType().GetFriendlyName();

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

            Roots = treeViewRoots;
        }

        private void CreateColumns()
        {
            AllColumns.AddRange(_defaultColumns);
            RebuildColumns();
        }

        private bool CanExpandGetterHandler(object model)
        {
            return (model as TreeObjectNode).Children != null;
        }

        private IEnumerable<TreeObjectNode> ChildrenGetterHandler(object model)
        {
            return (model as TreeObjectNode).Children;
        }

        private void CellRightClickHandler(object sender, CellRightClickEventArgs e)
        {
            var contextMenu = new ContextMenuStrip();
            contextMenu.SuspendLayout();

            var expandAllItem = new ToolStripMenuItem();
            expandAllItem.Text = "Expand All Rows";
            expandAllItem.Click += (o, e) => ExpandAll();
            contextMenu.Items.Add(expandAllItem);

            var collapseAllItem = new ToolStripMenuItem();
            collapseAllItem.Text = "Collapse All Rows";
            collapseAllItem.Click += (o, e) => CollapseAll();
            contextMenu.Items.Add(collapseAllItem);

            contextMenu.ResumeLayout();
            e.MenuStrip = contextMenu;
        }
    }
}