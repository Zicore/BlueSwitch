using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using Newtonsoft.Json;
using Zicore.Settings.Json;

namespace BlueSwitch.Base.IO
{
    [JsonObject("BlueSwitchProject")]
    public class BlueSwitchProject : JsonSerializable
    {
        public event EventHandler BeforeLoading;

        private static volatile int _currentSwitchId = 0;

        [JsonIgnore]
        public bool Ready { get; protected set; } = false;

        [JsonIgnore]
        public Dictionary<int, SwitchBase> ItemLookup { get; } = new Dictionary<int, SwitchBase>();

        [JsonIgnore]
        public Dictionary<int, HashSet<int>> InputLookup { get; } = new Dictionary<int, HashSet<int>>();

        [JsonProperty("Translation")]
        public PointF Translation { get; set; } = new PointF();

        [JsonProperty("Items")]
        public List<SwitchBase> Items { get; set; } = new List<SwitchBase>();

        [JsonProperty("Connections")]
        public List<Connection> Connections { get; set; } = new List<Connection>();

        [JsonProperty("Variables")]
        public Dictionary<string, Variable> Variables { get; } = new Dictionary<string, Variable>();

        public Variable GetVariable(string key)
        {
            if (!String.IsNullOrEmpty(key) && Variables.ContainsKey(key))
            {
                return Variables[key];
            }
            return null;
        }

        public string GetFreeVariablename(string key, int iterator)
        {
            if (!Variables.ContainsKey(key + iterator))
            {
                return key + iterator;
            }
            iterator++;
            return GetFreeVariablename(key, iterator);
        }

        public bool RenameVariable(string oldName, string newName)
        {
            if (Variables.ContainsKey(newName))
            {
                return false;
            }

            var variable = GetVariable(oldName);
            if (variable != null)
            {
                Variables.Remove(oldName);
                variable.Name = newName;
                Variables.Add(variable.Name, variable);
                return true;
            }
            return false;
        }

        private string _name;
        private string _description;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public float Zoom { get; set; } = 1.0f;

        public void Add(Engine renderingEngine, SwitchBase sw)
        {
            sw.Id = _currentSwitchId;
            ItemLookup.Add(_currentSwitchId, sw);
            Items.Add(sw);
            sw.Initialize(renderingEngine);
            _currentSwitchId++;
        }

        public void AddInternal(SwitchBase sw)
        {
            ItemLookup.Add(sw.Id, sw);
            _currentSwitchId = sw.Id + 1;
        }

        public void Remove(SwitchBase sw)
        {
            RemoveConnections(sw);
            ItemLookup.Remove(sw.Id);
            Items.Remove(sw);
        }

        public void AddConnectionToLookup(Connection connection)
        {
            if (!InputLookup.ContainsKey(connection.FromInputOutput.OriginId))
            {
                InputLookup[connection.FromInputOutput.OriginId] = new HashSet<int>();
            }

            InputLookup[connection.FromInputOutput.OriginId].Add(connection.FromInputOutput.InputOutputId);
        }

        public void AddConnection(Connection connection)
        {
            Connections.Add(connection);
            AddConnectionToLookup(connection);
        }

        public void RemoveConnection(Connection connection)
        {
            Connections.Remove(connection);

            if (!InputLookup.ContainsKey(connection.FromInputOutput.OriginId))
            {
                InputLookup[connection.FromInputOutput.OriginId] = new HashSet<int>();
            }

            InputLookup[connection.FromInputOutput.OriginId].Remove(connection.FromInputOutput.InputOutputId);
        }

        public bool IsConnected(SwitchBase sw, InputOutputBase io)
        {
            return InputLookup.ContainsKey(sw.Id) && InputLookup[sw.Id].Contains(io.Index);
        }

        public void RemoveConnections(SwitchBase sw)
        {
            var outputs = Connections.Where(x => x.ToInputOutput.Origin == sw).ToList();
            var inputs = Connections.Where(x => x.FromInputOutput.Origin == sw).ToList();

            foreach (var connection in outputs)
            {
                RemoveConnection(connection);
            }

            foreach (var connection in inputs)
            {
                RemoveConnection(connection);
            }
        }

        public void RemoveConnection(InputOutputBase io)
        {
            var outputs = Connections.Where(x => x.FromInputOutput.InputOutput == io).ToList();
            var inputs = Connections.Where(x => x.ToInputOutput.InputOutput == io).ToList();

            foreach (var connection in outputs)
            {
                RemoveConnection(connection);
            }

            foreach (var connection in inputs)
            {
                RemoveConnection(connection);
            }
        }

        public SwitchBase FindStart()
        {
            return Items.FirstOrDefault(switchBase => switchBase.IsStart);
        }

        public IList<Connection> FindPotentialStarts()
        {
            List<Connection> startSwitches = new List<Connection>(); // Nur Event Switches
            foreach (var connection in Connections)
            {
                if (connection.ToInputOutput.Origin.IsStart)
                {
                    startSwitches.Add(connection);
                }
            }
            return startSwitches;
        }

        public IList<Connection> FindInputs(SwitchBase sw)
        {
            return Connections.Where(x => x.ToInputOutput.Origin == sw).ToList();
        }

        public void SetOutputValue(SwitchBase sw, DataContainer data)
        {
            foreach (var connection in FindInputs(sw))
            {
                connection.FromInputOutput.InputOutput.Data = data;
            }
        }

        public void Initialize(Engine renderingEngine)
        {
            OnBeforeLoading();
            UpdateSwitches(renderingEngine);
            UpdateConnections();

            if (Items.Count > 0)
            {
                _currentSwitchId = Items.Max(x => x.Id) + 1;
            }

            Ready = true;
            renderingEngine.RequestRedraw();
        }

        private void UpdateSwitches(Engine renderingEngine)
        {
            foreach (var switchBase in Items)
            {
                switchBase.Initialize(renderingEngine);
            }

            foreach (var switchBase in Items)
            {
                ItemLookup.Add(switchBase.Id, switchBase);
            }
        }

        private void UpdateConnections()
        {
            foreach (var connection in Connections)
            {
                connection.UpdateConnection(this);
            }

            var invalidConnections = Connections.Where(x => x.ToInputOutput.InputOutput == null || x.FromInputOutput.InputOutput == null).ToList();

            foreach (var invalidConnection in invalidConnections)
            {
                Connections.Remove(invalidConnection);
            }

            foreach (var connection in Connections)
            {
                AddConnectionToLookup(connection);
            }
        }

        protected virtual void OnBeforeLoading()
        {
            BeforeLoading?.Invoke(this, EventArgs.Empty);
        }
    }
}
