using System;
using System.Timers;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Event;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Processing;
using BlueSwitch.Base.Trigger.Types;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class TickSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.Trigger;
        }

        private String _eventName = null;
        readonly Timer _timer = new Timer();

        protected override void OnInitialize(Engine renderingEngine)
        {
            UniqueName = "Tick";
            Description = "Tick";

            AddInput(new ActionSignature());
            AddInput(typeof(int), TextEdit.CreateNumeric(false));
            AddInput(typeof (string), new TextEdit());


            AddOutput(new OutputBase(new ActionSignature()));
            IsStart = true;

            RenderingEngine.ProcessorCompiler.Finished += ProcessorCompilerOnFinished;
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            base.OnProcess(p, node);

            _eventName = GetDataValueOrDefault<string>(2);

            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Interval = GetDataValueOrDefault<int>(1);
            _timer.Elapsed -= TimerOnElapsed;
            _timer.Elapsed += TimerOnElapsed;
            
        }

        private void ProcessorCompilerOnFinished(object sender, EventArgs eventArgs)
        {
            _timer.Elapsed -= TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (_eventName != null)
            {
                RenderingEngine.EventManager.Run(EventTypeBase.StartSingle, _eventName);
            }
        }
    }
}
