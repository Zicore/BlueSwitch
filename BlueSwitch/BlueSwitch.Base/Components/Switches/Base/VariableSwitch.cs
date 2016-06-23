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
            return Groups.Variable;
        }

        [JsonIgnore]
        public Variable Variable { get; protected set; }

        public String VariableKey { get; set; }

        [JsonIgnore]
        public String NameSuffix { get; set; }

        [JsonIgnore]
        public String NamePrefix { get; set; }

        public virtual void UpdateVariable(Variable variable)
        {
            if (Variable != null)
            {
                NameSuffix = $"({Variable.Name})";
            }
            else
            {
                NameSuffix = "";
            }

            UniqueName = $"{NamePrefix} {NameSuffix}";

            if (variable != null)
            {
                VariableKey = variable.Name;
            }
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            AutoDiscoverDisabled = true;
            IsCompact = true;
            //ColumnWidth = 120;
            base.OnInitialize(renderingEngine);
            Variable = renderingEngine.CurrentProject.GetVariable(VariableKey);
        }
    }
}
