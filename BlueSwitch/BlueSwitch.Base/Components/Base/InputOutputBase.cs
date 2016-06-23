using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Components.UI;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Components.Base
{
    public class InputOutputBase : DrawableBase
    {
        public InputOutputBase(Signature signature)
        {
            this.Signature = signature;
        }

        public UIComponent UIComponent { get; set; }

        public bool IsConnected(Engine e)
        {
            return e.CurrentProject.IsConnected(Parent, this);
        }

        [JsonIgnore]
        public String DisplayName
        {
            get { return $"Index:{Index} Value:{Data}"; }
        }

        private int _index = 0;

        [JsonIgnore]
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        [JsonIgnore]
        public Signature Signature { get; set; }


        public DataContainer Data { get; set; }
        
        [JsonIgnore]
        public static Brush SignatureCheckBrush { get; set; } = new SolidBrush(Color.Red);

        [JsonIgnore]
        public static Brush MouseOverBrush { get; set; } = new SolidBrush(Color.LightGreen);

        [JsonIgnore]
        public static Pen PenWhite { get; set; } = new Pen(Color.DimGray, 1.5f);

        [JsonIgnore]
        public static Pen Pen { get; set; } = new Pen(Color.Black, 1.0f);

        [JsonIgnore]
        public static Pen PenWhiteSmall { get; set; } = new Pen(Color.DimGray, 1.2f);

        [JsonIgnore]
        public static Pen PenSmall { get; set; } = new Pen(Color.Black, 1.0f);

        [JsonIgnore]
        public static Brush Brush { get; set; } = new SolidBrush(Color.SkyBlue);

        [JsonIgnore]
        public bool SignatureCheckFailed { get; set; } = false;

        public SwitchBase Parent { get; protected set; }
        
        private float offsetRight = 1.5f;
        private float offsetLeft = 1.5f;

        private float offsetTop = 1.5f;
        private float offsetBottom = 1.5f;

        public override RectangleF DescriptionBounds
        {
            get
            {
                var r = Parent.DescriptionBounds;
                
                RectangleF rect = new RectangleF(
                    r.X + offsetLeft,
                    r.Y + (Index * Parent.RowHeight) + offsetTop,
                    r.Width - (offsetRight + offsetLeft),
                    Parent.RowHeight - (Parent.DescriptionOffsetBottom + Parent.DescriptionOffsetTop + offsetTop + offsetBottom) 
                    );

                return rect;
            }
        }

        public void Initialize(Engine e, SwitchBase parent)
        {
            this.Parent = parent;
            UIComponent?.Initialize(e, this);
        }
        
        public override void UpdateMouseService(RenderingEngine e)
        {
            base.UpdateMouseService(e);
            SignatureCheckFailed = false;
            if (IsMouseOver && e.SelectionService.StartDrag && e.MouseService.LeftMouseDown)
            {
                SignatureCheckFailed = !e.SelectionService.IsSignatureMatching(this);
            }
            UIComponent?.UpdateMouseService(e);
        }

        

        protected virtual void DrawInputOutput(Graphics g, RenderingEngine e, DrawableBase parent, InputOutputBase previous)
        {
            var r = Rectangle;
            
            if (Signature is ActionSignature)
            {
               DrawAction(g,e,parent,previous,r);
            }
            else
            {
                DrawData(g, e, parent, previous, r);
            }
        }

        protected void DrawAction(Graphics g, Engine e, DrawableBase parent, InputOutputBase previous, RectangleF r)
        {
            float marginOffset = 0.20f;

            // Arrow Polygon
            float marginHeight = r.Height * marginOffset;
            var poly = new PointF[]
               {
                    new PointF(r.Left, r.Top + marginHeight),
                    new PointF(r.Left + 4, r.Top + marginHeight),
                    new PointF(r.Right - r.Width*0.25f, r.Y + r.Height*0.5f),
                    new PointF(r.Left + 4, r.Bottom - marginHeight),
                    new PointF(r.Left, r.Bottom - marginHeight),
               };


            if (SignatureCheckFailed)
            {
                g.FillPolygon(SignatureCheckBrush, poly);
            }
            else if (IsMouseOver)
            {
                g.FillPolygon(MouseOverBrush, poly);
            }
            else
            {
                g.FillPolygon(Signature.Brush, poly);
            }

            Pen.LineJoin = LineJoin.Round;
            PenWhite.LineJoin = LineJoin.Round;

            g.DrawPolygon(PenWhite, poly);
            g.DrawPolygon(Pen, poly);
        }

        public void DrawData(Graphics g, RenderingEngine e, DrawableBase parent, InputOutputBase previous, RectangleF r)
        {
            //if (e.PerformanceMode == PerformanceMode.HighQuality)
            //{
                float leftOffset = 5.5f;
                var marginHeight = r.Height*0.35f;

                // Small Arrow Polygon
                var poly = new PointF[]
                {
                    new PointF(r.Left + r.Width - leftOffset, r.Top + marginHeight),
                    new PointF(r.Right + r.Width*0.24f - leftOffset, r.Y + r.Height*0.5f),
                    new PointF(r.Left + r.Width - leftOffset, r.Bottom - marginHeight),
                };

                if (SignatureCheckFailed)
                {
                    g.FillPolygon(SignatureCheckBrush, poly);
                }
                else if (IsMouseOver)
                {
                    g.FillPolygon(MouseOverBrush, poly);
                }
                else
                {
                    g.FillPolygon(Signature.Brush, poly);
                }

                PenSmall.LineJoin = LineJoin.Round;
                PenWhiteSmall.LineJoin = LineJoin.Round;

                g.DrawPolygon(PenWhiteSmall, poly);
                g.DrawPolygon(PenSmall, poly);
            //}

            // Small Circle
            var radius = r.Height * 0.40f;
            var marginHeightCircle = r.Height * 0.5f - radius * 0.5f;

            var circle = new RectangleF(r.X, r.Y + marginHeightCircle, radius, radius);

            if (SignatureCheckFailed)
            {
                g.FillEllipse(SignatureCheckBrush, circle);
            }
            else if (IsMouseOver)
            {
                g.FillEllipse(MouseOverBrush, circle);
            }
            else
            {
                g.FillEllipse(Signature.Brush, circle);
            }

            g.DrawEllipse(PenWhite, circle);
            g.DrawEllipse(Pen, circle);
        }

        public virtual void Draw(Graphics g, RenderingEngine e, DrawableBase parent, InputOutputBase previous)
        {
            if (UIComponent != null && (UIComponent.IsDescription || !IsConnected(e)))
            {
                UIComponent.Draw(g, e, this);
            }
        }

        public virtual PointF GetTranslation(DrawableBase parent)
        {
            return Position;
        }

        public virtual PointF GetTranslationCenter(DrawableBase parent)
        {
            return Position;
        }

        public override void Update(RenderingEngine e, DrawableBase parent, DrawableBase previous)
        {
            base.Update(e, parent, previous);
            if (UIComponent != null && !IsConnected(e))
            {
                UIComponent.Update(e, this, previous);
            }
        }

        public override void UpdateMouseDown(Engine e, DrawableBase parent, DrawableBase previous)
        {
            base.UpdateMouseDown(e, parent, previous);
            if (UIComponent != null && !IsConnected(e))
            {
                UIComponent.UpdateMouseDown(e, this, previous);
            }
        }

        public override void UpdateMouseMove(RenderingEngine e, DrawableBase parent, DrawableBase previous)
        {
            base.UpdateMouseMove(e, parent, previous);
            if (UIComponent != null && !IsConnected(e))
            {
                UIComponent.UpdateMouseMove(e, this, previous);
            }
        }

        public override void UpdateMouseUp(Engine e, DrawableBase parent, DrawableBase previous)
        {
            base.UpdateMouseUp(e, parent, previous);
            if (UIComponent != null && !IsConnected(e))
            {
                UIComponent.UpdateMouseUp(e, this, previous);
            }
        }
    }
}
