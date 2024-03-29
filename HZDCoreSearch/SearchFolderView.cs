﻿using HZDCoreEditor.Util;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HZDCoreSearch;

public partial class SearchFolderView : Form
{
    private readonly string _sourceProcess;
    private bool _loading = true;
    private string[] CurrentSearches;
    private ByteSearch Search;

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
        if (btnSearch.Text == "Stop Search")
        {
            Search?.Stop();
            return;
        }

        btnSearch.Text = "Stop Search";

        CurrentSearches = tbSearch.Text.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        lbMatches.Items.Clear();

        Search = new ByteSearch(tbDir.Text);
        try
        {
            var byteSearches = CurrentSearches.Select((x, i) => (i.ToString(), ToBytes(x))).ToList();
            await Search.SearchAsync(byteSearches, x =>
            {
                foreach (var r in x.Results)
                {
                    var text = $"{r.Key} - {x.File} - {String.Join(", ", r.Positions)}";
                    this.BeginInvoke(new Action(() =>
                    {
                        lbMatches.Items.Add(text);
                    }));
                }
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show("Search Error: " + ex.Message);
        }

        Search = null;
        btnSearch.Text = "Search";
    }

    private static byte[] ToBytes(string data)
    {
        if (!Guid.TryParse(data, out Guid guid))
            return Encoding.UTF8.GetBytes(data);

        return guid.ToByteArray();
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
