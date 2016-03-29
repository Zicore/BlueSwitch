namespace BlueSwitch.Controls.Docking
{
    partial class HelpEditor
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
            this.lbSwitch = new System.Windows.Forms.Label();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.listInputs = new BlueSwitch.Controls.ListViewEx();
            this.columnTitleInput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDescriptionInput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnIndexInput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listOutputs = new BlueSwitch.Controls.ListViewEx();
            this.columnTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnIndexOutput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btAddOutput = new System.Windows.Forms.Button();
            this.btRemoveOutput = new System.Windows.Forms.Button();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
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
            this.treeView.Size = new System.Drawing.Size(210, 494);
            this.treeView.TabIndex = 0;
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
            this.tbSearch.Size = new System.Drawing.Size(210, 20);
            this.tbSearch.TabIndex = 6;
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            this.tbSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyDown);
            this.tbSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyUp);
            // 
            // textBoxEditor
            // 
            this.textBoxEditor.Location = new System.Drawing.Point(10, 489);
            this.textBoxEditor.Name = "textBoxEditor";
            this.textBoxEditor.Size = new System.Drawing.Size(185, 20);
            this.textBoxEditor.TabIndex = 8;
            this.textBoxEditor.Visible = false;
            this.textBoxEditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxEditor_KeyDown);
            // 
            // btAdd
            // 
            this.btAdd.Location = new System.Drawing.Point(1, 3);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(20, 20);
            this.btAdd.TabIndex = 9;
            this.btAdd.Text = "+";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // btRemove
            // 
            this.btRemove.Location = new System.Drawing.Point(23, 3);
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
            // lbSwitch
            // 
            this.lbSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSwitch.Location = new System.Drawing.Point(216, 1);
            this.lbSwitch.Name = "lbSwitch";
            this.lbSwitch.Size = new System.Drawing.Size(706, 20);
            this.lbSwitch.TabIndex = 11;
            this.lbSwitch.Text = "Select a switch";
            this.lbSwitch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(219, 53);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.listInputs);
            this.splitContainer.Panel1.Controls.Add(this.btAdd);
            this.splitContainer.Panel1.Controls.Add(this.btRemove);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.listOutputs);
            this.splitContainer.Panel2.Controls.Add(this.btAddOutput);
            this.splitContainer.Panel2.Controls.Add(this.btRemoveOutput);
            this.splitContainer.Size = new System.Drawing.Size(703, 468);
            this.splitContainer.SplitterDistance = 234;
            this.splitContainer.TabIndex = 13;
            // 
            // listInputs
            // 
            this.listInputs.AllowColumnReorder = true;
            this.listInputs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listInputs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnTitleInput,
            this.columnDescriptionInput,
            this.columnIndexInput});
            this.listInputs.DoubleClickActivation = true;
            this.listInputs.FullRowSelect = true;
            this.listInputs.GridLines = true;
            this.listInputs.Location = new System.Drawing.Point(0, 24);
            this.listInputs.Name = "listInputs";
            this.listInputs.Size = new System.Drawing.Size(703, 210);
            this.listInputs.TabIndex = 7;
            this.listInputs.UseCompatibleStateImageBehavior = false;
            this.listInputs.View = System.Windows.Forms.View.Details;
            this.listInputs.SubItemClicked += new BlueSwitch.Controls.SubItemEventHandler(this.listMetaData_SubItemClicked);
            this.listInputs.SubItemEndEditing += new BlueSwitch.Controls.SubItemEndEditingEventHandler(this.listMetaData_SubItemEndEditing);
            // 
            // columnTitleInput
            // 
            this.columnTitleInput.Text = "Title";
            this.columnTitleInput.Width = 134;
            // 
            // columnDescriptionInput
            // 
            this.columnDescriptionInput.Text = "Description";
            this.columnDescriptionInput.Width = 225;
            // 
            // columnIndexInput
            // 
            this.columnIndexInput.Text = "Index";
            // 
            // listOutputs
            // 
            this.listOutputs.AllowColumnReorder = true;
            this.listOutputs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listOutputs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnTitle,
            this.columnDescription,
            this.columnIndexOutput});
            this.listOutputs.DoubleClickActivation = true;
            this.listOutputs.FullRowSelect = true;
            this.listOutputs.GridLines = true;
            this.listOutputs.Location = new System.Drawing.Point(0, 25);
            this.listOutputs.Name = "listOutputs";
            this.listOutputs.Size = new System.Drawing.Size(703, 205);
            this.listOutputs.TabIndex = 7;
            this.listOutputs.UseCompatibleStateImageBehavior = false;
            this.listOutputs.View = System.Windows.Forms.View.Details;
            this.listOutputs.SubItemClicked += new BlueSwitch.Controls.SubItemEventHandler(this.listOutputs_SubItemClicked);
            this.listOutputs.SubItemEndEditing += new BlueSwitch.Controls.SubItemEndEditingEventHandler(this.listViewOutputs_SubItemEndEditing);
            // 
            // columnTitle
            // 
            this.columnTitle.Text = "Title";
            this.columnTitle.Width = 134;
            // 
            // columnDescription
            // 
            this.columnDescription.Text = "Description";
            this.columnDescription.Width = 225;
            // 
            // columnIndexOutput
            // 
            this.columnIndexOutput.Text = "Index";
            // 
            // btAddOutput
            // 
            this.btAddOutput.Location = new System.Drawing.Point(1, 3);
            this.btAddOutput.Name = "btAddOutput";
            this.btAddOutput.Size = new System.Drawing.Size(20, 20);
            this.btAddOutput.TabIndex = 9;
            this.btAddOutput.Text = "+";
            this.btAddOutput.UseVisualStyleBackColor = true;
            this.btAddOutput.Click += new System.EventHandler(this.btAddOutput_Click);
            // 
            // btRemoveOutput
            // 
            this.btRemoveOutput.Location = new System.Drawing.Point(23, 3);
            this.btRemoveOutput.Name = "btRemoveOutput";
            this.btRemoveOutput.Size = new System.Drawing.Size(20, 20);
            this.btRemoveOutput.TabIndex = 9;
            this.btRemoveOutput.Text = "-";
            this.btRemoveOutput.UseVisualStyleBackColor = true;
            this.btRemoveOutput.Click += new System.EventHandler(this.btRemoveOutput_Click);
            // 
            // tbTitle
            // 
            this.tbTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTitle.Location = new System.Drawing.Point(219, 27);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(703, 20);
            this.tbTitle.TabIndex = 14;
            this.tbTitle.TextChanged += new System.EventHandler(this.tbTitle_TextChanged);
            // 
            // HelpEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 521);
            this.Controls.Add(this.tbTitle);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.lbSwitch);
            this.Controls.Add(this.textBoxEditor);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.treeView);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "HelpEditor";
            this.TabText = "Components";
            this.Text = "Help Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MetaEditor_FormClosing);
            this.contextMenuStrip.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.TextBox textBoxEditor;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Button btRemove;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialogTags;
        private System.Windows.Forms.Label lbSwitch;
        private System.Windows.Forms.ColumnHeader columnTitle;
        private System.Windows.Forms.ColumnHeader columnDescription;
        private ListViewEx listOutputs;
        private ListViewEx listInputs;
        private System.Windows.Forms.ColumnHeader columnTitleInput;
        private System.Windows.Forms.ColumnHeader columnDescriptionInput;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.ColumnHeader columnIndexInput;
        private System.Windows.Forms.ColumnHeader columnIndexOutput;
        private System.Windows.Forms.Button btAddOutput;
        private System.Windows.Forms.Button btRemoveOutput;
    }
}
