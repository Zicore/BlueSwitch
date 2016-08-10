using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.FileSystem
{
    public class FilterFileSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.FileSystem;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "BlueSwitch.Base.Components.Switches.FileSystem.FilterFileSwitch";
            DisplayName = "Filter File";
            Description = "Filter File";

            AddInput(new ActionSignature());

            AddInput(typeof(string)); // File
            AddInput(typeof(string)); // Line Regex
            AddInput(typeof(string)); // Output File

            AddOutput(new ActionSignature());

            IsCompact = true;
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            string file = GetDataValueOrDefault<string>(1);
            string lineRegex = GetDataValueOrDefault<string>(2);
            string outputFile = GetDataValueOrDefault<string>(3);

            List<string> resultFile = new List<string>();

            Regex regex = new Regex(lineRegex);
            
            using (StreamReader sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();

                    if (line != null && regex.IsMatch(line))
                    {
                        resultFile.Add(line);
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                foreach (var line in resultFile)
                {
                    sw.WriteLine(line);
                }
            }

            base.OnProcess(p, node);
        }
    }
}
