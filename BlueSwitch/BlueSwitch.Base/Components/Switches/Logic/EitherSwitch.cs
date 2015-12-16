using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Logic
{
    public class EitherSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return GroupBase.Logic;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine);

            Name = "Either";
            Description = "Returns based on a bool the first value or the second";

            AddInput(typeof (bool), new CheckBox());

            AddInput(new AnySignature());
            AddInput(new AnySignature());

            AddOutput(new AnySignature());
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var boolValue = GetDataValueOrDefault<bool>(0);

            if (boolValue)
            {
                SetData(0, GetData(1));
            }
            else
            {
                SetData(0, GetData(2));
            }

            base.OnProcessData(p, node);
        }
    }
}
