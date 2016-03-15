using System;
using System.Drawing;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Components.UI
{
    public enum UIType
    {
        None,
        TextEdit,
        CheckBox,
        Button,
    }
    
    public abstract class UIComponent : DrawableBase
    {
        [JsonIgnore]
        public bool HasFocus { get; set; }

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

        private bool _isInitialized;

        public void Initialize(Engine renderingEngine, DrawableBase parent)
        {
            if (!_isInitialized)
            {
                Parent = parent;
                DescriptionHeight = 0;
                if (parent != null)
                {
                    ParentWidth = parent.ColumnWidth;
                }
                RenderingEngine = renderingEngine;
                OnInitialize(renderingEngine, parent);
                if (AutoStoreValue)
                {
                    LoadData();
                }
                _isInitialized = true;
            }
        }

        protected virtual void OnInitialize(Engine renderingEngine, DrawableBase parent)
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
                if (Parent == null)
                {
                    return new RectangleF(Position, Size);
                }

                var r = Parent.DescriptionBounds;
                return r;
            }
        }

        public abstract object GetData();
        public abstract void SaveData();
        public abstract void LoadData();
    }
}
