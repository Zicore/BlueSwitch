using System.Linq;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;

namespace BlueSwitch.Base.Components.Switches.Meta
{
    public class PrefabSwitch : SwitchBase
    {
        public Prefab Prefab { get; set; } = new Prefab();

        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine);
            AutoDiscoverDisabled = true;

            LoadInputOutputs();
        }

        private void LoadPrefabDetails()
        {
            DisplayName = Prefab.Name;
            Description = Prefab.Description;
        }
        
        private void LoadInputOutputs()
        {
            var outputDefinition = Prefab.Items.FirstOrDefault(x => x is OutputDefinitionSwitch);

            if (outputDefinition != null)
            {
                foreach (var item in outputDefinition.Inputs)
                {
                    AddOutput(item.Signature, item.UIComponent);
                }
            }

            var inputDefinition = Prefab.Items.FirstOrDefault(x => x is InputDefinitionSwitch);

            if (inputDefinition != null)
            {
                foreach (var item in inputDefinition.Outputs)
                {
                    AddOutput(item.Signature, item.UIComponent);
                }
            }
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.Meta;
        }
    }
}
