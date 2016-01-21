using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.IO;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class SetterSwitch : VariableSwitch
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.Setter;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine);

            AutoDiscoverDisabled = true;

            AddInput(new ActionSignature());
            if (Variable == null)
            {
                AddInput(typeof(object));
            }
            else
            {
                AddInput(Variable.NetValueType, Variable.CreateComponent());
            }
            
            AddOutput(new ActionSignature());

            NamePrefix = "Set";
            UpdateVariable(Variable);
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            base.OnProcess(p, node);
            var data = GetData(1);
            if (data != null && Variable != null)
            {
                Variable.Value = data.Value;
            }
        }
    }
}
