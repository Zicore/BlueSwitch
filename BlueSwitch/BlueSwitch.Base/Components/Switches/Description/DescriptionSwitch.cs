using System.Drawing;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Switches.Variables;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.Description
{
    public class DescriptionSwitch : TextEditBaseSwitch
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine); // Wichtig
            Name = "Description";
            MinRows = 1;
            DescriptionHeight = 0;
            ColumnWidth = 160;
            TextEdit.AutoResize = true;
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.Variable;
        }

        public override void DrawDescription(Graphics g, Engine e, DrawableBase parent)
        {
            
        }

        public override void DrawText(Graphics g, Engine e, DrawableBase parent)
        {
            
        }
    }
}
