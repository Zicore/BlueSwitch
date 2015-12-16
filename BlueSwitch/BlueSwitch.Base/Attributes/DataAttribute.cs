using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSwitch.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class DataProperty : Attribute
    {
        public string Name = "";
        public int Index = 0;
    }
}
