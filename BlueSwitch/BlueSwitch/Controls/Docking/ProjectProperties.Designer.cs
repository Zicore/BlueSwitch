namespace BlueSwitch.Controls.Docking
{
    partial class ProjectProperties
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
            this.tbProjectName = new System.Windows.Forms.TextBox();
            this.lbProjectName = new System.Windows.Forms.Label();
            this.lbSwitchesCount = new System.Windows.Forms.Label();
            this.lbConnectionsCount = new System.Windows.Forms.Label();
            this.lbSwitchesCountDisplay = new System.Windows.Forms.Label();
            this.lbConnectionsCountDisplay = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOkay = new System.Windows.Forms.Button();
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
            this.groupBox.Location = new System.Drawing.Point(12, 12);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(386, 115);
            this.groupBox.TabIndex = 1;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Project";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.52692F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.47308F));
            this.tableLayoutPanel.Controls.Add(this.lbConnectionsCountDisplay, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.lbSwitchesCountDisplay, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.lbSwitchesCount, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.tbProjectName, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.lbProjectName, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.lbConnectionsCount, 0, 2);
            this.tableLayoutPanel.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(374, 90);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // tbProjectName
            // 
            this.tbProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbProjectName.Location = new System.Drawing.Point(113, 3);
            this.tbProjectName.Name = "tbProjectName";
            this.tbProjectName.Size = new System.Drawing.Size(258, 20);
            this.tbProjectName.TabIndex = 0;
            // 
            // lbProjectName
            // 
            this.lbProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbProjectName.AutoSize = true;
            this.lbProjectName.Location = new System.Drawing.Point(3, 6);
            this.lbProjectName.Name = "lbProjectName";
            this.lbProjectName.Size = new System.Drawing.Size(104, 13);
            this.lbProjectName.TabIndex = 1;
            this.lbProjectName.Text = "Name";
            this.lbProjectName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbSwitchesCount
            // 
            this.lbSwitchesCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSwitchesCount.AutoSize = true;
            this.lbSwitchesCount.Location = new System.Drawing.Point(3, 31);
            this.lbSwitchesCount.Name = "lbSwitchesCount";
            this.lbSwitchesCount.Size = new System.Drawing.Size(104, 13);
            this.lbSwitchesCount.TabIndex = 2;
            this.lbSwitchesCount.Text = "Switches";
            this.lbSwitchesCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbConnectionsCount
            // 
            this.lbConnectionsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbConnectionsCount.AutoSize = true;
            this.lbConnectionsCount.Location = new System.Drawing.Point(3, 56);
            this.lbConnectionsCount.Name = "lbConnectionsCount";
            this.lbConnectionsCount.Size = new System.Drawing.Size(104, 13);
            this.lbConnectionsCount.TabIndex = 3;
            this.lbConnectionsCount.Text = "Connections";
            this.lbConnectionsCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbSwitchesCountDisplay
            // 
            this.lbSwitchesCountDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSwitchesCountDisplay.AutoSize = true;
            this.lbSwitchesCountDisplay.Location = new System.Drawing.Point(113, 31);
            this.lbSwitchesCountDisplay.Name = "lbSwitchesCountDisplay";
            this.lbSwitchesCountDisplay.Size = new System.Drawing.Size(258, 13);
            this.lbSwitchesCountDisplay.TabIndex = 4;
            this.lbSwitchesCountDisplay.Text = "0";
            this.lbSwitchesCountDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbConnectionsCountDisplay
            // 
            this.lbConnectionsCountDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbConnectionsCountDisplay.AutoSize = true;
            this.lbConnectionsCountDisplay.Location = new System.Drawing.Point(113, 56);
            this.lbConnectionsCountDisplay.Name = "lbConnectionsCountDisplay";
            this.lbConnectionsCountDisplay.Size = new System.Drawing.Size(258, 13);
            this.lbConnectionsCountDisplay.TabIndex = 5;
            this.lbConnectionsCountDisplay.Text = "0";
            this.lbConnectionsCountDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btCancel
            // 
            this.btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btCancel.Location = new System.Drawing.Point(132, 137);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 11;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btOkay
            // 
            this.btOkay.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btOkay.Location = new System.Drawing.Point(213, 137);
            this.btOkay.Name = "btOkay";
            this.btOkay.Size = new System.Drawing.Size(75, 23);
            this.btOkay.TabIndex = 10;
            this.btOkay.Text = "OK";
            this.btOkay.UseVisualStyleBackColor = true;
            this.btOkay.Click += new System.EventHandler(this.btOkay_Click);
            // 
            // ProjectProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 172);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOkay);
            this.Controls.Add(this.groupBox);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ProjectProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TabText = "Components";
            this.Text = "Project Properties";
            this.groupBox.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TextBox tbProjectName;
        private System.Windows.Forms.Label lbProjectName;
        private System.Windows.Forms.Label lbConnectionsCountDisplay;
        private System.Windows.Forms.Label lbSwitchesCountDisplay;
        private System.Windows.Forms.Label lbSwitchesCount;
        private System.Windows.Forms.Label lbConnectionsCount;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOkay;
    }
}
