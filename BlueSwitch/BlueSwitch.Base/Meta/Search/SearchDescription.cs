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
        public HashSet<SearchTag> Tags { get; } = new HashSet<SearchTag>();
       
        public void Add(String tag, String description = "")
        {
            Tags.Add(new SearchTag { Tag = tag, Description = description });
        }

        public void Add(SearchTag tag)
        {
            Tags.RemoveWhere(x => x.Tag == tag.Tag);
            Tags.Add(tag);
        }

        public void AddRange(IEnumerable<SearchTag> tags)
        {
            foreach (var searchTag in tags)
            {
                Add(searchTag);
            }
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
