using BrightIdeasSoftware;
using HZDCoreEditorUI.Util;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace HZDCoreEditorUI.UI
{
    public class ClassMemberTreeView : TreeListView
    {
        private readonly List<TreeDataNode> _children = new List<TreeDataNode>();
        private readonly OLVColumn[] _defaultColumns;

        public ClassMemberTreeView()
        {
            CanExpandGetter = CanExpandGetterHandler;
            ChildrenGetter = ChildrenGetterHandler;
            CellEditStarting += CellEditStartingHandler;
            CellEditFinishing += CellEditFinishingHandler;
            CellEditFinished += CellEditFinishedHandler;
            CellRightClick += CellRightClickHandler;
            BeforeSorting += BeforeSortingHandler;

            // Columns are hardcoded. Keep them cached in case the view needs to be reset.
            _defaultColumns = new OLVColumn[4];

            _defaultColumns[0] = new OLVColumn("Name", nameof(TreeDataNode.Name))
            {
                Width = 300,
                IsEditable = false,
            };

            _defaultColumns[1] = new OLVColumn("Value", nameof(TreeDataNode.Value))
            {
                Width = 500,
            };

            _defaultColumns[2] = new OLVColumn("Category", nameof(TreeDataNode.Category))
            {
                Width = 100,
                IsEditable = false,
            };

            _defaultColumns[3] = new OLVColumn("Type", nameof(TreeDataNode.TypeName))
            {
                Width = 200,
                IsEditable = false,
            };

            CreateColumns();
        }

        public ClassMemberTreeView(object baseObject) : this()
        {
            RebuildTreeFromObject(baseObject);
        }

        public void RebuildTreeFromObject(object baseObject)
        {
            _children.Clear();

            // Prepare each root node: class member variables act as children
            var objectType = baseObject.GetType();

            if (Type.GetTypeCode(objectType) != TypeCode.Object)
                return;

            var fields = objectType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
                _children.Add(TreeDataNode.CreateNode(baseObject, new FieldOrProperty(field)));

            RebuildViewRoots(true);
        }

        private void RebuildViewRoots(bool forceResort)
        {
            // A full reset needs to be performed due to bugs in OLV
            var oldState = SaveState();
            Reset();
            CreateColumns();
            RestoreState(oldState);

            if (forceResort)
                SortObjects(PrimarySortColumn.AspectName, PrimarySortOrder);

            Roots = _children;
        }

        private void CreateColumns()
        {
            AllColumns.AddRange(_defaultColumns);
            PrimarySortColumn = _defaultColumns[2];
            PrimarySortOrder = SortOrder.Descending;

            RebuildColumns();
        }

        private bool SortObjects(string aspectName, SortOrder order)
        {
            Func<TreeDataNode, TreeDataNode, int> compareFunc = null;
            static int compareNames(TreeDataNode x, TreeDataNode y) => string.Compare(x.Name, y.Name);

            switch (aspectName)
            {
                case nameof(TreeDataNode.Name):
                    compareFunc = compareNames;
                    break;

                case nameof(TreeDataNode.Value):
                    return false;

                case nameof(TreeDataNode.Category):
                    compareFunc = (x, y) =>
                    {
                        if (string.IsNullOrEmpty(x.Category))
                            return 1;

                        if (string.IsNullOrEmpty(y.Category))
                            return -1;

                        return string.Compare(x.Category, y.Category);
                    };
                    break;

                case nameof(TreeDataNode.TypeName):
                    compareFunc = (x, y) =>
                    {
                        return string.Compare(x.TypeName, y.TypeName);
                    };
                    break;

                default:
                    return false;
            }

            // Only sort the top level objects. The rest don't matter.
            _children.Sort((x, y) =>
            {
                int result = compareFunc(x, y);

                // Reverse the order if ascending
                if (order == SortOrder.Ascending)
                {
                    result = result switch
                    {
                        -1 => 1,
                        0 => 0,
                        1 => -1,
                        _ => result,
                    };
                }

                // Use the member name as a fallback so we have a stable sort order
                if (result == 0)
                    result = compareNames(x, y);

                return result;
            });

            return true;
        }

        private static bool CanExpandGetterHandler(object model)
        {
            return (model as TreeDataNode).HasChildren;
        }

        private static IEnumerable<TreeDataNode> ChildrenGetterHandler(object model)
        {
            return (model as TreeDataNode).Children;
        }

        private static void CellEditStartingHandler(object sender, CellEditEventArgs e)
        {
            var node = e.RowObject as TreeDataNode;

            if (!node.IsEditable)
            {
                e.Cancel = true;
                return;
            }

            var control = node.CreateEditControl(e.CellBounds);

            if (control != null)
            {
                // Don't auto dispose: the control will be destroyed before CellEditFinishedHandler runs
                e.AutoDispose = false;
                e.Control = control;
            }
        }

        private void CellEditFinishingHandler(object sender, CellEditEventArgs e)
        {
            // This has to be canceled or CellEditFinishedHandler will fire twice for each edit
            e.Cancel = true;
        }

        private void CellEditFinishedHandler(object sender, CellEditEventArgs e)
        {
            var node = e.RowObject as TreeDataNode;

            if (node.FinishEditControl(e.Control, () => { RefreshObjects(_children); }))
                RefreshItem(e.ListViewItem);
        }

        private void CellRightClickHandler(object sender, CellRightClickEventArgs e)
        {
            var contextMenu = new ContextMenuStrip();
            contextMenu.SuspendLayout();

            if (e.Model is TreeDataNode node)
                node.CreateContextMenuItems(contextMenu, () => { RefreshObjects(_children); });

            if (contextMenu.Items.Count > 0)
                contextMenu.Items.Add(new ToolStripSeparator());

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

        private static void BeforeSortingHandler(object sender, BeforeSortingEventArgs e)
        {
            var treeView = sender as ClassMemberTreeView;
            bool sortResult = treeView.SortObjects(e.ColumnToSort.AspectName, e.SortOrder);

            if (!sortResult)
                e.Canceled = true;

            if (!e.Canceled)
                treeView.RebuildViewRoots(false);

            e.Handled = true;
        }

    }
}
