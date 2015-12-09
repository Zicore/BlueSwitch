using System;
using System.Collections.Generic;
using System.Drawing;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.UI;

namespace BlueSwitch.Base.Components.Types
{
    public class SignatureSingle : Signature
    {
        public SignatureSingle(Type type)
        {
            Type = type;
        }

        public String Name { get; set; } = "SignatureType";

        public Type Type { get; protected set; }

        private static readonly Dictionary<Type, Brush> _brushes = new Dictionary<Type, Brush>
        {
            { typeof(bool), Brushes.DarkOrange },
            { typeof(int), Brushes.Blue },
            { typeof(float), Brushes.Yellow },
            { typeof(double), Brushes.Green },
            { typeof(string), Brushes.Tomato },
             { typeof(object), Brushes.Gray },
        };

        private static readonly Brush _brush = Brushes.SkyBlue;

        private static readonly Dictionary<Type, Pen> _pens = new Dictionary<Type, Pen>
        {
            { typeof(bool), new Pen(Brushes.DarkOrange, 2) },
            { typeof(int), new Pen(Brushes.Blue, 2) },
            { typeof(float), new Pen(Brushes.Yellow, 2) },
            { typeof(double), new Pen(Brushes.Green, 2) },
            { typeof(string), new Pen(Brushes.Tomato, 2) },
            { typeof(object), new Pen(Brushes.Gray, 2) },
        };

        private static readonly Pen _pen = new Pen(Color.Black, 2.0f);


        public override Brush Brush
        {
            get
            {
                if (_brushes.ContainsKey(Type))
                {
                    return _brushes[Type];
                }
                return _brush;
            }
        }

        public override Pen Pen
        {
            get
            {
                if (_pens.ContainsKey(Type))
                {
                    return _pens[Type];
                }
                return _pen;
            }
        }

        public virtual bool Matches(SignatureSingle signature)
        {
            return Match(this, signature);
        }

        public override Type BaseType
        {
            get { return Type; }
        }

        public override bool Matches(Signature signatureType)
        {
            if (signatureType is SignatureSingle)
            {
                return Match(this, (SignatureSingle)signatureType);
            }

            if (signatureType is SignatureList)
            {
                return Match((SignatureList)signatureType, this);
            }

            return base.Matches(signatureType);
        }

        public static OutputBase CreateOutput(Type type)
        {
            return new OutputBase(new SignatureSingle(type));
        }

        public static InputBase CreateInput(Type type)
        {
            return new InputBase(new SignatureSingle(type));
        }
    }
}
