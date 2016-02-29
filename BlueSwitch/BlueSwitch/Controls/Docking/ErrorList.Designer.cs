namespace BlueSwitch.Controls.Docking
{
    partial class ErrorList
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
            this.listErrors = new ListViewEx();
            this.columnNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnStep = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnNode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnException = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listErrors
            // 
            this.listErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnNumber,
            this.columnStep,
            this.columnName,
            this.columnNode,
            this.columnException,
            this.columnDescription});
            this.listErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listErrors.FullRowSelect = true;
            this.listErrors.Location = new System.Drawing.Point(0, 0);
            this.listErrors.Name = "listErrors";
            this.listErrors.Size = new System.Drawing.Size(1105, 261);
            this.listErrors.TabIndex = 0;
            this.listErrors.UseCompatibleStateImageBehavior = false;
            this.listErrors.View = System.Windows.Forms.View.Details;
            // 
            // columnNumber
            // 
            this.columnNumber.Text = "#";
            this.columnNumber.Width = 29;
            // 
            // columnStep
            // 
            this.columnStep.Text = "Step";
            this.columnStep.Width = 66;
            // 
            // columnName
            // 
            this.columnName.Text = "Origin";
            this.columnName.Width = 137;
            // 
            // columnNode
            // 
            this.columnNode.Text = "Origin Description";
            this.columnNode.Width = 164;
            // 
            // columnException
            // 
            this.columnException.Text = "Exception";
            this.columnException.Width = 94;
            // 
            // columnDescription
            // 
            this.columnDescription.Text = "Description";
            this.columnDescription.Width = 710;
            // 
            // ErrorList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1105, 261);
            this.Controls.Add(this.listErrors);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ErrorList";
            this.Text = "Error List";
            this.ResumeLayout(false);

        }

        #endregion

        private ListViewEx listErrors;
        private System.Windows.Forms.ColumnHeader columnNumber;
        private System.Windows.Forms.ColumnHeader columnStep;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnNode;
        private System.Windows.Forms.ColumnHeader columnException;
        private System.Windows.Forms.ColumnHeader columnDescription;
    }
}
