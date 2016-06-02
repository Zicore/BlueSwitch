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
using BlueSwitch.Controls.Helper;
using Newtonsoft.Json;
using WeifenLuo.WinFormsUI.Docking;

namespace BlueSwitch.Controls.Docking
{
    public partial class HelpEditor : DockContent
    {

        [JsonIgnore]
        public RenderingEngine RenderingEngine { get; set; }

        public HelpEditor(RenderingEngine renderingEngine)
        {
            RenderingEngine = renderingEngine;
            InitializeComponent();
            treeView.AllowDrop = true;

            _editors = new Control[] {
                textBoxEditor,// for all columns
                textBoxEditor,
                textBoxEditor
            };
        }

        private readonly Control[] _editors;

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
            if (e.SubItem >= 0 || e.SubItem <= 2)
            {
                listInputs.StartEditing(_editors[e.SubItem], e.Item, e.SubItem);
            }
        }

        private void listMetaData_SubItemEndEditing(object sender, SubItemEndEditingEventArgs e)
        {
            var tag = e.Item.Tag as HelpDescriptionEntry;
            if (tag != null)
            {
                if (e.SubItem == 0)
                {
                    tag.Title = e.DisplayText;
                }
                else if (e.SubItem == 1)
                {
                    tag.Description = e.DisplayText;
                }
                else if (e.SubItem == 2)
                {
                    try
                    {
                        tag.Index = int.Parse(e.DisplayText);
                    }
                    catch
                    {
                        tag.Index = 0;
                        e.DisplayText = "0";
                    }
                }
            }
        }

        private void listViewOutputs_SubItemEndEditing(object sender, SubItemEndEditingEventArgs e)
        {
            var tag = e.Item.Tag as HelpDescriptionEntry;
            if (tag != null)
            {
                if (e.SubItem == 0)
                {
                    tag.Title = e.DisplayText;
                }
                else if (e.SubItem == 1)
                {
                    tag.Description = e.DisplayText;
                }
                else if (e.SubItem == 2)
                {
                    try
                    {
                        tag.Index = int.Parse(e.DisplayText);
                    }
                    catch
                    {
                        tag.Index = 0;
                        e.DisplayText = "0";
                    }
                }
            }
        }

        private void AddInputEntry(HelpDescriptionEntry s, int index, bool addNew = false)
        {
            var help = UseOrCreateHelp();

            if (addNew)
            {
                if (help.Inputs.ContainsKey(index))
                {
                    index = help.Inputs.Keys.Max(x => x) + 1;
                }
                s.Title = "Input " + (index + 1);
            }

            listInputs.Items.Add(new ListViewItem(new[]
            {
                new ListViewItem.ListViewSubItem { Text = s.Title},
                new ListViewItem.ListViewSubItem { Text = s.Description},
                new ListViewItem.ListViewSubItem { Text = index.ToString() },
            }, 0)
            { Tag = s });
            if (addNew)
            {
                help.Inputs.Add(index, s);
            }
            s.Index = index;
        }

        private void AddOutputEntry(HelpDescriptionEntry s, int index, bool addNew = false)
        {
            var help = UseOrCreateHelp();

            if (addNew)
            {
                if (help.Outputs.ContainsKey(index))
                {
                    index = help.Outputs.Keys.Max(x => x) + 1;
                }
                s.Title = "Output " + (index + 1);
            }

            listOutputs.Items.Add(new ListViewItem(new[]
                {
                    new ListViewItem.ListViewSubItem {Text = s.Title},
                    new ListViewItem.ListViewSubItem {Text = s.Description},
                    new ListViewItem.ListViewSubItem {Text = index.ToString()},
                }, 0)
            { Tag = s });
            if (addNew)
            {
                help.Outputs.Add(index, s);
            }
            s.Index = index;
        }

        private HelpDescription UseOrCreateHelp()
        {
            HelpDescription help;
            if (RenderingEngine.HelpService.Items.ContainsKey(SelectedSwitch.UniqueName))
            {
                help = RenderingEngine.HelpService.Items[SelectedSwitch.UniqueName];
            }
            else
            {
                help = new HelpDescription { MainEntry = new HelpDescriptionEntry { Title = "Empty" } };
                RenderingEngine.HelpService.Items[SelectedSwitch.UniqueName] = help;
            }
            return help;
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
            listInputs.Items.Clear();
            listOutputs.Items.Clear();
            if (SelectedSwitch != null)
            {
                if (RenderingEngine.HelpService.Items.ContainsKey(SelectedSwitch.UniqueName))
                {
                    var help = UseOrCreateHelp();

                    tbTitle.Text = help.MainEntry.Title;

                    var items = RenderingEngine.HelpService.Items[SelectedSwitch.UniqueName];

                    foreach (var s in items.Inputs.OrderBy(x=>x.Key))
                    {
                        AddInputEntry(s.Value, s.Key);
                    }
                    foreach (var s in items.Outputs.OrderBy(x => x.Key))
                    {
                        AddOutputEntry(s.Value, s.Key);
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

            RenderingEngine.HelpService.ExportDefaultHelpDescription();
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            if (SelectedSwitch != null)
            {
                var entry = new HelpDescriptionEntry() { Title = "", Description = "" };
                AddInputEntry(entry, 0, true);
            }
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            if (SelectedSwitch != null)
            {
                if (RenderingEngine.HelpService.Items.ContainsKey(SelectedSwitch.UniqueName))
                {
                    var help = RenderingEngine.HelpService.Items[SelectedSwitch.UniqueName];

                    foreach (ListViewItem item in listInputs.SelectedItems)
                    {
                        var entry = item?.Tag as HelpDescriptionEntry;
                        if (entry != null)
                        {
                            help.Inputs.Remove(entry.Index);
                        }
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

        private void btAddOutput_Click(object sender, EventArgs e)
        {
            if (SelectedSwitch != null)
            {
                var entry = new HelpDescriptionEntry() { Title = "", Description = "" };
                AddOutputEntry(entry, 0, true);
            }
        }

        private void btRemoveOutput_Click(object sender, EventArgs e)
        {
            if (SelectedSwitch != null)
            {
                if (RenderingEngine.HelpService.Items.ContainsKey(SelectedSwitch.UniqueName))
                {
                    var help = RenderingEngine.HelpService.Items[SelectedSwitch.UniqueName];

                    foreach (ListViewItem item in listOutputs.SelectedItems)
                    {
                        var entry = item?.Tag as HelpDescriptionEntry;
                        if (entry != null)
                        {
                            help.Outputs.Remove(entry.Index);
                        }
                    }
                    UpdateList();
                }
            }
        }

        private void tbTitle_TextChanged(object sender, EventArgs e)
        {
            var help = UseOrCreateHelp();
            help.MainEntry.Title = tbTitle.Text;
        }

        private void listOutputs_SubItemClicked(object sender, SubItemEventArgs e)
        {
            if (e.SubItem >= 0 || e.SubItem <= 2)
            {
                listOutputs.StartEditing(_editors[e.SubItem], e.Item, e.SubItem);
            }
        }

        private void textBoxEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }
    }
}
