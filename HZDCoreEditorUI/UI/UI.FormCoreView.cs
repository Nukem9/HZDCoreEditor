using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Decima;
using HZDCoreEditor.Util;
using Newtonsoft.Json;

namespace HZDCoreEditorUI.UI
{
    public partial class FormCoreView : Form
    {
        private List<object> CoreObjectList;
        private string LoadedFilePath;

        private BrightIdeasSoftware.TreeListView tvMain;
        private BrightIdeasSoftware.TreeListView tvData;

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

            CoreObjectList = CoreBinary.Load(ofd.FileName, true);

            BuildObjectView();
            BuildDataView();
        }

        private void BuildObjectView()
        {
            tvMain = new BrightIdeasSoftware.TreeListView();
            tvMain.FullRowSelect = true;
            tvMain.Dock = DockStyle.Fill;

            tvMain.ItemSelectionChanged += TreeListView_ItemSelected;

            tvMain.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            TreeObjectNode.SetupTree(tvMain, CoreObjectList);

            splitContainer.Panel1.Controls.Clear();
            splitContainer.Panel1.Controls.Add(tvMain);
        }

        private void TreeListView_ItemSelected(object sender, EventArgs e)
        {
            var underlying = (tvMain.SelectedObject as TreeObjectNode)?.UnderlyingObject;

            if (underlying != null)
            {
                tvData.Clear();
                TreeDataNode.SetupTree(tvData, underlying);
            }
        }

        private void BuildDataView()
        {
            tvData = new BrightIdeasSoftware.TreeListView();
            tvData.FullRowSelect = true;
            tvData.Dock = DockStyle.Fill;
            tvData.VirtualMode = true;

            tvData.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            TreeDataNode.SetupTree(tvData, CoreObjectList[0]);

            splitContainer.Panel2.Controls.Clear();
            splitContainer.Panel2.Controls.Add(tvData);
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

            foreach (var node in tvMain.Objects.Cast<TreeObjectNode>())
            {
                tvMain.Expand(node);
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
                    tvMain.Expand(subNode);
                    if (SearchNode(subNode))
                        return true;
                }
            }

            if (node.UnderlyingObject != null)
            {
                tvMain.SelectObject(node, true);

                tvData.Clear();
                TreeDataNode.SetupTree(tvData, node.UnderlyingObject);

                foreach (var dNode in tvData.Objects.Cast<TreeDataNode>())
                {
                    if (SearchDataNode(dNode))
                    {
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
                var parents = GetParents(tvData.Objects.Cast<TreeDataNode>(), node);
                var nodeParent = parents.LastOrDefault();
                if (nodeParent?.Value.StartsWith("Ref<") != true)
                {
                    foreach (var p in parents)
                        tvData.Expand(p);

                    tvData.SelectObject(node, true);
                    SearchNext = SearchIndex;
                    return true;
                }
            }

            return false;
        }

        private List<TreeDataNode> GetParents(IEnumerable<TreeDataNode> roots, TreeDataNode node)
        {
            var parents = new List<TreeDataNode>();
            foreach (var root in roots)
            {
                if (FindNodeParents(parents, root, node))
                {
                    if (parents.Any()) parents.Add(root);
                    break;
                }
            }

            parents.Reverse();
            return parents;
        }
        private bool FindNodeParents(List<TreeDataNode> parents, TreeDataNode curNode, TreeDataNode searchNode)
        {
            if (curNode.Children?.Any() == true)
            {
                foreach (var subNode in curNode.Children)
                {
                    if (FindNodeParents(parents, subNode, searchNode))
                    {
                        parents.Add(curNode);
                        return true;
                    }
                }
            }

            return ReferenceEquals(searchNode, curNode);
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
                    TypeNameHandling = cbExportTypes.Checked ? TypeNameHandling.Objects : TypeNameHandling.None,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Converters = new List<JsonConverter>() { new BaseGGUUIDConverter() }
                });
                File.WriteAllText(sfd.FileName, json);
            }
        }
    }
}
