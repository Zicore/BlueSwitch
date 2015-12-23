using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.IO;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class VariableSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return GroupBase.Variable;
        }

        [JsonIgnore]
        public Variable Variable { get; protected set; }

        public String VariableKey { get; set; }

        protected override void OnInitialize(Engine renderingEngine)
        {
            AutoDiscoverDisabled = true;

            base.OnInitialize(renderingEngine);
            Variable = renderingEngine.CurrentProject.GetVariable(VariableKey);
        }
    }
}
