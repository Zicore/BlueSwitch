using System.Linq;
using BlueSwitch.Base.IO;
using BlueSwitch.Base.Services;

namespace BlueSwitch.Base.Components.Base
{
    public class Connection
    {
        private InputOutputSelector _fromInputOutput;
        private InputOutputSelector _toInputOutput;
        
        public Connection()
        {

        }

        public Connection(InputOutputSelector fromInputOutput, InputOutputSelector toInputOutput)
        {
            FromInputOutput = fromInputOutput;
            ToInputOutput = toInputOutput;
        }
        
        public InputOutputSelector FromInputOutput
        {
            get { return _fromInputOutput; }
            set
            {
                _fromInputOutput = value;
            }
        }
        
        public InputOutputSelector ToInputOutput
        {
            get { return _toInputOutput; }
            set
            {
                _toInputOutput = value;
            }
        }

        public void UpdateConnection(BlueSwitchProject project)
        {
            var switchTo = project.ItemLookup[ToInputOutput.OriginId];
            var switchFrom = project.ItemLookup[FromInputOutput.OriginId];

            if (ToInputOutput.IsInput)
            {
                ToInputOutput.Origin = switchTo;
                ToInputOutput.InputOutput = switchTo.Inputs.FirstOrDefault(x => x.Index == ToInputOutput.InputOutputId);

                FromInputOutput.Origin = switchFrom;
                FromInputOutput.InputOutput = switchFrom.Outputs.FirstOrDefault(x => x.Index == FromInputOutput.InputOutputId);
            }
            else
            {
                ToInputOutput.Origin = switchTo;
                ToInputOutput.InputOutput = switchTo.Outputs.FirstOrDefault(x => x.Index == ToInputOutput.InputOutputId);

                FromInputOutput.Origin = switchFrom;
                FromInputOutput.InputOutput = switchFrom.Inputs.FirstOrDefault(x => x.Index == FromInputOutput.InputOutputId);
            }
        }
    }
}
