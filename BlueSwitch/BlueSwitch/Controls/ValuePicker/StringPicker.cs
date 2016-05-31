using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace BlueSwitch.Controls.ValuePicker
{
    public partial class StringPicker : DockContent
    {
        public StringPicker(object value)
        {
            InitializeComponent();
            if (value == null)
            {
                tbValue.Text = "";
            }
            else
            {
                tbValue.Text = value.ToString();
            }
        }

        public object Value { get; set; }

        private void btOkay_Click(object sender, EventArgs e)
        {
            Value = tbValue.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btBrowseFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbValue.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btBrowseFile_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                tbValue.Text = saveFileDialog.FileName;
            }
        }
    }
}
