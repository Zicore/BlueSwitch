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

        public void Process(Processor processor, Engine renderingEngine)
        {
            Processor = processor;
            Process(Root, renderingEngine);
        }

        public Processor Processor { protected set; get; }

        private void Process(ProcessingNode<T> node, Engine renderingEngine)
        {
            do
            {
                node.Repeat = false;
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
                    for (int i = 0; i < node.Next.Count; i++)
                    {
                        var n = node.Next[i];
                        if (node.Skip == null)
                        {
                            Process(n, renderingEngine);
                        }
                        else
                        {
                            if (n.Connection.ToInputOutput.InputOutputId != node.Skip.OutputIndex)
                            {
                                Process(n, renderingEngine);
                            }
                        }
                    }
                }
            }
            while (node.Repeat && !CancellationTokenSource.IsCancellationRequested);
        }

        private void ProcessDataRoot(ProcessingNode<T> node, Engine renderingEngine)
        {
            var dataNodes = node.BacktrackData;

            foreach (var layer in dataNodes.OrderByDescending(x => x.Key))
            {
                foreach (var dataNode in layer.Value)
                {
                    if (renderingEngine.DebugMode)
                    {
                        Log.Debug("Processing: {0}", dataNode.Value.Name);
                    }

                    dataNode.Value.ProcessData(Processor, dataNode);
                    RelayOutput(Processor, dataNode, renderingEngine);
                }
            }
        }

        private void RelayOutput(Processor p, ProcessingNode<T> node, Engine renderingEngine)
        {
            var connections = renderingEngine.CurrentProject.Connections.Where(x => x.ToInputOutput.Origin == node.Value);

            foreach (var c in connections)
            {
                c.FromInputOutput.InputOutput.Data = c.ToInputOutput.InputOutput.Data;
            }
        }

        public void Stop()
        {
            if (IsActive)
            {
                CancellationTokenSource.Cancel();
            }

            OnFinished();
            IsActive = false;
        }
    }
}
