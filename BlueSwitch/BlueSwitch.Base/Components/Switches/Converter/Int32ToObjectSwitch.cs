﻿using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Converter
{
    public class Int32ToObjectSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.Converter;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "BlueSwitch.Base.Components.Switches.Converter.Int32.ToObject";
            DisplayName = "Int32->Object";
            Description = "Converts an int32-element into an object.";
            AddInput(typeof (int));
            AddOutput(typeof (object));
            IsCompact = true;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var data = GetData(0);
            SetData(0, new DataContainer(data?.Value));
        }
    }
}
