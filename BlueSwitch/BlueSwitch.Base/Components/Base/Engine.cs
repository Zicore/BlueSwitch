using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Diagnostics;
using BlueSwitch.Base.IO;
using BlueSwitch.Base.Processing;
using BlueSwitch.Base.Services;
using BlueSwitch.Base.Trigger;
using BlueSwitch.Base.Trigger.Types;
using Newtonsoft.Json;
using NLog;

namespace BlueSwitch.Base.Components.Base
{
    public abstract class Engine
    {
        public string GetRuntimePath()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(path, "BlueSwitch.Runtime.exe");
        }

        public void StartInRuntime()
        {
            var processPath = GetRuntimePath();
            try
            {
                Process.Start(processPath, CurrentProject.FilePath);
            }
            catch(Exception ex)
            {
                _log.Warn(ex);
            }
        }

        public event EventHandler DebugModeChanged;

        public static NamespaceResolverService StaticNamespaceResolver { get; private set; }
        public NamespaceResolverService NamespaceResolver { get; }
        public HelpService HelpService { get; }
        public SearchService SearchService { get; }
        public BlueSwitchProject CurrentProject { get; set; } = new BlueSwitchProject();
        public ReflectionService ReflectionService { get; } = new ReflectionService();
        public EventManager EventManager { get; }
        public ProcessorCompiler ProcessorCompiler { get; }

        public bool Running
        {
            get { return ProcessorCompiler.Items.Count(x => x.IsActive) > 0; }
        }

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

        public event EventHandler BeforeLoading;
        public event EventHandler ProjectLoaded;

        protected Engine()
        {
            ProcessorCompiler = new ProcessorCompiler(this);
            EventManager = new EventManager(this);
            SearchService = new SearchService(this);
            HelpService = new HelpService(this);
            NamespaceResolver = new NamespaceResolverService(this);
            StaticNamespaceResolver = NamespaceResolver; // We need one static instance for resolving at Deserialization Time
        }


        public SwitchBase AddComponent(SwitchBase sourceComponent, PointF pt)
        {
            SwitchBase switchBase = Activator.CreateInstance(sourceComponent.GetType()) as SwitchBase;

            if (sourceComponent is VariableSwitch && switchBase is VariableSwitch)
            {
                var variableComponent = switchBase as VariableSwitch;
                variableComponent.VariableKey = ((VariableSwitch)sourceComponent).VariableKey;
            }

            if (switchBase != null)
            {
                switchBase.Position = pt;
                CurrentProject.Add(this, switchBase);
                return switchBase;
            }
            return null;
        }

        public SwitchBase FindSwitch(String key)
        {
            if (AvailableSwitchesDict.ContainsKey(key))
            {
                return AvailableSwitchesDict[key];
            }
            return null;
        }

        public void LoadAddons()
        {
            AvailableSwitches = ReflectionService.LoadAddons(this);

            //AvailableSwitchesForSearch = new List<SwitchBase>(AvailableSwitches);
            

            PopulateResolver();

            SearchService.Initialize();
        }

        private void PopulateResolver()
        {
            NamespaceResolver.Items.AddRange(AvailableSwitches);
        }

        public void AddAvailableSwitch(SwitchBase sw)
        {
            if (!AvailableSwitchesDict.ContainsKey(sw.UniqueName))
            {
                AvailableSwitchesDict.Add(sw.UniqueName, sw);
            }
            else
            {
                var collidingSwitch = AvailableSwitchesDict[sw.UniqueName];
                ProcessorCompiler.AddError(
                    new ExceptionEntry
                    {
                        Exception = new ArgumentException(
                        $"Switch Name {sw.UniqueName} is not unique ({sw.FullTypeName}) it collides with {collidingSwitch.DisplayName} ({collidingSwitch.FullTypeName}).\r\n" +
                        $"Make sure the UniqueName is unique or search, help and other features wont work correctly!"),
                        Name = $"Switch {sw.UniqueName} is not unique."
                    });
            }
        }

        public Dictionary<string, SwitchBase> AvailableSwitchesDict { get; set; } = new Dictionary<string, SwitchBase>();

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

        public void NewProject()
        {
            OnBeforeLoading();
            CurrentProject = new BlueSwitchProject();
            CurrentProject.Initialize(this);
            OnProjectLoaded();
        }

        public void LoadProject(String filePath)
        {
            OnBeforeLoading();
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

        protected virtual void OnBeforeLoading()
        {
            BeforeLoading?.Invoke(this, EventArgs.Empty);
        }
    }
}
