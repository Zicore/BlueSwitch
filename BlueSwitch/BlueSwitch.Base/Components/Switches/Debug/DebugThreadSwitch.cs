using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Processing;
using NLog;

namespace BlueSwitch.Base.Components.Switches.Debug
{
    public class DebugThreadSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.Debug;
        }

        private static Logger _log = LogManager.GetCurrentClassLogger();

        private static volatile int _counter = 0;

        private static object _lockObject = new object();

        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine);

            UniqueName = "BlueSwitch.Base.Components.Switches.Debug.DebugThreadSwitch";
            DisplayName = "Debug Thread";
            Description = "Throws an exception";

            AddInput(new ActionSignature());
            AddOutput(new ActionSignature());
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(() =>
                {
                    while (!p.IsCancellationRequested)
                    {
                        var name = Thread.CurrentThread.Name;
                        var id = Thread.CurrentThread.ManagedThreadId;

                        _counter++;

                        _log.Warn($"Id:{id} Name:{name} Counter:{_counter}");
                    }
                });
                thread.Start();
            }

            base.OnProcess(p, node);
        }
        
    }
}
