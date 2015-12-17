using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSwitch.Base.Diagnostics
{
    public class ExceptionEntryEventArgs : EventArgs
    {
        public ExceptionEntry ExceptionEntry { get; set; }
    }
}
