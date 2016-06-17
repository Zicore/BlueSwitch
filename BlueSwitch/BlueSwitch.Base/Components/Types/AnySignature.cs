using System;
using System.Drawing;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Components.Types
{
    public class AnySignature : Signature
    {
        private static readonly Brush _brush = new SolidBrush(Color.DimGray);
        private static readonly Pen _pen = new Pen(Color.DimGray, 2);

        [JsonIgnore]
        public override Pen Pen => _pen;

        [JsonIgnore]
        public override Brush Brush => _brush;

        public AnySignature()
        {
            
        }

        public override bool Matches(Signature signature)
        {
            return true;
        }
    }
}
