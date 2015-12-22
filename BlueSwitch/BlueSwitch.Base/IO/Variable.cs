using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Float,
        Double,
    }


    public class Variable
    {
        public String Name { get; set; }
        public ValueType ValueType { get; set; }
        public StructureType StructureType { get; set; }
        public object Value { get; set; }

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
                case ValueType.Float:
                    return typeof(float);
                case ValueType.Double:
                    return typeof(double);
                default:
                    return typeof (object);
            }
        }

        public Type NetValueType
        {
            get { return GetTypeByIndex(ValueType);  }
        }

    }
}
