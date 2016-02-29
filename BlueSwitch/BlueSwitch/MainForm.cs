using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.IO;
using BlueSwitch.Base.Processing;
using BlueSwitch.Base.Services;
using BlueSwitch.Base.Trigger.Types;
using BlueSwitch.Controls.Docking;
using LabelPrinter;
using Newtonsoft.Json;
using NLog;
using WeifenLuo.WinFormsUI.Docking;
using Zicore.Settings.Json;

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
            //project.Add(new Input { Position = new PointF(100, 150) });
            //project.Add(new DateTimeCalculatorSwitch { Position = new PointF(200, 200) });
            //project.Add(new Input { Position = new PointF(600, 300) });
            //project.Add(new Input2 { Position = new PointF(800, 300) });
            //project.Add(new EqualsSwitch { Position = new PointF(800, 200) });
            //project.Add(new AndSwitch { Position = new PointF(400, 300) });
            //project.Add(new DisplaySwitch { Position = new PointF(400, 400) });
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
        private MetaEditor _metaEditor;

        private PropertiesEditor _properties;

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
            
            _metaEditor = new MetaEditor(Renderer.RenderingEngine);
            _switchesTree = new SwitchesTree(Renderer.RenderingEngine);
            _errorList = new ErrorList(Renderer.RenderingEngine);
            _triggerExample = new TriggerExample(Renderer.RenderingEngine);
            //_properties = new PropertiesEditor(Renderer.RenderingEngine);
            _variableEditor = new VariableEditor(Renderer.RenderingEngine);
            
            _switchesTree.HideOnClose = true;
            _switchesTree.Show(dockPanel, DockState.DockLeft);

            _metaEditor.HideOnClose = true;

            _triggerExample.HideOnClose = true;
            _triggerExample.Show(dockPanel, DockState.DockRight);

            //_properties.HideOnClose = true;
            
            _variableEditor.HideOnClose = true;
            _variableEditor.Show(_switchesTree.Pane, DockAlignment.Bottom, 0.4);

            _errorList.HideOnClose = true;
            _errorList.Show(Renderer.DockPanel, DockState.DockBottom);

            dockPanel.DockLeftPortion = 220;
            dockPanel.DockRightPortion = 220;
            
            Renderer.InitializeEngine();

            _switchesTree.UpdateTree();
            _metaEditor.UpdateTree();
            dockPanel.ResumeLayout(true,true);

            var userprofile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            LoadFile(Path.Combine(userprofile,"scenario.bs.json"));
        }

        private ProcessorCompiler compiler;

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            Renderer.RenderingEngine.CurrentProject.Save(saveFileDialog.FileName);
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
                    Renderer.RenderingEngine.CurrentProject.Save(Renderer.RenderingEngine.CurrentProject.FilePath);
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

        private void iOEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _properties.Show(dockPanel, DockState.DockRight);
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
                
                Renderer.AddComponent(sw, new PointF(x, y));
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
    }
}
