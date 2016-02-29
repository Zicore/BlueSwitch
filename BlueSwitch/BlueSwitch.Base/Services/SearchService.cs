using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Meta.Search;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Services
{
    public class SearchService
    {
        public Engine Engine { get; set; }

        public Dictionary<string, SearchDescription> Items { get; set; } = new Dictionary<string, SearchDescription>();

        public SearchService(Engine engine)
        {
            Engine = engine;
        }

        public void Initialize()
        {
            // Prepare Code
            TestTags();
        }

        private void TestTags()
        {
            //Items["Int32.Add"] = new SearchDescription("Int32.Add")
            //{
            //    { "Add" },
            //    { "Math" },
            //    { "Calculation" }
            //};
        }

        public void AddTag(SwitchBase sw, string tag, string description = "")
        {
            AddTag(sw, new SearchTag(tag, description));
        }

        public void AddTag(SwitchBase sw, SearchTag tag)
        {
            var swResult = FindSwitch(sw.UniqueName);
            if (swResult != null)
            {
                var key = swResult.UniqueName;
                if (SearchDescriptionAvailable(key))
                {
                    var searchDescription = FindSearchDescription(key);
                    searchDescription.Add(tag);
                }
                else
                {
                    AddSearchDescription(new SearchDescription(key) { tag });
                }
            }
        }

        public void AddTags(SwitchBase sw, IEnumerable<string> tags)
        {
            List<SearchTag> searchTags = tags.Select(tag => new SearchTag(tag)).ToList();
            AddTags(sw,searchTags);
        }

        public void AddTags(SwitchBase sw, IEnumerable<SearchTag> tags)
        {
            var swResult = FindSwitch(sw.UniqueName);
            if (swResult != null)
            {
                var key = swResult.UniqueName;
                SearchDescription searchDescription;
                if (SearchDescriptionAvailable(key))
                {
                    searchDescription = FindSearchDescription(key);
                }
                else
                {
                    searchDescription = new SearchDescription(key);
                    AddSearchDescription(searchDescription);
                }

                searchDescription.Tags.AddRange(tags);
            }
        }

        public bool SearchDescriptionAvailable(string key)
        {
            return Items.ContainsKey(key);
        }

        public void AddSearchDescription(SearchDescription searchDescription)
        {
            if (!Items.ContainsKey(searchDescription.Key))
            {
                Items.Add(searchDescription.Key, searchDescription);
            }
        }

        public SearchDescription FindSearchDescription(string key)
        {
            if (Items.ContainsKey(key))
            {
                return Items[key];
            }
            return null;
        }

        public void Search(IList<SwitchBase> datasource, Dictionary<string, SearchEntry> entries, string query)
        {
            var namedMatches = datasource.Where(x => Contains(x.UniqueName, query, StringComparison.OrdinalIgnoreCase));
            var directMatches = datasource.Where(x => x.UniqueName.Equals(query, StringComparison.OrdinalIgnoreCase));

            List<SearchDescription> matches = new List<SearchDescription>();

            var relevantTags = datasource.Where(switchBase => Items.ContainsKey(switchBase.UniqueName)).ToDictionary(switchBase => switchBase.UniqueName, switchBase => Items[switchBase.UniqueName]);

            foreach (var item in relevantTags)
            {
                if (item.Value.Tags.Any(x => Contains(x.Tag, query, StringComparison.OrdinalIgnoreCase)))
                {
                    matches.Add(item.Value);
                }
            }

            foreach (var namedMatch in namedMatches)
            {
                QueryMatch(entries, query, namedMatch.UniqueName, 5);
            }

            foreach (var directMatch in directMatches)
            {
                QueryMatch(entries, query, directMatch.UniqueName, 5);
            }

            foreach (var match in matches)
            {
                QueryMatch(entries, query, match.Key);
            }
        }

        public Dictionary<string, SearchEntry> Search(string queryString)
        {
            Dictionary<string, SearchEntry> entries = new Dictionary<string, SearchEntry>();

            string[] queries = queryString.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries); // split by space, so we can make a deep search

            var datasource = Engine.AvailableSwitches;

            foreach (var query in queries)
            {
                entries.Clear();
                Search(datasource, entries, query);
                datasource = entries.Values.Select(x => x.Item).ToList();
            }

            return entries;
        }

        private void QueryMatch(Dictionary<string, SearchEntry> entries, string query, string match, int relevance = 0)
        {
            var sw = FindSwitch(match);
            if (sw != null)
            {
                if (!entries.ContainsKey(match))
                {
                    var entry = new SearchEntry { Item = sw };
                    entries.Add(match, entry);
                    entry.AddRelevance(relevance);
                }
                else
                {
                    entries[match].AddRelevance(10);
                }
            }
        }

        public SwitchBase FindSwitch(String key)
        {
            if (Engine.AvailableSwitchesDict.ContainsKey(key))
            {
                return Engine.AvailableSwitchesDict[key];
            }
            return null;
        }

        public static bool Contains(string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public void ExportSearchDescription(String filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.None;
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jw, JsonConvert.SerializeObject(this.Items));
                }
            }
        }
    }
}
