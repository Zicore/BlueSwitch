using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;

namespace BlueSwitch.Controls.Helper
{
    public static class TreeViewHelper
    {
        public static void UpdateTree(TreeView treeView, IEnumerable<SwitchBase> items)
        {

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
                        var node = groupNode.Nodes.Add(switchBase.UniqueName, switchBase.DisplayName);
                        node.Tag = switchBase;
                    }
                }
            }

            treeView.ExpandAll();
            treeView.EndUpdate();
        }
    }
}
