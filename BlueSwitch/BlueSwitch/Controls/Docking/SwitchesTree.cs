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
            //tbProject.Text = RenderingEngine.CurrentProject.Name;
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
            //var av = new List<SwitchBase>(RenderingEngine.AvailableSwitches.Where(x=>!x.AutoDiscoverDisabled));

            //if (TradeFairMode)
            //{
            //    switches.Clear();
            //    switches.Add(av.First(x => x.UniqueName == "Start"));
            //    switches.Add(av.First(x => x.UniqueName == "Branch"));
            //    switches.Add(av.First(x => x.UniqueName == "Delay"));
            //    switches.Add(av.First(x => x.UniqueName == "Restart"));

            //    switches.Add(av.First(x => x.UniqueName == "Display"));
            //    switches.Add(av.First(x => x.UniqueName == "OpcUa Connection"));
            //    switches.Add(av.First(x => x.UniqueName == "Read OpcUa Variable"));
            //    switches.Add(av.First(x => x.UniqueName == "Write OpcUa Variable"));

            //    switches.Add(av.First(x => x.UniqueName == "Tweet.Available"));
            //    switches.Add(av.First(x => x.UniqueName == "Tweet.Display"));

            //    switches.Add(av.First(x => x.UniqueName == "Or"));
            //    switches.Add(av.First(x => x.UniqueName == "And"));

            //    switches.Add(av.First(x => x.UniqueName == "String"));
            //    switches.Add(av.First(x => x.UniqueName == "Description"));

            //    switches.Add(av.First(x => x.UniqueName == "Increment"));
            //}
            
            var items = new List<SwitchBase>(RenderingEngine.AvailableSwitches.Where(x => !x.AutoDiscoverDisabled));

            var query = tbSearch.Text;

            if (!String.IsNullOrEmpty(query))
            {
                var searchResult = RenderingEngine.SearchService.Search(tbSearch.Text);

                items.Clear();

                if (searchResult.Count > 0)
                {
                    items.AddRange(searchResult.Values.OrderByDescending(x => x.Relevance).Select(x => x.Item));
                }
            }

            treeView.BeginUpdate();
            treeView.Nodes.Clear();

            Dictionary<String, GroupBase> groups = new Dictionary<string, GroupBase>();

            foreach (var switchBase in items)
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

                foreach (var switchBase in items)
                {
                    if (switchBase.Group.Name == g.Value.Name)
                    {
                        var node = groupNode.Nodes.Add(switchBase.UniqueName, switchBase.UniqueName);
                        node.Tag = switchBase;
                    }
                }
            }

            treeView.ExpandAll();
            treeView.EndUpdate();
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
            //RenderingEngine.CurrentProject.Name = tbProject.Text;
        }

        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                tbSearch.Text = "";
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            UpdateTree();
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
            }
        }
    }
}
