namespace BlueSwitch.Controls.Docking
{
    partial class SearchEditor
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.treeView = new System.Windows.Forms.TreeView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.textBoxEditor = new System.Windows.Forms.TextBox();
            this.btAdd = new System.Windows.Forms.Button();
            this.btRemove = new System.Windows.Forms.Button();
            this.saveFileDialogTags = new System.Windows.Forms.SaveFileDialog();
            this.listMetaData = new BlueSwitch.Controls.ListViewEx();
            this.columnTag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTagDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lbSwitch = new System.Windows.Forms.Label();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView.BackColor = System.Drawing.SystemColors.Control;
            this.treeView.ContextMenuStrip = this.contextMenuStrip;
            this.treeView.Location = new System.Drawing.Point(0, 27);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(210, 609);
            this.treeView.TabIndex = 0;
            this.treeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(108, 26);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // tbSearch
            // 
            this.tbSearch.Location = new System.Drawing.Point(0, 1);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(158, 20);
            this.tbSearch.TabIndex = 6;
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            this.tbSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyDown);
            this.tbSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyUp);
            // 
            // textBoxEditor
            // 
            this.textBoxEditor.Location = new System.Drawing.Point(12, 540);
            this.textBoxEditor.Name = "textBoxEditor";
            this.textBoxEditor.Size = new System.Drawing.Size(185, 20);
            this.textBoxEditor.TabIndex = 8;
            this.textBoxEditor.Visible = false;
            // 
            // btAdd
            // 
            this.btAdd.Location = new System.Drawing.Point(190, 1);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(20, 20);
            this.btAdd.TabIndex = 9;
            this.btAdd.Text = "+";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // btRemove
            // 
            this.btRemove.Location = new System.Drawing.Point(164, 1);
            this.btRemove.Name = "btRemove";
            this.btRemove.Size = new System.Drawing.Size(20, 20);
            this.btRemove.TabIndex = 9;
            this.btRemove.Text = "-";
            this.btRemove.UseVisualStyleBackColor = true;
            this.btRemove.Click += new System.EventHandler(this.btRemove_Click);
            // 
            // saveFileDialogTags
            // 
            this.saveFileDialogTags.Filter = "Meta Json Dateien|*.meta.json";
            this.saveFileDialogTags.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialogTags_FileOk);
            // 
            // listMetaData
            // 
            this.listMetaData.AllowColumnReorder = true;
            this.listMetaData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listMetaData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnTag,
            this.columnTagDescription});
            this.listMetaData.DoubleClickActivation = true;
            this.listMetaData.FullRowSelect = true;
            this.listMetaData.GridLines = true;
            this.listMetaData.Location = new System.Drawing.Point(216, 27);
            this.listMetaData.Name = "listMetaData";
            this.listMetaData.Size = new System.Drawing.Size(644, 609);
            this.listMetaData.TabIndex = 7;
            this.listMetaData.UseCompatibleStateImageBehavior = false;
            this.listMetaData.View = System.Windows.Forms.View.Details;
            this.listMetaData.SubItemClicked += new BlueSwitch.Controls.SubItemEventHandler(this.listMetaData_SubItemClicked);
            this.listMetaData.SubItemEndEditing += new BlueSwitch.Controls.SubItemEndEditingEventHandler(this.listMetaData_SubItemEndEditing);
            // 
            // columnTag
            // 
            this.columnTag.Text = "Tag";
            this.columnTag.Width = 202;
            // 
            // columnTagDescription
            // 
            this.columnTagDescription.Text = "Tag Description";
            this.columnTagDescription.Width = 190;
            // 
            // lbSwitch
            // 
            this.lbSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSwitch.Location = new System.Drawing.Point(216, 1);
            this.lbSwitch.Name = "lbSwitch";
            this.lbSwitch.Size = new System.Drawing.Size(644, 20);
            this.lbSwitch.TabIndex = 11;
            this.lbSwitch.Text = "Select a switch";
            this.lbSwitch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MetaEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 636);
            this.Controls.Add(this.lbSwitch);
            this.Controls.Add(this.btRemove);
            this.Controls.Add(this.btAdd);
            this.Controls.Add(this.textBoxEditor);
            this.Controls.Add(this.listMetaData);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.treeView);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MetaEditor";
            this.TabText = "Components";
            this.Text = "Meta Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MetaEditor_FormClosing);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.TextBox tbSearch;
        private ListViewEx listMetaData;
        private System.Windows.Forms.ColumnHeader columnTag;
        private System.Windows.Forms.ColumnHeader columnTagDescription;
        private System.Windows.Forms.TextBox textBoxEditor;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Button btRemove;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialogTags;
        private System.Windows.Forms.Label lbSwitch;
    }
}
