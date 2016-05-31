using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using BlueSwitch.Base.IO;
using BlueSwitch.Base.Services;
using Newtonsoft.Json;
using XnaGeometry;

namespace BlueSwitch.Base.Components.Base
{
    public class Connection : DrawableBase
    {
        private InputOutputSelector _fromInputOutput;
        private InputOutputSelector _toInputOutput;

        public Connection()
        {

        }

        public Connection(InputOutputSelector fromInputOutput, InputOutputSelector toInputOutput)
        {
            FromInputOutput = fromInputOutput;
            ToInputOutput = toInputOutput;
        }

        public InputOutputSelector FromInputOutput
        {
            get { return _fromInputOutput; }
            set
            {
                _fromInputOutput = value;
            }
        }

        public InputOutputSelector ToInputOutput
        {
            get { return _toInputOutput; }
            set
            {
                _toInputOutput = value;
            }
        }

        public void UpdateConnection(BlueSwitchProject project)
        {
            var switchTo = project.ItemLookup[ToInputOutput.OriginId];
            var switchFrom = project.ItemLookup[FromInputOutput.OriginId];

            if (ToInputOutput.IsInput)
            {
                ToInputOutput.Origin = switchTo;
                ToInputOutput.InputOutput = switchTo.Inputs.FirstOrDefault(x => x.Index == ToInputOutput.InputOutputId);

                FromInputOutput.Origin = switchFrom;
                FromInputOutput.InputOutput = switchFrom.Outputs.FirstOrDefault(x => x.Index == FromInputOutput.InputOutputId);
            }
            else
            {
                ToInputOutput.Origin = switchTo;
                ToInputOutput.InputOutput = switchTo.Outputs.FirstOrDefault(x => x.Index == ToInputOutput.InputOutputId);

                FromInputOutput.Origin = switchFrom;
                FromInputOutput.InputOutput = switchFrom.Inputs.FirstOrDefault(x => x.Index == FromInputOutput.InputOutputId);
            }
        }

        // Graphics Calculation below

        const float offsetYH = 5.0f;
        const float offsetXH = 5.0f;

        const float offsetY = 10.0f;
        const float offsetX = 10.0f;

        [JsonIgnore]
        protected static Pen LinePen = new Pen(Color.FromArgb(200, 30, 30, 30), 4.0f) { LineJoin = LineJoin.Round, EndCap = LineCap.Round, StartCap = LineCap.Round };

        [JsonIgnore]
        protected static Pen LinePenHighlight = new Pen(Color.LightCoral, 2.0f) { LineJoin = LineJoin.Round, EndCap = LineCap.Round, StartCap = LineCap.Round };

        [JsonIgnore]
        protected static Pen LinePenSelected = new Pen(Color.LimeGreen, 2.0f) { LineJoin = LineJoin.Round, EndCap = LineCap.Round, StartCap = LineCap.Round };
        
        [JsonIgnore]
        public static Pen MouseOverPen { get; set; } = new Pen(Color.LightCoral, 2.0f);

        [JsonIgnore]
        public static Pen SelectedPen { get; set; } = new Pen(Color.LimeGreen, 2.0f);

        public static float CalculateExtensionX(PointF p1, PointF p2)
        {
            float ext = (Math.Max(p1.X, p2.X) - Math.Min(p1.X, p2.X)) * 0.85f;

            ext = Math.Min(ext, 100);
            ext = Math.Max(30, ext);
            return ext;
        }

        public static float CalculateExtensionY(PointF p1, PointF p2)
        {
            float ext = (Math.Max(p1.Y, p2.Y) - Math.Min(p1.Y, p2.Y)) * 0.25f;

            ext = Math.Min(ext, 20);
            ext = Math.Max(2, ext);
            return ext;
        }

        public static PointF CalculateB1(PointF p1, PointF p2, float extensionX, float extensionY)
        {
            return new PointF(p1.X - extensionX, p1.Y - extensionY);
        }

        public static PointF CalculateB2(PointF p1, PointF p2, float extensionX, float extensionY)
        {
            return new PointF(p2.X + extensionX, p2.Y + extensionY);
        }

        public void Draw(RenderingEngine e, Graphics g)
        {
            var p1 = FromInputOutput.InputOutput.GetTranslationCenter(FromInputOutput.Origin);
            var p2 = ToInputOutput.InputOutput.GetTranslationCenter(ToInputOutput.Origin);
            var pen = ToInputOutput.InputOutput.Signature.Pen;
            var p = CalculatePath(e, g, pen, p1, p2);
            IsMouseOver = UpdatePathMouseOver(e, p);
            Draw(e,g,pen,p,p1,p2,this);
        }

        public static void Draw(RenderingEngine e, Graphics g, Pen pen, PointF p1, PointF p2)
        {
            var p = CalculatePath(e, g, pen, p1, p2);
            Draw(e,g,pen, p, p1, p2);
        }

        public static void Draw(RenderingEngine e,Graphics g, Pen pen, GraphicsPath p, PointF p1, PointF p2, Connection c = null)
        {
            if (c != null && (c.IsMouseOver || c.IsSelected))
            {
                var r = CalculateSelectionBounds(e, g, p1, p2);
                if (c.IsMouseOver)
                {
                    g.DrawPath(LinePen, p);
                    g.DrawPath(LinePenHighlight, p);

                    g.DrawRectangle(MouseOverPen, r.X,r.Y,r.Width,r.Height);
                }
                else
                {
                    g.DrawPath(LinePen, p);
                    g.DrawPath(LinePenSelected, p);

                    g.DrawRectangle(SelectedPen, r.X, r.Y, r.Width, r.Height);
                }
            }
            else
            {
                g.DrawPath(LinePen, p);
                g.DrawPath(pen, p);
            }
        }

        public static GraphicsPath CalculatePath(RenderingEngine e, Graphics g, Pen pen, PointF p1, PointF p2)
        {
            var extX = CalculateExtensionX(p1, p2);
            var extY = CalculateExtensionY(p1, p2);
            var b1 = CalculateB1(p1, p2, extX, extY);
            var b2 = CalculateB2(p1, p2, extX, extY);

            GraphicsPath p = new GraphicsPath();
            p.AddBezier(p1, b1, b2, p2);
            return p;
        }

        public static RectangleF CalculateSelectionBounds(RenderingEngine e, Graphics g, PointF p1, PointF p2)
        {
            var x = Math.Min(p1.X, p2.X);
            var width = Math.Max(p1.X, p2.X) - x;

            var y = Math.Min(p1.Y, p2.Y);
            var height = Math.Max(p1.Y, p2.Y) - y;

            return new RectangleF(x - offsetXH,y - offsetYH,width + offsetX, height+ offsetY);
        }

        private static bool UpdatePathMouseOver(RenderingEngine e, GraphicsPath path)
        {
            GraphicsPath pHit = new GraphicsPath();
            pHit.AddBezier(path.PathPoints[0], path.PathPoints[1], path.PathPoints[2], path.PathPoints[3]);
            pHit.Widen(new Pen(Color.Black, 9));
            return pHit.IsVisible(e.TranslatedMousePosition);
        }
    }
}
