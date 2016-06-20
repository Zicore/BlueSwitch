using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;

namespace BlueSwitch.Base.Meta.Help
{
    public enum HelpType
    {
        Main,
        Input,
        Output,
    }

    public class HelpDescription
    {
        public HelpDescriptionEntry MainEntry { get; set; } = new HelpDescriptionEntry();
        public Dictionary<int, HelpDescriptionEntry> Inputs { get; set; } = new Dictionary<int, HelpDescriptionEntry>();
        public Dictionary<int, HelpDescriptionEntry> Outputs { get; set; } = new Dictionary<int, HelpDescriptionEntry>();

        public void Draw(Graphics g, SwitchBase sw, DrawableBase parent, RenderingEngine e)
        {
            MainEntry.Draw(g,sw,parent, e);
            for (int i = 0; i < sw.InputsSet.Count; i++)
            {
                if (Inputs.ContainsKey(i))
                {
                    Inputs[i].DrawInput(g,sw,i, e);
                }
            }

            for (int i = 0; i < sw.OutputsSet.Count; i++)
            {
                if (Outputs.ContainsKey(i))
                {
                    Outputs[i].DrawOutput(g, sw, i, e);
                }
            }
        }
    }
}
