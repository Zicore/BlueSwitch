using System.Drawing;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Components.Switches.IO
{
    [JsonObject("SwitchBase")]
    public class DisplaySwitch : SwitchBase
    {
        public DisplaySwitch()
        {
            
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            AddInput(new ActionSignature());
            AddInput(new InputBase(new AnySignature()));

            AddOutput(new ActionSignature());
            AddOutput(new OutputBase(new AnySignature()));
            Name = "Display";
            
        }

        public override GroupBase OnSetGroup()
        {
            return GroupBase.IO;
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            RenderingEngine.RequestRedraw();
            base.OnProcess(p, node);
        }

        public override void DrawText(Graphics g, Engine e, DrawableBase parent)
        {
            var data = GetData(1);

            if (data != null)
            {
                DrawDescriptionText(g, e, parent, data.Value.ToString());
            }

            SetData(1,GetData(1));
            

            base.DrawText(g, e, parent);
        }
    }
}
