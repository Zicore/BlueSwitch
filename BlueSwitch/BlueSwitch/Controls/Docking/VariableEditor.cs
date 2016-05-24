using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.IO;
using BlueSwitch.Base.Reflection;
using WeifenLuo.WinFormsUI.Docking;
using ValueType = BlueSwitch.Base.IO.ValueType;

namespace BlueSwitch.Controls.Docking
{
    public partial class VariableEditor : DockContent
    {
        public RenderingEngine RenderingEngine { get; set; }
        public Variable SelectedVariable { get; set;}

        public VariableEditor(RenderingEngine renderingEngine)
        {
            RenderingEngine = renderingEngine;
            RenderingEngine.ProjectLoaded += RenderingEngineOnProjectLoaded;
            RenderingEngine.BeforeLoading += RenderingEngineOnBeforeLoading;
            InitializeComponent();

            Editors = new Control[] {
                comboBoxEditor,// for column 0
                textBoxEditor,// for column 1
            };

            var items = Enum.GetNames(typeof (BlueSwitch.Base.IO.ValueType));

            comboBoxEditor.Items.AddRange(items);
            comboBoxEditor.SelectedIndexChanged += new EventHandler(control_SelectedValueChanged);
        }

        private void RenderingEngineOnBeforeLoading(object sender, EventArgs eventArgs)
        {
            listVariables.Items.Clear();
        }

        private void RenderingEngineOnProjectLoaded(object sender, EventArgs eventArgs)
        {
            foreach (var variable in RenderingEngine.CurrentProject.Variables)
            {
                AddEntry(variable.Value);
            }
        }

        private void control_SelectedValueChanged(object sender, EventArgs e)
        {
            listVariables.EndEditing(true);
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            Variable variable = new Variable();
            variable.Name = RenderingEngine.CurrentProject.GetFreeVariablename("MyVariable",0);
            variable.ValueType = ValueType.Bool;
            
            AddEntry(variable);

            RenderingEngine.CurrentProject.Variables.Add(variable.Name, variable);
        }

        private void AddEntry(Variable variable)
        {
            listVariables.Items.Add(new ListViewItem(new[]
            {
                new ListViewItem.ListViewSubItem { Text = variable.ValueType.ToString()},
                new ListViewItem.ListViewSubItem { Text = variable.Name },
                new ListViewItem.ListViewSubItem { Text = variable.Value?.ToString() },
            }, 0)
            { Tag = variable });
        }

        private Control[] Editors;

        private void listVariables_SubItemEndEditing(object sender, SubItemEndEditingEventArgs e)
        {
            var variable = e.Item.Tag as Variable;
            if (variable != null)
            {
                if (e.SubItem == 0)
                {
                    var types = Enum.GetValues(typeof (BlueSwitch.Base.IO.ValueType));
                    var valueType = (BlueSwitch.Base.IO.ValueType)types.GetValue(comboBoxEditor.SelectedIndex);
                    variable.ValueType = valueType;
                    variable.Value = TypeExtensions.GetDefault(variable.NetValueType);
                    RefreshValues();
                }
                else if (e.SubItem == 1)
                {
                    string newName = e.DisplayText;
                    if (variable.Name != newName)
                    {
                        if (!RenderingEngine.CurrentProject.RenameVariable(variable.Name, newName))
                        {
                            e.DisplayText = variable.Name;
                        }
                    }
                }
                
            }
        }

        private void listVariables_SubItemClicked(object sender, SubItemEventArgs e)
        {
            if (e.SubItem == 0 || e.SubItem == 1)
            {
                listVariables.StartEditing(Editors[e.SubItem], e.Item, e.SubItem);
            }
        }
        
        private void listVariables_SubItemBeginEditing(object sender, SubItemEventArgs e)
        {
           
        }

        private void listVariables_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var listViewItem = e.Item as ListViewItem;
            Variable item = listViewItem?.Tag as Variable;
            if (item != null)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            var selectedItems = listVariables.SelectedItems;
            foreach (ListViewItem selectedItem in selectedItems)
            {
                var variable = selectedItem.Tag as Variable;
                if (variable != null)
                {
                    RenderingEngine.CurrentProject.Variables.Remove(variable.Name);
                    listVariables.Items.Remove(selectedItem);
                }
            }
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            RefreshValues();
        }

        private void RefreshValues()
        {
            var items = listVariables.Items;
            foreach (ListViewItem item in items)
            {
                var variable = item.Tag as Variable;
                if (variable != null)
                {
                    item.SubItems[2].Text = variable.Value?.ToString();
                }
            }
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            SelectedVariable = null;
            var selectedItems = listVariables.SelectedItems;
            bool cancel = true;
            if (listVariables.SelectedItems.Count == 1)
            {
                foreach (ListViewItem selectedItem in selectedItems)
                {
                    var variable = selectedItem.Tag as Variable;
                    SelectedVariable = variable;
                    if (SelectedVariable != null)
                    {
                        if (SelectedVariable.NetValueType == typeof (string))
                        {
                            cancel = false;
                            break;
                        }
                    }
                }
            }
            e.Cancel = cancel;
        }

        private void pickFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedVariable != null)
            {
                if (SelectedVariable.NetValueType == typeof (string))
                {
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        SelectedVariable.Value = saveFileDialog.FileName;
                        RefreshValues();
                    }
                }
            }
        }

        private void pickFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedVariable != null)
            {
                if (SelectedVariable.NetValueType == typeof(string))
                {
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        SelectedVariable.Value = folderBrowserDialog.SelectedPath;
                        RefreshValues();
                    }
                }
            }
        }
    }
}
