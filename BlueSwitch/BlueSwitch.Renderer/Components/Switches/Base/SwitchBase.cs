using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Drawing.Extended;
using BlueSwitch.Base.IO;
using BlueSwitch.Base.Processing;
using Newtonsoft.Json;

namespace BlueSwitch.Base.Components.Switches.Base
{
    [JsonObject("SwitchBase")]
    public abstract class SwitchBase : DrawableBase
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// This helps to store values to the json file and back
        /// </summary>
        public ValueStore ValueStore { get; } = new ValueStore();

        [JsonIgnore]
        public bool DebugMode { get; set; } = false;

        [JsonIgnore]
        public bool DebugDataMode { get; set; } = false;

        public int MinRows { get; set; } = 1;
        public float RowHeight = 18.0f;


        [JsonIgnore]
        public bool IsStart { get; set; }

        protected SwitchBase()
        {
            
        }

        [JsonIgnore]
        public bool HasActionOutput { get; protected set; }

        private string _name;

        [JsonIgnore]
        public String Name
        {
            get
            {
                if (String.IsNullOrEmpty(_name))
                {
                    return GetType().Name;
                }
                return _name;
            }
            set { _name = value; }
        }

        [JsonIgnore]
        public string Description { get; protected set; }

        [JsonIgnore]
        public String DisplayName
        {
            get { return $"{Name} {Id}";}
        }

        [JsonIgnore]
        public GroupBase Group { get; set; } = GroupBase.Base;

        public void SetData(int index, DataContainer data)
        {
            Outputs[index].Data = data;
        }

        public DataContainer GetData(int index)
        {
            var input = Inputs[index];
            
            if (input.UIComponent != null && !input.IsConnected(RenderingEngine))
            {
                input.Data = new DataContainer(input.UIComponent.GetData());
            }

            return input.Data;
        }

        public T GetDataValueOrDefault<T>(int index)
        {
            var data = GetData(index);
            if (data != null)
            {
                var result = (T) Convert.ChangeType(data.Value, typeof (T));
                return result;
            }
            else
            {
                return default(T);
            }
        }

        public DataContainer GetOutputData(int index)
        {
            return Outputs[index].Data;
        }

        public abstract GroupBase OnSetGroup();

        public void SetGroup()
        {
            Group = OnSetGroup();
        }

        public String Extension { get; set; }

        [JsonIgnore]
        public static Pen Pen { get; set; } = new Pen(Color.Black, 1.0f);

        [JsonIgnore]
        public static Pen DescriptionPen { get; set; } = new Pen(Color.FromArgb(180,30,30,30), 0.5f);
        
        [JsonIgnore]
        public static Pen MouseOverPen { get; set; } = new Pen(Color.LightCoral, 2.0f);

        [JsonIgnore]
        public static Pen SelectedPen { get; set; } = new Pen(Color.LimeGreen, 2.0f);

        [JsonIgnore]
        public static Brush MouseOverBrush { get; set; } = new SolidBrush(Color.LightGreen);

        protected static Font FontSmall = new Font(new FontFamily("Calibri"), 9, FontStyle.Regular);

        protected static Font FontVerySmall = new Font(new FontFamily("Calibri"), 7, FontStyle.Regular);

        [JsonIgnore]
        public List<InputBase> Inputs { get; set; } = new List<InputBase>();

        [JsonIgnore]
        public List<OutputBase> Outputs { get; set; } = new List<OutputBase>();

        [JsonIgnore]
        public List<UIComponent> Components { get; set; } = new List<UIComponent>();

        [JsonIgnore]
        public override SizeF Size
        {
            get
            {
                var rows = Math.Max(Math.Max(Inputs.Count, Outputs.Count), MinRows);
                return new SizeF(ColumnWidth, DescriptionHeight + rows * RowHeight);
            }
        }

        [JsonIgnore]
        public override float DescriptionOffsetLeft
        {
            get
            {
                if (Inputs.Count > 0)
                {
                    return 16.0f;
                }
                return 2.0f;
            }
        }

        [JsonIgnore]
        public override float DescriptionOffsetRight
        {
            get
            {
                if (Outputs.Count > 0)
                {
                    return 16.0f;
                }
                return 2.0f;
            }
        }

        [JsonIgnore]
        public override float DescriptionOffsetTop
        {
            get
            {
                //if (Outputs.Count > 0 && Inputs.Count > 0)
                //{
                //    return 2.0f;
                //}
                return 2.0f;
            }
        }

        [JsonIgnore]
        public override float DescriptionOffsetBottom
        {
            get
            {
                //if (Outputs.Count > 0 && Inputs.Count > 0)
                //{
                //    return 2.0f;
                //}
                return 2.0f;
            }
        }

        [JsonIgnore]
        public RectangleF SelectionBounds
        {
            get
            {
                const float offsetYH = 4.0f;
                const float offsetXH = 4.0f;

                const float offsetY = 8.0f;
                const float offsetX = 8.0f;

                var r = Rectangle;
                RectangleF selectionBounds = new RectangleF(r.X - offsetXH, r.Y - offsetYH, r.Width + offsetX, r.Height + offsetY);
                return selectionBounds;
            }
        }

        public override RectangleF GetTranslatedRectangle(PointF translation)
        {
            return SelectionBounds;
        }

        public InputBase AddInput(Signature signature, UIComponent uiComponent = null)
        {
            return AddInput(new InputBase(signature), uiComponent);
        }

        public OutputBase AddOutput(Signature signature, UIComponent uiComponent = null)
        {
            return AddOutput(new OutputBase(signature), uiComponent);
        }

        public InputBase AddInput(InputBase input, UIComponent uiComponent = null)
        {
            int index = Inputs.Count;
            input.Index = index;
            input.UIComponent = uiComponent;
            Inputs.Add(input);
            return input;
        }

        public OutputBase AddOutput(OutputBase output, UIComponent uiComponent = null)
        {
            int index = Outputs.Count;
            output.Index = index;
            Outputs.Add(output);
            output.UIComponent = uiComponent;
            if (output.Signature is ActionSignature)
            {
                HasActionOutput = true;
            }
            return output;
        }

        public void AddOutput(Type type, UIComponent uiComponent = null)
        {
            AddOutput(SignatureSingle.CreateOutput(type), uiComponent);
        }

        public void AddInput(Type type, UIComponent uiComponent = null)
        {
            AddInput(SignatureSingle.CreateInput(type), uiComponent);
        }


        // --------------------------------------------------------------------------------

        public override void Draw(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            DrawBody(g, e, parent);

            DrawInputOutputs(g, e, parent);

            DrawSelection(g, e, parent);

            DrawDescription(g,e,parent);
            DrawGlyph(g, e, parent);
            DrawText(g, e, parent);

            DrawComponents(g, e, parent);

            base.Draw(g, e, this);
        }

        public virtual void DrawInputOutputs(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            InputOutputBase lastInput = null;
            foreach (var input in Inputs)
            {
                input.Draw(g, e, this, lastInput);
                lastInput = input;
            }

            InputOutputBase lastOutput = null;
            foreach (var output in Outputs)
            {
                output.Draw(g, e, this, lastOutput);
                lastOutput = output;
            }
        }

        public virtual Brush GetMainBrush(RectangleF rectangle)
        {
            var brush = new LinearGradientBrush(rectangle, Color.Black, Color.Black, 90, true);

            ColorBlend cb = new ColorBlend();

            cb.Positions = new[] { 0, 0.2f, 0.5f, 1 };
            cb.Colors = new Color[] { Color.FromArgb(120, 0, 0, 0), Color.FromArgb(80, 0, 0, 0), Color.FromArgb(80, 0, 0, 0), Color.FromArgb(60, 30, 160, 255) };

            brush.InterpolationColors = cb;

            return brush;
        }

        public virtual Brush GetDebugBrush(RectangleF rectangle)
        {
            var brush = new LinearGradientBrush(rectangle, Color.Black, Color.Black, 90, true);

            ColorBlend cb = new ColorBlend();

            cb.Positions = new[] { 0, 0.2f, 0.5f, 1 };
            cb.Colors = new Color[] { Color.FromArgb(120, 0, 0, 0), Color.FromArgb(150, 255, 0, 0), Color.FromArgb(150, 255, 0, 0), Color.FromArgb(60, 30, 160, 255) };

            brush.InterpolationColors = cb;

            return brush;
        }

        public virtual Brush GetDebugDataBrush(RectangleF rectangle)
        {
            var brush = new LinearGradientBrush(rectangle, Color.Black, Color.Black, 90, true);

            ColorBlend cb = new ColorBlend();

            cb.Positions = new[] { 0, 0.2f, 0.5f, 1 };
            cb.Colors = new Color[] { Color.FromArgb(120, 0, 0, 0), Color.FromArgb(160, 180, 10, 0), Color.FromArgb(160, 180, 10, 0), Color.FromArgb(60, 30, 160, 255) };

            brush.InterpolationColors = cb;

            return brush;
        }

        public virtual Brush GetMainSelectionBrush(RectangleF rectangle)
        {
            var brush = new LinearGradientBrush(rectangle, Color.Black, Color.Black, 90, true);

            ColorBlend cb = new ColorBlend();

            cb.Positions = new[] { 0, 0.2f, 0.5f, 1 };
            cb.Colors = new Color[] { Color.FromArgb(120, 0, 0, 0), Color.FromArgb(80, 30, 144, 255), Color.FromArgb(80, 30, 144, 255), Color.FromArgb(60, 30, 160, 255) };

            brush.InterpolationColors = cb;

            return brush;
        }

        public virtual void DrawBody(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            ExtendedGraphics extendedGraphics = new ExtendedGraphics(g);

            var r = Rectangle;

            Brush brush;

            if (IsSelected)
            {
                brush = GetMainSelectionBrush(r);
            }
            else if (DebugMode)
            {
                brush = GetDebugBrush(r);
            }
            else if (DebugDataMode)
            {
                brush = GetDebugDataBrush(r);
            }
            else
            {
                brush = GetMainBrush(r);
            }

            float radius = 4;
            
            extendedGraphics.FillRoundRectangle(brush, r.X, r.Y, r.Width, r.Height, radius);
            extendedGraphics.DrawRoundRectangle(Pen,r.X, r.Y, r.Width, r.Height, radius);
        }

        public virtual void DrawComponents(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            foreach (var uiComponent in Components)
            {
                uiComponent.Draw(g,e,this);
            }
        }

        public virtual void DrawDescription(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            ExtendedGraphics extendedGraphics = new ExtendedGraphics(g);

            var rect = Rectangle;

            const float offset = 2f;
            const float offset2 = 4f;

            var r = new RectangleF(rect.X + offset, rect.Y + offset, rect.Width - offset2, DescriptionHeight - offset2);

            float radius = 2;

            var brush = new SolidBrush(Color.FromArgb(80,0,0,0));
            
            extendedGraphics.FillRoundRectangle(brush, r.X, r.Y, r.Width, r.Height, radius);
            extendedGraphics.DrawRoundRectangle(DescriptionPen, r.X, r.Y, r.Width, r.Height, radius);
        }

        public virtual void DrawText(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            var transform = g.Transform;

            g.TranslateTransform(Position.X, Position.Y);

            g.DrawString(Name + " " + Extension, FontSmall, Brushes.Black, new PointF(3, 1));
            g.DrawString(Name + " " + Extension, FontSmall, Brushes.White, new PointF(2, 0));

            g.Transform = transform;
        }

        public virtual void DrawDescriptionText(Graphics g, RenderingEngine e, DrawableBase parent, String text)
        {
            var r = DescriptionBounds;

            StringFormat format = new StringFormat( StringFormatFlags.NoClip);

            r = new RectangleF(r.X + 2,r.Y +2,r.Width,r.Height);

            var r1 = new RectangleF(r.X + 0.5f, r.Y + 0.5f, r.Width,r.Height);
            var r2 = new RectangleF(r.X , r.Y , r.Width, r.Height);

            g.DrawString(text, FontVerySmall, Brushes.Black, r1, format);
            g.DrawString(text, FontVerySmall, Brushes.White, r2, format);
        }
        
        public virtual void DrawGlyph(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            ExtendedGraphics extendedGraphics = new ExtendedGraphics(g);
            
            float radius = 2;

            var brush = new SolidBrush(Color.FromArgb(30, 0, 0, 0));

            var r = DescriptionBounds;

            extendedGraphics.FillRoundRectangle(brush, r.X, r.Y, r.Width, r.Height, radius);
            //extendedGraphics.DrawRoundRectangle(DescriptionPen, r.X, r.Y, r.Width, r.Height, radius);
        }

        public virtual void DrawSelection(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            if (IsMouseOver)
            {
                DrawSelection(g, e, parent, MouseOverPen);
            }

            if (IsSelected)
            {
                DrawSelection(g, e, parent, SelectedPen);
            }
        }

        // --------------------------------------------------------------------------------

        public override void Update(RenderingEngine e, DrawableBase parent, DrawableBase previous)
        {
            InputOutputBase lastInput = null;
            foreach (var input in Inputs)
            {
                input.Update(e, this, lastInput);
                input.UpdateMouseService(e);
                lastInput = input;
            }

            InputOutputBase lastOutput = null;
            foreach (var output in Outputs)
            {
                output.Update(e, this, lastOutput);
                output.UpdateMouseService(e);
                lastOutput = output;
            }

            UIComponent lastUiComponent = null;
            foreach (var uiComponent in Components)
            {
                uiComponent.Update(e,this, lastUiComponent);
                uiComponent.UpdateMouseService(e);
                lastUiComponent = uiComponent;
            }
        }

        public override void UpdateMouseUp(RenderingEngine e, DrawableBase parent, DrawableBase previous)
        {
            UIComponent lastUiComponent = null;
            foreach (var uiComponent in Components)
            {
                uiComponent.UpdateMouseUp(e,this, lastUiComponent);
                lastUiComponent = uiComponent;
            }

            InputOutputBase lastIo = null;
            foreach (var input in Inputs)
            {
                input.UpdateMouseUp(e, this, lastIo);
                lastIo = input;
            }
            base.UpdateMouseUp(e, parent, previous);
        }

        public override void UpdateMouseDown(RenderingEngine e, DrawableBase parent, DrawableBase previous)
        {
            UIComponent lastUiComponent = null;
            foreach (var uiComponent in Components)
            {
                uiComponent.UpdateMouseDown(e, this, lastUiComponent);
                lastUiComponent = uiComponent;
            }

            InputOutputBase lastIo = null;
            foreach (var input in Inputs)
            {
                input.UpdateMouseDown(e, this, lastIo);
                lastIo = input;
            }
            base.UpdateMouseDown(e, parent, previous);
        }

        //public override void UpdateKeyboardUp(RenderingEngine e, DrawableBase parent, DrawableBase previous)
        //{
        //    UIComponent lastUiComponent = null;
        //    foreach (var uiComponent in Components)
        //    {
        //        uiComponent.UpdateKeyboardUp(e, this, lastUiComponent);
        //        lastUiComponent = uiComponent;
        //    }
        //    base.UpdateKeyboardUp(e, parent, previous);
        //}

        public override void UpdateMouseMove(RenderingEngine e, DrawableBase parent, DrawableBase previous)
        {
            //Console.WriteLine("IsSelected: {0} LeftDown:{1}", IsSelected, e.MouseService.LeftMouseDown);

            if (e.SelectionService.Input == null && e.SelectionService.Output == null)
            {
                if (IsSelected && e.MouseService.LeftMouseDown)
                {
                    Move(e, parent, previous);
                }
            }

            InputOutputBase lastIo = null;
            foreach (var input in Inputs)
            {
                input.UpdateMouseMove(e, this, lastIo);
                lastIo = input;
            }
        }

        public void Move(RenderingEngine e, DrawableBase parent, DrawableBase previous)
        {
            var p = Position;
            var m = e.TranslatedMousePosition;

            PointF shift = new PointF(p.X - e.SelectionService.MouseLeftDownMovePositionLast.X, p.Y - e.SelectionService.MouseLeftDownMovePositionLast.Y);
            Position = new PointF(m.X + shift.X, m.Y + shift.Y);

            Cursor.Current = Cursors.SizeAll;

        }

        public void DrawSelection(Graphics g, RenderingEngine e, DrawableBase parent, Pen pen)
        {
            DrawRectangleF(g, pen, SelectionBounds);
        }

        // --------------------------------------------------------------------------------

        public void Process<T>(Processor p, ProcessingNode<T> node) where T : SwitchBase
        {
            if (p.RenderingEngine.DebugMode)
            {
                OnProcessDebug(p, node);
            }
            OnProcess(p,node);
            DebugMode = false;
        }

        public void ProcessData<T>(Processor p, ProcessingNode<T> node) where T : SwitchBase
        {
            if (p.RenderingEngine.DebugMode)
            {
                OnProcessDataDebug(p, node);
            }
            OnProcessData(p, node);
            DebugDataMode = false;
        }

        public void OnProcessDebug<T>(Processor p, ProcessingNode<T> node) where T : SwitchBase
        {
            DebugMode = true;
            p.Redraw();
            p.Wait(p.RenderingEngine.DebugTime);
        }

        public void OnProcessDataDebug<T>(Processor p, ProcessingNode<T> node) where T : SwitchBase
        {
            DebugDataMode = true;
            p.Redraw();
            p.Wait(p.RenderingEngine.DebugTime);
        }

        protected virtual void OnProcess<T>(Processor p, ProcessingNode<T> node) where T : SwitchBase { }
        protected virtual void OnProcessData<T>(Processor p, ProcessingNode<T> node) where T : SwitchBase { }
        

        public void Initialize(RenderingEngine engine)
        {
            RenderingEngine = engine;
            OnInitialize(engine);
            foreach (var inputBase in Inputs)
            {
                inputBase.Initialize(engine, this);
            }
            foreach (var outputBase in Outputs)
            {
                outputBase.Initialize(engine, this);
            }
            foreach (var component in Components)
            {
                component.Initialize(engine, this);
            }
            Group = OnSetGroup();
        }

        protected virtual void OnInitialize(RenderingEngine engine)
        {

        }

        public virtual void OnRegisterEvents(ProcessingTree<SwitchBase> tree, RenderingEngine engine)
        {
            
        }
    }
}
