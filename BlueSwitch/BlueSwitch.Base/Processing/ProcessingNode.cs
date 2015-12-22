using System.Collections.Generic;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;

namespace BlueSwitch.Base.Processing
{
    public class ProcessingNode<T> where T : SwitchBase
    {
        public ProcessingNode(T value, Connection io) : this(value, io, false){ }

        public ProcessingNode(T value, Connection io, bool isData)
        {
            Value = value;
            IsData = isData;
            Connection = io;
        }

        public Dictionary<int, List<ProcessingNode<T>>> BacktrackData { get; } = new Dictionary<int,List<ProcessingNode<T>>>();

        public bool IsData { get; set; }

        public T Value { get; set; }

        public List<ProcessingNode<T>> Next { get; set; } = new List<ProcessingNode<T>>();
        public List<ProcessingNode<T>> Previous { get; set; } = new List<ProcessingNode<T>>();

        public List<ProcessingNode<T>> NextData { get; set; } = new List<ProcessingNode<T>>();
        public List<ProcessingNode<T>> PreviousData { get; set; } = new List<ProcessingNode<T>>();

        public SkipNode Skip { get; set; }

        private int _outputIndex = -1;

        public int OutputIndex
        {
            get { return _outputIndex; }
            set { _outputIndex = value; }
        }

        public int InputIndex
        {
            get { return Connection.FromInputOutput.InputOutputId; }
        }

        public override string ToString()
        {
            return $"{Value.Name}";
        }

        private Connection _connection;

        public Connection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        public bool Repeat { get; set; }
    }

    public class SkipNode
    {
        public SkipNode(int index)
        {
            OutputIndex = index;
        }
        private int _outputIndex;

        public int OutputIndex
        {
            get { return _outputIndex; }
            set { _outputIndex = value; }
        }
    }
}
