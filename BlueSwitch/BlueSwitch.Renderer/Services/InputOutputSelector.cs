using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Services
{
    public class InputOutputSelector
    {
        private SwitchBase _origin;
        private InputOutputBase _inputOutput;
        
        private bool _isInput;
        public bool IsInputJson
        {
            get { return this.InputOutput is InputBase; }
            set { _isInput = value; }
        }

        [JsonIgnore]
        public bool IsInput
        {
            get { return _isInput; }
        }

        public int OriginId { get; set; }
        public int InputOutputId { get; set; }

        [JsonIgnore]
        public SwitchBase Origin
        {
            get { return _origin; }
            set
            {
                _origin = value;
                if (value != null)
                {
                    OriginId = value.Id;
                }
            }
        }

        [JsonIgnore]
        public InputOutputBase InputOutput
        {
            get { return _inputOutput; }
            set
            {
                _inputOutput = value;
                if (value != null)
                {
                    InputOutputId = value.Index;
                }
            }
        }

        public InputOutputSelector(SwitchBase origin, InputOutputBase inputOutput)
        {
            Origin = origin;
            InputOutput = inputOutput;
        }
    }
}
