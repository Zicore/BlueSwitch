using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.SwitchMath
{
    public class AddSwitch : SwitchBase
    {
        protected override void OnInitialize(RenderingEngine engine)
        {
            AddInput(typeof(double));
            AddInput(typeof(double));
            AddInput(typeof(double));

            AddInput(typeof(double));
            AddInput(typeof(double));
            AddInput(typeof(double));

            AddInput(typeof(double));
            AddInput(typeof(double));
            AddInput(typeof(double));

            AddOutput(typeof(double));
            Name = "Add";
            Description = "Add";
        }

        public override GroupBase OnSetGroup()
        {
            return GroupBase.Math;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            double sum = 0;
            for (int i = 0; i < Inputs.Count; i++)
            {
                sum += GetDataValueOrDefault<double>(i);
            }

            SetData(0, new DataContainer(sum));
        }
    }
}
