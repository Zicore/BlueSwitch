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
    public partial class NumberPicker : DockContent
    {
        public NumberPicker(object value)
        {
            InitializeComponent();
            if (value == null)
            {
                tbValue.Value = 0;
            }
            else
            {
                tbValue.Value = Convert.ToDecimal(value);
            }
        }

        public void PrepareByType(Type type)
        {
            if (type == typeof(int))
            {
                tbValue.Maximum = int.MaxValue;
                tbValue.Minimum = int.MinValue;
                tbValue.DecimalPlaces = 0;
            }
            if (type == typeof(short))
            {
                tbValue.Maximum = short.MaxValue;
                tbValue.Minimum = short.MinValue;
                tbValue.DecimalPlaces = 0;
            }
            if (type == typeof(decimal))
            {
                tbValue.Maximum = decimal.MaxValue;
                tbValue.Minimum = decimal.MinValue;
                tbValue.DecimalPlaces = 8;
            }

            if (type == typeof(float))
            {
                tbValue.Maximum = long.MaxValue;
                tbValue.Minimum = long.MinValue;
                tbValue.DecimalPlaces = 6;
            }

            if (type == typeof(double))
            {
                tbValue.Maximum = long.MaxValue;
                tbValue.Minimum = long.MinValue;
                tbValue.DecimalPlaces = 10;
            }
        }

        public object Value { get; set; }

        private void btOkay_Click(object sender, EventArgs e)
        {
            Value = tbValue.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
