using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using BlueSwitch.Base.IO;
using BlueSwitch.Base.Services;
using XnaGeometry;

namespace BlueSwitch.Base.Components.Base
{
    public class Connection
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

        protected static Pen LinePen = new Pen(Color.FromArgb(200, 30, 30, 30), 4.0f) { LineJoin = LineJoin.Round, EndCap = LineCap.Round, StartCap = LineCap.Round };
        protected static Pen LinePenHighlight = new Pen(Color.FromArgb(200, 80, 80, 80), 4.0f) { LineJoin = LineJoin.Round, EndCap = LineCap.Round, StartCap = LineCap.Round };

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

            var extX = CalculateExtensionX(p1, p2);
            var extY = CalculateExtensionY(p1, p2);
            var b1 = CalculateB1(p1, p2, extX, extY);
            var b2 = CalculateB2(p1, p2, extX, extY);
            var pen = ToInputOutput.InputOutput.Signature.Pen;

            GraphicsPath p = new GraphicsPath();
            p.AddBezier(p1, b1, b2, p2);
            GraphicsPath pHit = new GraphicsPath();
            pHit.AddBezier(p1, b1, b2, p2);
            pHit.Widen(new Pen(Color.Black, 8));

            if (pHit.IsVisible(e.TranslatedMousePosition))
            {
                g.DrawPath(LinePen, p);
                g.DrawPath(Pens.Lime, p);
            }
            else
            {
                g.DrawPath(LinePen, p);
                g.DrawPath(pen, p);
            }
        }

        public static void DrawRaw(Connection c,RenderingEngine e, Graphics g, PointF p2)
        {
            
        }
    }
}
