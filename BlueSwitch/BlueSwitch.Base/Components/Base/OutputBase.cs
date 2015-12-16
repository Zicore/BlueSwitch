using System.Drawing;
using BlueSwitch.Base.Components.Types;

namespace BlueSwitch.Base.Components.Base
{
    public class OutputBase : InputOutputBase
    {
        public override SizeF Size
        {
            get { return new SizeF(14.0f, 18.0f); }
        }

        public virtual SizeF SizeMargin
        {
            get { return new SizeF(Size.Width + Size.Width - 0.1f, Size.Height + Size.Height - 0.1f); }
        }

        public OutputBase(Signature signature) 
            : base(signature)
        {
        }

        public override void Draw(Graphics g, RenderingEngine e, DrawableBase parent, InputOutputBase previous)
        {
            var transform = g.Transform;
            
            g.TranslateTransform(Translation.X, Translation.Y);
            
            DrawInputOutput(g,e,parent,previous);

            g.Transform = transform;

            base.Draw(g, e, parent, previous);
        }

        public override void Update(RenderingEngine e, DrawableBase parent, DrawableBase previous)
        {
            base.Update(e, parent, previous);
            Translation = GetTranslation(parent);
        }

        public override PointF GetTranslation(DrawableBase parent)
        {
            return new PointF( parent.Position.X + parent.Size.Width - Size.Width, parent.DescriptionHeight + parent.Position.Y + Size.Height * Index);
        }

        public override PointF GetTranslationCenter(DrawableBase parent)
        {
            return new PointF((parent.Position.X + parent.Size.Width - Size.Width) + Size.Width * 0.5f, (parent.DescriptionHeight + parent.Position.Y + Size.Height * Index) + Size.Height * 0.5f);
        }
    }
}
