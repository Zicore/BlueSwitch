﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.IO;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class GetterSwitch : VariableSwitch
    {
        public override GroupBase OnSetGroup()
        {
            return GroupBase.Setter;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine);

            AutoDiscoverDisabled = true;

            if (Variable == null)
            {
                AddOutput(typeof(object));
            }
            else
            {
                AddOutput(Variable.NetValueType);
            }

            if (Variable != null)
            {
                Name = $"Get ({Variable.Name})";
            }
            else
            {
                Name = "Get";
            }
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            base.OnProcessData(p, node);
            SetData(0,new DataContainer(Variable.Value));
        }
    }
}
