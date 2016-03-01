using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Switches.Base;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Meta.Search
{
    [JsonObject]
    public class SearchDescription
    {
        public SearchDescription(String key)
        {
            Key = key;
        }

        public SearchDescription()
        {
            
        }

        [JsonProperty]
        public String Key { get; set; }

        [JsonProperty]
        public List<SearchTag> Tags { get; } = new List<SearchTag>();
       
        public void Add(String tag, String description = "")
        {
            Tags.Add(new SearchTag { Tag = tag, Description = description });
        }

        public void Add(SearchTag tag)
        {
            Tags.Add(tag);
        }

        //public IEnumerator<SearchTag> GetEnumerator()
        //{
        //    return Tags.GetEnumerator();
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}
    }
}
