using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSwitch.Base.Meta.Search
{
    public class SearchTag
    {
        public string Tag { get; set; }
        public string Description { get; set; }

        public SearchTag()
        {
            
        }

        public SearchTag(string tag, string description = "")
        {
            this.Tag = tag;
            this.Description = description;
        }
    }
}
