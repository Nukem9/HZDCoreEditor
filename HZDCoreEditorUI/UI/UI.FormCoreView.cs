using Decima;
using Decima.HZD;
using HZDCoreEditor.Util;
using HZDCoreEditorUI.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace HZDCoreEditorUI.UI
{
    public partial class FormCoreView : Form
    {
        private readonly CmdOptions _cmdOptions;
        private const string NotesFile = "notes.json";

        private List<object> CoreObjectList;
        private string LoadedFilePath;
        private string RootDir;

        private List<object> UndoLog = new List<object>();
        private int UndoPosition = 0;
        private bool IgnoreUndo = false;

        private CoreObjectListTreeView tvMain;
        private ClassMemberTreeView tvData;
        private Timer _notesTimer;
        private Dictionary<(string Path, string Id), (string Note, DateTime Date)> _notes;

        public FormCoreView(CmdOptions cmd)
        {
            _cmdOptions = cmd;
            _notesTimer = new Timer()
            {
                Interval = 500
            };
            _notesTimer.Tick += NotesTimer_Tick;
            _notes = LoadNotes() ?? new Dictionary<(string Path, string Id), (string Note, DateTime Date)>();

            InitializeComponent();
            CreateObjectView();
            CreateDataView();

            BindMouseEvents(this);
        }

        private void FormCoreView_Load(object sender, EventArgs e)
        {
            bool fileLoaded = false;

            if (!string.IsNullOrEmpty(_cmdOptions.File))
            {
                LoadFile(_cmdOptions.File);
                fileLoaded = true;
            }

            if (!string.IsNullOrEmpty(_cmdOptions.Search))
            {
                txtSearch.Text = _cmdOptions.Search;

                if (fileLoaded)
                    btnSearch.PerformClick();
            }

            if (!string.IsNullOrEmpty(_cmdOptions.ObjectId))
            {
                if (LoadedFilePath != null)
                    SelectNodeById(_cmdOptions.ObjectId);
            }
        }

        private void CreateObjectView()
        {
            tvMain = new CoreObjectListTreeView();
            tvMain.FullRowSelect = true;
            tvMain.Dock = DockStyle.Fill;
            tvMain.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            tvMain.ItemSelectionChanged += TreeListView_ItemSelected;

            pnlMain.Controls.Clear();
            pnlMain.Controls.Add(tvMain);
        }

        private void CreateDataView()
        {
            tvData = new ClassMemberTreeView();
            tvData.FullRowSelect = true;
            tvData.Dock = DockStyle.Fill;
            tvData.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;

            pnlData.Controls.Clear();
            pnlData.Controls.Add(tvData);
        }

        private void OpenFile()
        {
            SearchLast = null;
            var ofd = new OpenFileDialog();

            if (!string.IsNullOrEmpty(LoadedFilePath))
            {
                ofd.InitialDirectory = Path.GetDirectoryName(LoadedFilePath);
                ofd.FileName = Path.GetFileName(LoadedFilePath);
            }

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            LoadFile(ofd.FileName);
        }

        private void LoadFile(string path)
        {
            UndoLog.Clear();
            UndoPosition = 0;
            LoadedFilePath = path;
            this.Text = "Core - " + Path.GetFileName(LoadedFilePath);

            var fullPath = Path.GetFullPath(LoadedFilePath);
            var nameRoots = Names.RootNames.Select(x => fullPath.IndexOf(x)).Where(x => x >= 0).ToList();
            if (!nameRoots.Any())
            {
                txtFile.Text = LoadedFilePath;
                RootDir = null;
            }
            else
            {
                txtFile.Text = Path.ChangeExtension(fullPath, null).Substring(nameRoots.Min()).Replace("\\", "/");
                RootDir = fullPath.Substring(0, nameRoots.Min());
            }

            CoreObjectList = CoreBinary.FromFile(path, true).Objects.ToList();
            tvMain.RebuildTreeFromObjects(CoreObjectList);
        }

        private void TreeListView_ItemSelected(object sender, EventArgs e)
        {
            var underlying = (tvMain.SelectedObject as TreeObjectNode)?.UnderlyingObject;

            if (underlying != null)
            {
                if (!IgnoreUndo)
                    AddUndo();

                tvData.RebuildTreeFromObject(underlying);
                txtType.Text = underlying.GetType().GetFriendlyName();

                _saveNotes = false;
                if (underlying is RTTIRefObject obj && _notes.TryGetValue((txtFile.Text, obj.ObjectUUID?.ToString()), out var note))
                    txtNotes.Text = note.Note;
                else
                    txtNotes.Text = "";
                _saveNotes = true;
            }
        }

        private int SearchNext = -1;
        private int SearchIndex = -1;
        private string SearchLast = null;

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                IgnoreUndo = true;
                SearchIndex = 0;
                if (SearchLast != txtSearch.Text)
                    SearchNext = -1;
                SearchLast = txtSearch.Text;

                foreach (var node in tvMain.Objects.Cast<TreeObjectNode>())
                {
                    tvMain.Expand(node);
                    if (SearchNode(node))
                    {
                        AddUndo();
                        tvMain.SelectedItem?.EnsureVisible();
                        return;
                    }
                }

                SearchNext = -1;
                MessageBox.Show("No more entries found");
            }
            finally
            {
                IgnoreUndo = false;
            }
        }

        private void AddUndo()
        {
            if (UndoLog.Count - (UndoPosition + 1) > 0)
                UndoLog.RemoveRange(UndoPosition + 1, UndoLog.Count - (UndoPosition + 1));
            if (UndoLog.Count > 0)
                UndoPosition++;
            UndoLog.Add(tvMain.SelectedObject);
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
                tvData.RebuildTreeFromObject(node.UnderlyingObject);

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
            if (node.Children != null)
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

            if (node.Value?.ToString().Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase) == true)
            {
                var parents = GetParents(tvData.Objects.Cast<TreeDataNode>(), node);
                var nodeParent = parents.LastOrDefault();
                if (nodeParent?.Value?.ToString().StartsWith("Ref<") != true)
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
                btnSearch.PerformClick();
        }

        private void FormCoreView_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            try
            {
                if (files.Any())
                    LoadFile(files.First());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load file: " + ex.Message);
            }
        }

        private void FormCoreView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            Process.Start("HZDCoreSearch.exe", Process.GetCurrentProcess().ProcessName);
        }

        private void txtSearch_MouseClick(object sender, MouseEventArgs e) => ((TextBox)sender).SelectAll();
        private void txtFile_MouseClick(object sender, MouseEventArgs e) => ((TextBox)sender).SelectAll();
        private void txtType_MouseClick(object sender, MouseEventArgs e) => ((TextBox)sender).SelectAll();

        private void cmsData_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmsData.Items[0].Enabled = GetSelectedRef() != null;
        }

        private void tsmFollow_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedRef();
            if (selected.Type == BaseRef.Types.InternalLink || selected.Type == BaseRef.Types.UUIDRef)
            {
                SelectNodeById(selected.GUID);
            }
            if (selected.Type == BaseRef.Types.ExternalLink || selected.Type == BaseRef.Types.StreamingRef)
            {
                if (RootDir == null)
                {
                    MessageBox.Show("Unable to find root directory.", "External Follow Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var path = Path.Combine(RootDir, selected.ExternalFile + ".core");
                if (!File.Exists(path))
                {
                    MessageBox.Show("Unable to find file.", "External Follow Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //load localization text
                if (selected.GetType().IsGenericType && selected.GetType().GetGenericArguments().Any(x => x == typeof(LocalizedTextResource)))
                {
                    var core = CoreBinary.FromFile(path);
                    var match = core.Objects.FirstOrDefault(x => x is LocalizedTextResource asResource && asResource.ObjectUUID == selected.GUID) as LocalizedTextResource;
                    var text = match == null ? "null" : match.GetStringForLanguage(ELanguage.English);

                    MessageBox.Show(text, "Localization Text", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Process.Start(Process.GetCurrentProcess().ProcessName, $"\"{path}\" -o \"{selected.GUID}\"");
                }
            }
        }

        private void SelectNodeById(BaseGGUUID objectId)
        {
            bool search(IEnumerable<TreeObjectNode> nodes)
            {
                foreach (var node in nodes)
                {
                    tvMain.Expand(node);
                    if (node.Children != null && search(node.Children))
                        return true;

                    var id = node.UUID;
                    if (id != null && id == objectId)
                    {
                        tvMain.SelectObject(node, true);
                        tvMain.SelectedItem?.EnsureVisible();
                        return true;
                    }
                }
                return false;
            }

            search(tvMain.Objects.Cast<TreeObjectNode>());
        }

        private BaseRef GetSelectedRef()
        {
            if (tvData?.SelectedObject is TreeDataNode selected)
            {
                if (selected.Value is BaseRef cr)
                    return cr;
                if (selected.ParentObject is BaseRef pr)
                    return pr;
            }

            return null;
        }

        private void FormCoreView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.XButton1)
            {
                IgnoreUndo = true;
                if (UndoPosition > 0)
                {
                    UndoPosition--;
                    tvMain.SelectObject(UndoLog[UndoPosition], true);
                }
                IgnoreUndo = false;
            }
            else if (e.Button == MouseButtons.XButton2)
            {
                IgnoreUndo = true;
                if (UndoPosition < UndoLog.Count - 1)
                {
                    UndoPosition++;
                    tvMain.SelectObject(UndoLog[UndoPosition], true);
                }
                IgnoreUndo = false;
            }
        }

        public void BindMouseEvents(Control control)
        {
            control.MouseDown += FormCoreView_MouseDown;

            foreach (Control c in control.Controls)
            {
                BindMouseEvents(c);
            }
        }

        private void txtNotes_TextChanged(object sender, EventArgs e)
        {
            if (_saveNotes)
            {
                _notesTimer.Stop();
                _notesTimer.Start();
            }
        }

        private void NotesTimer_Tick(object sender, EventArgs e)
        {
            _notesTimer.Stop();

            var obj = (tvMain.SelectedObject as TreeObjectNode)?.UnderlyingObject as RTTIRefObject;
            if (obj == null)
                return;

            _notes[(txtFile.Text, obj.ObjectUUID?.ToString())] = (txtNotes.Text, DateTime.Now);

            var updated = LoadNotes();
            if (updated != null)
            {
                foreach (var newNote in updated)
                {
                    if (_notes.TryGetValue(newNote.Key, out var note))
                    {
                        if (newNote.Value.Item2 > note.Date)
                            _notes[newNote.Key] = newNote.Value;
                    }
                    else
                    {
                        _notes.Add(newNote.Key, newNote.Value);
                    }
                }
            }

            SaveNotes();
        }

        private bool _saveNotes = true;
        private readonly object _noteLock = new object();
        private void SaveNotes()
        {
            lock (_noteLock)
            {
                var json = JsonConvert.SerializeObject(_notes.Select(x => (x.Key, x.Value)), Formatting.Indented);
                File.WriteAllText(NotesFile, json);
            }
        }
        private Dictionary<(string, string), (string, DateTime)> LoadNotes()
        {
            lock (_noteLock)
            {
                if (File.Exists(NotesFile))
                {
                    var json = File.ReadAllText(NotesFile);
                    var noteList = JsonConvert.DeserializeObject<List<((string, string), (string, DateTime))>>(json);
                    return noteList?.ToDictionary(x => x.Item1, x => x.Item2);
                }
            }

            return null;
        }

        private void exportObjectsToJson(bool exportTypes)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "Json files (*.json)|*.json|All files (*.*)|*.*",
                FileName = Path.GetFileNameWithoutExtension(LoadedFilePath) + ".json"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var json = JsonConvert.SerializeObject(CoreObjectList, new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    TypeNameHandling = exportTypes ? TypeNameHandling.Objects : TypeNameHandling.None,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Converters = new List<JsonConverter>() { new BaseGGUUIDConverter() }
                });

                File.WriteAllText(sfd.FileName, json);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "Decima CoreBinary files (*.core)|*.core|All files (*.*)|*.*",
                FileName = Path.GetFileName(LoadedFilePath),
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var coreBinary = new CoreBinary();

                foreach (var obj in CoreObjectList)
                    coreBinary.AddObject(obj);

                coreBinary.ToFile(sfd.FileName, FileMode.Create);
            }
        }

        private void saveAsArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "Decima Archive files (*.bin)|*.bin",
                FileName = "Patch_MyEdits.bin",
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using var ms = new MemoryStream();
                var coreBinary = new CoreBinary();

                foreach (var obj in CoreObjectList)
                    coreBinary.AddObject(obj);

                coreBinary.ToData(new BinaryWriter(ms));
                ms.Position = 0;

                using var packfileWriter = new PackfileWriter(sfd.FileName, false, FileMode.Create);
                packfileWriter.BuildFromStreamList(new List<(string CorePath, Stream Stream)> { ($"{txtFile.Text}.core", ms) });
            }
        }

        private void exportAsJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportObjectsToJson(false);
        }

        private void exportAsJSONWithTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportObjectsToJson(true);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void goToPreviousSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void goToNextSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void expandAllTreesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tvMain.ExpandAll();
            tvData.ExpandAll();
        }
    }
}
