using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Drawing.Extended;
using BlueSwitch.Base.Meta.Help;
using BlueSwitch.Base.Meta.Search;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Services
{
    public class HelpService
    {
        public bool Enabled { get; set; } = true;

        public Engine Engine { get; set; }

        [JsonProperty]
        public Dictionary<string, HelpDescription> Items { get; set; } = new Dictionary<string, HelpDescription>();

        public HelpService(Engine engine)
        {
            this.Engine = engine;
            ImportDefaultHelp();
        }
        
        public HelpDescription FindHelp(String key)
        {
            if (Items.ContainsKey(key))
            {
                return Items[key];
            }
            return null;
        }

        public void Add(HelpDescription helpDesc, string key)
        {
            var sw = Engine.FindSwitch(key);
            if (sw != null)
            {
                if (Items.ContainsKey(key))
                {
                    var currentHelp = Items[key];
                    foreach (var hi in helpDesc.Inputs)
                    {
                        if (!currentHelp.Inputs.ContainsKey(hi.Key))
                        {
                            currentHelp.Inputs[hi.Key] = hi.Value;
                        }
                    }

                    foreach (var ho in helpDesc.Outputs)
                    {
                        if (!currentHelp.Outputs.ContainsKey(ho.Key))
                        {
                            currentHelp.Outputs[ho.Key] = ho.Value;
                        }
                    }
                }
                else
                {
                    Items[key] = helpDesc;
                }
            }
        }

        public void AddInput(HelpDescriptionEntry helpEntry, string key, int index)
        {
            var sw = Engine.FindSwitch(key);
            if (sw != null)
            {
                if (Items.ContainsKey(key))
                {
                    var currentHelp = Items[key];
                    currentHelp.Inputs[index] = helpEntry;
                }
            }
        }

        public void AddInput(HelpDescriptionEntry helpEntry, SwitchBase sw, int index)
        {
            AddInput(helpEntry, sw.UniqueName, index);
        }

        public void AddOutput(HelpDescriptionEntry helpEntry, string key, int index)
        {
            var sw = Engine.FindSwitch(key);
            if (sw != null)
            {
                if (Items.ContainsKey(key))
                {
                    var currentHelp = Items[key];
                    currentHelp.Outputs[index] = helpEntry;
                }
            }
        }

        public void AddOutput(HelpDescriptionEntry helpEntry, SwitchBase sw, int index)
        {
            AddOutput(helpEntry, sw.UniqueName, index);
        }

        public void Draw(Graphics g, SwitchBase sw, DrawableBase parent)
        {
            if (Enabled)
            {
                if (Items.ContainsKey(sw.UniqueName) && sw.IsMouseOver)
                {
                    Items[sw.UniqueName].Draw(g, sw, parent);
                }
            }
        }

        public void ExportHelpDescription(String filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(JsonConvert.SerializeObject(this.Items));
            }
        }

        public void ExportDefaultHelpDescription()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Meta", "Help", "help.meta.json");
            ExportHelpDescription(path);
        }

        public void ExportHelpDescription(String filePath, string key)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                var dict = new Dictionary<string, HelpDescription>();
                if (Items.ContainsKey(key))
                {
                    dict[key] = Items[key];
                }
                sw.Write(JsonConvert.SerializeObject(dict));
            }
        }

        public void ImportHelpDescription(String filePath)
        {
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    var importedHelp = JsonConvert.DeserializeObject<Dictionary<string, HelpDescription>>(sr.ReadToEnd());
                    if (importedHelp != null)
                    {
                        Items = new Dictionary<string, HelpDescription>(importedHelp);
                        foreach (var help in importedHelp)
                        {
                            Add(help.Value, help.Key);
                        }
                    }
                }
            }
        }

        public void ImportDefaultHelp()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Meta", "Help", "help.meta.json");
            ImportHelpDescription(filePath);
        }
    }
}
