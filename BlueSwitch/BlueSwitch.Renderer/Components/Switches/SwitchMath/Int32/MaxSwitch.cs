using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.SwitchMath.Int32
{
    public class MaxSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return GroupBase.Math;
        }

        protected override void OnInitialize(RenderingEngine engine)
        {
            base.OnInitialize(engine);

            Name = "Max.Int32";

            AddInput(typeof(int), TextEdit.CreateNumberic());
            AddInput(typeof(int), TextEdit.CreateNumberic());

            AddOutput(typeof(int));
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            base.OnProcessData(p, node);

            var a = GetDataValueOrDefault<int>(0);
            var b = GetDataValueOrDefault<int>(1);

            SetData(0, new DataContainer(Math.Max(a,b)));
        }
    }
}
