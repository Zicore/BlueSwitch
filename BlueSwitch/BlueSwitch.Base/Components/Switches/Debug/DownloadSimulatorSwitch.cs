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

namespace BlueSwitch.Base.Components.Switches.Debug
{
    public class DownloadSimulatorSwitch : SwitchBase
    {
        public override GroupBase OnSetGroup()
        {
            return Groups.Debug;
        }

        protected override void OnInitialize(Engine renderingEngine)
        {
            base.OnInitialize(renderingEngine);

            UniqueName = "BlueSwitch.Base.Components.Switches.Debug.DownloadTest";
            DisplayName = "Download Test";
            Description = "Download Test";

            AddInput(new ActionSignature());
            AddInput(new ActionSignature());
            AddOutput(new ActionSignature());

            AddOutput(typeof(int));
        }

        private DownloadInfo info;

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            if (node.InputIndex == 0)
            {
                if (info == null)
                {
                    info = new DownloadInfo();
                }

                if (!info.IsDownloading)
                {
                    info.IsDownloading = true;

                    Task.Factory.StartNew(() =>
                    {
                        while (info.IsDownloading)
                        {
                            Thread.Sleep(500);
                            info.Status++;
                            if (info.Status >= 100)
                            {
                                info.Status = 100;
                                info.IsDownloading = false;
                            }
                        }
                    });
                }
            }

            base.OnProcess(p, node);
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            SetData(1, new DataContainer(info));
            base.OnProcessData(p, node);
        }
    }
}
