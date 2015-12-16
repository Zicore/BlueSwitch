using System;

namespace BlueSwitch.Base.Components.Base
{
    public class DataContainer
    {
        public DataContainer()
        {
            
        }

        public DataContainer(Object value)
        {
            this.Value = value;
        }

        public Object Value { get; set; }

        public override string ToString()
        {
            return $"{Value}";
        }
    }
}
