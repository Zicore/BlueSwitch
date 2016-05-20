using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BlueSwitch.Base;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Services;
using Newtonsoft.Json;
using WeifenLuo.WinFormsUI.Docking;

namespace BlueSwitch.Controls.Docking
{
    public partial class ContextTree : DockContent
    {
        [JsonIgnore]
        public SwitchBase SelectedSwitch { get; set; }

        [JsonIgnore]
        public RenderingEngine RenderingEngine { get; set; }

        [JsonIgnore]
        public InputOutputSelector IO { get; set; }

        public ContextActionEventArgs ContextActionEventArgs { get; private set; }

        public event EventHandler Finished;

        public ContextTree(RenderingEngine renderingEngine, ContextActionEventArgs args)
        {
            ContextActionEventArgs = args;
            IO = args.Selector;
            RenderingEngine = renderingEngine;
            InitializeComponent();
            treeView.AllowDrop = true;
        }

        public void UpdateTree()
        {

            var items = new List<SwitchBase>(RenderingEngine.AvailableSwitches.Where(x => !x.AutoDiscoverDisabled));

            if (IO != null)
            {
                // Filter by specified IO Selector
                if (IO.IsInput)
                {
                    items = items.Where(x => x.Outputs.Any(j => j.Signature.Matches(IO.InputOutput.Signature))).ToList();
                }
                else
                {
                    items = items.Where(x => x.Inputs.Any(j => j.Signature.Matches(IO.InputOutput.Signature))).ToList();
                }
            }

            var query = tbSearch.Text;

            if (!String.IsNullOrEmpty(query))
            {
                var searchResult = RenderingEngine.SearchService.Search(tbSearch.Text, items);

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

        private void ContextTree_Load(object sender, EventArgs e)
        {
            UpdateTree();
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectedSwitch = treeView.SelectedNode?.Tag as SwitchBase;
        }

        private void treeView_DoubleClick(object sender, EventArgs e)
        {
            FinishContext();
        }

        private void treeView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FinishContext();
            }
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void FinishContext()
        {
            SelectedSwitch = treeView.SelectedNode?.Tag as SwitchBase;
            OnFinished();
            Close();
        }

        protected virtual void OnFinished()
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }

        private void ContextTree_FormClosing(object sender, FormClosingEventArgs e)
        {
            SelectedSwitch = treeView.SelectedNode?.Tag as SwitchBase;
            OnFinished();
        }
    }
}
