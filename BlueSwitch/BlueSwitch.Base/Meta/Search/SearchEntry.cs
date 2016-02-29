using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Switches.Base;

namespace BlueSwitch.Base.Meta.Search
{
    public class SearchEntry
    {
        public SwitchBase Item { get; set; }
        public int Relevance { get; private set; }

        public SearchEntry()
        {
            Relevance = 10;
        }

        public void AddRelevance(int value)
        {
            Relevance += value;
        }
    }
}
