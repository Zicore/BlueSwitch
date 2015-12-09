using System;
using System.Drawing;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Components.UI
{
    public abstract class UIComponent : DrawableBase
    {
        [JsonIgnore]
        protected float ParentWidth = 0;

        [JsonIgnore]
        public bool ReadOnly { get; set; }

        [JsonIgnore]
        public bool Enabled { get; set; } = true;

        [JsonIgnore]
        public bool IsDescription { get; set; } = false;

        [JsonIgnore]
        public bool AutoStoreValue { get; set; } = true;

        [JsonIgnore]
        public DrawableBase Parent { get; protected set; }

        public void Initialize(RenderingEngine engine, DrawableBase parent)
        {
            Parent = parent;
            DescriptionHeight = 0;
            ParentWidth = parent.ColumnWidth;
            RenderingEngine = engine;
            OnInitialize(engine, parent);
            if (AutoStoreValue)
            {
                LoadData();
            }
        }

        protected virtual void OnInitialize(RenderingEngine engine, DrawableBase parent)
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
            Size = new SizeF(r.Width,r.Height);
        }

        public override RectangleF DescriptionBounds
        {
            get
            {
                var r = Parent.DescriptionBounds;

                return r;
            }
        }

        public abstract object GetData();
        public abstract void SaveData();
        public abstract void LoadData();
    }
}
