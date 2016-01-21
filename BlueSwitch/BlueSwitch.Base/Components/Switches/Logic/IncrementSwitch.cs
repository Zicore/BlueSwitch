using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Logic
{
    public class IncrementSwitch : SwitchBase
    {
        public IncrementSwitch()
        {

        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            AddInput(new ActionSignature(), edit);
            AddInput(typeof(int), new TextEdit { NumberMode = true, AllowDecimalPoint = false });
            AddInput(typeof(bool), new CheckBox());
            AddOutput(new ActionSignature());
            AddOutput(typeof(int));
            Name = "Increment";
            edit.Text = Value.ToString();
        }

        private TextEdit edit = new TextEdit { ReadOnly = true, IsDescription = true, AutoStoreValue = false};

        public override GroupBase OnSetGroup()
        {
            return Groups.Logic;
        }

        private int _value;

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            SetData(1, new DataContainer(Value));

            base.OnProcess(p, node);
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var number = GetDataValueOrDefault<int>(1);

            Value += number;

            var reset = GetDataValueOrDefault<bool>(2);

            if (reset)
            {
                Value = 0;
            }

            edit.Text = Value.ToString();

            SetData(1, new DataContainer(Value));

            base.OnProcessData(p, node);
        }
    }
}
