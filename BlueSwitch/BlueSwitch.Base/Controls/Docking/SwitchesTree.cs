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
    public partial class SwitchesTree : DockContent
    {
        public SwitchesTree()
        {
            InitializeComponent();
            treeView.AllowDrop = true;
        }

        public void UpdateTree(List<SwitchBase> switches)
        {
            Dictionary<String,GroupBase> groups = new Dictionary<string, GroupBase>();

            foreach (var switchBase in switches)
            {
                if (!groups.ContainsKey(switchBase.Group.Name))
                {
                    groups.Add(switchBase.Group.Name,switchBase.Group);
                }
            }

            foreach (var g in groups.OrderBy(x=>x.Key))
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
    }
}
