using System;
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
        private Logger Log = LogManager.GetCurrentClassLogger();


        public MainForm(string[] args)
        {
            InitializeComponent();

           

            InitializeDockingControls();
            //Log.Warn(args.Length);
            //var userprofile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            Renderer.RenderingEngine.NewProject();

            if (args.Length >= 1)
            {
                String filePath = args[0];
                Log.Warn(filePath);
                if (File.Exists(filePath))
                {
                    LoadFile(filePath);
                }
            }
            var project = Renderer.RenderingEngine.CurrentProject;
            compiler = Renderer.RenderingEngine.ProcessorCompiler;

            Renderer.RenderingEngine.ProcessorCompiler.CompileStart += ProcessorCompilerOnCompileStart;
            Renderer.RenderingEngine.ProcessorCompiler.CompileFinished += ProcessorCompilerOnCompileFinished;

            Renderer.RenderingEngine.ProcessorCompiler.Started += ProcessorCompilerOnStarted;
            Renderer.RenderingEngine.ProcessorCompiler.Finished += ProcessorCompilerOnFinished;

            Renderer.RenderingEngine.DebugModeChanged += RenderingEngineOnDebugModeChanged;
            Renderer.RenderingEngine.SelectionService.ContextAction += SelectionServiceOnContextAction;

        }

        private void RenderingEngineOnProjectLoaded(object sender, EventArgs eventArgs)
        {
            UpdateTitle();
            _switchesTree.UpdateTree();
        }

        private void UpdateTitle()
        {
            var project = Renderer.RenderingEngine.CurrentProject;
            if (!string.IsNullOrEmpty(project.Name))
            {
                if (!string.IsNullOrEmpty(project.FilePath))
                {
                    Text = $"Blue Switch Designer - {project.Name} - {project.FilePath}";
                }
                else
                {
                    Text = $"Blue Switch Designer - {project.Name}";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(project.FilePath))
                {
                    Text = $"Blue Switch Designer - {project.FilePath}";
                }
                else
                {
                    Text = $"Blue Switch Designer - Unnamed Project";
                }
            }
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

        private ProcessorCompiler compiler;

        private RendererBase Renderer;
        private SwitchesTree _switchesTree;
        private SearchEditor _metaEditor;
        private HelpEditor _helpEditor;
        
        private ErrorList _errorList;

        private VariableEditor _variableEditor;

        private TriggerExample _triggerExample;

        private DeserializeDockContent _deserializeDockContent;

        private void InitializeDockingControls()
        {
            dockPanel.SuspendLayout();


            dockPanel.Theme = new VS2015LightTheme();
            dockPanel.BackColor = SystemColors.Control;
            dockPanel.DockBackColor = SystemColors.Control;

            this.Renderer = new RendererBase();
            Renderer.BackColor = SystemColors.ControlDark;
            
            Renderer.AllowDrop = true;
            Renderer.HideOnClose = true;
            
            _metaEditor = new SearchEditor(Renderer.RenderingEngine);
            _helpEditor = new HelpEditor(Renderer.RenderingEngine);
            _switchesTree = new SwitchesTree(Renderer.RenderingEngine);
            _errorList = new ErrorList(Renderer.RenderingEngine);
            _triggerExample = new TriggerExample(Renderer.RenderingEngine);
            _variableEditor = new VariableEditor(Renderer.RenderingEngine);


            _switchesTree.HideOnClose = true;
            _metaEditor.HideOnClose = true;
            _helpEditor.HideOnClose = true;
            _triggerExample.HideOnClose = true;
            _variableEditor.HideOnClose = true;
            _errorList.HideOnClose = true;

            dockPanel.DockLeftPortion = 220;
            dockPanel.DockRightPortion = 220;

            LoadStateFromXml();

            Renderer.RenderingEngine.ProjectLoaded += RenderingEngineOnProjectLoaded;

            Renderer.InitializeEngine();

            _switchesTree.UpdateTree();
            _metaEditor.UpdateTree();
            _helpEditor.UpdateTree();


            dockPanel.ResumeLayout(true,true);
        }
        
        private void LoadStateFromXml()
        {

            try
            {
                dockPanel.LoadFromXml(RendererBase.DockSaveState, GetContentFromPersistString);
                Renderer.Show(dockPanel);
            }
            catch
            {
                Renderer.Show(dockPanel);
                _switchesTree.Show(dockPanel, DockState.DockLeft);
                _triggerExample.Show(dockPanel, DockState.DockRight);
                _variableEditor.Show(dockPanel, DockState.DockRight);
                _errorList.Show(Renderer.DockPanel, DockState.DockBottomAutoHide);
            }
        }

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
            UpdateTitle();
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
            UpdateTitle();
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
            UpdateTitle();
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
            Renderer.RenderingEngine.NewProject();
            UpdateTitle();
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
            UpdateTitle();
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

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(RendererBase).ToString())
                return Renderer;
            else if (persistString == typeof(HelpEditor).ToString())
                return _helpEditor;
            else if (persistString == typeof(SearchEditor).ToString())
                return _metaEditor;
            else if (persistString == typeof(ErrorList).ToString())
                return _errorList;
            else if (persistString == typeof(VariableEditor).ToString())
                return _variableEditor;
            else if (persistString == typeof(SwitchesTree).ToString())
                return _switchesTree;
            else if (persistString == typeof(TriggerExample).ToString())
                return _triggerExample;
            else
            {
                
                return null;
            }
        }

        private void CloseAllContents()
        {
            // we don't want to create another instance of tool window, set DockPanel to null
            _helpEditor.DockPanel = null;
            _metaEditor.DockPanel = null;
            _variableEditor.DockPanel = null;
            _errorList.DockPanel = null;

            //CloseAllDocuments();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Renderer.RenderingEngine.SaveSettings();
            dockPanel.SaveAsXml(RendererBase.DockSaveState);
        }

        private void errorListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _errorList.Show(dockPanel, DockState.DockBottomAutoHide);
        }

        private void rendererToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Renderer.Show(dockPanel);
        }

        private void variablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _variableEditor.Show(dockPanel, DockState.DockRight);
        }

        private void registerExtensionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Win32.FileExtensionHelper.SetAssociation(".bs", "BlueSwitchFile", Application.ExecutablePath, "BlueSwitch Script");
        }

        private void timerDrawCheck_Tick(object sender, EventArgs e)
        {
            lbPerformance.Text = $"Draw: {Renderer.DrawTime.TotalMilliseconds} FPS: {(1000.0 / Renderer.DrawTime.TotalMilliseconds):0.00}";
        }
    }
}
