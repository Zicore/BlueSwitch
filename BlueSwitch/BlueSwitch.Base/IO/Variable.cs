using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.UI;

namespace BlueSwitch.Base.IO
{
    public enum StructureType
    {
        None,
        Scalar,
        Array
    }

    public enum ValueType
    {
        Bool,
        Byte,
        Int,
        Long,
        Float,
        Double,
        String
    }


    public class Variable
    {
        public String Name { get; set; }
        public ValueType ValueType { get; set; }
        public StructureType StructureType { get; set; }
        public object Value { get; set; }
        public bool IsReadOnly { get; set; }

        public static Type GetTypeByIndex(ValueType type)
        {
            switch (type)
            {
                case ValueType.Bool:
                    return typeof(bool);
                case ValueType.Byte:
                    return typeof(byte);
                case ValueType.Int:
                    return typeof(int);
                case ValueType.Long:
                    return typeof(long);
                case ValueType.Float:
                    return typeof(float);
                case ValueType.Double:
                    return typeof(double);
                case ValueType.String:
                    return typeof(string);
                default:
                    return typeof (object);
            }
        }

        public UIComponent CreateComponent()
        {
            UIComponent component = null;

            switch (ValueType)
            {
                case ValueType.Bool:
                    component = new CheckBox { ReadOnly = IsReadOnly, AutoStoreValue = true };
                    break;
                case ValueType.Byte:
                    component = new TextEdit { ReadOnly = IsReadOnly, AllowDecimalPoint = false, NumberMode = true,  AutoStoreValue = true };
                    break;
                case ValueType.Int:
                    component = new TextEdit { ReadOnly = IsReadOnly, AllowDecimalPoint = false, NumberMode = true, AutoStoreValue = true };
                    break;
                case ValueType.Long:
                    component = new TextEdit { ReadOnly = IsReadOnly, AllowDecimalPoint = false, NumberMode = true, AutoStoreValue = true };
                    break;
                case ValueType.Float:
                    component = new TextEdit { ReadOnly = IsReadOnly, AllowDecimalPoint = true, NumberMode = true, AutoStoreValue = true };
                    break;
                case ValueType.Double:
                    component = new TextEdit { ReadOnly = IsReadOnly, AllowDecimalPoint = true, NumberMode = true, AutoStoreValue = true };
                    break;
                case ValueType.String:
                    component = new TextEdit { ReadOnly = IsReadOnly, NumberMode = false,AutoStoreValue = true };
                    break;
                default:
                    component = null;
                    break;
            }

            return component;
        }

        public Type NetValueType
        {
            get { return GetTypeByIndex(ValueType);  }
        }

    }
}
