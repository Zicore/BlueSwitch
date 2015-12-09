using System;
using System.Collections.Generic;

namespace BlueSwitch.Base.Components.Types
{
    public class SignatureList : Signature
    {
        public SignatureList(Type[] types)
        {
            Types.AddRange(types);
        }

        public String Name { get; set; } = "SignatureType";

        public List<Type> Types { get; protected set; } = new List<Type>();

        public override Type BaseType {
            get
            {
                if (Types.Count > 0)
                {
                    return Types[0];
                }
                return null;
            }
            
        }

        public override bool Matches(Signature signatureType)
        {
            if (signatureType is SignatureSingle)
            {
                return Match(this,(SignatureSingle) signatureType);
            }

            if (signatureType is SignatureList)
            {
                return Match(this,(SignatureList) signatureType);
            }

            return base.Matches(signatureType);
        }
    }
}
