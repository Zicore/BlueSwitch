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

namespace BlueSwitch.Base.Components.Switches.CodeFlow
{
    public class WhileLoopSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "While";

            AddInput(new ActionSignature());
            AddInput(typeof(bool), new CheckBox { AutoStoreValue = false });

            AddOutput(new ActionSignature());
            AddOutput(new ActionSignature());

        }

        public override GroupBase OnSetGroup()
        {
            return Groups.CodeFlow;
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            var data = GetDataValueOrDefault<bool>(1);

            if (!data)
            {
                node.Skip = new SkipNode(0);
            }
            else
            {
                node.Repeat = true;
                node.Skip = new SkipNode(1);
            }

            base.OnProcess(p, node);
        }
    }
}
