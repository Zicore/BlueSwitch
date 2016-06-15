using System;
using System.Collections.Generic;
using System.Threading;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;

namespace BlueSwitch.Base.Processing
{
    public class Processor
    {
        public Processor(Engine renderingEngine, CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
            RenderingEngine = renderingEngine;
        }

        public ProcessingNode<SwitchBase> CurrentNode { get; set; }
        public ProcessingNode<SwitchBase> CurrentDataNode { get; set; }

        public int Step { get; set; }

        private readonly CancellationTokenSource _cancellationTokenSource;

        public List<ProcessingNode<SwitchBase>> Connections { get; set; } = new List<ProcessingNode<SwitchBase>>();

        public Engine RenderingEngine { get; set; }

        public void Redraw()
        {
            RenderingEngine.RequestRedraw();
        }

        public bool Restarting { get; protected set; }

        public bool IsCancellationRequested => _cancellationTokenSource.IsCancellationRequested;

        public void MarkForRestart()
        {
            Restarting = true;
        }

        public void Wait(TimeSpan ts)
        {
            _cancellationTokenSource.Token.WaitHandle.WaitOne(ts);
        }

        public void Wait()
        {
            _cancellationTokenSource.Token.WaitHandle.WaitOne();
        }
    }
}
