using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using Newtonsoft.Json;
using WeifenLuo.WinFormsUI.Docking;

namespace BlueSwitch.Controls.Docking
{
    public partial class SwitchesTree : DockContent
    {
        private bool _tradeFairMode = false;

        [JsonIgnore]
        public RenderingEngine RenderingEngine { get; set; }

        [JsonIgnore]
        public bool TradeFairMode
        {
            get { return _tradeFairMode; }
            set
            {
                _tradeFairMode = value;
                UpdateTree();
            }
        }

        public SwitchesTree(RenderingEngine renderingEngine)
        {
            RenderingEngine = renderingEngine;
            RenderingEngine.DebugModeChanged += RenderingEngineOnDebugModeChanged;
            RenderingEngine.ProjectLoaded += RenderingEngineOnProjectLoaded;
            InitializeComponent();
            treeView.AllowDrop = true;
            UpdateCheckState();
        }

        private void RenderingEngineOnProjectLoaded(object sender, EventArgs eventArgs)
        {
            tbProject.Text = RenderingEngine.CurrentProject.Name;
        }

        private void RenderingEngineOnDebugModeChanged(object sender, EventArgs eventArgs)
        {
            UpdateCheckState();
        }

        private void UpdateCheckState()
        {
            btnDebug.CheckedChanged -= btnDebug_CheckedChanged;
            btnDebug.Checked = RenderingEngine.DebugMode;
            btnDebug.CheckedChanged += btnDebug_CheckedChanged;
        }

        public void UpdateTree()
        {
            var av = new List<SwitchBase>(RenderingEngine.AvailableSwitches.Where(x=>!x.AutoDiscoverDisabled));
            var switches = new List<SwitchBase>(RenderingEngine.AvailableSwitches.Where(x => !x.AutoDiscoverDisabled));
            if (TradeFairMode)
            {
                switches.Clear();
                switches.Add(av.First(x => x.Name == "Start"));
                switches.Add(av.First(x => x.Name == "Branch"));
                switches.Add(av.First(x => x.Name == "Delay"));
                switches.Add(av.First(x => x.Name == "Restart"));

                switches.Add(av.First(x => x.Name == "Display"));
                switches.Add(av.First(x => x.Name == "OpcUa Connection"));
                switches.Add(av.First(x => x.Name == "Read OpcUa Variable"));
                switches.Add(av.First(x => x.Name == "Write OpcUa Variable"));

                switches.Add(av.First(x => x.Name == "Tweet.Available"));
                switches.Add(av.First(x => x.Name == "Tweet.Display"));

                switches.Add(av.First(x => x.Name == "Or"));
                switches.Add(av.First(x => x.Name == "And"));

                switches.Add(av.First(x => x.Name == "String"));
                switches.Add(av.First(x => x.Name == "Description"));

                switches.Add(av.First(x => x.Name == "Increment"));
            }

            treeView.Nodes.Clear();

            Dictionary<String, GroupBase> groups = new Dictionary<string, GroupBase>();

            foreach (var switchBase in switches)
            {
                if (!groups.ContainsKey(switchBase.Group.Name))
                {
                    groups.Add(switchBase.Group.Name, switchBase.Group);
                }
            }

            foreach (var g in groups.OrderBy(x => x.Key))
            {
                var groupNode = treeView.Nodes.Add(g.Value.Name, g.Value.Name);
                groupNode.Tag = g.Value;

                foreach (var switchBase in switches)
                {
                    if (switchBase.Group.Name == g.Value.Name)
                    {
                        var node = groupNode.Nodes.Add(switchBase.Name, switchBase.Name);
                        node.Tag = switchBase;
                    }
                }
            }

            treeView.ExpandAll();
        }

        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var treeNode = e.Item as TreeNode;
            DrawableBase item = treeNode?.Tag as SwitchBase;
            if (item != null)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            RenderingEngine.CompileAndStart();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            RenderingEngine.Stop();
        }

        private void btnDebug_CheckedChanged(object sender, EventArgs e)
        {
            RenderingEngine.DebugMode = !RenderingEngine.DebugMode;
        }

        private void tbProject_TextChanged(object sender, EventArgs e)
        {
            RenderingEngine.CurrentProject.Name = tbProject.Text;
        }
    }
}
