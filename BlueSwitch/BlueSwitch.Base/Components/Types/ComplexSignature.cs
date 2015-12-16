using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Components.Types
{
    public class ComplexSignature : Signature
    {
        private static readonly Brush _brush = new SolidBrush(Color.LightBlue);
        private static readonly Pen _pen = new Pen(Color.LightBlue, 2);

        [JsonIgnore]
        public override Pen Pen => _pen;

        [JsonIgnore]
        public override Brush Brush => _brush;

        public ComplexSignature()
        {

        }

        public override bool Matches(Signature signature)
        {
            return signature is ComplexSignature;
        }
    }
}
