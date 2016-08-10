﻿using System;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Converter
{
    public class DateTimeToStringSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.Converter;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "BlueSwitch.Base.Components.Switches.Converter.DateTime.ToString";
            DisplayName = "DateTime->String";
            Description = "Converts a DateTime-element into a string.";
            AddInput(typeof (DateTime));
            AddInput(typeof(string));
            AddOutput(typeof (string));
            IsCompact = true;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var data = GetDataValueOrDefault<DateTime>(0);

            var format = GetDataValueOrDefault<string>(1);

            if (!string.IsNullOrEmpty(format))
            {
                SetData(0, new DataContainer(data.ToString(format)));
            }
            else
            {
                SetData(0, new DataContainer(data));
            }
        }
    }
}
