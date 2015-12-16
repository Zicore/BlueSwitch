using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.IO;
using BlueSwitch.Base.Processing;
using BlueSwitch.Base.Trigger;
using BlueSwitch.Base.Trigger.Types;
using Newtonsoft.Json;
using NLog;
using Zicore.Settings.Json;

namespace BlueSwitch.Base.Components.Base
{
    public abstract class Engine
    {
        public event EventHandler DebugModeChanged;

        public BlueSwitchProject CurrentProject { get; set; } = new BlueSwitchProject();
        public ReflectionService ReflectionService { get; set; } = new ReflectionService();
        public EventManager EventManager { get; }
        public ProcessorCompiler ProcessorCompiler { get; }

        protected Logger _log = LogManager.GetCurrentClassLogger();

        protected bool _debugMode = true;

        public Dictionary<string, object> DebugValues = new Dictionary<string, object>();

        public List<SwitchBase> AvailableSwitches;

        [JsonIgnore]
        public bool DebugMode
        {
            get { return _debugMode; }
            set
            {
                if (value != _debugMode)
                {
                    _debugMode = value;
                    OnDebugModeChanged();
                }
            }
        }
        public event EventHandler ProjectLoaded;

        public Engine()
        {
            ProcessorCompiler = new ProcessorCompiler(this);
            EventManager = new EventManager(this);
        }

        public void LoadAddons()
        {
            AvailableSwitches = ReflectionService.LoadAddons(this);
        }

        [JsonIgnore]
        public TimeSpan DebugTime { get; set; } = new TimeSpan(0, 0, 0, 0, 300);

        protected virtual void OnDebugModeChanged()
        {
            DebugModeChanged?.Invoke(this, EventArgs.Empty);
        }

        public virtual event EventHandler Redraw;

        public void RequestRedraw()
        {
            Redraw?.Invoke(this, EventArgs.Empty);
        }

        public virtual event EventHandler DebugValueUpdated;

        public void UpdateValue(String key, object value)
        {
            DebugValues[key] = value;
            OnDebugValueUpdated();
        }

        [JsonIgnore]
        public bool DesignMode { get; set; } = true;

        protected void ProcessorCompilerOnFinished(object sender, EventArgs eventArgs)
        {
            if (ProcessorCompiler.Items.Count(x => x.IsActive) == 0)
            {
                DesignMode = true;
            }
        }

        protected void ProcessorCompilerOnCompileStart(object sender, EventArgs eventArgs)
        {
            DesignMode = false;
        }

        public void LoadProject(String filePath)
        {
            CurrentProject = new BlueSwitchProject();
            try
            {
                CurrentProject = JsonSerializable.Load<BlueSwitchProject>(filePath);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            CurrentProject.Initialize(this);
            OnProjectLoaded();
        }

        public void Stop()
        {
            ProcessorCompiler.Stop(CurrentProject);
        }

        public void Compile()
        {
            ProcessorCompiler.Compile(CurrentProject);
        }

        public void Start()
        {
            EventManager.Run(EventTypeBase.Start);
        }

        public void CompileAndStart()
        {
            Stop();
            Compile();
            Start();
        }

        protected virtual void OnProjectLoaded()
        {
            ProjectLoaded?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnDebugValueUpdated()
        {
            DebugValueUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
