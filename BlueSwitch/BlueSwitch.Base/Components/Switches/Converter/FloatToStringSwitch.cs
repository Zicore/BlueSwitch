﻿using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Converter
{
    public class FloatToStringSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return GroupBase.Converter;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            Name = "Float.ToString";
            AddInput(typeof (float));
            AddOutput(typeof (string));
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var data = GetData(0);
            SetData(0, new DataContainer(data?.Value?.ToString()));
        }
    }
}
