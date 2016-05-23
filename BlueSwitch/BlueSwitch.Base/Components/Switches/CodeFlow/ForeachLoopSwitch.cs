using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.CodeFlow
{
    public class ForeachLoopSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "Foreach";

            AddInput(new ActionSignature());
            AddInput(typeof(IEnumerable));

            AddOutput(new ActionSignature());
            AddOutput(new AnySignature());
            AddOutput(new ActionSignature());
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.CodeFlow;
        }

        private IEnumerable _currentEnumerable;
        private IEnumerator _currentEnumerator;

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            if (_currentEnumerable == null)
            {
                var data = GetData(1);
                if (data?.Value is IEnumerable)
                {
                    _currentEnumerable = (IEnumerable) data.Value;
                    _currentEnumerator = _currentEnumerable.GetEnumerator();
                }
            }
            
            if(_currentEnumerable != null)
            {
                if (_currentEnumerator.MoveNext())
                {
                    SetData(1, new DataContainer(_currentEnumerator.Current));
                    node.Repeat = true;
                    node.Skip = new SkipNode(2);
                }
                else
                {
                    node.Skip = new SkipNode(0);
                    
                    _currentEnumerable = null;
                    _currentEnumerator = null;
                }
            }

            base.OnProcess(p, node);
        }
    }
}
