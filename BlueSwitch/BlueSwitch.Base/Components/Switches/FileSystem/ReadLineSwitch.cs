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
    public class ReadLineSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.FileSystem;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "BlueSwitch.Base.Components.Switches.FileSystem.ReadLine";
            DisplayName = "Read Line";

            AddInput(new ActionSignature());

            AddInput(typeof(string));

            AddOutput(new ActionSignature());
            //AddOutput(typeof(bool));
            AddOutput(typeof(string));
            AddOutput(new ActionSignature());

            IsCompact = true;
        }
        
        private StreamReader _reader;

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            if (_reader == null)
            {
                string filePath = GetDataValueOrDefault<string>(1);
                _reader = new StreamReader(filePath);
            }

            if (_reader != null)
            {
                if (!_reader.EndOfStream)
                {
                    SetData(1, new DataContainer(_reader.ReadLine()));
                    node.Repeat = true;
                    node.Skip = new SkipNode(2);
                }
                else
                {
                    node.Skip = new SkipNode(0);
                    _reader.Close();
                    _reader = null;
                }
            }

            base.OnProcess(p, node);
        }

        protected override void OnCleanUp<T>(Processor p, ProcessingNode<T> node)
        {
            if (_reader != null)
            {
                _reader.Close();
                _reader = null;
            }

            base.OnCleanUp(p, node);
        }
    }
}
