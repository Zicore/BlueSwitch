using System;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Drawing.Extended;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Components.UI
{
    public class Button : UIComponent
    {
        public Button()
        {
            ReadOnly = false;
            Text = "Properties";
        }

        //private bool _carretVisible = false;

        //private int _carret;
        private string _text = String.Empty;

        public event EventHandler Click;

        private static readonly StringFormat StringFormat = new StringFormat(StringFormat.GenericDefault.FormatFlags | StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoClip);

        [JsonIgnore]
        public bool HasFocus { get; set; }

        [JsonIgnore]
        public bool AutoResize { get; set; }

        [JsonIgnore]
        public static Pen DescriptionPen { get; set; } = new Pen(Color.FromArgb(180, 30, 30, 30), 0.5f);

        [JsonIgnore]
        protected static Font FontSmall = new Font(new FontFamily("Calibri"), 9, FontStyle.Regular);
        
        [JsonIgnore]
        public bool NumberMode {get;set;}

        [JsonIgnore]
        public bool AllowDecimalPoint { get; set; }


        public string Text
        {
            get { return _text; }
            set
            {
                if (value == null)
                {
                    _text = string.Empty;
                }
                else
                {
                    _text = value;
                }
            }
        }

        protected override void OnInitialize(Engine renderingEngine, DrawableBase parent)
        {
            base.OnInitialize(renderingEngine, parent);
        }
        
        public override void Draw(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            ExtendedGraphics extendedGraphics = new ExtendedGraphics(g);

            var r = DescriptionBounds;

            float radius = 2;

            Brush brush;

            if (IsMouseOver)
            {
                brush = new SolidBrush(Color.FromArgb(150, 155, 22, 0));
            }
            else if (HasFocus)
            {
                brush = new SolidBrush(Color.FromArgb(150, 50, 125, 15));
            }
            else if (ReadOnly)
            {
                brush = new SolidBrush(Color.FromArgb(110, 250, 200, 0));
            }
            else
            {
                brush = new SolidBrush(Color.FromArgb(80, 0, 0, 0));
            }

            if (!ReadOnly)
            {
                extendedGraphics.FillRoundRectangle(brush, r.X, r.Y, r.Width, r.Height, radius);
            }

            extendedGraphics.DrawRoundRectangle(DescriptionPen, r.X, r.Y, r.Width, r.Height, radius);

            PointF fontPoint = new PointF(r.X + 1, r.Y - 2);

            var renderingHint = g.TextRenderingHint;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            RectangleF fontRect1 = new RectangleF(fontPoint.X, fontPoint.Y, r.Width, r.Height);
            RectangleF fontRect2 = new RectangleF(fontPoint.X + 0.5f, fontPoint.Y + 0.5f, r.Width, r.Height);

            g.DrawString(Text, FontSmall, Brushes.Black, fontRect1, StringFormat);
            g.DrawString(Text, FontSmall, Brushes.White, fontRect2, StringFormat);
            
            if (AutoResize)
            {
                SizeF sizeFull = new SizeF();
                if (Text.Length > 0)
                {
                    sizeFull = g.MeasureString(Text, FontSmall, fontPoint, StringFormat);
                }

                float widthDiff = parent.Size.Width - r.Width;
                float minWidth = Math.Max(ParentWidth, sizeFull.Width + widthDiff);
                parent.ColumnWidth = minWidth;
            }

            g.TextRenderingHint = renderingHint;

            base.Draw(g, e, parent);
        }
        
        public override void UpdateMouseUp(Engine e, DrawableBase parent, DrawableBase previous)
        {
            HasFocus = false;
            if (IsMouseOver)
            {
                HasFocus = true;
                OnClick();
            }

            base.UpdateMouseUp(e, parent, previous);
        }

        protected virtual void OnTextChanged()
        {
            if (AutoStoreValue)
            {
                SaveData();
            }
        }

        public override object GetData()
        {
            return null;
        }

        public override void SaveData()
        {
            
        }

        public override void LoadData()
        {
            
        }

        protected virtual void OnClick()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }
    }
}
