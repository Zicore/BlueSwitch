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
using BlueSwitch.Base.Diagnostics;
using WeifenLuo.WinFormsUI.Docking;

namespace BlueSwitch.Controls.Docking
{
    public partial class ErrorList : DockContent
    {
        public ErrorList(Engine engine)
        {
            InitializeComponent();
            Engine = engine;
            Engine.ProcessorCompiler.ErrorAdded +=ProcessorCompilerOnErrorAdded;
            Engine.ProcessorCompiler.ErrorCleared += ProcessorCompilerOnErrorCleared;
        }

        private void ProcessorCompilerOnErrorCleared(object sender, EventArgs eventArgs)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    listErrors.Items.Clear();
                }));
            }
            else
            {
                listErrors.Items.Clear();
            }
        }

        private void ProcessorCompilerOnErrorAdded(object sender, ExceptionEntryEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    AddEntry(e.ExceptionEntry);
                }));
            }
            else
            {
                AddEntry(e.ExceptionEntry);
            }
        }

        public Engine Engine { get; set; }

        private void AddEntry(ExceptionEntry entry)
        {
            listErrors.Items.Add(new ListViewItem(new []
            {
                new ListViewItem.ListViewSubItem { Text = listErrors.Items.Count.ToString() + 1 }, // LFD
                new ListViewItem.ListViewSubItem { Text = entry.Step.ToString() }, // Step
                new ListViewItem.ListViewSubItem { Text = entry.Node.Value.Name }, // Name
                new ListViewItem.ListViewSubItem { Text = entry.Node.Value.Description }, // Node
                new ListViewItem.ListViewSubItem { Text = entry.Exception.GetType().Name }, // Exception
                new ListViewItem.ListViewSubItem { Text = entry.Exception.ToString() }, // Exception
            },0));
        }
    }
}
