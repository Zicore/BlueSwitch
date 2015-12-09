using System.Drawing;
using System.Drawing.Drawing2D;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.IO
{
    public class ToggleSwitch : SwitchBase
    {
        protected override void OnInitialize(RenderingEngine engine)
        {
            AddOutput(typeof (bool));
            SetData(0,new DataContainer(true));
            Name = "Toggle";
            Description = "Schalter";
        }

        public override GroupBase OnSetGroup()
        {
            return GroupBase.IO;
        }

        private bool _active = false;

        public override Brush GetMainBrush(RectangleF rectangle)
        {
            if (_active)
            {
                var brush = new LinearGradientBrush(rectangle, Color.Black, Color.Black, 90, true);

                ColorBlend cb = new ColorBlend();

                cb.Positions = new[] {0, 0.2f, 0.5f, 1};
                cb.Colors = new Color[]
                {
                    Color.FromArgb(120, 0, 0, 0), Color.FromArgb(80, 255, 144, 0), Color.FromArgb(80, 255, 144, 0),
                    Color.FromArgb(60, 30, 160, 255)
                };

                brush.InterpolationColors = cb;

                return brush;
            }
            else
            {
                return base.GetMainBrush(rectangle);
            }
        }

        public override Brush GetMainSelectionBrush(RectangleF rectangle)
        {
            if (_active)
            {
                var brush = new LinearGradientBrush(rectangle, Color.Black, Color.Black, 90, true);

                ColorBlend cb = new ColorBlend();

                cb.Positions = new[] { 0, 0.2f, 0.5f, 1 };
                cb.Colors = new Color[]
                {
                    Color.FromArgb(120, 30, 144, 255), Color.FromArgb(80, 30, 144, 255), Color.FromArgb(80, 255, 144, 0), Color.FromArgb(60, 30, 160, 255)
                };

                brush.InterpolationColors = cb;

                return brush;
            }
            else
            {
                return base.GetMainBrush(rectangle);
            }
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            SetData(0,new DataContainer(_active));
        }

        public override void UpdateMouseDown(RenderingEngine e, DrawableBase parent, DrawableBase previous)
        {
            if (IsMouseOver && IsSelected)
            {
                _active = !_active;
            }
            base.UpdateMouseDown(e, parent, previous);
        }
    }
}
