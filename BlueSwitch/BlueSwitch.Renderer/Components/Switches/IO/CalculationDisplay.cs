using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Renderer.Components.Base;
using BlueSwitch.Renderer.Components.Event;
using BlueSwitch.Renderer.Components.Switches.Base;
using BlueSwitch.Renderer.Processing;

namespace BlueSwitch.Renderer.Components.Switches.IO
{
    public class CalculationDisplay : SwitchBase
    {
        private double _value;

        public override GroupBase OnSetGroup()
        {
            return GroupBase.IO;
        }

        protected override void OnInitialize(RenderingEngine engine)
        {
            Name = "Calc";
            ColumnWidth = 80;
        }
        
        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            base.OnProcess(p, node);
        }

        public void OnClickTrigger(object sender, TriggerEventArgs e)
        {
            if (e.Data != null)
            {
                _value += Convert.ToDouble(e.Data.Value);

                Name = $"Calc {_value}";
            }
        }
    }
}
