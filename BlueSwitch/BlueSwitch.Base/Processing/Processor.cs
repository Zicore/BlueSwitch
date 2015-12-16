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

        private int _step;

        public int Step
        {
            get { return _step; }
            set { _step = value; }
        }

        private CancellationTokenSource _cancellationTokenSource;

        List<ProcessingNode<SwitchBase>> _connections = new List<ProcessingNode<SwitchBase>>();
        public List<ProcessingNode<SwitchBase>> Connections
        {
            get { return _connections; }
            set { _connections = value; }
        }
        
        public Engine RenderingEngine { get; set; }

        public void Redraw()
        {
            RenderingEngine.RequestRedraw();
        }

        private bool _restarting;

        public bool Restarting
        {
            get { return _restarting; }
            protected set { _restarting = value; }
        }

        public void MarkForRestart()
        {
            Restarting = true;
        }

        public void Wait(TimeSpan ts)
        {
            //Thread.Sleep(ts);
            _cancellationTokenSource.Token.WaitHandle.WaitOne(ts);
        }

        public void Wait()
        {
            _cancellationTokenSource.Token.WaitHandle.WaitOne();
        }
    }
}
