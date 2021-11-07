using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZDCoreEditorUI.UI
{
    partial class FormCoreView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.pnlData = new System.Windows.Forms.Panel();
            this.cmsData = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmFollow = new System.Windows.Forms.ToolStripMenuItem();
            this.txtType = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearchAll = new System.Windows.Forms.Button();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsArchiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exportAsJSONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAsJSONWithTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToPreviousSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToNextSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.expandAllTreesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.cmsData.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer.Location = new System.Drawing.Point(6, 56);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.txtFile);
            this.splitContainer.Panel1.Controls.Add(this.pnlMain);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.txtNotes);
            this.splitContainer.Panel2.Controls.Add(this.pnlData);
            this.splitContainer.Panel2.Controls.Add(this.txtType);
            this.splitContainer.Size = new System.Drawing.Size(1772, 814);
            this.splitContainer.SplitterDistance = 744;
            this.splitContainer.TabIndex = 0;
            this.splitContainer.Text = "splitContainer1";
            // 
            // txtFile
            // 
            this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile.Location = new System.Drawing.Point(0, 0);
            this.txtFile.Name = "txtFile";
            this.txtFile.ReadOnly = true;
            this.txtFile.Size = new System.Drawing.Size(744, 23);
            this.txtFile.TabIndex = 2;
            this.txtFile.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtFile_MouseClick);
            // 
            // pnlMain
            // 
            this.pnlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMain.Location = new System.Drawing.Point(0, 29);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(744, 814);
            this.pnlMain.TabIndex = 1;
            // 
            // txtNotes
            // 
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.Location = new System.Drawing.Point(340, 0);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(684, 23);
            this.txtNotes.TabIndex = 2;
            this.txtNotes.TextChanged += new System.EventHandler(this.txtNotes_TextChanged);
            // 
            // pnlData
            // 
            this.pnlData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlData.ContextMenuStrip = this.cmsData;
            this.pnlData.Location = new System.Drawing.Point(0, 29);
            this.pnlData.Name = "pnlData";
            this.pnlData.Size = new System.Drawing.Size(1024, 814);
            this.pnlData.TabIndex = 0;
            // 
            // cmsData
            // 
            this.cmsData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmFollow});
            this.cmsData.Name = "cmsData";
            this.cmsData.ShowImageMargin = false;
            this.cmsData.Size = new System.Drawing.Size(140, 26);
            this.cmsData.Opening += new System.ComponentModel.CancelEventHandler(this.cmsData_Opening);
            // 
            // tsmFollow
            // 
            this.tsmFollow.Name = "tsmFollow";
            this.tsmFollow.Size = new System.Drawing.Size(139, 22);
            this.tsmFollow.Text = "Follow Reference";
            this.tsmFollow.Click += new System.EventHandler(this.tsmFollow_Click);
            // 
            // txtType
            // 
            this.txtType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtType.Location = new System.Drawing.Point(0, 0);
            this.txtType.Name = "txtType";
            this.txtType.ReadOnly = true;
            this.txtType.Size = new System.Drawing.Size(334, 23);
            this.txtType.TabIndex = 1;
            this.txtType.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtType_MouseClick);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(421, 27);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 23);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(6, 27);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(409, 23);
            this.txtSearch.TabIndex = 3;
            this.txtSearch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtSearch_MouseClick);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // btnSearchAll
            // 
            this.btnSearchAll.Location = new System.Drawing.Point(527, 27);
            this.btnSearchAll.Name = "btnSearchAll";
            this.btnSearchAll.Size = new System.Drawing.Size(100, 23);
            this.btnSearchAll.TabIndex = 7;
            this.btnSearchAll.Text = "Search All";
            this.btnSearchAll.UseVisualStyleBackColor = true;
            this.btnSearchAll.Click += new System.EventHandler(this.btnSearchAll_Click);
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(1784, 24);
            this.mainMenuStrip.TabIndex = 8;
            this.mainMenuStrip.Text = "mainMenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveAsArchiveToolStripMenuItem,
            this.toolStripSeparator2,
            this.exportAsJSONToolStripMenuItem,
            this.exportAsJSONWithTypesToolStripMenuItem,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(188, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // saveAsArchiveToolStripMenuItem
            // 
            this.saveAsArchiveToolStripMenuItem.Name = "saveAsArchiveToolStripMenuItem";
            this.saveAsArchiveToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.saveAsArchiveToolStripMenuItem.Text = "Save As Archive...";
            this.saveAsArchiveToolStripMenuItem.Click += new System.EventHandler(this.saveAsArchiveToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(188, 6);
            // 
            // exportAsJSONToolStripMenuItem
            // 
            this.exportAsJSONToolStripMenuItem.Name = "exportAsJSONToolStripMenuItem";
            this.exportAsJSONToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.exportAsJSONToolStripMenuItem.Text = "Export as Json...";
            this.exportAsJSONToolStripMenuItem.Click += new System.EventHandler(this.exportAsJSONToolStripMenuItem_Click);
            // 
            // exportAsJSONWithTypesToolStripMenuItem
            // 
            this.exportAsJSONWithTypesToolStripMenuItem.Name = "exportAsJSONWithTypesToolStripMenuItem";
            this.exportAsJSONWithTypesToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.exportAsJSONWithTypesToolStripMenuItem.Text = "Export as Typed Json...";
            this.exportAsJSONWithTypesToolStripMenuItem.Click += new System.EventHandler(this.exportAsJSONWithTypesToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(188, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToPreviousSelectionToolStripMenuItem,
            this.goToNextSelectionToolStripMenuItem,
            this.toolStripSeparator4,
            this.expandAllTreesToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // goToPreviousSelectionToolStripMenuItem
            // 
            this.goToPreviousSelectionToolStripMenuItem.Name = "goToPreviousSelectionToolStripMenuItem";
            this.goToPreviousSelectionToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.goToPreviousSelectionToolStripMenuItem.Text = "Go to Previous Object";
            this.goToPreviousSelectionToolStripMenuItem.Click += new System.EventHandler(this.goToPreviousSelectionToolStripMenuItem_Click);
            // 
            // goToNextSelectionToolStripMenuItem
            // 
            this.goToNextSelectionToolStripMenuItem.Name = "goToNextSelectionToolStripMenuItem";
            this.goToNextSelectionToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.goToNextSelectionToolStripMenuItem.Text = "Go to Next Object";
            this.goToNextSelectionToolStripMenuItem.Click += new System.EventHandler(this.goToNextSelectionToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(186, 6);
            // 
            // expandAllTreesToolStripMenuItem
            // 
            this.expandAllTreesToolStripMenuItem.Name = "expandAllTreesToolStripMenuItem";
            this.expandAllTreesToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.expandAllTreesToolStripMenuItem.Text = "Expand All Trees";
            this.expandAllTreesToolStripMenuItem.Click += new System.EventHandler(this.expandAllTreesToolStripMenuItem_Click);
            // 
            // FormCoreView
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1784, 877);
            this.Controls.Add(this.mainMenuStrip);
            this.Controls.Add(this.btnSearchAll);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.splitContainer);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "FormCoreView";
            this.Text = "Core Viewer";
            this.Load += new System.EventHandler(this.FormCoreView_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormCoreView_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormCoreView_DragEnter);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormCoreView_MouseDown);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.cmsData.ResumeLayout(false);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearchAll;
        private System.Windows.Forms.Panel pnlData;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.ContextMenuStrip cmsData;
        private System.Windows.Forms.ToolStripMenuItem tsmFollow;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exportAsJSONToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportAsJSONWithTypesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToPreviousSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToNextSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem expandAllTreesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsArchiveToolStripMenuItem;
    }
}