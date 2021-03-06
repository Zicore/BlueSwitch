﻿using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Converter
{
    public class ObjectToStringSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.Converter;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "BlueSwitch.Base.Components.Switches.Converter.Object.ToString";
            DisplayName = "Object->String";
            Description = "Converts an object into a string.";
           
            AddInput(typeof(object));
            AddOutput(typeof(string));
            IsCompact = true;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var data = GetData(0);
            SetData(0, new DataContainer(data?.Value.ToString()));
        }
    }
}
