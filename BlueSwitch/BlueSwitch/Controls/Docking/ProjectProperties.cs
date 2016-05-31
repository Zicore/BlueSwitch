using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.IO;
using BlueSwitch.Base.Meta.Help;
using BlueSwitch.Base.Meta.Search;
using BlueSwitch.Base.Reflection;
using Newtonsoft.Json;
using WeifenLuo.WinFormsUI.Docking;

namespace BlueSwitch.Controls.Docking
{
    public partial class ProjectProperties : DockContent
    {

        [JsonIgnore]
        public RenderingEngine RenderingEngine { get; set; }

        public ProjectProperties(RenderingEngine renderingEngine)
        {
            RenderingEngine = renderingEngine;
            InitializeComponent();

            tbProjectName.Text = RenderingEngine.CurrentProject.Name;
            lbSwitchesCountDisplay.Text = RenderingEngine.CurrentProject.Items.Count.ToString();
            lbConnectionsCountDisplay.Text = RenderingEngine.CurrentProject.Connections.Count.ToString();
        }

        private bool ValidateData()
        {
            try
            {
                RenderingEngine.CurrentProject.Name = tbProjectName.Text;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void btOkay_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                Close();
            }
            else
            {
                //MessageBox.Show("Error ???");
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
