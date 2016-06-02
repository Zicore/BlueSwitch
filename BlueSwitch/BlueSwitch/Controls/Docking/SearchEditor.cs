using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.IO;
using BlueSwitch.Base.Meta.Search;
using BlueSwitch.Base.Reflection;
using BlueSwitch.Controls.Helper;
using Newtonsoft.Json;
using WeifenLuo.WinFormsUI.Docking;

namespace BlueSwitch.Controls.Docking
{
    public partial class SearchEditor : DockContent
    {

        [JsonIgnore]
        public RenderingEngine RenderingEngine { get; set; }
        
        public SearchEditor(RenderingEngine renderingEngine)
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

            TreeViewHelper.UpdateTree(treeView, items);
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


        private SwitchBase SelectedSwitch { get; set; }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectedSwitch = treeView.SelectedNode?.Tag as SwitchBase;
            if (SelectedSwitch != null)
            {
                lbSwitch.Text = $"Unique: {SelectedSwitch.UniqueName} -> Display:{SelectedSwitch.DisplayName}";
            }
            UpdateList();
        }

        private void UpdateList()
        {
            listMetaData.Items.Clear();
            if (SelectedSwitch != null)
            {
                var search = RenderingEngine.SearchService.FindSearchDescription(SelectedSwitch.UniqueName);
                if (search != null)
                {
                    foreach (var s in search.Tags)
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
            if (SelectedSwitch != null)
            {
                var search = RenderingEngine.SearchService.FindSearchDescription(SelectedSwitch.UniqueName);
                var tag = new SearchTag("Empty");
                search?.Tags.Add(tag);
                AddEntry(tag);
            }
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            if (SelectedSwitch != null)
            {
                var search = RenderingEngine.SearchService.FindSearchDescription(SelectedSwitch.UniqueName);
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

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialogTags.ShowDialog();
        }

        private void saveFileDialogTags_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (SelectedSwitch != null)
            {
                RenderingEngine.SearchService.ExportSearchDescription(saveFileDialogTags.FileName, SelectedSwitch.UniqueName);
            }
        }
    }
}
