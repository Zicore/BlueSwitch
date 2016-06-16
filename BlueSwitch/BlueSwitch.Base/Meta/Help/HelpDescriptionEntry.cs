using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Drawing.Extended;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Meta.Help
{
    public class HelpDescriptionEntry
    {
        [JsonIgnore]
        public static Font FontTitle = new Font("Calibri", 8);

        [JsonIgnore]
        public static Font FontDescription = new Font("Calibri", 7);

        [JsonIgnore]
        public static Pen Pen { get; set; } = new Pen(Color.Black, 1.0f);

        public string Title { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }

        public void Draw(Graphics g, SwitchBase sw, DrawableBase parent, RenderingEngine e)
        {
            const float offset = 10;
            const float radius = 4;
            var r = sw.SelectionBounds;

            StringFormat format = new StringFormat();

            var textBounds = g.MeasureString(this.Title, FontTitle, r.Size, format);

            r = new RectangleF(r.X, r.Y - offset - textBounds.Height, r.Width, textBounds.Height);

            ExtendedGraphics extendedGraphics = new ExtendedGraphics(g, e);

            var brush = GetMainBrush(r);

            extendedGraphics.FillRoundRectangle(brush, r.X, r.Y, r.Width, r.Height, radius);
            extendedGraphics.DrawRoundRectangle(Pen, r.X, r.Y, r.Width, r.Height, radius);

            r = new RectangleF(r.X + 4, r.Y, r.Width, r.Height);

            var r1 = new RectangleF(r.X + 0.5f, r.Y + 0.5f, r.Width, r.Height);
            var r2 = new RectangleF(r.X, r.Y, r.Width, r.Height);

            g.DrawString(Title, FontTitle, Brushes.Black, r1, format);
            g.DrawString(Title, FontTitle, Brushes.White, r2, format);
        }

        public void DrawInput(Graphics g, SwitchBase sw, int index, RenderingEngine e)
        {
            const float maxHelpHeight = 400;
            const float offsetWidth = 5;
            const float offsetTextX = 32;
            const float radius = 4;
            var io = sw.Inputs[index];
            if (io.IsMouseOver)
            {
                var r = io.DescriptionBounds;
                var swBounds = sw.DescriptionBounds;
                swBounds.Height = maxHelpHeight;
                StringFormat format = new StringFormat();
                //format.FormatFlags = StringFormatFlags;
                var textBounds = g.MeasureString(this.Title, FontTitle, swBounds.Size, format);
                var swSizeDesc = new SizeF(240, swBounds.Height);
                var textDescBounds = g.MeasureString(this.Description, FontDescription, swSizeDesc, format);
                textDescBounds.Height = Math.Max(textDescBounds.Height, 8);

                var rText = new RectangleF(r.X - textBounds.Width - offsetTextX, r.Y, textBounds.Width + offsetWidth, textBounds.Height);
                var rDesc = new RectangleF(r.X - textDescBounds.Width - offsetTextX, r.Y + textBounds.Height + 2, textDescBounds.Width + offsetWidth, textDescBounds.Height);

                ExtendedGraphics extendedGraphics = new ExtendedGraphics(g, e);

                var brush = GetMainBrush(rText);
                var brushDesc = GetMainBrush(rDesc);

                extendedGraphics.FillRoundRectangle(brush, rText.X, rText.Y, rText.Width, rText.Height, radius);
                extendedGraphics.DrawRoundRectangle(Pen, rText.X, rText.Y, rText.Width, rText.Height, radius);

                extendedGraphics.FillRoundRectangle(brushDesc, rDesc.X, rDesc.Y, rDesc.Width, rDesc.Height, radius);
                extendedGraphics.DrawRoundRectangle(Pen, rDesc.X, rDesc.Y, rDesc.Width, rDesc.Height, radius);

                rText = new RectangleF(rText.X , rText.Y, rText.Width, rText.Height);
                rDesc = new RectangleF(rDesc.X , rDesc.Y, rDesc.Width, rDesc.Height);

                var r1 = new RectangleF(rText.X + 4 + 0.5f, rText.Y + 0.5f, rText.Width, rText.Height);
                var r2 = new RectangleF(rText.X + 4, rText.Y, rText.Width, rText.Height);

                var r3 = new RectangleF(rDesc.X + 4 + 0.5f, rDesc.Y + 0.5f, rDesc.Width, rDesc.Height);
                var r4 = new RectangleF(rDesc.X + 4, rDesc.Y, rDesc.Width, rDesc.Height);

                g.DrawString(Title, FontTitle, Brushes.Black, r1, format);
                g.DrawString(Title, FontTitle, Brushes.White, r2, format);

                g.DrawString(Description, FontDescription, Brushes.Black, r3, format);
                g.DrawString(Description, FontDescription, Brushes.White, r4, format);
            }
        }

        public void DrawOutput(Graphics g, SwitchBase sw, int index, RenderingEngine e)
        {
            const float maxHelpHeight = 400;
            const float offsetWidth = 5;
            const float offsetTextX = 24;
            const float radius = 4;

            var io = sw.Outputs[index];
            if (io.IsMouseOver)
            {
                var r = io.DescriptionBounds;
                var swBounds = sw.DescriptionBounds;
                swBounds.Height = maxHelpHeight;
                StringFormat format = new StringFormat();

                var textBounds = g.MeasureString(this.Title, FontTitle, swBounds.Size, format);
                var swSizeDesc = new SizeF(240, swBounds.Height);
                var textDescBounds = g.MeasureString(this.Description, FontDescription, swSizeDesc, format);
                textDescBounds.Height = Math.Max(textDescBounds.Height, 8);

                var rText = new RectangleF(r.X + offsetTextX + swBounds.Width, r.Y, textBounds.Width + offsetWidth, textBounds.Height);
                var rDesc = new RectangleF(r.X + offsetTextX + swBounds.Width, r.Y + textBounds.Height + 2, textDescBounds.Width + offsetWidth, textDescBounds.Height);

                ExtendedGraphics extendedGraphics = new ExtendedGraphics(g, e);

                var brush = GetMainBrush(rText);
                var brushDesc = GetMainBrush(rDesc);

                extendedGraphics.FillRoundRectangle(brush, rText.X, rText.Y, rText.Width, rText.Height, radius);
                extendedGraphics.DrawRoundRectangle(Pen, rText.X, rText.Y, rText.Width, rText.Height, radius);

                extendedGraphics.FillRoundRectangle(brushDesc, rDesc.X, rDesc.Y, rDesc.Width, rDesc.Height, radius);
                extendedGraphics.DrawRoundRectangle(Pen, rDesc.X, rDesc.Y, rDesc.Width, rDesc.Height, radius);

                rText = new RectangleF(rText.X + 4, rText.Y, rText.Width, rText.Height);
                rDesc = new RectangleF(rDesc.X + 4, rDesc.Y, rDesc.Width, rDesc.Height);

                var r1 = new RectangleF(rText.X - 1 + 0.5f, rText.Y + 0.5f , rText.Width, rText.Height);
                var r2 = new RectangleF(rText.X - 1, rText.Y , rText.Width, rText.Height);

                var r3 = new RectangleF(rDesc.X - 1 + 0.5f, rDesc.Y + 0.5f, rDesc.Width, rDesc.Height);
                var r4 = new RectangleF(rDesc.X - 1, rDesc.Y , rDesc.Width , rDesc.Height);

                g.DrawString(Title, FontTitle, Brushes.Black, r1, format);
                g.DrawString(Title, FontTitle, Brushes.White, r2, format);

                g.DrawString(Description, FontDescription, Brushes.Black, r3, format);
                g.DrawString(Description, FontDescription, Brushes.White, r4, format);
            }
        }

        public virtual Brush GetMainBrush(RectangleF rectangle)
        {
            var brush = new LinearGradientBrush(rectangle, Color.Black, Color.Black, 90, true);

            ColorBlend cb = new ColorBlend();

            cb.Positions = new[] { 0, 0.2f, 0.5f, 1 };
            cb.Colors = new Color[] { Color.FromArgb(200, 50, 50, 50), Color.FromArgb(200, 50, 50,50), Color.FromArgb(200, 0, 0, 0), Color.FromArgb(200, 30, 160, 255) };

            brush.InterpolationColors = cb;

            return brush;
        }
    }
}
