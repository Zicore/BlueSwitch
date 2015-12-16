using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.IO;

namespace BlueSwitch.Base.Processing
{
    public class ProcessorCompiler
    {
        public ProcessorCompiler(Engine renderingEngine)
        {
            RenderingEngine = renderingEngine;
        }

        public event EventHandler CompileStart;
        public event EventHandler CompileFinished;

        public event EventHandler Started;
        public event EventHandler Finished;



        public Engine RenderingEngine { get; }

        private List<ProcessingTree<SwitchBase>> _items = new List<ProcessingTree<SwitchBase>>();

        public List<ProcessingTree<SwitchBase>> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public void Stop(BlueSwitchProject project)
        {
            foreach (var processingTree in Items)
            {
                if (processingTree.Processor != null)
                {
                    processingTree.CancellationTokenSource.Cancel();
                }
            }
        }

        public void Compile(BlueSwitchProject project)
        {
            OnCompileStart();
            RenderingEngine.EventManager.Items.Clear();
            Items.Clear();
            var starts = project.FindPotentialStarts();

            foreach (var connection in starts)
            {
                var tree = new ProcessingTree<SwitchBase>
                {
                    Root = new ProcessingNode<SwitchBase>(connection.ToInputOutput.Origin, connection)
                };

                tree.Root.Value.OnRegisterEvents(tree, RenderingEngine);
                
                var current = tree.Root;
                ResolveTree(current, current, project);
                Items.Add(tree);
            }
            OnCompileFinished();
            //ResolveEvents(starts, project);
        }
        
        private void ResolveTree(ProcessingNode<SwitchBase> start, ProcessingNode<SwitchBase> node, BlueSwitchProject project)
        {
            ResolveData(node, node, project);
            BacktrackData(node,node,RenderingEngine,0);

            foreach (var c in project.Connections.Where(x=>x.ToInputOutput.InputOutput.Signature is ActionSignature).OrderBy(x=>x.ToInputOutput.InputOutputId)) // sorgt für korrekte reihenfolge
            {
                if (node.Value == c.ToInputOutput.Origin)
                {
                    var nextNode = new ProcessingNode<SwitchBase>(c.FromInputOutput.Origin, c);
                    nextNode.Previous.Add( node );
                    node.Next.Add(nextNode);
                    node.OutputIndex = c.ToInputOutput.InputOutputId;
                    ResolveTree(start,nextNode, project);
                }
            }
        }

        private void ResolveData(ProcessingNode<SwitchBase> start, ProcessingNode<SwitchBase> node, BlueSwitchProject project)
        {
            var items =
                project.Connections.Where(x => !(x.ToInputOutput.InputOutput.Signature is ActionSignature))
                    .OrderBy(x => x.ToInputOutput.InputOutputId);
            
            foreach (var c in items) // sorgt für korrekte reihenfolge
            {
                if (node.Value == c.FromInputOutput.Origin )
                {
                    var nextNode = new ProcessingNode<SwitchBase>(c.ToInputOutput.Origin, c, true);

                    if (node.Previous.Count == 0)
                    {
                        nextNode.NextData.Add(node);
                    }

                    node.PreviousData.Add(nextNode);

                    if (c.ToInputOutput.Origin != start.Value)
                    {
                        ResolveData(start, nextNode, project);
                    }
                }
            }
        }

        public void RunAll(Engine renderingEngine)
        {
            foreach (var processingTree in Items)
            {
                Run(renderingEngine,processingTree);
            }
        }


        private void BacktrackData(ProcessingNode<SwitchBase> start, ProcessingNode<SwitchBase> node, Engine renderingEngine, int layer)
        {
            layer++;
            foreach (var prev in node.PreviousData.Where(x=>!x.Value.HasActionOutput))
            {
                BacktrackData(start, prev, renderingEngine, layer);
            }

            if (start != node && !node.Value.IsStart)
            {
                if (!start.BacktrackData.ContainsKey(layer))
                {
                    start.BacktrackData[layer] = new List<ProcessingNode<SwitchBase>>();
                }

                if (!start.BacktrackData[layer].Contains(node))
                {
                    start.BacktrackData[layer].Add(node);
                }
            }
        }


        public void Run(Engine renderingEngine, ProcessingTree<SwitchBase> processingTree)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            processingTree.CancellationTokenSource = tokenSource;

            Task.Run(() =>
            {
                try
                {
                    processingTree.Started -= ProcessingTreeOnStarted;
                    processingTree.Finished -= ProcessingTreeOnFinished;

                    processingTree.Started += ProcessingTreeOnStarted;
                    processingTree.Finished += ProcessingTreeOnFinished;

                    processingTree.IsActive = true;
                    processingTree.OnStarted();
                    Processor processor;
                    do
                    {
                        processor = processingTree.Process(renderingEngine, tokenSource);
                    } while (processor.Restarting && !tokenSource.IsCancellationRequested);
                }
                catch (TaskCanceledException)
                {
                    processingTree.IsActive = false;
                    processingTree.OnFinished();
                }
                finally
                {
                    processingTree.IsActive = false;
                    processingTree.OnFinished();
                }

            }, tokenSource.Token);
        }

        private void ProcessingTreeOnFinished(object sender, EventArgs eventArgs)
        {
            OnFinished(sender);
        }

        private void ProcessingTreeOnStarted(object sender, EventArgs eventArgs)
        {
            OnStarted(sender);
        }

        public virtual void OnStarted(object sender)
        {
            Started?.Invoke(sender, EventArgs.Empty);
        }

        public virtual void OnFinished(object sender)
        {
            Finished?.Invoke(sender, EventArgs.Empty);
        }

        protected virtual void OnCompileStart()
        {
            CompileStart?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnCompileFinished()
        {
            CompileFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}
