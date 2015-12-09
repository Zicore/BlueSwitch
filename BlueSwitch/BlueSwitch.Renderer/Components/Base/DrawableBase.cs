using System.Drawing;
using BlueSwitch.Base.Utils;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Components.Base
{
    public class DrawableBase
    {
        private PointF _position;
        private SizeF _size;
        
        [JsonIgnore]
        public RenderingEngine RenderingEngine { get; protected set; }

        [JsonIgnore]
        public bool IsSelected { get; set; }

        public PointF Position
        {
            get { return _position; }
            set { _position = value; }
        }

        [JsonIgnore]
        public float ColumnWidth { get; set; } = 100.0f;

        [JsonIgnore]
        public virtual SizeF Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public PointF Translation { get; set; }

        [JsonIgnore]
        public bool IsMouseOver { get; set; } = false;

        [JsonIgnore]
        public RectangleF Rectangle
        {
            get
            {
                return new RectangleF(Position,Size);
            }
        }

        public virtual float DescriptionOffsetTop { get; } = 4;
        public virtual float DescriptionOffsetLeft { get; } = 18;
        public virtual float DescriptionOffsetRight { get; } = 18;
        public virtual float DescriptionOffsetBottom { get; } = 4;

        [JsonIgnore]
        public virtual RectangleF DescriptionBounds
        {
            get
            {
                var rect = Rectangle;

                var r = new RectangleF(rect.X + DescriptionOffsetLeft, rect.Y + DescriptionHeight + DescriptionOffsetTop, rect.Width - DescriptionOffsetRight - DescriptionOffsetLeft, rect.Height - DescriptionHeight - DescriptionOffsetBottom  - DescriptionOffsetTop);
                return r;
            }
        }

        [JsonIgnore]
        public float DescriptionHeight { get; set; } = 16;

        [JsonIgnore]
        public PointF Center
        {
            get { return Position + MathUtils.GetRectangleCenter(Size); }
            set { this.Position = new PointF(value.X, value.Y) - MathUtils.GetRectangleCenter(Size); }
        }

        public void DrawRectangleF(Graphics g, Pen pen, RectangleF r)
        {
            g.DrawRectangle(pen,r.X,r.Y,r.Width,r.Height);
        }
        
        public virtual RectangleF GetTranslatedRectangle(PointF translation)
        {
            return new RectangleF(translation.X + Position.X, translation.Y + Position.Y, Size.Width , Size.Height);
        }

        public virtual void Update(RenderingEngine e, DrawableBase parent, DrawableBase previous)
        {
            Translation = new PointF(Position.X,Position.Y);
        }

        public virtual void UpdateMouseService(RenderingEngine e)
        {
            IsMouseOver = e.MouseService.IsOver(this, Translation);
        }

        public virtual void UpdateMouseMove(RenderingEngine e, DrawableBase parent, DrawableBase previous)
        {
            
        }

        public virtual void UpdateMouseDown(RenderingEngine e, DrawableBase parent, DrawableBase previous)
        {

        }
        
        public virtual void UpdateMouseUp(RenderingEngine e, DrawableBase parent, DrawableBase previous)
        {

        }

        public virtual void Draw(Graphics g, RenderingEngine e, DrawableBase parent)
        {

        }
    }
}
