using System;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Logic
{
    public class EqualsSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "BlueSwitch.Base.Components.Switches.Logic.Equals.Base";
            DisplayName = "Equals";
            Description = "Returns true if both input elements are equal else returns false";

            AddInput(new InputBase(new AnySignature()));
            AddInput(new InputBase(new AnySignature()));
            AddOutput(typeof(bool));

            DrawGlyphImage = true;
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.Logic;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var a = GetData(0);
            var b = GetData(1);
            
            if (a != null && b != null)
            {
                SetData(0, new DataContainer(a.Value.Equals(b.Value)));
            }
            else
            {
                SetData(0, new DataContainer(false));
            }
            
            base.OnProcess(p, node);
        }
    }
}
