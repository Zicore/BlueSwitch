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
    public class TextEdit : UIComponent
    {
        private bool _caretVisible = false;

        private int _caret;
        private string _text = String.Empty;

        [JsonIgnore]
        public int Caret
        {
            get { return _caret; }
            protected set
            {
                _caret = value;
                VerifyCaret(Text);
            }
        }

        public void VerifyCaret(String value)
        {
            if (_caret < 0)
            {
                _caret = 0;
            }
            if (_caret > Text.Length)
            {
                _caret = Text.Length;
            }
        }

        public event EventHandler TextChanged;

        private readonly static StringFormat StringFormat = new StringFormat(StringFormat.GenericDefault.FormatFlags | StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoClip);

        [JsonIgnore]
        public bool HasFocus { get; set; }

        [JsonIgnore]
        public bool AutoResize { get; set; }

        [JsonIgnore]
        public static Pen DescriptionPen { get; set; } = new Pen(Color.FromArgb(180, 30, 30, 30), 0.5f);

        [JsonIgnore]
        protected static Font FontSmall = new Font(new FontFamily("Calibri"), 9, FontStyle.Regular);

        [JsonIgnore]
        public static Pen CaretPen { get; set; } = new Pen(Color.FromArgb(255, 30, 30, 30), 0.5f);

        [JsonIgnore]
        public static Pen CaretPen2 { get; set; } = new Pen(Color.FromArgb(255, 255, 255, 255), 0.5f);

        [JsonIgnore]
        public bool NumberMode {get;set;}

        [JsonIgnore]
        public bool AllowDecimalPoint { get; set; }

        [JsonIgnore]
        public decimal NumberValue
        {
            get
            {
                if (NumberMode)
                {
                    decimal result = 0;
                    decimal.TryParse(Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out result);
                    return result;
                }
                return 0;
            }
        }

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
                    if (_text != value)
                    {
                        _text = value;
                        OnTextChanged();
                    }
                }

                if (NumberMode)
                {
                    VerifyNumber();
                }
            }
        }

        private void VerifyNumber()
        {
            if (NumberMode)
            {
                if (!Text.EndsWith("."))
                {
                    decimal result = 0;
                    if (decimal.TryParse(Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out result))
                    {
                        _text = NumberValue.ToString(CultureInfo.InvariantCulture);
                    }
                }
            }
        }

        /// <summary>
        /// Creates a numeric Text Edit UI Element
        /// </summary>
        /// <returns>The numeric TextEdit UI Element</returns>
        public static TextEdit CreateNumberic()
        {
            return new TextEdit {NumberMode = true, AllowDecimalPoint = false};
        }

        protected override void OnInitialize(RenderingEngine engine, DrawableBase parent)
        {
            base.OnInitialize(engine, parent);
            
            engine.KeyboardService.KeyPress += KeyboardServiceOnKeyPress;
            engine.KeyboardService.KeyUp += KeyboardServiceOnKeyUp;
            engine.KeyboardService.CaretTick += KeyboardServiceOnCaretTick;
        }

        private void KeyboardServiceOnCaretTick(object sender, EventArgs eventArgs)
        {
            if (HasFocus && !ReadOnly && RenderingEngine.DesignMode)
            {
                _caretVisible = !_caretVisible;
                RenderingEngine.RequestRedraw();
            }
        }

        private void KeyboardServiceOnKeyUp(object sender, KeyEventArgs e)
        {
            if (HasFocus && !ReadOnly && RenderingEngine.DesignMode)
            {
                if (e.KeyCode == Keys.Left)
                {
                    Caret--;
                    return;
                }

                if (e.KeyCode == Keys.Right)
                {
                    Caret++;
                    return;
                }

                if (e.KeyCode == Keys.Delete)
                {
                    RemovePreviousChar();
                    return;
                }
            }
        }

        private void RemovePreviousChar()
        {
            if (Caret < Text.Length)
            {
                Text = Text.Remove(Caret, 1);
            }
        }

        private void RemoveLastChar()
        {
            if (Caret > 0)
            {
                Text = Text.Remove(Caret - 1, 1);
                Caret--;
            }
        }

        protected virtual void KeyboardServiceOnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (HasFocus && !ReadOnly && RenderingEngine.DesignMode)
            {
                if (e.KeyChar == (char)Keys.Back)
                {
                    RemoveLastChar();
                    return;
                }

                if (!NumberMode)
                {
                    if (e.KeyChar == (char) Keys.Enter)
                    {
                        Text = Text.Insert(Caret, Environment.NewLine);
                        return;
                    }
                }

                if (Char.IsControl(e.KeyChar)) return;

                if (NumberMode)
                {
                    if (Char.IsDigit(e.KeyChar))
                    {
                        Text = Text.Insert(Caret, Char.ToString(e.KeyChar));
                        Caret++;
                    }

                    if (AllowDecimalPoint)
                    {
                        if (e.KeyChar == '.' || e.KeyChar == ',')
                        {
                            if (Text.Contains("."))
                            {
                                Text = Text.Remove(Text.IndexOf(".", StringComparison.CurrentCultureIgnoreCase),1);
                                VerifyCaret(Text);
                            }

                            Text = Text.Insert(Caret, ".");
                            Caret++;
                        }
                    }
                }
                else
                {
                    Text = Text.Insert(Caret, Char.ToString(e.KeyChar));
                    Caret++;
                }
                
            }
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
            
            if (HasFocus && _caretVisible)
            {
                var sub = Text.Substring(0, Caret);
                
                SizeF size = new SizeF();

                if (sub.Length > 0)
                {
                    size = g.MeasureString(sub, FontSmall, fontPoint, StringFormat);
                }


                float minHeight = Math.Max(size.Height, FontSmall.Size + 4f);

                PointF caretPoint = new PointF(fontPoint.X + Math.Max(size.Width - 1.0f, 1.5f), fontPoint.Y + 3f);
                PointF caretPoint2 = new PointF(fontPoint.X + Math.Max(size.Width - 1.5f,1.0f), fontPoint.Y + 3f);

                g.DrawLine(CaretPen, caretPoint, new PointF(caretPoint.X, caretPoint.Y + minHeight - 4f));
                g.DrawLine(CaretPen2, caretPoint2, new PointF(caretPoint2.X, caretPoint.Y + minHeight - 4f));
            }


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
            }

            base.UpdateMouseUp(e, parent, previous);
        }

        protected virtual void OnTextChanged()
        {
            if (AutoStoreValue)
            {
                SaveData();
            }
            //TextChanged?.Invoke(this, EventArgs.Empty);
        }

        public override object GetData() 
        {
            if (NumberMode)
            {
                return NumberValue;
            }
            return Text;
        }

        public override void SaveData()
        {
            var sw = Parent as SwitchBase;
            sw?.ValueStore.Store("TextEdit.Data", Text);
            var io = Parent as InputOutputBase;
            io?.Parent?.ValueStore.Store($"TextEdit.{io.Parent.Id}.Data",Text);
        }

        public override void LoadData()
        {
            var sw = Parent as SwitchBase;
            if (sw != null)
            {
                Text = sw.ValueStore.GetOrDefault<string>("TextEdit.Data");
            }
            var io = Parent as InputOutputBase;
            if (io != null)
            {
                Text = io.Parent?.ValueStore.GetOrDefault<string>($"TextEdit.{io.Parent.Id}.Data");
            }
        }
    }
}
