using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Event;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Meta.Help;
using BlueSwitch.Base.Processing;
using BlueSwitch.Base.Trigger.Types;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class UnknownSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.Trigger;
        }

        public String UnknownTypeName { get; set; }

        public UnknownSwitch(String typeName)
        {
            UnknownTypeName = typeName;
        }

        public UnknownSwitch()
        {
            
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            this.AutoDiscoverDisabled = true;
            UniqueName = "UnknownSwitch";
            Description = "UnknownSwitch";


            AddInput(new InputBase(new ActionSignature()));
            AddInput(new AnySignature());
            AddInput(new AnySignature());
            AddInput(new AnySignature());
            AddInput(new AnySignature());
            AddInput(new AnySignature());
            AddInput(new AnySignature());
            AddInput(new AnySignature());
            AddInput(new AnySignature());

            AddOutput(new OutputBase(new ActionSignature()));
            AddOutput(new AnySignature());
            AddOutput(new AnySignature());
            AddOutput(new AnySignature());
            AddOutput(new AnySignature());
            AddOutput(new AnySignature());
            AddOutput(new AnySignature());
            AddOutput(new AnySignature());
            AddOutput(new AnySignature());
        }
        

        public override Brush GetMainBrush(RectangleF rectangle, RenderingEngine e)
        {
            var brush = new LinearGradientBrush(rectangle, Color.Black, Color.Black, 90, true);

            ColorBlend cb = new ColorBlend();

            cb.Positions = new[] { 0, 0.2f, 0.5f, 1 };
            cb.Colors = new Color[] { Color.FromArgb(120, 0, 0, 0), Color.FromArgb(150, 180, 0, 0), Color.FromArgb(150, 200, 0, 0), Color.FromArgb(200, 200, 0, 0) };

            brush.InterpolationColors = cb;

            return brush;
        }
    }
}
