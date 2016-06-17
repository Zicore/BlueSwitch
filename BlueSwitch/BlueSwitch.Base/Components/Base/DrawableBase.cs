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
        public bool IsCompact { get; set; }

        [JsonIgnore]
        public Engine RenderingEngine { get; protected set; }

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
        [JsonIgnore]
        public virtual float DescriptionOffsetTop { get; set; } = 2;

        [JsonIgnore]
        public virtual float DescriptionOffsetLeft { get; set; } = 16;

        [JsonIgnore]
        public virtual float DescriptionOffsetRight { get; set; } = 16;

        [JsonIgnore]
        public virtual float DescriptionOffsetBottom { get; set; } = 2;
        
        [JsonIgnore]
        public virtual float DescriptionOffsetLeftCompact
        {
            get { return IsCompact ? DescriptionOffsetLeft : 2; }
        }

        [JsonIgnore]
        public virtual float DescriptionOffsetRightCompact
        {
            get { return IsCompact ? DescriptionOffsetRight : 2; }
        }

        [JsonIgnore]
        public virtual RectangleF DescriptionBounds
        {
            get
            {
                var rect = Rectangle;

                var r = new RectangleF(rect.X + DescriptionOffsetLeft, rect.Y + InputOutputDescriptionHeight + DescriptionOffsetTop, rect.Width - DescriptionOffsetRight - DescriptionOffsetLeft, rect.Height - InputOutputDescriptionHeight - DescriptionOffsetBottom  - DescriptionOffsetTop);
                return r;
            }
        }

        public virtual SizeF SizeUntilExtraRow
        {
            get { return Size; }
        }

        [JsonIgnore]
        public float DescriptionHeight { get; set; } = 16;

        [JsonIgnore]
        public virtual float InputOutputDescriptionHeight
        {
            get
            {
                float descriptionHeight = DescriptionHeight;
                if (IsCompact)
                {
                    descriptionHeight = -1;
                }
                return descriptionHeight;
            }
        }

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

        public virtual void UpdateMouseDown(Engine e, DrawableBase parent, DrawableBase previous)
        {

        }
        
        public virtual void UpdateMouseUp(Engine e, DrawableBase parent, DrawableBase previous)
        {

        }

        public virtual void Draw(Graphics g, RenderingEngine e, DrawableBase parent)
        {

        }
    }
}
