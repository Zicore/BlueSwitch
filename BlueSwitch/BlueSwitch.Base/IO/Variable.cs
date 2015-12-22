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

    public class Variable
    {
        public String Name { get; set; }
        public Type ValueType { get; set; }
        public StructureType StructureType { get; set; }
        public object Value { get; set; } 
    }
}
