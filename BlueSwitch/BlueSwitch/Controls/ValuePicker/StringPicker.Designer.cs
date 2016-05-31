namespace BlueSwitch.Controls.ValuePicker
{
    partial class StringPicker
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
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tbValue = new System.Windows.Forms.TextBox();
            this.lbProjectName = new System.Windows.Forms.Label();
            this.btBrowseFolder = new System.Windows.Forms.Button();
            this.btBrowseFile = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOkay = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox.Controls.Add(this.tableLayoutPanel);
            this.groupBox.Location = new System.Drawing.Point(1, 4);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(601, 53);
            this.groupBox.TabIndex = 2;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "String";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel.Controls.Add(this.tbValue, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.lbProjectName, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.btBrowseFolder, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.btBrowseFile, 3, 0);
            this.tableLayoutPanel.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(589, 28);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // tbValue
            // 
            this.tbValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbValue.Location = new System.Drawing.Point(123, 4);
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new System.Drawing.Size(367, 20);
            this.tbValue.TabIndex = 0;
            // 
            // lbProjectName
            // 
            this.lbProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbProjectName.AutoSize = true;
            this.lbProjectName.Location = new System.Drawing.Point(3, 7);
            this.lbProjectName.Name = "lbProjectName";
            this.lbProjectName.Size = new System.Drawing.Size(114, 13);
            this.lbProjectName.TabIndex = 1;
            this.lbProjectName.Text = "Value";
            this.lbProjectName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btBrowseFolder
            // 
            this.btBrowseFolder.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btBrowseFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btBrowseFolder.Location = new System.Drawing.Point(496, 3);
            this.btBrowseFolder.Name = "btBrowseFolder";
            this.btBrowseFolder.Size = new System.Drawing.Size(42, 22);
            this.btBrowseFolder.TabIndex = 13;
            this.btBrowseFolder.Text = "..";
            this.btBrowseFolder.UseVisualStyleBackColor = true;
            this.btBrowseFolder.Click += new System.EventHandler(this.btBrowseFolder_Click);
            // 
            // btBrowseFile
            // 
            this.btBrowseFile.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btBrowseFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btBrowseFile.Location = new System.Drawing.Point(544, 3);
            this.btBrowseFile.Name = "btBrowseFile";
            this.btBrowseFile.Size = new System.Drawing.Size(42, 22);
            this.btBrowseFile.TabIndex = 14;
            this.btBrowseFile.Text = "...";
            this.btBrowseFile.UseVisualStyleBackColor = true;
            this.btBrowseFile.Click += new System.EventHandler(this.btBrowseFile_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btCancel.Location = new System.Drawing.Point(233, 63);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 13;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btOkay
            // 
            this.btOkay.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btOkay.Location = new System.Drawing.Point(314, 63);
            this.btOkay.Name = "btOkay";
            this.btOkay.Size = new System.Drawing.Size(75, 23);
            this.btOkay.TabIndex = 12;
            this.btOkay.Text = "OK";
            this.btOkay.UseVisualStyleBackColor = true;
            this.btOkay.Click += new System.EventHandler(this.btOkay_Click);
            // 
            // StringPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 95);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOkay);
            this.Controls.Add(this.groupBox);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "StringPicker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.groupBox.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TextBox tbValue;
        private System.Windows.Forms.Label lbProjectName;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOkay;
        private System.Windows.Forms.Button btBrowseFolder;
        private System.Windows.Forms.Button btBrowseFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}
