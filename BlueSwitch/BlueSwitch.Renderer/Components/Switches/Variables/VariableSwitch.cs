using System;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Variables
{
    public abstract class VariableSwitch : SwitchBase
    {
        protected override void OnInitialize(RenderingEngine engine)
        {
            Name = "Variable";

            TextEdit = new TextEdit();
            TextEdit.AutoResize = true;
            //TextEdit.TextChanged += TextEditOnTextChanged;
            //TextEdit.Text = ValueStore.GetOrDefault<string>("Value");

            Components.Add(TextEdit);
        }

        //private void TextEditOnTextChanged(object sender, EventArgs eventArgs)
        //{
        //    ValueStore.Store("Value", TextEdit.Text);
        //}

        protected TextEdit TextEdit;

        public override GroupBase OnSetGroup()
        {
            return GroupBase.Variable;
        }
        
        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            //if (Value != null)
            //{
            //    SetData(0, new DataContainer(Value));
            //}
        }
    }
}
