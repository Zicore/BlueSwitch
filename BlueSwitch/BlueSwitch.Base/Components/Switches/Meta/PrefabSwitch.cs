using System.Linq;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Components.Switches.Meta
{
    public class PrefabSwitch : SwitchBase
    {
        [JsonIgnore]
        public Prefab Prefab { get; set; }

        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine);
            AutoDiscoverDisabled = true;

            LoadInputOutputs();
            LoadPrefabDetails();

            UniqueName = "BlueSwitch.Base.Components.Switches.Meta.Prefab";
        }

        private void LoadPrefabDetails()
        {
            if (Prefab != null)
            {
                DisplayName = Prefab.Name;
                Description = Prefab.Description;
            }
        }
        
        private void LoadInputOutputs()
        {
            if (Prefab != null)
            {
                var outputDefinition = Prefab.Project.Items.FirstOrDefault(x => x is OutputDefinitionSwitch);

                if (outputDefinition != null)
                {
                    outputDefinition.Initialize(RenderingEngine);
                    foreach (var item in outputDefinition.Inputs)
                    {
                        AddOutput(item.Signature, item.UIComponent);
                    }
                }

                var inputDefinition = Prefab.Project.Items.FirstOrDefault(x => x is InputDefinitionSwitch);

                if (inputDefinition != null)
                {
                    inputDefinition.Initialize(RenderingEngine);
                    foreach (var item in inputDefinition.Outputs)
                    {
                        AddInput(item.Signature, item.UIComponent);
                    }
                }
            }
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.Prefabs;
        }
    }
}
