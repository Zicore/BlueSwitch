using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Diagnostics;
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

        public List<ExceptionEntry> Errors { get; } = new List<ExceptionEntry>();

        public event EventHandler<ExceptionEntryEventArgs> ErrorAdded;
        public event EventHandler ErrorCleared;

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
                processingTree.Stop();
            }
        }

        public void Compile(BlueSwitchProject project)
        {
            Errors.Clear();

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

                tree.UpdateConnections(project.ConnectionsForCompilation);
                tree.Root.Value.OnRegisterEvents(tree, RenderingEngine);
                
                var current = tree.Root;
                ResolveTree(tree,current, current);

                tree.Started -= ProcessingTreeOnStarted;
                tree.Started += ProcessingTreeOnStarted;
                tree.Finished -= ProcessingTreeOnFinished;
                tree.Finished += ProcessingTreeOnFinished;

                Items.Add(tree);
            }

            OnCompileFinished();
        }
        
        private void ResolveTree(ProcessingTree<SwitchBase> root,ProcessingNode<SwitchBase> start, ProcessingNode<SwitchBase> node)
        {
            ResolveData(root,node, node);
            BacktrackData(node,node,RenderingEngine,0);

            var items =
                root.ConnectionsForCompilation.Where(x => x.ToInputOutput.InputOutput.Signature is ActionSignature)
                    .OrderBy(x => x.ToInputOutput.InputOutputId);

            foreach (var c in items) // sorgt für korrekte reihenfolge
            {
                if (node.Value == c.ToInputOutput.Origin)
                {
                    var nextNode = new ProcessingNode<SwitchBase>(c.FromInputOutput.Origin, c);
                    nextNode.Previous.Add( node );
                    node.Next.Add(nextNode);
                    node.OutputIndex = c.ToInputOutput.InputOutputId;
                    ResolveTree(root,start,nextNode);
                }
            }
        }

        private void ResolveData(ProcessingTree<SwitchBase> root,ProcessingNode<SwitchBase> start, ProcessingNode<SwitchBase> node)
        {
            var items =
                root.ConnectionsForCompilation.Where(x => !(x.ToInputOutput.InputOutput.Signature is ActionSignature))
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

                    if (!start.ChainedNodes.Contains(nextNode.Value.Id) && c.ToInputOutput.Origin != start.Value)
                    {
                        start.ChainedNodes.Add(nextNode.Value.Id);
                        ResolveData(root,start, nextNode);
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
                Processor processor = null;
                try
                {
                    processor = new Processor(renderingEngine, tokenSource);


                    processingTree.IsActive = true;
                    processingTree.OnStarted();
                    
                    do
                    {
                        processingTree.Process(processor ,renderingEngine);
                    } while (processor.Restarting && !tokenSource.IsCancellationRequested);
                }
                catch (TaskCanceledException)
                {
                    processingTree.IsActive = false;
                    processingTree.OnFinished();
                }
                catch (Exception ex)
                {
                    if (processor != null)
                    {
                        ExceptionEntry entry = new ExceptionEntry { Exception = ex, Step = processor.Step, Node = processor.CurrentNode, Tree = processingTree};
                        processor.CurrentNode.Value.CleanUp(processor, processor.CurrentNode);
                        Errors.Add(entry);
                        OnErrorAdded(new ExceptionEntryEventArgs { ExceptionEntry = entry});
                    }
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

        protected virtual void OnErrorAdded(ExceptionEntryEventArgs e)
        {
            ErrorAdded?.Invoke(this, e);
        }

        protected virtual void OnErrorCleared()
        {
            ErrorCleared?.Invoke(this, EventArgs.Empty);
        }

        public void AddError(ExceptionEntry entry)
        {
            Errors.Add(entry);
            OnErrorAdded(new ExceptionEntryEventArgs { ExceptionEntry = entry });
        }
    }
}
