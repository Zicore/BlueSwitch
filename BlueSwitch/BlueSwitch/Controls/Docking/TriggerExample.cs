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
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Trigger.Types;
using Newtonsoft.Json;

namespace BlueSwitch.Controls.Docking
{
    public partial class TriggerExample : DockContent
    {
        [JsonIgnore]
        public RenderingEngine RenderingEngine { get; set; }
        
        public TriggerExample(RenderingEngine renderingEngine)
        {
            RenderingEngine = renderingEngine;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RenderingEngine.EventManager.Run(EventTypeBase.StartSingle, "1");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RenderingEngine.EventManager.Run(EventTypeBase.StartSingle, "2");
        }
    }
}
