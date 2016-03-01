using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Meta.Search
{
    public class SearchTag
    {
        [JsonProperty]
        public string Tag { get; set; }

        [JsonProperty]
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
