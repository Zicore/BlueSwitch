using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;

namespace BlueSwitch.Controls.Docking
{
    partial class RendererBase
    {

        private void InitializeComponent()
        {
            this.components = new Container();
            this._contextMenuStrip = new ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new ToolStripMenuItem();
            this._contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _contextMenuStrip
            // 
            this._contextMenuStrip.Items.AddRange(new ToolStripItem[] {
            this.removeToolStripMenuItem});
            this._contextMenuStrip.Name = "_contextMenuStrip";
            this._contextMenuStrip.Size = new Size(153, 48);
            this._contextMenuStrip.Opening += new CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new Size(152, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // RendererBase
            // 
            this.AllowDrop = true;
            this.BackColor = SystemColors.ControlDark;
            this.ClientSize = new Size(315, 285);
            this.ContextMenuStrip = this._contextMenuStrip;
            this.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "RendererBase";
            this._contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
