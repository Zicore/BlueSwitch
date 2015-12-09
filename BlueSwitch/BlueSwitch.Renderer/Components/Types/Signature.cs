using System;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Components.Types
{
    public abstract class Signature
    {
        private static Brush _brush = new SolidBrush(Color.SkyBlue);
        private static Pen _pen = new Pen(Color.Black, 2.0f);

        public virtual bool Matches(Signature signatureType)
        {
            return signatureType.GetType() == this.GetType();
        }

        [JsonIgnore]
        public virtual Pen Pen => _pen;

        [JsonIgnore]
        public virtual Brush Brush => _brush;

        //public static bool Match(Signature signature1, Signature signature2)
        //{
        //    if (signature1 is SignatureSingle)
        //    {
        //        return Match(signature1,(SignatureSingle)signature2);
        //    }

        //    if (signature1 is SignatureList)
        //    {
        //        return Match(signature1,(SignatureList)signature2);
        //    }

        //    return signature1.Matches(signature2);
        //}

        public virtual Type BaseType
        {
            get { return null; }
        }
        
        public static bool Match(SignatureList list1, SignatureList list2)
        {
            if (list1.Types.Any(type => list2.Types.Any(type2 => type == type2)))
            {
                return true;
            }
            return false;
        }

        public static bool Match(SignatureList list, SignatureSingle single)
        {
            return list.Types.Any(x => x == single.Type);
        }

        public static bool Match(SignatureSingle single1, SignatureSingle single2)
        {
            return single1.Type == single2.Type;
        }
    }
}
