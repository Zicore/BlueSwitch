namespace BlueSwitch.Controls.Docking
{
    partial class PropertiesEditor
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btSetValue = new System.Windows.Forms.Button();
            this.comboBoxInputs = new System.Windows.Forms.ComboBox();
            this.comboBoxSwitches = new System.Windows.Forms.ComboBox();
            this.textBoxValue = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.btSetValue);
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxInputs);
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxSwitches);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxValue);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Size = new System.Drawing.Size(284, 459);
            this.splitContainer1.SplitterDistance = 87;
            this.splitContainer1.TabIndex = 0;
            // 
            // btSetValue
            // 
            this.btSetValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSetValue.Location = new System.Drawing.Point(245, 54);
            this.btSetValue.Name = "btSetValue";
            this.btSetValue.Size = new System.Drawing.Size(39, 20);
            this.btSetValue.TabIndex = 6;
            this.btSetValue.Text = ">>";
            this.btSetValue.UseVisualStyleBackColor = true;
            this.btSetValue.Click += new System.EventHandler(this.btSetValue_Click);
            // 
            // comboBoxInputs
            // 
            this.comboBoxInputs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxInputs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInputs.FormattingEnabled = true;
            this.comboBoxInputs.Location = new System.Drawing.Point(0, 27);
            this.comboBoxInputs.Name = "comboBoxInputs";
            this.comboBoxInputs.Size = new System.Drawing.Size(284, 21);
            this.comboBoxInputs.TabIndex = 5;
            this.comboBoxInputs.SelectedIndexChanged += new System.EventHandler(this.comboBoxInputs_SelectedIndexChanged);
            // 
            // comboBoxSwitches
            // 
            this.comboBoxSwitches.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSwitches.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSwitches.FormattingEnabled = true;
            this.comboBoxSwitches.Location = new System.Drawing.Point(0, 0);
            this.comboBoxSwitches.Name = "comboBoxSwitches";
            this.comboBoxSwitches.Size = new System.Drawing.Size(284, 21);
            this.comboBoxSwitches.TabIndex = 4;
            this.comboBoxSwitches.SelectedIndexChanged += new System.EventHandler(this.comboBoxSwitches_SelectedIndexChanged);
            // 
            // textBoxValue
            // 
            this.textBoxValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxValue.Location = new System.Drawing.Point(0, 54);
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.Size = new System.Drawing.Size(239, 20);
            this.textBoxValue.TabIndex = 3;
            // 
            // PropertiesEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(284, 459);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PropertiesEditor";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.ComboBox comboBoxInputs;
        private System.Windows.Forms.ComboBox comboBoxSwitches;
        private System.Windows.Forms.Button btSetValue;
    }
}
