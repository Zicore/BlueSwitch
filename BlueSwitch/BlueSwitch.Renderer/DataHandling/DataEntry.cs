using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlueSwitch.Base.Attributes
{
    public class DataEntry
    {
        public Type DataType { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
    }
}
