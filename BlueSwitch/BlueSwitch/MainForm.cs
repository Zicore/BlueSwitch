﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlueSwitch.Base;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Event;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.IO;
using BlueSwitch.Base.Processing;
using BlueSwitch.Base.Services;
using BlueSwitch.Base.Trigger.Types;
using BlueSwitch.Controls.Docking;
using BlueSwitch.Runtime;
using LabelPrinter;
using Newtonsoft.Json;
using NLog;
using WeifenLuo.WinFormsUI.Docking;

namespace BlueSwitch
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeDockingControls();

            var project = Renderer.RenderingEngine.CurrentProject;
            compiler = Renderer.RenderingEngine.ProcessorCompiler;

            Renderer.RenderingEngine.ProcessorCompiler.CompileStart += ProcessorCompilerOnCompileStart;
            Renderer.RenderingEngine.ProcessorCompiler.CompileFinished += ProcessorCompilerOnCompileFinished;

            Renderer.RenderingEngine.ProcessorCompiler.Started += ProcessorCompilerOnStarted;
            Renderer.RenderingEngine.ProcessorCompiler.Finished += ProcessorCompilerOnFinished;

            Renderer.RenderingEngine.DebugModeChanged += RenderingEngineOnDebugModeChanged;
            Renderer.RenderingEngine.SelectionService.ContextAction += SelectionServiceOnContextAction;
        }

        private void SelectionServiceOnContextAction(object sender, ContextActionEventArgs e)
        {
            if (!Renderer.RenderingEngine.SelectionService.ActionActive)
            {
                ContextTree contextTree = new ContextTree(Renderer.RenderingEngine, e)
                {
                    StartPosition = FormStartPosition.Manual,
                    Location = Cursor.Position
                };
                contextTree.Finished -= ContextTreeOnFinished;
                contextTree.Finished += ContextTreeOnFinished;
                contextTree.Show();
            }
        }

        private void ContextTreeOnFinished(object sender, EventArgs eventArgs)
        {
            ContextTree contextTree = sender as ContextTree;
            bool canceled = contextTree?.SelectedSwitch == null;

            if (!canceled)
            {
                var p = contextTree.ContextActionEventArgs.Location;
                var sw = Renderer.RenderingEngine.AddComponent(contextTree.SelectedSwitch,Renderer.RenderingEngine.TranslatePoint(p));
                Renderer.RenderingEngine.SelectionService.FinishContextAction(false, sw);
            }
            else
            {
                Renderer.RenderingEngine.SelectionService.FinishContextAction(true, null);
            }
        }

        private void RenderingEngineOnDebugModeChanged(object sender, EventArgs eventArgs)
        {
            debugModeToolStripMenuItem.Checked = Renderer.RenderingEngine.DebugMode;
        }

        private void ProcessorCompilerOnFinished(object sender, EventArgs eventArgs)
        {
            labelBuildProgress.Text = "Ready";
        }

        private void ProcessorCompilerOnStarted(object sender, EventArgs eventArgs)
        {
            labelBuildProgress.Text = "Started...";
        }

        private void ProcessorCompilerOnCompileFinished(object sender, EventArgs eventArgs)
        {
            labelBuildProgress.Text = "Finished";
        }

        private void ProcessorCompilerOnCompileStart(object sender, EventArgs eventArgs)
        {
            labelBuildProgress.Text = "Building...";
        }

        private Logger _log = LogManager.GetCurrentClassLogger();

        private RendererBase Renderer;
        private SwitchesTree _switchesTree;
        private SearchEditor _metaEditor;
        private HelpEditor _helpEditor;
        
        private ErrorList _errorList;

        private VariableEditor _variableEditor;

        private TriggerExample _triggerExample;

        private void InitializeDockingControls()
        {
            dockPanel.SuspendLayout();
            dockPanel.BackColor = SystemColors.Control;
            dockPanel.DockBackColor = SystemColors.Control;

            this.Renderer = new RendererBase();
            Renderer.BackColor = SystemColors.ControlDark;
            Renderer.Show(dockPanel);
            Renderer.AllowDrop = true;
            Renderer.HideOnClose = true;
            
            _metaEditor = new SearchEditor(Renderer.RenderingEngine);
            _helpEditor = new HelpEditor(Renderer.RenderingEngine);
            _switchesTree = new SwitchesTree(Renderer.RenderingEngine);
            _errorList = new ErrorList(Renderer.RenderingEngine);
            _triggerExample = new TriggerExample(Renderer.RenderingEngine);
            _variableEditor = new VariableEditor(Renderer.RenderingEngine);


            _switchesTree.HideOnClose = true;
            _switchesTree.Show(dockPanel, DockState.DockLeft);

            _metaEditor.HideOnClose = true;
            _helpEditor.HideOnClose = true;

            _triggerExample.HideOnClose = true;
            _triggerExample.Show(dockPanel, DockState.DockRight);
            
            _variableEditor.HideOnClose = true;
            _variableEditor.Show(dockPanel, DockState.DockRight);

            _errorList.HideOnClose = true;
            _errorList.Show(Renderer.DockPanel, DockState.DockBottomAutoHide);

            dockPanel.DockLeftPortion = 220;
            dockPanel.DockRightPortion = 220;
            
            Renderer.InitializeEngine();

            _switchesTree.UpdateTree();
            _metaEditor.UpdateTree();
            _helpEditor.UpdateTree();
            dockPanel.ResumeLayout(true,true);

            var userprofile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            LoadFile(Path.Combine(userprofile,"scenario.bs.json"));
        }

        private ProcessorCompiler compiler;

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            Renderer.RenderingEngine.Save(saveFileDialog.FileName);
        }

        private void LoadFile(String filePath)
        {
            Renderer.RenderingEngine.LoadProject(filePath);
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            LoadFile(openFileDialog.FileName);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(Renderer.RenderingEngine.CurrentProject.FilePath))
            {
                try
                {
                    Renderer.RenderingEngine.Save(Renderer.RenderingEngine.CurrentProject.FilePath);
                }
                catch
                {
                    SaveAs();
                }
            }
            else
            {
                SaveAs();
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }
        
        private void toolStripCompile_Click(object sender, EventArgs e)
        {
            compiler.Compile(Renderer.RenderingEngine.CurrentProject);
        }
        
        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            Renderer.RenderingEngine.Stop();
        }

        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            Renderer.RenderingEngine.NewProject();
        }
       

        private void componentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _switchesTree.Show(dockPanel, DockState.DockLeft);
        }
        
        private void rendererToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Renderer.Show(dockPanel, DockState.Document);
        }

        private void debugModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Renderer.RenderingEngine.DebugMode = !Renderer.RenderingEngine.DebugMode;
        }

        private void toolStripRun_Click(object sender, EventArgs e)
        {
            Renderer.RenderingEngine.CompileAndStart();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Renderer.RenderingEngine.CompileAndStart();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Renderer.RenderingEngine.Stop();
        }

        private void buildProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Renderer.RenderingEngine.Compile();
        }

        private void SaveAs()
        {
            saveFileDialog.ShowDialog();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (consoleToolStripMenuItem.Checked)
            {
                ConsoleUtils.ShowConsole();
            }
            else
            {
                ConsoleUtils.HideConsole();
            }
        }

        private void tradeFairModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _switchesTree.TradeFairMode = tradeFairModeToolStripMenuItem.Checked;
        }

        private void performanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sw = new ReplicatorSwitch();
            Random rnd = new Random();
            for (int i = 0; i < 125; i++)
            {
                var x = rnd.Next(2, Renderer.ClientRectangle.Width);
                var y = rnd.Next(2, Renderer.ClientRectangle.Height);

                Renderer.RenderingEngine.AddComponent(sw, new PointF(x, y));
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

        private void exportSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialogTags.ShowDialog();
        }

        private void metaEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _metaEditor.HideOnClose = true;
            _metaEditor.Show();
        }

        private void saveFileDialogTags_FileOk(object sender, CancelEventArgs e)
        {
            Renderer.RenderingEngine.SearchService.ExportSearchDescription(saveFileDialogTags.FileName);
        }

        private void importTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialogTags.ShowDialog();
        }

        private void openFileDialogTags_FileOk(object sender, CancelEventArgs e)
        {

            Renderer.RenderingEngine.SearchService.ImportSearchDescription(openFileDialogTags.FileName);
        }

        private void helpEnabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Renderer.RenderingEngine.HelpService.Enabled = helpEnabledToolStripMenuItem.Checked;
        }

        private void helpEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _helpEditor.HideOnClose = true;
            _helpEditor.Show();
        }

        private void saveFileDialogHelp_FileOk(object sender, CancelEventArgs e)
        {
            Renderer.RenderingEngine.HelpService.ExportHelpDescription(saveFileDialogHelp.FileName);
        }

        private void openFileDialogHelp_FileOk(object sender, CancelEventArgs e)
        {
            Renderer.RenderingEngine.HelpService.ImportHelpDescription(openFileDialogHelp.FileName);
        }

        private void exportHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialogHelp.ShowDialog();
        }

        private void importHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialogHelp.ShowDialog();
        }

        private void startToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Renderer.RenderingEngine.StartInRuntime();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectProperties p = new ProjectProperties(Renderer.RenderingEngine);
            p.ShowDialog();
        }

        private void toolStripButtonLoad_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }

        private void highQualityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            highPerformanceToolStripMenuItem.Checked = false;

            Renderer.RenderingEngine.Settings.PerformanceMode = PerformanceMode.HighQuality;
            Renderer.RenderingEngine.RequestRedraw();
        }

        private void highPerformanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            highQualityToolStripMenuItem.Checked = false;

            Renderer.RenderingEngine.Settings.PerformanceMode = PerformanceMode.HighPerformance;

            Renderer.RenderingEngine.RequestRedraw();
        }

        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Renderer.RenderingEngine.Settings.DrawGrid = gridToolStripMenuItem.Checked;

            Renderer.RenderingEngine.RequestRedraw();
        }
    }
}
