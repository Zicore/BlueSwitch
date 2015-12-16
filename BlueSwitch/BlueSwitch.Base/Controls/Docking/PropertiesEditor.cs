using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlueSwitch.Renderer.Components.Base;
using BlueSwitch.Renderer.Components.Switches.Base;
using WeifenLuo.WinFormsUI.Docking;

namespace BlueSwitch.Renderer.Controls.Docking
{
    public partial class PropertiesEditor : DockContent
    {
        public RenderingEngine RenderingEngine { get; set; }

        public PropertiesEditor(RenderingEngine renderingEngine)
        {
            RenderingEngine = renderingEngine;
            InitializeComponent();
            RenderingEngine.MouseService.MouseUp += MouseServiceOnMouseUp;   
        }

        private void MouseServiceOnMouseUp(object sender, MouseEventArgs e)
        {
            var items = RenderingEngine.CurrentProject.Items.Where(x => x.IsSelected).ToList();

            comboBoxSwitches.DisplayMember = "DisplayName";
            comboBoxSwitches.DataSource = items;
        }

        private void comboBoxSwitches_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = comboBoxSwitches.SelectedItem as SwitchBase;
            if (selectedItem != null)
            {
                comboBoxInputs.DisplayMember = "DisplayName";
                comboBoxInputs.DataSource = selectedItem.Inputs;
            }
        }

        private void btSetValue_Click(object sender, EventArgs e)
        {
            var selectedInput = comboBoxInputs.SelectedItem as InputOutputBase;

            if (selectedInput != null)
            {
                selectedInput.Data = new DataContainer(Convert.ChangeType(textBoxValue.Text, selectedInput.Signature.BaseType));
            }
        }

        private void comboBoxInputs_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedInput = comboBoxInputs.SelectedItem as InputOutputBase;

            if (selectedInput?.Data != null)
            {
                textBoxValue.Text = selectedInput.Data.Value.ToString();
            }
            else
            {
                textBoxValue.Text = "";
            }
        }
    }
}
