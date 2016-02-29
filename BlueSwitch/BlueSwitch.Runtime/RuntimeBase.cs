using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Diagnostics;
using NLog;
using NLog.Fluent;
using System.IO;

namespace BlueSwitch.Runtime
{
    public class RuntimeBase
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        Engine engine = new RuntimeEngine();

        private CancellationTokenSource token = new CancellationTokenSource();
        private Task task;

        public void Start(string[] args)
        {
            if (args.Length < 1)
            {
                Log.Error("No filepath in arguments");
                return;
            }

            String filePath = args[0];

            if (!File.Exists(filePath))
            {
                Log.Error($"File {filePath} doesn't exist");
                return;
            }

            Log.Debug("Starting BlueSwitch.Runtime");

            engine.DebugMode = false;

            engine.ProcessorCompiler.Started += ProcessorCompilerOnStarted;
            engine.ProcessorCompiler.CompileFinished += ProcessorCompilerOnCompileFinished;
            engine.ProcessorCompiler.CompileStart += ProcessorCompilerOnCompileStart;
            engine.ProcessorCompiler.ErrorAdded += ProcessorCompilerOnErrorAdded;

            engine.LoadAddons();

            engine.ProjectLoaded += EngineOnProjectLoaded;
            engine.LoadProject(filePath);

            engine.CompileAndStart();

            task = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    task.Wait(100);
                }
            }, token.Token);

            engine.ProcessorCompiler.Finished += ProcessorCompilerOnFinished;
            task.Wait();
        }

        private void ProcessorCompilerOnStarted(object sender, EventArgs eventArgs)
        {
            Log.Warn("Started");
        }

        private void ProcessorCompilerOnCompileFinished(object sender, EventArgs eventArgs)
        {
            Log.Warn("Compilation finished...");
        }

        private void ProcessorCompilerOnCompileStart(object sender, EventArgs eventArgs)
        {
            Log.Warn("Compiling...");
        }

        private void ProcessorCompilerOnErrorAdded(object sender, ExceptionEntryEventArgs e)
        {
            Log.Warn("Error Added" + e.ExceptionEntry);
        }

        private void EngineOnProjectLoaded(object sender, EventArgs e)
        {
            Log.Warn($"Project \"{engine.CurrentProject.Name}\" from {engine.CurrentProject.FilePath} loaded ");
        }

        private void ProcessorCompilerOnFinished(object sender, EventArgs e)
        {
            Log.Warn("Finished");
            token.Cancel();
        }

    }
}
