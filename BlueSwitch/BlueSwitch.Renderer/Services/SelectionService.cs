using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Base;

namespace BlueSwitch.Base.Services
{
    public class SelectionService
    {
        public bool SelectedItemsAvailable { get; private set; } = false;

        public RenderingEngine RenderingEngine { get; set; }

        public PointF MouseRightDownMoveTranslationPositionLast { get; set; } = new PointF(0, 0);
        public PointF MouseLeftDownMovePositionLast { get; set; } = new PointF(0, 0);
        public PointF MouseMiddleDownMovePositionLast { get; set; } = new PointF(0, 0);

        public SelectionService(RenderingEngine renderingEngine)
        {
            RenderingEngine = renderingEngine;
            RenderingEngine.MouseService.MouseMove += MouseService_MouseMove;
            RenderingEngine.MouseService.MouseDown += MouseServiceOnMouseDown;
            RenderingEngine.MouseService.MouseUp += MouseServiceOnMouseUp;
        }

        private void MouseService_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        public bool StartDrag { get; set; } = false;
        public InputOutputSelector Input { get; set; }
        public InputOutputSelector Output { get; set; }
        
        public bool InputOutputAvailable
        {
            get { return Input != null || Output != null; }
        }

        public bool NotComplete
        {
            get { return Input == null || Output == null; }
        }

        public bool SignatureMatching
        {
            get
            {
                return Input.InputOutput.Signature.Matches(Output.InputOutput.Signature) || Output.InputOutput.Signature.Matches(Input.InputOutput.Signature); // In this case the input is allways input and output allways output
            }
        }

        public bool IsSignatureMatching(InputOutputBase io)
        {
            if (SelectedInputOutput == null)
            {
                return false;
            }
            // This makes sure we are allways chechking both types
            return SelectedInputOutput.InputOutput.Signature.Matches(io.Signature) || io.Signature.Matches(SelectedInputOutput.InputOutput.Signature); 
        }

        public event EventHandler InComplete;

        protected virtual void OnInComplete()
        {
            InComplete?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Completed;
        
        protected virtual void OnCompleted()
        {
            Completed?.Invoke(this, EventArgs.Empty);
        }

        public InputOutputSelector SelectedInputOutput
        {
            get
            {
                if (Input != null)
                {
                    return Input;
                }
                else
                {
                    return Output;
                }
            }
        }
        
        private void MouseServiceOnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var mouseOverItems = RenderingEngine.CurrentProject.Items.Where(x => x.IsMouseOver);

                foreach (var selectedItem in mouseOverItems)
                {
                    selectedItem.IsSelected = true;

                    List<InputOutputBase> inputOutputs = new List<InputOutputBase>();

                    inputOutputs.AddRange(selectedItem.Inputs);
                    inputOutputs.AddRange(selectedItem.Outputs);

                    var mouseOverInputOutput = inputOutputs.FirstOrDefault(x => x.IsMouseOver);

                    if (mouseOverInputOutput != null)
                    {
                        var selector = new InputOutputSelector(selectedItem, mouseOverInputOutput);

                        if (Input?.InputOutput is InputBase && mouseOverInputOutput is OutputBase)
                        {
                            Output = selector;
                        }

                        if (Input?.InputOutput is OutputBase && mouseOverInputOutput is InputBase)
                        {
                            Output = selector;
                        }

                        if (Output?.InputOutput is InputBase && mouseOverInputOutput is OutputBase)
                        {
                            Input = selector;
                        }

                        if (Output?.InputOutput is OutputBase && mouseOverInputOutput is InputBase)
                        {
                            Input = selector;
                        }
                    }
                }

                if (NotComplete || !SignatureMatching)
                {
                    OnInComplete();
                }
                else
                {
                    OnCompleted();
                }

                Input = null;
                Output = null;

                StartDrag = false;
            }
        }

        private void MouseServiceOnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                MouseRightDownMoveTranslationPositionLast = e.Location;
            }

            if (e.Button == MouseButtons.Left)
            {
                SelectedItemsAvailable = false;
                MouseLeftDownMovePositionLast = RenderingEngine.TranslatedMousePosition;

                if (!StartDrag)
                {
                    Input = null;
                    Output = null;

                    foreach (var item in RenderingEngine.CurrentProject.Items)
                    {
                        item.IsSelected = false;
                    }

                    var mouseOverItems = RenderingEngine.CurrentProject.Items.Where(x => x.IsMouseOver).ToList();

                    if (mouseOverItems.Count > 0)
                    {
                        SelectedItemsAvailable = true;
                        foreach (var selectedItem in mouseOverItems)
                        {
                            selectedItem.IsSelected = true;

                            List<InputOutputBase> inputOutputs = new List<InputOutputBase>();

                            inputOutputs.AddRange(selectedItem.Inputs);
                            inputOutputs.AddRange(selectedItem.Outputs);

                            var mouseOverInputOutput = inputOutputs.FirstOrDefault(x => x.IsMouseOver);

                            if (mouseOverInputOutput != null)
                            {
                                if (mouseOverInputOutput is InputBase)
                                {

                                    Input = new InputOutputSelector(selectedItem, mouseOverInputOutput);
                                }
                                if (mouseOverInputOutput is OutputBase)
                                {
                                    Output = new InputOutputSelector(selectedItem, mouseOverInputOutput);
                                }
                            }
                        }
                    }
                }
                StartDrag = true;
            }

            if (e.Button == MouseButtons.Middle)
            {
                MouseMiddleDownMovePositionLast = e.Location;
            }
        }

        public void RemoveSelected()
        {
            if (RenderingEngine.DesignMode)
            {
                var p = RenderingEngine.CurrentProject;
                var selected = p.Items.Where(x => x.IsSelected).ToList();

                foreach (var sw in selected)
                {
                    RenderingEngine.CurrentProject.Remove(sw);
                }
            }
        }
    }
}
