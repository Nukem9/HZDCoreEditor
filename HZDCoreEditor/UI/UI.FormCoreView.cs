using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HZDCoreEditor.UI
{
    public partial class FormCoreView : Form
    {
        private readonly List<object> CoreObjectList;

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

            treeListView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            TreeObjectNode.SetupTree(treeListView, CoreObjectList);

            splitContainer.Panel1.Controls.Add(treeListView);
        }

        private void TreeListView_ItemSelected(object sender, EventArgs e)
        {
            var underlying = (TV1.SelectedObject as TreeObjectNode)?.UnderlyingObject;

            if (underlying != null)
            {
                TV2.Clear();
                TreeDataNode.SetupTree(TV2, underlying);
            }
        }

        private void BuildDataView()
        {
            var treeListView = new BrightIdeasSoftware.TreeListView();
            TV2 = treeListView;
            treeListView.Dock = DockStyle.Fill;

            treeListView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            TreeDataNode.SetupTree(treeListView, CoreObjectList[0]);

            splitContainer.Panel2.Controls.Add(treeListView);
        }
    }
}
