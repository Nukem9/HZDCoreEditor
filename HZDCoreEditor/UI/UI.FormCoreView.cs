using Decima;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HZDCoreEditor.Util;
using Newtonsoft.Json;

namespace HZDCoreEditor.UI
{
    public partial class FormCoreView : Form
    {
        private List<object> CoreObjectList;
        private string LoadedFilePath;

        private BrightIdeasSoftware.TreeListView TV1;
        private BrightIdeasSoftware.TreeListView TV2;

        public FormCoreView()
        {
            InitializeComponent();
        }

        private void FormCoreView_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                this.BeginInvoke(new Action(() => OpenFile(true)));
            });
        }

        private void OpenFile(bool exit = false)
        {
            SearchLast = null;
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                if (exit)
                    Application.Exit();
                return;
            }

            LoadedFilePath = ofd.FileName;
            this.Text = "FormCoreView - " + LoadedFilePath;

            CoreObjectList = CoreBinary.Load(ofd.FileName);

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

            splitContainer.Panel1.Controls.Clear();
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

            splitContainer.Panel2.Controls.Clear();
            splitContainer.Panel2.Controls.Add(treeListView);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFile();
        }


        private int SearchNext = -1;
        private int SearchIndex = -1;
        private string SearchLast = null;
        private string[] NonExand =
        {
            "GGUUID"
        };

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchIndex = 0;
            if (SearchLast != txtSearch.Text)
                SearchNext = -1;
            SearchLast = txtSearch.Text;

            foreach (var node in TV1.Objects.Cast<TreeObjectNode>())
            {
                TV1.Expand(node);
                if (SearchNode(node))
                    return;
            }

            SearchNext = -1;
            MessageBox.Show("No more entries found");
        }

        private bool SearchNode(TreeObjectNode node)
        {
            if (node.Children != null)
            {
                foreach (var subNode in node.Children)
                {
                    TV1.Expand(subNode);
                    if (SearchNode(subNode))
                        return true;
                }
            }

            if (node.UnderlyingObject != null)
            {
                TV1.SelectObject(node, true);

                TV2.Clear();
                TreeDataNode.SetupTree(TV2, node.UnderlyingObject);

                foreach (var dNode in TV2.Objects.Cast<TreeDataNode>())
                {
                    if (SearchDataNode(dNode))
                    {
                        if (!NonExand.Any(x => dNode.TypeName.Contains(x)))
                            TV2.Expand(dNode);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool SearchDataNode(TreeDataNode node)
        {
            if (node.Children != null && !NonExand.Any(x => node.TypeName.Contains(x)))
            {
                foreach (var subNode in node.Children)
                {
                    if (SearchDataNode(subNode))
                    {
                        TV2.Expand(node);
                        return true;
                    }
                }
            }

            if (SearchNext >= 0)
            {
                if (SearchIndex >= SearchNext)
                    SearchNext = -1;
                else
                {
                    SearchIndex++;
                    return false;
                }
            }

            SearchIndex++;

            if (node.Value?.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase) == true)
            {
                TV2.SelectObject(node, true);
                SearchNext = SearchIndex;
                return true;
            }

            return false;
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearch_Click(null, null);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            sfd.FileName = Path.GetFileNameWithoutExtension(LoadedFilePath) + ".json";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var json = JsonConvert.SerializeObject(CoreObjectList, new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    TypeNameHandling = TypeNameHandling.Objects,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Converters = new List<JsonConverter>() { new BaseGGUUIDConverter() }
                });
                File.WriteAllText(sfd.FileName, json);
            }
        }
    }
}
