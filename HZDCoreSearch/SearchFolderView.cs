using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HZDCoreEditor.Util;

namespace HZDCoreSearch
{
    public partial class SearchFolderView : Form
    {
        private readonly string _sourceProcess;
        private bool _loading = true;
        private string[] CurrentSearches;

        public SearchFolderView(string sourceProcess)
        {
            _sourceProcess = sourceProcess;
            InitializeComponent();

            SettingsManager.Load();
            tbDir.Text = SettingsManager.Settings.SearchFolderDir;
            tbSearch.Text = SettingsManager.Settings.SearchFolderText;

            _loading = false;
        }

        private async void tbDir_TextChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            SettingsManager.Settings.SearchFolderDir = tbDir.Text;
            await SettingsManager.Save();
        }

        private async void tbSearch_TextChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            SettingsManager.Settings.SearchFolderText = tbSearch.Text;
            await SettingsManager.Save();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            btnSearch.Enabled = false;

            CurrentSearches = tbSearch.Text.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            lbMatches.Items.Clear();

            await Task.Run(() =>
            {
                try
                {
                    var byteSearches = CurrentSearches.Select((x, i) => (i.ToString(), ToBytes(x))).ToList();

                    SearchDirs(tbDir.Text, byteSearches);
                }
                catch (Exception ex)
                {
                    if (!(ex?.InnerException is OperationCanceledException))
                        MessageBox.Show("Search Error: " + ex.Message);
                }
            });

            btnSearch.Enabled = true;
        }
        
        private void SearchDirs(string dir, List<(string Key, byte[] Data)> patterns)
        {
            var matches = new ConcurrentBag<string>();
            var ptq = new ParallelTasks<string>(Environment.ProcessorCount, f =>
            {
                var data = File.ReadAllBytes(f);
                foreach (var pattern in patterns)
                {
                    var pos = SearchBytePattern(pattern.Data, data);
                    if (pos.Any())
                    {
                        var text = $"{pattern.Key} - {f} - {String.Join(", ", pos)}";
                        this.BeginInvoke(new Action(() =>
                        {
                            lbMatches.Items.Add(text);
                        }));
                    }
                }
            });
            ptq.Start();

            RecurseDirs(dir, f => ptq.AddItem(f));

            ptq.CompleteAdding();
            ptq.WaitForComplete();
        }

        private static byte[] ToBytes(string data)
        {
            if (!Guid.TryParse(data, out Guid guid))
                return Encoding.UTF8.GetBytes(data);

            return guid.ToByteArray();
        }

        private static void RecurseDirs(string dir, Action<string> action)
        {
            foreach (var d in Directory.GetDirectories(dir))
                RecurseDirs(d, action);
            foreach (var f in Directory.GetFiles(dir))
                action(f);
        }

        private static List<int> SearchBytePattern(byte[] pattern, byte[] bytes)
        {
            List<int> positions = new List<int>();

            int patternLength = pattern.Length;
            int totalLength = bytes.Length;
            byte firstMatchByte = pattern[0];

            for (int i = 0; i < totalLength; i++)
            {
                if (firstMatchByte == bytes[i] && totalLength - i >= patternLength)
                {
                    if (BytesEqual(bytes, i, pattern))
                    {
                        positions.Add(i);
                        i += patternLength - 1;
                    }
                }
            }
            return positions;
        }

        private static bool BytesEqual(ReadOnlySpan<byte> b1, int b2Idx, ReadOnlySpan<byte> b2)
        {
            return b1.Slice(b2Idx, b2.Length).SequenceEqual(b2);
        }

        private void lbMatches_DoubleClick(object sender, EventArgs e)
        {
            var (path, idx) = GetSelectedItem();
            
            if (path != null && !String.IsNullOrEmpty(_sourceProcess))
                Process.Start(_sourceProcess, $"\"{path}\" -s \"{CurrentSearches[idx]}\"");
        }

        private void lbMatches_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                var (path, idx) = GetSelectedItem();
                if (path != null)
                    Clipboard.SetData(DataFormats.StringFormat, path);
            }
        }

        private (string File, int Index) GetSelectedItem()
        {
            var selected = lbMatches.SelectedItem?.ToString();
            if (String.IsNullOrEmpty(selected))
                return (null, -1);

            var si = selected.IndexOf(" - ") + 3;
            var ei = selected.LastIndexOf(" - ");

            var file = selected.Substring(si, ei - si);
            var path = Path.Combine(tbDir.Text, file);

            var idx = int.Parse(selected.Substring(0, si - 3));
            return (path, idx);
        }
    }
}
