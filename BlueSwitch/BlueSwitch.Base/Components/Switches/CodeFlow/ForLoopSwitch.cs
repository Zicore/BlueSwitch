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
    public class ForLoopSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "BlueSwitch.Base.Components.Switches.CodeFlow.For";
            DisplayName = "For";
            Description = "A for-Loop";

            AddInput(new ActionSignature());
            AddInput(typeof(int), TextEdit.CreateNumeric());

            AddOutput(new ActionSignature());
            AddOutput(typeof(int));
            AddOutput(new ActionSignature());
            IsCompact = true;
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.CodeFlow;
        }

        private int index = 0;

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            var data = GetDataValueOrDefault<int>(1);

            if (index < data)
            {
                index++;
                node.Repeat = true;
                node.Skip = new SkipNode(2);
            }
            else
            {
                index = 0;
                node.Skip = new SkipNode(0);
            }

            SetData(1, new DataContainer(index-1));

            //if (RenderingEngine.DebugMode)
            //{
            //    Name = $"For (i={index} < {data})";
            //}

            base.OnProcess(p, node);
        }
    }
}
