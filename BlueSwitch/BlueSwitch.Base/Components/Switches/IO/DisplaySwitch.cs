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
            UniqueName = "BlueSwitch.Base.Components.Switches.IO.Display";
            DisplayName = "Display";
            Description = "Display";
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.IO;
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            RenderingEngine.RequestRedraw();

            SetData(1, new DataContainer(GetData(1)?.Value));

            base.OnProcess(p, node);
        }

        public override void DrawText(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            var data = GetData(1);

            if (data != null)
            {
                DrawDescriptionText(g, e, parent, data.Value.ToString());
            }
            

            base.DrawText(g, e, parent);
        }
    }
}
