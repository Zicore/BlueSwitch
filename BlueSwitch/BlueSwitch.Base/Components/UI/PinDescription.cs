using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Types;

namespace BlueSwitch.Base.Components.UI
{
    public class PinDescription
    {
        public Signature Signature { get; set; }
        public Type ComponentType { get; set; }
        public UIType UiType { get; set; }

        public bool IsNumeric { get; set; }
        public bool IsDecimalNumber { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsAutoResize { get; set; }
        public bool IsAutoStoreValue { get; set; }

        public PinDescription(Type componentType, UIType uiType)
        {
            ComponentType = componentType;
            UiType = uiType;
        }

        public PinDescription(Signature signature, UIType uiType)
        {
            Signature = signature;
            UiType = uiType;
        }

        public UIComponent CreateComponent()
        {
            UIComponent component = null;
            switch (UiType)
            {
                case UIType.None:
                    component = null;
                    break;
                case UIType.TextEdit:
                    component = new TextEdit { ReadOnly = IsReadOnly,AllowDecimalPoint = IsDecimalNumber, NumberMode = IsNumeric,AutoResize = IsAutoResize};
                    break;
                case UIType.CheckBox:
                    component = new CheckBox { ReadOnly = IsReadOnly, AutoResize = IsAutoResize };
                    break;
                case UIType.Button:
                    component = new Button { ReadOnly = IsReadOnly, AutoResize = IsAutoResize };
                    break;
                default:
                    component = null;
                    break;
            }
            
            return component;
        }
    }
}
