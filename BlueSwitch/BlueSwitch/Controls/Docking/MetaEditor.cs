using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.IO;
using BlueSwitch.Base.Meta.Search;
using BlueSwitch.Base.Reflection;
using Newtonsoft.Json;
using WeifenLuo.WinFormsUI.Docking;

namespace BlueSwitch.Controls.Docking
{
    public partial class MetaEditor : DockContent
    {

        [JsonIgnore]
        public RenderingEngine RenderingEngine { get; set; }
        
        public MetaEditor(RenderingEngine renderingEngine)
        {
            RenderingEngine = renderingEngine;
            InitializeComponent();
            treeView.AllowDrop = true;

            Editors = new Control[] {
                textBoxEditor,// for all columns
            };
        }

        private Control[] Editors;

        public void UpdateTree()
        {
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
        private void listMetaData_SubItemClicked(object sender, SubItemEventArgs e)
        {
            if (e.SubItem >= 0 || e.SubItem <= 1)
            {
                listMetaData.StartEditing(Editors[0], e.Item, e.SubItem);
            }
        }

        private void listMetaData_SubItemEndEditing(object sender, SubItemEndEditingEventArgs e)
        {
            var tag = e.Item.Tag as SearchTag;
            if (tag != null)
            {
                if (e.SubItem == 0)
                {
                    tag.Tag = e.DisplayText;
                }
                else if (e.SubItem == 1)
                {
                    tag.Description = e.DisplayText;
                }
            }
        }

        private void AddEntry(SearchTag s)
        {
            listMetaData.Items.Add(new ListViewItem(new[]
            {
                new ListViewItem.ListViewSubItem { Text = s.Tag},
                new ListViewItem.ListViewSubItem { Text = s.Description},
            }, 0)
            { Tag = s });
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateList();
        }

        private void UpdateList()
        {
            listMetaData.Items.Clear();
            var sw = treeView.SelectedNode?.Tag as SwitchBase;
            if (sw != null)
            {
                var search = RenderingEngine.SearchService.FindSearchDescription(sw.UniqueName);
                if (search != null)
                {
                    foreach (var s in search)
                    {
                        AddEntry(s);
                    }
                }
            }
        }

        private void MetaEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Visible = false;
            }
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            var sw = treeView.SelectedNode?.Tag as SwitchBase;
            if (sw != null)
            {
                var search = RenderingEngine.SearchService.FindSearchDescription(sw.UniqueName);
                var tag = new SearchTag("Empty");
                search?.Tags.Add(tag);
                AddEntry(tag);
            }
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            var sw = treeView.SelectedNode?.Tag as SwitchBase;
            if (sw != null)
            {
                var search = RenderingEngine.SearchService.FindSearchDescription(sw.UniqueName);
                if (search != null)
                {
                    foreach (ListViewItem item in listMetaData.SelectedItems)
                    {
                        var searchTag = item?.Tag as SearchTag;
                        search.Tags.Remove(searchTag);
                    }
                    UpdateList();
                }
            }
        }
    }
}
