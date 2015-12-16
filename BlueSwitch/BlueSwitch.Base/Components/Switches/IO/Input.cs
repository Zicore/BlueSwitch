using System.Drawing;
using BlueSwitch.Renderer.Components.Base;
using BlueSwitch.Renderer.Components.Switches.Base;
using BlueSwitch.Renderer.Processing;
using Newtonsoft.Json;

namespace BlueSwitch.Renderer.Components.Switches.IO
{
    public class Input : SwitchBase
    {
        protected override void OnInitialize()
        {
            AddOutput(typeof (bool));
            SetData(0,new DataContainer(true));
            Name = "Input1";
            Description = "Schalter";
        }

        public override GroupBase OnSetGroup()
        {
            return GroupBase.IO;
        }

        private bool _active = false;

        [JsonIgnore]
        public Brush ActiveBrush { get; set; } = new SolidBrush(Color.Orange);

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            SetData(0,new DataContainer(_active));
        }

        public override void UpdateMouseUp(RenderingEngine e, ComponentBase parent, InputOutputBase previous)
        {
            if (IsMouseOver && IsSelected)
            {
                _active = !_active;
            }

            base.UpdateMouseUp(e, parent, previous);
        }
        
        public override void DrawBody(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            if (_active)
            {
                var r = new RectangleF(Position, Size);
                g.FillRectangle(ActiveBrush, r);
                DrawRectangleF(g, Pen, r);
            }
            else
            {
                base.DrawBody(g,e,parent);
            }
        }
    }
}
