using BrightIdeasSoftware;
using HZDCoreEditorUI.Util;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace HZDCoreEditorUI.UI
{
    public class ClassMemberTreeView : TreeListView
    {
        private readonly List<TreeDataNode> _children = new List<TreeDataNode>();

        public ClassMemberTreeView()
        {
            CanExpandGetter = CanExpandGetterHandler;
            ChildrenGetter = ChildrenGetterHandler;
            CellEditStarting += CellEditStartingHandler;
            BeforeSorting += BeforeSortingHandler;

            // Create columns
            var nameCol = new OLVColumn("Name", nameof(TreeDataNode.Name))
            {
                Width = 300,
                IsEditable = false,
            };
            Columns.Add(nameCol);

            var valueCol = new OLVColumn("Value", nameof(TreeDataNode.Value))
            {
                Width = 500,
            };
            Columns.Add(valueCol);

            var categoryCol = new OLVColumn("Category", nameof(TreeDataNode.Category))
            {
                Width = 100,
                IsEditable = false,
            };
            Columns.Add(categoryCol);

            var typeCol = new OLVColumn("Type", nameof(TreeDataNode.TypeName))
            {
                Width = 200,
                IsEditable = false,
            };
            Columns.Add(typeCol);
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

            RefreshViewRoots();
        }

        private void RefreshViewRoots(IEnumerable<TreeDataNode> children = null)
        {
            if (children == null)
                children = _children;

            Roots = null;
            Roots = children;
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
            var m = e.RowObject as TreeDataNode;

            if (!m.IsEditable)
                e.Cancel = true;
        }

        private static void BeforeSortingHandler(object sender, BeforeSortingEventArgs e)
        {
            var treeView = sender as ClassMemberTreeView;
            var linqQuery = treeView._children.AsEnumerable();

            // Only sort the top level objects. The rest don't matter.
            switch (e.ColumnToSort.AspectName)
            {
                case nameof(TreeDataNode.Name):
                    linqQuery = linqQuery.OrderBy(x => x.Name);
                    break;

                case nameof(TreeDataNode.Value):
                    break;

                case nameof(TreeDataNode.Category):
                    linqQuery = linqQuery.OrderBy(x => x.Category).ThenBy(x => x.Name);
                    break;

                case nameof(TreeDataNode.TypeName):
                    linqQuery = linqQuery.OrderBy(x => x.TypeName);
                    break;
            }

            treeView.RefreshViewRoots(linqQuery);
            e.Handled = true;
        }
    }
}
