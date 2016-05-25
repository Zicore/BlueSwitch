using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Event;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Utils;
using XnaGeometry;

namespace BlueSwitch.Base.Services
{
    public class SelectionService
    {
        public bool SelectedItemsAvailable
        {
            get { return CurrentSelection.Count > 0; }
        }

        public bool SelectedItemsConnectionAvailable
        {
            get { return CurrentSelectionConnection.Count > 0; }
        }
        private PointF _lastDestinationPoint;

        public RenderingEngine RenderingEngine { get; set; }

        public PointF MouseRightDownMoveTranslationPositionLast { get; set; } = new PointF(0, 0);
        public PointF MouseLeftDownMovePositionLast { get; set; } = new PointF(0, 0);
        public PointF MouseLeftDownPositionLast { get; set; } = new PointF(0, 0);
        public PointF MouseMiddleDownMovePositionLast { get; set; } = new PointF(0, 0);

        public PointF SelectionRectangleStart { get; set; } = new PointF();
        public PointF SelectionRectangleEnd { get; set; } = new PointF();

        IList<SwitchBase> CurrentSelection { get; set; } = new List<SwitchBase>();
        IList<Connection> CurrentSelectionConnection { get; set; } = new List<Connection>();

        private bool _actionActive = false;

        public bool ActionActive
        {
            get { return _actionActive; }
        }

        public RectangleF SelectionRectangle
        {
            get
            {
                PointF topLeft = new PointF(
                    Math.Min(SelectionRectangleStart.X, SelectionRectangleEnd.X),
                    Math.Min(SelectionRectangleStart.Y, SelectionRectangleEnd.Y));

                PointF bottomRight = new PointF(
                    Math.Max(SelectionRectangleStart.X, SelectionRectangleEnd.X),
                    Math.Max(SelectionRectangleStart.Y, SelectionRectangleEnd.Y));

                SizeF size = new SizeF(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);

                return new RectangleF(topLeft, size);
            }
        }

        public RectangleF SelectionRectangleTranslated
        {
            get
            {
                var project = RenderingEngine.CurrentProject;
                var r = SelectionRectangle;
                return new RectangleF(
                    r.X / project.Zoom - project.Translation.X,
                    r.Y / project.Zoom - project.Translation.Y,
                    r.Width / project.Zoom,
                    r.Height / project.Zoom
                    );
            }
        }

        public bool IsSelectionMoving
        {
            get
            {
                Vector2 a = new Vector2(MouseLeftDownPositionLast.X, MouseLeftDownPositionLast.Y);
                Vector2 b = new Vector2(MouseLeftDownMovePositionLast.X, MouseLeftDownMovePositionLast.Y);

                return SelectedItemsAvailable && Vector2.Distance(a, b) > 0.001f;
            }
        }

        //[JsonIgnore]
        //public PointF TranslatedMousePosition
        //{
        //    get { return new PointF(MouseService.Position.X / CurrentProject.Zoom - CurrentProject.Translation.X, MouseService.Position.Y / CurrentProject.Zoom - CurrentProject.Translation.Y); }
        //}

        //public PointF TranslatePoint(PointF mouse)
        //{
        //    return new PointF(mouse.X / CurrentProject.Zoom - CurrentProject.Translation.X, mouse.Y / CurrentProject.Zoom - CurrentProject.Translation.Y);
        //}

        public SelectionService(RenderingEngine renderingEngine)
        {
            RenderingEngine = renderingEngine;
            RenderingEngine.MouseService.MouseMove += MouseService_MouseMove;
            RenderingEngine.MouseService.MouseDown += MouseServiceOnMouseDown;
            RenderingEngine.MouseService.MouseUp += MouseServiceOnMouseUp;
        }

        private void MouseService_MouseMove(object sender, MouseEventArgs e)
        {
            if (StartSelectionRectangle && RenderingEngine.MouseService.LeftMouseDown)
            {
                SelectionRectangleEnd = e.Location;
            }
        }

        public bool StartSelectionRectangle { get; set; } = false;
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
            // This makes sure we are allways checking both types
            return SelectedInputOutput.InputOutput.Signature.Matches(io.Signature) || io.Signature.Matches(SelectedInputOutput.InputOutput.Signature);
        }

        private ContextActionEventArgs _currentActionArgs;

        public event EventHandler<ContextActionEventArgs> ContextAction;

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

        public PointF DestinationConnectionPosition
        {
            get
            {
                if (ActionActive)
                {
                    return _lastDestinationPoint;
                }
                else
                {
                    return RenderingEngine.TranslatedMousePosition;
                }
            }
        }

        public static bool SelectorOriginEquals(InputOutputSelector sel1, InputOutputSelector sel2)
        {
            return sel1?.OriginId == sel2?.OriginId;
        }

        public static bool SelectorEquals(InputOutputSelector sel1, InputOutputSelector sel2)
        {
            return sel1?.OriginId == sel2?.OriginId && sel1?.InputOutputId == sel2?.InputOutputId;
        }

        private void MouseServiceOnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!StartSelectionRectangle)
                {
                    var mouseOverItems = RenderingEngine.CurrentProject.Items.Where(x => x.IsMouseOver);
                    InputOutputSelector selector = null;
                    foreach (var selectedItem in mouseOverItems)
                    {
                        selectedItem.IsSelected = true;

                        List<InputOutputBase> inputOutputs = new List<InputOutputBase>();

                        inputOutputs.AddRange(selectedItem.Inputs);
                        inputOutputs.AddRange(selectedItem.Outputs);

                        var mouseOverInputOutput = inputOutputs.FirstOrDefault(x => x.IsMouseOver);

                        if (mouseOverInputOutput != null)
                        {
                            selector = SetSelector(selectedItem, mouseOverInputOutput);
                            break;
                        }
                    }

                    if (NotComplete || !SignatureMatching)
                    {
                        if (InputOutputAvailable && !SelectorEquals(SelectedInputOutput, selector))
                        {
                            _lastDestinationPoint = RenderingEngine.TranslatedMousePosition;
                            _currentActionArgs = new ContextActionEventArgs { Selector = SelectedInputOutput, Location = e.Location };
                            OnContextAction(_currentActionArgs);
                            _actionActive = true;
                        }
                        else
                        {
                            OnInComplete();
                            FinishContextActionIntern();
                        }
                    }
                    else
                    {
                        OnCompleted();
                        FinishContextActionIntern();
                    }
                }
                else
                {
                    var selectedItems = RenderingEngine.CurrentProject.Items.Where(x => RectangleF.Intersect(x.Rectangle, SelectionRectangleTranslated) != RectangleF.Empty);

                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.IsSelected = true;
                        CurrentSelection.Add(selectedItem);
                    }
                }

                StartDrag = false;
            }
        }

        private InputOutputSelector SetSelector(SwitchBase sw, InputOutputBase sourceIO)
        {
            var selector = new InputOutputSelector(sw, sourceIO);

            if (Input?.InputOutput is InputBase && sourceIO is OutputBase)
            {
                Output = selector;
            }

            if (Input?.InputOutput is OutputBase && sourceIO is InputBase)
            {
                Output = selector;
            }

            if (Output?.InputOutput is InputBase && sourceIO is OutputBase)
            {
                Input = selector;
            }

            if (Output?.InputOutput is OutputBase && sourceIO is InputBase)
            {
                Input = selector;
            }

            return selector;
        }

        public void FinishContextAction(bool canceled, SwitchBase createSwitch)
        {
            if (canceled)
            {
                OnInComplete();
            }
            else
            {
                List<InputOutputBase> inputOutputs = new List<InputOutputBase>();
                if (Input != null)
                {
                    inputOutputs.AddRange(createSwitch.Outputs);
                }
                else
                {
                    inputOutputs.AddRange(createSwitch.Inputs);
                }
                var matchingIO = inputOutputs.FirstOrDefault(x => x.Signature.Matches(SelectedInputOutput.InputOutput.Signature));
                SetSelector(createSwitch, matchingIO);
                OnCompleted();
            }
            FinishContextActionIntern();
        }

        private void FinishContextActionIntern()
        {
            _actionActive = false;
            Input = null;
            Output = null;
        }

        private void MouseServiceOnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                MouseRightDownMoveTranslationPositionLast = e.Location;
            }

            if (e.Button == MouseButtons.Left)
            {
                StartSelectionRectangle = false;
                MouseLeftDownMovePositionLast = RenderingEngine.TranslatedMousePosition;
                MouseLeftDownPositionLast = RenderingEngine.TranslatedMousePosition;

                if (!StartDrag)
                {
                    Input = null;
                    Output = null;

                    var mouseOverItems = RenderingEngine.CurrentProject.Items.Where(x => x.IsMouseOver).ToList();
                    var mouseOverConnections = RenderingEngine.CurrentProject.Connections.Where(x => x.IsMouseOver).ToList();

                    if (mouseOverItems.Count == 0 && mouseOverConnections.Count == 0)
                    {
                        DeselectAll();
                    }

                    if (mouseOverItems.Count > 0)
                    {
                        foreach (var selectedItem in mouseOverItems)
                        {
                            if (!CurrentSelection.Contains(selectedItem))
                            {
                                DeselectAll();
                                CurrentSelection.Add(selectedItem);
                            }
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

                    if (mouseOverConnections.Count > 0)
                    {
                        foreach (var selectedItem in mouseOverConnections)
                        {
                            if (!CurrentSelectionConnection.Contains(selectedItem))
                            {
                                DeselectAll();
                                CurrentSelectionConnection.Add(selectedItem);
                            }
                            selectedItem.IsSelected = true;
                        }
                    }
                }

                if (!SelectedItemsAvailable)
                {
                    StartSelectionRectangle = true;
                    SelectionRectangleStart = e.Location;
                }
                StartDrag = true;
            }

            if (e.Button == MouseButtons.Middle)
            {
                MouseMiddleDownMovePositionLast = e.Location;
            }
        }

        public void DeselectAll()
        {
            foreach (var item in RenderingEngine.CurrentProject.Items)
            {
                item.IsSelected = false;
            }
            CurrentSelection.Clear();

            foreach (var item in RenderingEngine.CurrentProject.Connections)
            {
                item.IsSelected = false;
            }
            CurrentSelectionConnection.Clear();
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

        protected virtual void OnContextAction(ContextActionEventArgs e)
        {
            ContextAction?.Invoke(this, e);
        }
    }
}
