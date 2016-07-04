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
    public class CheckBox : UIComponent
    {
        public CheckBox()
        {
            ReadOnly = false;
        }

        public bool Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnValueChanged();
                }
            }
        }

        //private bool _carretVisible = false;

        //private int _carret;
        private string _text = String.Empty;
        private bool _value = false;

        public event EventHandler Click;

        private readonly static StringFormat StringFormat = new StringFormat(StringFormat.GenericDefault.FormatFlags | StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoClip);

        [JsonIgnore]
        public bool HasFocus { get; set; }

        [JsonIgnore]
        public bool AutoResize { get; set; }

        [JsonIgnore]
        public static Pen DescriptionPen { get; set; } = new Pen(Color.FromArgb(180, 30, 30, 30), 0.5f);

        [JsonIgnore]
        protected static Font FontSmall = new Font(new FontFamily("Calibri"), 7, FontStyle.Regular);
       
        private static Pen _checkBoxPen = new Pen(Color.Black,1);
        private static Brush _checkBoxCheckedBrush = new SolidBrush(Color.FromArgb(130, 0, 180, 20));
        private static Brush _checkBoxUncheckedBrush = new SolidBrush(Color.FromArgb(80, 0, 0, 20));

        public string Text
        {
            get
            {
                if (Value)
                {
                    return Value.ToString();
                }
                else
                {
                    return Value.ToString();
                }
            }
        }

        public override void Draw(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            ExtendedGraphics extendedGraphics = new ExtendedGraphics(g,e);

            var r = DescriptionBounds;

            r.Height -= 1;
            r.Y += 1;

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

            float checkBoxWidth = r.Height - 2;
            float checkBoxOffset = 1.5f;
            var checkBoxRect = new RectangleF(r.X + r.Width - (checkBoxWidth + checkBoxOffset), r.Y + checkBoxOffset, checkBoxWidth, r.Height - checkBoxOffset * 2.0f);
            
            if (Value)
            {
                extendedGraphics.FillRoundRectangle(_checkBoxCheckedBrush, checkBoxRect.X, checkBoxRect.Y, checkBoxRect.Width, checkBoxRect.Height, radius);
            }
            else
            {
                extendedGraphics.FillRoundRectangle(_checkBoxUncheckedBrush, checkBoxRect.X, checkBoxRect.Y, checkBoxRect.Width, checkBoxRect.Height, radius);
            }
            extendedGraphics.DrawRoundRectangle(_checkBoxPen, checkBoxRect.X, checkBoxRect.Y, checkBoxRect.Width, checkBoxRect.Height, radius);

            extendedGraphics.DrawRoundRectangle(DescriptionPen, r.X, r.Y, r.Width, r.Height, radius);

            PointF fontPoint = new PointF(r.X - 1, r.Y - 2);

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
        
        public override void UpdateMouseUp(RenderingEngine e, DrawableBase parent, DrawableBase previous)
        {
            HasFocus = false;
            if (IsMouseOver)
            {
                HasFocus = true;
                Value = !Value;
                OnClick();
            }

            base.UpdateMouseUp(e, parent, previous);
        }

        protected virtual void OnValueChanged()
        {
            if (AutoStoreValue)
            {
                SaveData();
            }
        }

        public override object GetData()
        {
            return Value;
        }

        protected virtual void OnClick()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }

        public override void SaveData()
        {
            var sw = Parent as SwitchBase;
            sw?.ValueStore.Store("CheckBox.Data", Value);
            var io = Parent as InputOutputBase;
            io?.Parent?.ValueStore.Store($"CheckBox.{io.Parent.Id}.Data", Value);
        }

        public override void LoadData()
        {
            var sw = Parent as SwitchBase;
            if (sw != null)
            {
                Value = sw.ValueStore.GetOrDefault<bool>("CheckBox.Data");
            }
            var io = Parent as InputOutputBase;
            if (io != null)
            {
                Value = io.Parent.ValueStore.GetOrDefault<bool>($"CheckBox.{io.Parent.Id}.Data");
            }
        }
    }
}
