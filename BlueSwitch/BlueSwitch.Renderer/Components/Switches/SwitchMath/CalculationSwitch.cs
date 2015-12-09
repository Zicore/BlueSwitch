using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.SwitchMath
{
    public class CalculationSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return GroupBase.Math;
        }

        protected override void OnInitialize(RenderingEngine engine)
        {
            base.OnInitialize(engine);
            Name = "Calculation";

            AddInput(new ActionSignature());
            AddOutput(new ActionSignature());

            AddInput(typeof (double));
            AddInput(typeof (bool));
        }

        private double _value = 0;

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            base.OnProcess(p, node);

            var value = GetDataValueOrDefault<double>(1);
            _value =_value + value;
            if (GetDataValueOrDefault<bool>(2))
            {
                _value = 0;
            }
        }
    }
}
