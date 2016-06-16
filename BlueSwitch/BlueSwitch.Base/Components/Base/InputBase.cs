using System.Drawing;
using BlueSwitch.Base.Components.Types;

namespace BlueSwitch.Base.Components.Base
{
    public class InputBase : InputOutputBase
    {
        public override SizeF Size
        {
            get { return new SizeF(14.0f,18.0f); }
        }
        
        public InputBase(Signature signature) : base(signature)
        {
        }

        public override void Draw(Graphics g, RenderingEngine e, DrawableBase parent, InputOutputBase previous)
        {
            var transform = g.Transform;

            g.TranslateTransform(Translation.X,Translation.Y);

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
            var descHeight = parent.InputOutputDescriptionHeight;
            return new PointF(parent.Position.X + Size.Width * 0.25f, descHeight + parent.Position.Y + Size.Height * Index);
        }

        public override PointF GetTranslationCenter(DrawableBase parent)
        {
            var descHeight = parent.InputOutputDescriptionHeight;
            return new PointF((parent.Position.X) + Size.Width * 0.5f, (descHeight + parent.Position.Y + Size.Height * Index) + Size.Height * 0.5f);
        }
    }
}
