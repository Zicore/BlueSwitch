using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Switches.Base;

namespace BlueSwitch.Base.Meta.Search
{
    public class SearchDescription : IEnumerable<SearchTag>
    {
        public SearchDescription(String key)
        {
            Key = key;
        }

        public String Key { get; set; }

        public List<SearchTag> Tags { get; } = new List<SearchTag>();

        public void Add(String tag, String description = "")
        {
            Tags.Add(new SearchTag { Tag = tag, Description = description});
        }

        public void Add(SearchTag tag)
        {
            Tags.Add(tag);
        }

        public IEnumerator<SearchTag> GetEnumerator()
        {
            return Tags.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
