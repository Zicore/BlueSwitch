using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Base;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Components.UI
{
    public class AddPinComponent : DrawableBase
    {
        [JsonIgnore]
        protected float ParentWidth = 0;

        [JsonIgnore]
        public DrawableBase Parent { get; protected set; }

        public event EventHandler<MouseEventArgs> Click;
        public event EventHandler<KeyEventArgs> KeyUp;

        public bool IsInput { get; set; }

        public void Initialize(Engine engine, DrawableBase parent)
        {
            Parent = parent;
            DescriptionHeight = 0;
            ParentWidth = parent.ColumnWidth;
            RenderingEngine = engine;
            OnInitialize(engine, parent);

            if (engine is RenderingEngine)
            {
                var renderingEngine = engine as RenderingEngine;
                renderingEngine.MouseService.MouseUp += MouseServiceOnMouseUp;
                renderingEngine.KeyboardService.KeyUp += KeyboardServiceOnKeyUp;
            }
        }

        private void KeyboardServiceOnKeyUp(object sender, KeyEventArgs e)
        {
            if (IsMouseOver)
            {
                OnKeyUp(e);
            }
        }

        private void MouseServiceOnMouseUp(object sender, MouseEventArgs e)
        {
            if (IsMouseOver)
            {
                OnClick(e);
            }
        }

        public PinDescription Description { get; set; }

        public AddPinComponent(bool isInput)
        {
            IsInput = isInput;
        }
        
        protected virtual void OnInitialize(Engine renderingEngine, DrawableBase parent)
        {
            
        }

        public virtual PointF GetTranslation(DrawableBase parent)
        {
            var r = DescriptionBounds;
            return new PointF(r.X, r.Y);
        }

        public override void Update(RenderingEngine e, DrawableBase parent, DrawableBase previous)
        {
            Translation = GetTranslation(parent);
            var r = parent.DescriptionBounds;
            //Size = new SizeF(r.Width, r.Height);
        }


        [JsonIgnore]
        public static Pen PenWhite { get; set; } = new Pen(Color.DimGray, 1.5f);

        [JsonIgnore]
        public static Pen Pen { get; set; } = new Pen(Color.Black, 2.0f);

        [JsonIgnore]
        public static Brush Background { get; set; } = new SolidBrush(Color.White);

        [JsonIgnore]
        public static Brush BackgroundMouseOver { get; set; } = new SolidBrush(Color.DeepSkyBlue);

        [JsonIgnore]
        public static Brush BackgroundMouseDown { get; set; } = new SolidBrush(Color.DarkOrange);

        [JsonIgnore]
        public override SizeF Size
        {
            get { return new SizeF(8.0f, 8.0f); }
        }

        public override void Draw(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            var r = DescriptionBounds;
            

            float part = r.Height / 8.0f;
            float mid = part*3;
            
            var rW = new RectangleF(r.X, r.Y + mid, r.Width, part * 2);
            var rH = new RectangleF(r.X + mid, r.Y, part * 2, r.Height);

            var bg = Background;
            if (IsMouseOver)
            {
                if (e.MouseService.LeftMouseDown)
                {
                    bg = BackgroundMouseDown;
                }
                else
                {
                    bg = BackgroundMouseOver;
                }
            }

            g.DrawRectangle(Pen, rW.X, rW.Y, rW.Width, rW.Height);
            g.DrawRectangle(Pen, rH.X, rH.Y, rH.Width, rH.Height);

            g.FillRectangle(bg, rH);
            g.FillRectangle(bg, rW);
            
            base.Draw(g, e, parent);
        }

        public override RectangleF DescriptionBounds
        {
            get
            {
                RectangleF r;

                if (IsInput)
                {
                    r = new RectangleF(
                        Parent.Rectangle.X + 4,
                        Parent.Rectangle.Y + Parent.SizeUntilExtraRow.Height + 2,
                        Size.Width,
                        Size.Height
                        );
                }
                else
                {
                    r = new RectangleF(
                        Parent.Rectangle.X + ParentWidth - Size.Width - 5,
                        Parent.Rectangle.Y + Parent.SizeUntilExtraRow.Height + 2,
                        Size.Width,
                        Size.Height
                        );
                }

                return r;
            }
        }

        protected virtual void OnClick(MouseEventArgs e)
        {
            Click?.Invoke(this, e);
        }

        protected virtual void OnKeyUp(KeyEventArgs e)
        {
            KeyUp?.Invoke(this, e);
        }
    }
}
