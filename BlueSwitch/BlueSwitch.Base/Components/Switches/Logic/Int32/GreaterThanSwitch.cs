﻿using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Logic.Int32
{
    public class GreaterThanSwitchSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            AddInput(typeof(int), new TextEdit { NumberMode = true, AllowDecimalPoint = false });
            AddInput(typeof(int), new TextEdit { NumberMode = true, AllowDecimalPoint = false });
            AddOutput(typeof(bool));

            UniqueName = "BlueSwitch.Base.Components.Switches.Logic.Int32.Int32.GreaterThan";
            DisplayName = "Int32.GreaterThan";
            Description = "Checks if Int32(x) is greater than Int32(y)";
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.LogicInt32;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var a = GetDataValueOrDefault<int>(0);
            var b = GetDataValueOrDefault<int>(1);

            SetData(0, new DataContainer(a > b));

            base.OnProcess(p, node);
        }
    }
}
