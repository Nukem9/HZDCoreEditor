namespace HZDCoreEditorUI.UI
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using Decima;
    using Decima.HZD;
    using HZDCoreEditor.Util;
    using HZDCoreEditorUI.Util;
    using Newtonsoft.Json;

    public partial class FormCoreView : Form
    {
        private const string _notesFile = "notes.json";

        private readonly Program.CmdOptions _cmdOptions;
        private readonly CoreObjectListTreeView _objectsTreeView;
        private readonly ClassMemberTreeView _membersTreeView;
        private readonly object _noteLock = new object();

        private List<object> _coreObjectList;
        private object _lastSelectedObject;

        private string _loadedFilePath;
        private string _rootDir;

        private List<object> _undoLog = new List<object>();
        private int _undoPosition = 0;
        private bool _ignoreUndo = false;

        private Timer _notesTimer;
        private Dictionary<(string Path, string Id), (string Note, DateTime Date)> _notes;
        private bool _saveNotes = true;

        private int _searchNext = -1;
        private int _searchIndex = -1;
        private string _searchLast = null;

        public FormCoreView(Program.CmdOptions options)
        {
            _cmdOptions = options;
            _notesTimer = new Timer()
            {
                Interval = 500,
            };
            _notesTimer.Tick += NotesTimer_Tick;
            _notes = LoadNotes() ?? new Dictionary<(string Path, string Id), (string Note, DateTime Date)>();

            InitializeComponent();

            // Left panel
            _objectsTreeView = new CoreObjectListTreeView();
            _objectsTreeView.FullRowSelect = true;
            _objectsTreeView.Dock = DockStyle.Fill;
            _objectsTreeView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            _objectsTreeView.ItemSelectionChanged += ObjectsTreeView_ItemSelected;

            pnlMain.Controls.Clear();
            pnlMain.Controls.Add(_objectsTreeView);

            // Right panel
            _membersTreeView = new ClassMemberTreeView();
            _membersTreeView.FullRowSelect = true;
            _membersTreeView.Dock = DockStyle.Fill;
            _membersTreeView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            _membersTreeView.CellRightClick += MembersTreeView_CellRightClick;

            pnlData.Controls.Clear();
            pnlData.Controls.Add(_membersTreeView);

            // Recursively register mouse events
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
                if (_loadedFilePath != null)
                    SelectNodeByGUID(_cmdOptions.ObjectId);
            }
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

        private void FormCoreView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.XButton1)
                goToPreviousSelectionToolStripMenuItem.PerformClick();
            else if (e.Button == MouseButtons.XButton2)
                goToNextSelectionToolStripMenuItem.PerformClick();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearch.PerformClick();
        }

        private void TxtNotes_TextChanged(object sender, EventArgs e)
        {
            if (_saveNotes)
            {
                _notesTimer.Stop();
                _notesTimer.Start();
            }
        }

        private void TxtSearch_MouseClick(object sender, MouseEventArgs e) => ((TextBox)sender).SelectAll();

        private void TxtFile_MouseClick(object sender, MouseEventArgs e) => ((TextBox)sender).SelectAll();

        private void TxtType_MouseClick(object sender, MouseEventArgs e) => ((TextBox)sender).SelectAll();

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                _ignoreUndo = true;
                _searchIndex = 0;
                if (_searchLast != txtSearch.Text)
                    _searchNext = -1;
                _searchLast = txtSearch.Text;

                foreach (var node in _objectsTreeView.Objects.Cast<TreeObjectNode>())
                {
                    _objectsTreeView.Expand(node);
                    if (SearchNode(node))
                    {
                        AddUndoEntry(_objectsTreeView.SelectedObject);
                        _objectsTreeView.SelectedItem?.EnsureVisible();
                        return;
                    }
                }

                _searchNext = -1;
                MessageBox.Show("No more entries found");
            }
            finally
            {
                _ignoreUndo = false;
            }
        }

        private void BtnSearchAll_Click(object sender, EventArgs e)
        {
            Process.Start("HZDCoreSearch.exe", Process.GetCurrentProcess().ProcessName);
        }

        private void TsmFollow_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedRef();

            if (selected.Type == BaseRef.Types.InternalLink || selected.Type == BaseRef.Types.UUIDRef)
            {
                SelectNodeByGUID(selected.GUID);
            }

            if (selected.Type == BaseRef.Types.ExternalLink || selected.Type == BaseRef.Types.StreamingRef)
            {
                if (_rootDir == null)
                {
                    MessageBox.Show("Unable to find root directory.", "External Follow Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var path = Path.Combine(_rootDir, selected.ExternalFile + ".core");
                if (!File.Exists(path))
                {
                    MessageBox.Show("Unable to find file.", "External Follow Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Load localization text
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

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "Decima CoreBinary files (*.core)|*.core|All files (*.*)|*.*",
                FileName = Path.GetFileName(_loadedFilePath),
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var coreBinary = new CoreBinary();

                foreach (var obj in _coreObjectList)
                    coreBinary.AddObject(obj);

                coreBinary.ToFile(sfd.FileName, FileMode.Create);
            }
        }

        private void SaveAsArchiveToolStripMenuItem_Click(object sender, EventArgs e)
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

                foreach (var obj in _coreObjectList)
                    coreBinary.AddObject(obj);

                coreBinary.ToData(new BinaryWriter(ms));
                ms.Position = 0;

                using var packfileWriter = new PackfileWriter(sfd.FileName, false, FileMode.Create);
                packfileWriter.BuildFromStreamList(new List<(string CorePath, Stream Stream)> { ($"{txtFile.Text}.core", ms) });
            }
        }

        private void ExportAsJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportObjectsToJson(false);
        }

        private void ExportAsJSONWithTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportObjectsToJson(true);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void GoToPreviousSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ignoreUndo = true;

            if (_undoPosition > 0)
            {
                _undoPosition--;
                SelectNodeByObject(_undoLog[_undoPosition]);
            }

            _ignoreUndo = false;
        }

        private void GoToNextSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ignoreUndo = true;

            if (_undoPosition < _undoLog.Count - 1)
            {
                _undoPosition++;
                SelectNodeByObject(_undoLog[_undoPosition]);
            }

            _ignoreUndo = false;
        }

        private void ExpandAllTreesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _objectsTreeView.ExpandAll();
            _membersTreeView.ExpandAll();
        }

        private void MembersTreeView_CellRightClick(object sender, BrightIdeasSoftware.CellRightClickEventArgs e)
        {
            // "Export Array..."
            byte[] asBytes = null;

            if (e.Model is TreeDataListNode listNode)
            {
                if (listNode.GetList() is List<byte> list)
                    asBytes = list.ToArray();
            }
            else if (e.Model is TreeDataArrayNode arrayNode)
            {
                if (arrayNode.GetArray() is byte[] bytes)
                    asBytes = bytes;
            }

            if (asBytes != null)
            {
                var exportArray = new ToolStripMenuItem();
                exportArray.Text = "Export Array...";
                exportArray.Click += (o, e) => ExportByteArrayToFile(asBytes);
                e.MenuStrip.Items.Insert(0, exportArray);
            }

            // "Follow Reference"
            if (GetSelectedRef() != null)
            {
                e.MenuStrip.Items.Insert(0, new ToolStripSeparator());

                var menuItem = new ToolStripMenuItem();
                menuItem.Text = "Follow Reference";
                menuItem.Click += TsmFollow_Click;
                e.MenuStrip.Items.Insert(0, menuItem);
            }
        }

        private void ObjectsTreeView_ItemSelected(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            var selectedNode = ((BrightIdeasSoftware.TreeListView)sender).SelectedObject as TreeObjectNode;
            var targetObject = selectedNode?.UnderlyingObject;

            // Ignore spurious selection changes
            if (_lastSelectedObject == targetObject)
                return;

            _lastSelectedObject = targetObject;

            if (targetObject != null)
            {
                if (!_ignoreUndo)
                    AddUndoEntry(selectedNode);

                _membersTreeView.RebuildTreeFromObject(targetObject);
                txtType.Text = selectedNode.TypeName;

                _saveNotes = false;

                if (targetObject is RTTIRefObject obj && _notes.TryGetValue((txtFile.Text, obj.ObjectUUID?.ToString()), out var note))
                    txtNotes.Text = note.Note;
                else
                    txtNotes.Text = string.Empty;

                _saveNotes = true;
            }
        }

        private void NotesTimer_Tick(object sender, EventArgs e)
        {
            _notesTimer.Stop();

            var obj = (_objectsTreeView.SelectedObject as TreeObjectNode)?.UnderlyingObject as RTTIRefObject;
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

        private void ExportByteArrayToFile(byte[] data)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "Raw data (*.*)|*.*",
                FileName = "array.dat",
            };

            if (sfd.ShowDialog() == DialogResult.OK)
                File.WriteAllBytes(sfd.FileName, data);
        }

        private void OpenFile()
        {
            _searchLast = null;
            var ofd = new OpenFileDialog();

            if (!string.IsNullOrEmpty(_loadedFilePath))
            {
                ofd.InitialDirectory = Path.GetDirectoryName(_loadedFilePath);
                ofd.FileName = Path.GetFileName(_loadedFilePath);
            }

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            LoadFile(ofd.FileName);
        }

        private void LoadFile(string path)
        {
            _undoLog.Clear();
            _undoPosition = 0;
            _loadedFilePath = path;
            Text = "Core - " + Path.GetFileName(_loadedFilePath);

            var fullPath = Path.GetFullPath(_loadedFilePath);
            var nameRoots = Names.RootNames.Select(x => fullPath.IndexOf(x)).Where(x => x >= 0).ToList();
            if (!nameRoots.Any())
            {
                txtFile.Text = _loadedFilePath;
                _rootDir = null;
            }
            else
            {
                txtFile.Text = Path.ChangeExtension(fullPath, null).Substring(nameRoots.Min()).Replace("\\", "/");
                _rootDir = fullPath.Substring(0, nameRoots.Min());
            }

            _coreObjectList = CoreBinary.FromFile(path, true).Objects.ToList();
            _objectsTreeView.RebuildTreeFromObjects(_coreObjectList);
            _membersTreeView.ClearObjects();
        }

        private void AddUndoEntry(object obj)
        {
            if (_undoLog.Count - (_undoPosition + 1) > 0)
                _undoLog.RemoveRange(_undoPosition + 1, _undoLog.Count - (_undoPosition + 1));

            if (_undoLog.Count > 0)
                _undoPosition++;

            _undoLog.Add(obj);
        }

        private bool SearchNode(TreeObjectNode node)
        {
            if (node.Children != null)
            {
                foreach (var subNode in node.Children)
                {
                    _objectsTreeView.Expand(subNode);

                    if (SearchNode(subNode))
                        return true;
                }
            }

            if (node.UnderlyingObject != null)
            {
                _objectsTreeView.SelectObject(node, true);

                foreach (var dataNode in _membersTreeView.Objects.Cast<TreeDataNode>())
                {
                    if (SearchDataNode(dataNode))
                        return true;
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
                        return true;
                }
            }

            if (_searchNext >= 0)
            {
                if (_searchIndex >= _searchNext)
                {
                    _searchNext = -1;
                }
                else
                {
                    _searchIndex++;
                    return false;
                }
            }

            _searchIndex++;

            if (node.Value?.ToString().Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase) == true)
            {
                var parents = GetNodeParents(_membersTreeView.Objects.Cast<TreeDataNode>(), node);
                var nodeParent = parents.LastOrDefault();
                if (nodeParent?.Value?.ToString().StartsWith("Ref<") != true)
                {
                    foreach (var p in parents)
                        _membersTreeView.Expand(p);

                    _membersTreeView.SelectObject(node, true);
                    _searchNext = _searchIndex;
                    return true;
                }
            }

            return false;
        }

        private List<TreeDataNode> GetNodeParents(IEnumerable<TreeDataNode> roots, TreeDataNode node)
        {
            var parents = new List<TreeDataNode>();

            foreach (var root in roots)
            {
                if (FindNodeParents(parents, root, node))
                {
                    if (parents.Any())
                        parents.Add(root);

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

        private bool SelectNodeByPredicate(Predicate<TreeObjectNode> selector)
        {
            bool Search(IEnumerable<TreeObjectNode> nodes)
            {
                foreach (var node in nodes)
                {
                    // Try this node first
                    if (selector(node))
                    {
                        _objectsTreeView.SelectObject(node, true);
                        _objectsTreeView.SelectedItem?.EnsureVisible();
                        return true;
                    }

                    // Then expand children under it
                    bool wasExpanded = _objectsTreeView.IsExpanded(node);

                    if (!wasExpanded)
                        _objectsTreeView.Expand(node);

                    if (node.Children != null && Search(node.Children))
                        return true;

                    if (!wasExpanded)
                        _objectsTreeView.Collapse(node);
                }

                return false;
            }

            return Search(_objectsTreeView.Objects.Cast<TreeObjectNode>());
        }

        private bool SelectNodeByGUID(BaseGGUUID objectGUID)
        {
            return SelectNodeByPredicate((node) => node.UUID != null && node.UUID == objectGUID);
        }

        private bool SelectNodeByObject(object obj)
        {
            return SelectNodeByPredicate((node) => ReferenceEquals(node, obj));
        }

        private BaseRef GetSelectedRef()
        {
            if (_membersTreeView.SelectedObject is TreeDataNode selected)
            {
                if (selected.Value is BaseRef cr)
                    return cr;

                if (selected.ParentObject is BaseRef pr)
                    return pr;
            }

            return null;
        }

        private void BindMouseEvents(Control control)
        {
            control.MouseDown += FormCoreView_MouseDown;

            foreach (Control c in control.Controls)
                BindMouseEvents(c);
        }

        private void SaveNotes()
        {
            lock (_noteLock)
            {
                var json = JsonConvert.SerializeObject(_notes.Select(x => (x.Key, x.Value)), Formatting.Indented);
                File.WriteAllText(_notesFile, json);
            }
        }

        private Dictionary<(string, string), (string, DateTime)> LoadNotes()
        {
            lock (_noteLock)
            {
                if (File.Exists(_notesFile))
                {
                    var json = File.ReadAllText(_notesFile);
                    var noteList = JsonConvert.DeserializeObject<List<((string, string), (string, DateTime))>>(json);
                    return noteList?.ToDictionary(x => x.Item1, x => x.Item2);
                }
            }

            return null;
        }

        private void ExportObjectsToJson(bool exportTypes)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "Json files (*.json)|*.json|All files (*.*)|*.*",
                FileName = Path.GetFileNameWithoutExtension(_loadedFilePath) + ".json",
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var json = JsonConvert.SerializeObject(_coreObjectList, new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    TypeNameHandling = exportTypes ? TypeNameHandling.Objects : TypeNameHandling.None,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Converters = new List<JsonConverter>() { new BaseGGUUIDConverter() },
                });

                File.WriteAllText(sfd.FileName, json);
            }
        }
    }
}
