using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Components.Switches.FileSystem
{
    class MergeFilesSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.FileSystem;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "BlueSwitch.Base.Components.Switches.FileSystem.MergeFilesSwitch";
            DisplayName = "Merge Files";
            Description = "Merge files into one.";

            AddInput(new ActionSignature());

            AddInput(typeof(string)); // Folder
            AddInput(typeof(string)); // File Pattern
            AddInput(typeof(string)); // Output File

            AddOutput(new ActionSignature());

            IsCompact = true;
        }
        
        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            string folderPath = GetDataValueOrDefault<string>(1);
            string fileFilter = GetDataValueOrDefault<string>(2);
            string outputFile = GetDataValueOrDefault<string>(3);

            HashSet<string> set = new HashSet<string>();

            var files = Directory.GetFiles(folderPath, fileFilter, SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        set.Add(line);
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                foreach (var line in set)
                {
                    sw.WriteLine(line);
                }
            }

            base.OnProcess(p, node);
        }

        protected override void OnCleanUp<T>(Processor p, ProcessingNode<T> node)
        {
            base.OnCleanUp(p, node);
        }
    }
}
