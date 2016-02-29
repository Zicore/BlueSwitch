namespace BlueSwitch.Controls.Docking
{
    partial class VariableEditor
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
            this.btAdd = new System.Windows.Forms.Button();
            this.textBoxEditor = new System.Windows.Forms.TextBox();
            this.comboBoxEditor = new System.Windows.Forms.ComboBox();
            this.btRemove = new System.Windows.Forms.Button();
            this.btRefresh = new System.Windows.Forms.Button();
            this.listVariables = new ListViewEx();
            this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // btAdd
            // 
            this.btAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAdd.Location = new System.Drawing.Point(187, -1);
            this.btAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(74, 27);
            this.btAdd.TabIndex = 1;
            this.btAdd.Text = "Add";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // textBoxEditor
            // 
            this.textBoxEditor.Location = new System.Drawing.Point(12, 506);
            this.textBoxEditor.Name = "textBoxEditor";
            this.textBoxEditor.Size = new System.Drawing.Size(200, 20);
            this.textBoxEditor.TabIndex = 4;
            this.textBoxEditor.Visible = false;
            // 
            // comboBoxEditor
            // 
            this.comboBoxEditor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEditor.FormattingEnabled = true;
            this.comboBoxEditor.Location = new System.Drawing.Point(12, 479);
            this.comboBoxEditor.Name = "comboBoxEditor";
            this.comboBoxEditor.Size = new System.Drawing.Size(200, 21);
            this.comboBoxEditor.TabIndex = 5;
            this.comboBoxEditor.Visible = false;
            // 
            // btRemove
            // 
            this.btRemove.Location = new System.Drawing.Point(-1, -1);
            this.btRemove.Margin = new System.Windows.Forms.Padding(0);
            this.btRemove.Name = "btRemove";
            this.btRemove.Size = new System.Drawing.Size(68, 27);
            this.btRemove.TabIndex = 1;
            this.btRemove.Text = "Remove";
            this.btRemove.UseVisualStyleBackColor = true;
            this.btRemove.Click += new System.EventHandler(this.btRemove_Click);
            // 
            // btRefresh
            // 
            this.btRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btRefresh.Location = new System.Drawing.Point(67, -1);
            this.btRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(120, 27);
            this.btRefresh.TabIndex = 1;
            this.btRefresh.Text = "Refresh";
            this.btRefresh.UseVisualStyleBackColor = true;
            this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
            // 
            // listVariables
            // 
            this.listVariables.AllowColumnReorder = true;
            this.listVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listVariables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnType,
            this.columnName,
            this.columnValue});
            this.listVariables.DoubleClickActivation = true;
            this.listVariables.FullRowSelect = true;
            this.listVariables.GridLines = true;
            this.listVariables.Location = new System.Drawing.Point(0, 29);
            this.listVariables.Name = "listVariables";
            this.listVariables.Size = new System.Drawing.Size(261, 510);
            this.listVariables.TabIndex = 2;
            this.listVariables.UseCompatibleStateImageBehavior = false;
            this.listVariables.View = System.Windows.Forms.View.Details;
            this.listVariables.SubItemClicked += new SubItemEventHandler(this.listVariables_SubItemClicked);
            this.listVariables.SubItemBeginEditing += new SubItemEventHandler(this.listVariables_SubItemBeginEditing);
            this.listVariables.SubItemEndEditing += new SubItemEndEditingEventHandler(this.listVariables_SubItemEndEditing);
            this.listVariables.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listVariables_ItemDrag);
            // 
            // columnType
            // 
            this.columnType.Text = "Type";
            this.columnType.Width = 65;
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.Width = 87;
            // 
            // columnValue
            // 
            this.columnValue.Text = "Value";
            this.columnValue.Width = 90;
            // 
            // VariableEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 538);
            this.Controls.Add(this.comboBoxEditor);
            this.Controls.Add(this.textBoxEditor);
            this.Controls.Add(this.listVariables);
            this.Controls.Add(this.btRefresh);
            this.Controls.Add(this.btRemove);
            this.Controls.Add(this.btAdd);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "VariableEditor";
            this.Text = "Variables";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btAdd;
        private ListViewEx listVariables;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnType;
        private System.Windows.Forms.TextBox textBoxEditor;
        private System.Windows.Forms.ComboBox comboBoxEditor;
        private System.Windows.Forms.Button btRemove;
        private System.Windows.Forms.ColumnHeader columnValue;
        private System.Windows.Forms.Button btRefresh;
    }
}
