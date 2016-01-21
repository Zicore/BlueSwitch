using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Processing;

namespace BlueSwitch.Base.Trigger.Types
{
    public class EventBase
    {
        public EventBase(ProcessingTree<SwitchBase> tree)
        {
            Tree = tree;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        
        public ProcessingTree<SwitchBase> Tree { get; private set; }

        public void AssignTree(ProcessingTree<SwitchBase> tree)
        {
            Tree = tree;
        }

        public void Run(Engine renderingEngine, ProcessorCompiler compiler)
        {
            compiler.Run(renderingEngine, Tree);
        }
    }
}
