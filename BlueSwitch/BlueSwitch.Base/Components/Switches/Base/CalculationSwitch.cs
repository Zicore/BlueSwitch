using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlueSwitch.Renderer.Components.Base;
using BlueSwitch.Renderer.Components.Types;
using BlueSwitch.Renderer.Processing;

namespace BlueSwitch.Renderer.Components.Switches.Base
{
    public class CalculationSwitch : SwitchBase
    {
        public CalculationSwitch()
        {
            
        }

        protected override void OnInitialize(RenderingEngine engine)
        {
            AddInput(new InputBase(new AnySignature()));
            AddOutput(new OutputBase(new AnySignature()));
            Name = "Calculation";
        }

        public override GroupBase OnSetGroup()
        {
            return GroupBase.IO;
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);

                Name = "Calculation " + i;

                p.Redraw();
            }

            base.OnProcess(p, node);
        }
    }
}
