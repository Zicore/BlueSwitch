using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.IO
{
    public class ValueStoreSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine);
            UniqueName = "BlueSwitch.Base.Components.Switches.IO.ValueStore";
            DisplayName = "ValueStore";
            Description = "ValueStore";

            AddInput(new ActionSignature());
            AddOutput(new ActionSignature());

            AddInput(new SignatureList(new Type[] { typeof(float), typeof(string), typeof(double), typeof(int) }));
            AddOutput(new SignatureList(new Type[] { typeof(float), typeof(string), typeof(double), typeof(int) }));

            AddInput(typeof (string));
            AddOutput(typeof (bool));
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.IO;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var keyData = GetData(2);
            string key = (string) keyData?.Value;
            bool exists = false;
            if (!String.IsNullOrEmpty(key))
            {
                exists = ValueStore.Values.ContainsKey(key);
            }
            SetData(2, new DataContainer(exists));
            

            base.OnProcessData(p, node);
        }
    }
}
