using System.Drawing;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Components.Types
{
    public class ActionSignature : Signature
    {
        private static readonly Brush _brush = new SolidBrush(Color.White);
        private static readonly Pen _pen = new Pen(Color.White, 2);

        [JsonIgnore]
        public override Pen Pen => _pen;

        [JsonIgnore]
        public override Brush Brush => _brush;

        public ActionSignature()
        {
            
        }

        public override bool Matches(Signature signature)
        {
            return signature is ActionSignature;
        }
    }
}
