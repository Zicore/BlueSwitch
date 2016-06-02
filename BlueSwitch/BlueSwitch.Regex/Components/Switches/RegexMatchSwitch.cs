using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Regex.Components.Switches
{
    public class RegexMatchSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return BlueSwitch.Regex.Base.Groups.Regex;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "BlueSwitch.RegexMatch.Base";
            DisplayName = "Regex Match";

            AddInput(typeof(string));
            AddInput(typeof(string));

            AddOutput(typeof(bool));
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var pattern = GetDataValueOrDefault<string>(0);
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);

            var input = GetDataValueOrDefault<string>(1);

            bool ismatch = regex.IsMatch(input);

            SetData(0,ismatch);

            base.OnProcessData(p, node);
        }
    }
}
