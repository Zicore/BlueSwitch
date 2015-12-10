using System;
using System.Linq;
using System.Threading;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using NLog;

namespace BlueSwitch.Base.Processing
{
    public class ProcessingTree<T> where T : SwitchBase
    {
        private CancellationTokenSource _cancellationTokenSource;
        private volatile bool _isActive = false;

        public CancellationTokenSource CancellationTokenSource
        {
            get { return _cancellationTokenSource; }
            set { _cancellationTokenSource = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public event EventHandler Started;
        public event EventHandler Finished;

        public virtual void OnStarted()
        {
            Started?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnFinished()
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ProcessingNode<T> Root { get; set; }

        public Processor Process(RenderingEngine renderingEngine, CancellationTokenSource cancellationTokenSource)
        {
            CancellationTokenSource = cancellationTokenSource;

            Processor = new Processor(renderingEngine, CancellationTokenSource);
            Process(Root, renderingEngine);
            return Processor;
        }

        public Processor Processor { protected set; get; }

        private void Process(ProcessingNode<T> node, RenderingEngine renderingEngine)
        {
            ProcessDataRoot(node, renderingEngine);

            if (renderingEngine.DebugMode)
            {
                Log.Warn("Processing: {0}", node.Value.Name);
            }
            node.Value.Process(Processor, node);
            node.Value.ProcessData(Processor, node);

            RelayOutput(Processor, node, renderingEngine);

            if (node.Next != null && node.Next.Count > 0)
            {
                foreach (var n in node.Next)
                {
                    if (node.Skip == null)
                    {
                        Process(n, renderingEngine);
                    }
                    else
                    {
                        //TODO: IF Verzweigung hier realisieren connection finden mit entsprechender origin
                        var connection = renderingEngine.CurrentProject.Connections.Find(x => x.FromInputOutput.Origin == node.Value);
                        if (n.Connection.ToInputOutput.InputOutputId != node.Skip.OutputIndex)
                        {
                            Process(n, renderingEngine);
                        }
                    }
                }
            }
        }

        private void ProcessDataRoot(ProcessingNode<T> node, RenderingEngine renderingEngine)
        {
            var dataNodes = node.BacktrackData;

            //Backtrack(node, node, renderingEngine, dataNodes, 0);

            foreach (var layer in dataNodes.OrderByDescending(x=>x.Key))
            {
                foreach (var dataNode in layer.Value)
                {
                    if (renderingEngine.DebugMode)
                    {
                        Log.Debug("Processing: {0}", dataNode.Value.Name);
                    }
                    dataNode.Value.ProcessData(Processor, node);
                    RelayOutput(Processor, dataNode, renderingEngine);
                }
            }
        }

        private void RelayOutput(Processor p, ProcessingNode<T> node, RenderingEngine renderingEngine)
        {
            var connections = renderingEngine.CurrentProject.Connections.Where(x => x.ToInputOutput.Origin == node.Value);

            foreach (var c in connections)
            {
                c.FromInputOutput.InputOutput.Data = c.ToInputOutput.InputOutput.Data;
            }
        }
    }
}
