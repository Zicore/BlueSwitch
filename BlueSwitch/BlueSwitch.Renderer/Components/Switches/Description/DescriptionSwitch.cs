using System.Drawing;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Switches.Variables;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Description
{
    public class DescriptionSwitch : VariableSwitch
    {
        protected override void OnInitialize(RenderingEngine engine)
        {
            base.OnInitialize(engine); // Wichtig
            Name = "Description";
            MinRows = 1;
            DescriptionHeight = 0;
            ColumnWidth = 160;
            TextEdit.AutoResize = true;
        }

        public override GroupBase OnSetGroup()
        {
            return GroupBase.Variable;
        }

        public override void DrawDescription(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            
        }

        public override void DrawText(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            
        }
    }
}
