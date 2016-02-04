using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Diagnostics
{
    public class ExceptionEntry
    {
        public Exception Exception { get; set; }
        public ProcessingTree<SwitchBase> Tree { get; set; }
        public ProcessingNode<SwitchBase> Node { get; set; }
        public int Step { get; set; }

        public override string ToString()
        {
            return $"Exception: {Exception}, Tree: {Tree}, Node: {Node}, Step: {Step}";
        }
    }
}
