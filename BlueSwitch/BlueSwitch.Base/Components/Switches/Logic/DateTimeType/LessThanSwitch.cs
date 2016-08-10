using BlueSwitch.Base.Components.Switches.Base;
using System;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Logic.DateTimeType
{
    class LessThanSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine);

            AddInput(typeof(DateTime));
            AddInput(typeof(DateTime));
            AddOutput(typeof(bool));

            DisplayName = "DateTimeType.LessThan";
            UniqueName = "BlueSwitch.Base.Components.Switches.Logic.DateTimeType.DateTimeType.LessThan";
            Description = "Checks if DateTime(x) is less than DateTime(y).";
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.LogicDateTime;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var a = GetData(0);
            var b = GetData(1);

            if(a != null && b != null)
            {
                SetData(0, new DataContainer((DateTime)a.Value < (DateTime)b.Value));
            }

            base.OnProcessData<T>(p, node);
        }
    }
}
