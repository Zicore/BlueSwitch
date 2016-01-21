using System;
using System.Text;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Text
{
    public class RandomTextSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            //ActivateInputAdd(new PinDescription(typeof(string), UIType.TextEdit) {}, 2);
            AddInput(typeof (string), new TextEdit());
            AddInput(typeof (int), TextEdit.CreateNumeric(false));

            AddOutput(typeof(string));
            Name = "Random String";
            Description = "Random String";
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.Text;
        }

        private static Random random = new Random();

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var characters = GetDataValueOrDefault<int>(1);
            var charset = GetDataValueOrDefault<string>(0);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < characters; i++)
            {
                var index = random.Next(0, charset.Length);
                sb.Append(charset[index]);
            }

            SetData(0, new DataContainer(sb.ToString()));
        }
    }
}
