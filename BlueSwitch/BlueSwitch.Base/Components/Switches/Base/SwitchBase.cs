﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Types;
using BlueSwitch.Base.Components.UI;
using BlueSwitch.Base.Drawing.Extended;
using BlueSwitch.Base.IO;
using BlueSwitch.Base.Meta.Search;
using BlueSwitch.Base.Processing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace BlueSwitch.Base.Components.Switches.Base
{
    public class SwitchJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(SwitchBase));
        }

        public override bool CanWrite { get { return false; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            try
            {
                reader = token.CreateReader();
                var result = serializer.Deserialize(reader);
                return result;
            }
            catch (JsonSerializationException ex)
            {
                reader = token.CreateReader();
                object value = Activator.CreateInstance(typeof(UnknownSwitch));
                serializer.Populate(reader, value);
                UnknownSwitch sw = value as UnknownSwitch;

                if (sw != null)
                {
                    var resolved = Engine.StaticNamespaceResolver.Resolve(sw);
                    if (resolved != null)
                    {
                        reader = token.CreateReader();
                        serializer.Populate(reader, resolved);
                        return resolved;
                    }
                }

                return value;
            }
            finally
            {

            }
        }
    }

    [JsonConverter(typeof(SwitchJsonConverter))]
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
        public bool DebugMode { get; set; }

        [JsonIgnore]
        public bool DebugDataMode { get; set; }

        [JsonIgnore]
        public int ExtraRows { get; set; } = 0;

        [JsonIgnore]
        public int MinRows { get; set; } = 1;

        [JsonIgnore]
        public float RowHeight = 18.0f;

        [JsonIgnore]
        public bool IsStart { get; set; }

        [JsonIgnore]
        public bool AutoDiscoverDisabled { get; set; }

        [JsonIgnore]
        int MinVariableInputs { get; set; } = 0;

        [JsonIgnore]
        int MinVariableOutputs { get; set; } = 0;

        [JsonIgnore]
        public int TotalInputs
        {
            get { return ExtraVariableInputs + MinVariableInputs; }
        }

        [JsonIgnore]
        public int TotalOutputs
        {
            get { return ExtraVariableOutputs + MinVariableOutputs; }
        }

        public int ExtraVariableInputs { get; set; } = 0;

        public int ExtraVariableOutputs { get; set; } = 0;

        [JsonIgnore]
        public bool HasVariableInputs { get; set; } = false;

        [JsonIgnore]
        public bool HasVariableOutputs { get; set; } = false;

        readonly AddPinComponent _addInputPin = new AddPinComponent(true);
        readonly AddPinComponent _addOutputPin = new AddPinComponent(false);

        public void ActivateInputAdd(PinDescription description, int minInputs)
        {
            HasVariableInputs = true;
            _addInputPin.Description = description;
            MinVariableInputs = minInputs;
            ExtraRows = 1;
        }

        public void ActivateOutputAdd(PinDescription description, int minOutputs)
        {
            HasVariableOutputs = true;
            _addOutputPin.Description = description;
            MinVariableOutputs = minOutputs;
            ExtraRows = 1;
        }

        protected SwitchBase()
        {

        }

        [JsonIgnore]
        public bool HasActionOutput { get; protected set; }

        private string _uniqueNameJson;
        private string _uniqueName;
        private string _displayName;

        [JsonIgnore]
        public string UniqueName
        {
            get
            {
                if (String.IsNullOrEmpty(_uniqueName))
                {
                    return TypeName;
                }
                return _uniqueName;
            }
            set
            {
                _uniqueName = value;
            }
        }

        // Unique Name Json Handling
        public virtual string UniqueNameJson
        {
            get
            {
                if (String.IsNullOrEmpty(_uniqueNameJson))
                {
                    return TypeNameJson;
                }
                return _uniqueNameJson;
            }
            set { _uniqueNameJson = value; }
        }

        [JsonIgnore]
        public string TypeNameJson { get; private set; }

        [JsonIgnore]
        public string FullTypeNameJson { get; private set; }

        public virtual string TypeName
        {
            get { return GetType().Name; }
            set { TypeNameJson = value; } // Json
        }

        public virtual string FullTypeName
        {
            get { return GetType().FullName; }
            set { FullTypeNameJson = value; } // Json
        }

        [JsonIgnore]
        public string Description { get; protected set; }

        [JsonIgnore]
        public string DisplayName
        {
            get
            {
                if (String.IsNullOrEmpty(_displayName))
                {
                    return UniqueName;
                }
                return _displayName;
            }
            set { _displayName = value; }
        }

        [JsonIgnore]
        public GroupBase Group { get; set; } = Groups.Base;

        public void SetData(int index, object value)
        {
            SetData(index, new DataContainer(value));
        }

        public void SetData(int index, DataContainer data)
        {
            OutputsSet[index].Data = data;
        }

        public DataContainer GetData(int index)
        {
            var input = InputsSet[index];

            if (input.UIComponent != null && !input.IsConnected(RenderingEngine))
            {
                input.Data = new DataContainer(input.UIComponent.GetData());
            }

            return input.Data;
        }

        public DataContainer GetData<T>(int index)
        {
            var data = GetData(index);

            if (data.Value != null)
            {
                data.Value = (T)Convert.ChangeType(data.Value, typeof(T));
            }
            else
            {
                data.Value = default(T);
            }

            return data;
        }

        public T GetDataValueOrDefault<T>(int index)
        {
            var data = GetData(index);
            if (data?.Value != null)
            {
                if (data.Value is T)
                {
                    return (T)data.Value;
                }

                var result = (T)Convert.ChangeType(data.Value, typeof(T));
                return result;
            }
            else
            {
                return default(T);
            }
        }

        public DataContainer GetOutputData(int index)
        {
            return OutputsSet[index].Data;
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
        public static Pen DescriptionPen { get; set; } = new Pen(Color.FromArgb(180, 30, 30, 30), 0.5f);

        [JsonIgnore]
        public static Pen MouseOverPen { get; set; } = new Pen(Color.LightCoral, 2.0f);

        [JsonIgnore]
        public static Pen SelectedPen { get; set; } = new Pen(Color.LimeGreen, 2.0f);

        [JsonIgnore]
        public static Brush MouseOverBrush { get; set; } = new SolidBrush(Color.LightGreen);

        protected static Font FontSmall = new Font(new FontFamily("Calibri"), 8, FontStyle.Regular);
        protected static Font FontSmall2 = new Font(new FontFamily("Calibri"), 9, FontStyle.Regular);

        protected static Font FontVerySmall = new Font(new FontFamily("Calibri"), 7, FontStyle.Regular);

        [JsonIgnore]
        public Dictionary<int,InputBase> InputsSet { get; set; } = new Dictionary<int, InputBase>();

        [JsonIgnore]
        public IEnumerable<InputBase> Inputs {
            get { return InputsSet.Values; }
        }

        [JsonIgnore]
        public Dictionary<int, OutputBase> OutputsSet { get; set; } = new Dictionary<int,OutputBase>();

        [JsonIgnore]
        public IEnumerable<OutputBase> Outputs
        {
            get { return OutputsSet.Values; }
        }

        [JsonIgnore]
        public List<UIComponent> Components { get; set; } = new List<UIComponent>();

        public int RowModifier
        {
            get { return IsCompact ? -1 : 0; }
        }

        [JsonIgnore]
        public override SizeF Size
        {
            get
            {
                var rows = Math.Max(Math.Max(InputsSet.Count, OutputsSet.Count), MinRows) + ExtraRows + RowModifier;
                return new SizeF(ColumnWidth, DescriptionHeight + rows * RowHeight);
            }
        }

        [JsonIgnore]
        public override SizeF SizeUntilExtraRow
        {
            get
            {
                var rows = Math.Max(Math.Max(InputsSet.Count, OutputsSet.Count), MinRows) + RowModifier;
                return new SizeF(ColumnWidth, DescriptionHeight + rows * RowHeight);
            }
        }

        [JsonIgnore]
        public override float DescriptionOffsetLeft
        {
            get
            {
                if (InputsSet.Count > 0)
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
                if (OutputsSet.Count > 0)
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
            int index = InputsSet.Count;
            input.Index = index;
            input.UIComponent = uiComponent;
            InputsSet.Add(index,input);
            return input;
        }

        public OutputBase AddOutput(OutputBase output, UIComponent uiComponent = null)
        {
            int index = OutputsSet.Count;
            output.Index = index;
            OutputsSet.Add(index,output);
            output.UIComponent = uiComponent;
            if (output.Signature is ActionSignature)
            {
                HasActionOutput = true;
            }
            return output;
        }

        public InputOutputBase AddOutput(Type type, UIComponent uiComponent = null)
        {
            return AddOutput(SignatureSingle.CreateOutput(type), uiComponent);
        }

        public InputOutputBase AddInput(Type type, UIComponent uiComponent = null)
        {
            return AddInput(SignatureSingle.CreateInput(type), uiComponent);
        }

        private void AddInput(PinDescription description)
        {
            UIComponent ui = description.CreateComponent();
            InputOutputBase io;
            if (description.ComponentType != null)
            {
                io = AddInput(description.ComponentType, ui);
            }
            else
            {
                io = AddInput(description.Signature, ui);
            }

            io.Initialize(RenderingEngine, this);
        }

        private void AddOutput(PinDescription description)
        {
            UIComponent ui = description.CreateComponent();
            InputOutputBase io;
            if (description.ComponentType != null)
            {
                io = AddOutput(description.ComponentType, ui);
            }
            else
            {
                io = AddOutput(description.Signature, ui);
            }
            io.Initialize(RenderingEngine, this);
        }

        // --------------------------------------------------------------------------------

        public override void Draw(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            DrawBody(g, e, parent);

            DrawInputOutputs(g, e, parent);

            DrawSelection(g, e, parent);

            DrawDescription(g, e, parent);
            
            DrawGlyph(g, e, parent);
            
            DrawText(g, e, parent);

            DrawComponents(g, e, parent);

            if (HasVariableInputs)
            {
                _addInputPin.Draw(g, e, this);
            }

            if (HasVariableOutputs)
            {
                _addOutputPin.Draw(g, e, this);
            }

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

        public virtual void DrawBody(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            ExtendedGraphics extendedGraphics = new ExtendedGraphics(g,e);

            var r = Rectangle;

            Brush brush;

            if (IsSelected)
            {
                brush = GetMainSelectionBrush(r,e);
            }
            else if (DebugMode)
            {
                brush = GetDebugBrush(r, e);
            }
            else if (DebugDataMode)
            {
                brush = GetDebugDataBrush(r, e);
            }
            else
            {
                brush = GetMainBrush(r, e);
            }

            const float radius = 3;

            extendedGraphics.FillRoundRectangle(brush, r.X, r.Y, r.Width, r.Height, radius);
            extendedGraphics.DrawRoundRectangle(Pen, r.X, r.Y, r.Width, r.Height, radius);
        }

        public virtual void DrawComponents(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            foreach (var uiComponent in Components)
            {
                uiComponent.Draw(g, e, this);
            }
        }
        
        public virtual void DrawDescription(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            ExtendedGraphics extendedGraphics = new ExtendedGraphics(g, e);

            var rect = Rectangle;

            const float offset = 2f;
            const float offset2 = 4f;

            float compactOffset = DescriptionOffsetLeftCompact;
            float compactOffset2 = DescriptionOffsetLeftCompact + DescriptionOffsetRightCompact;

            var r = new RectangleF(rect.X + compactOffset, rect.Y + offset, rect.Width - compactOffset2, DescriptionHeight - offset2);

            float radius = 2;

            var brush = new SolidBrush(Color.FromArgb(80, 0, 0, 0));

            extendedGraphics.FillRoundRectangle(brush, r.X, r.Y, r.Width, r.Height, radius);
            extendedGraphics.DrawRoundRectangle(DescriptionPen, r.X, r.Y, r.Width, r.Height, radius);
        }

        public virtual void DrawText(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            var transform = g.Transform;

            float compactOffset = DescriptionOffsetLeftCompact;
            float compactOffset2 = DescriptionOffsetLeftCompact + DescriptionOffsetRightCompact;

            g.TranslateTransform(Position.X + compactOffset, Position.Y + 1);

            var renderingHint = g.TextRenderingHint;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            g.DrawString(DisplayName + " " + Extension, FontSmall, Brushes.Black, new PointF(-1, 1));
            g.DrawString(DisplayName + " " + Extension, FontSmall, Brushes.White, new PointF(-0.5f, 0.5f));

            g.TextRenderingHint = renderingHint;

            g.Transform = transform;
        }

        public virtual void DrawDescriptionText(Graphics g, RenderingEngine e, DrawableBase parent, String text)
        {
            var r = DescriptionBounds;

            StringFormat format = new StringFormat(StringFormatFlags.NoClip);

            r = new RectangleF(r.X + 2, r.Y + 2, r.Width, r.Height);

            var r1 = new RectangleF(r.X + 0.5f, r.Y + 0.5f, r.Width, r.Height);
            var r2 = new RectangleF(r.X, r.Y, r.Width, r.Height);

            var renderingHint = g.TextRenderingHint;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            g.DrawString(text, FontVerySmall, Brushes.Black, r1, format);
            g.DrawString(text, FontVerySmall, Brushes.White, r2, format);

            g.TextRenderingHint = renderingHint;
        }

        public virtual void DrawGlyph(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            var r = DescriptionBounds;
            if (r.Height > 0)
            {
                ExtendedGraphics extendedGraphics = new ExtendedGraphics(g, e);

                float radius = 2;

                var brush = new SolidBrush(Color.FromArgb(30, 0, 0, 0));
                extendedGraphics.FillRoundRectangle(brush, r.X, r.Y, r.Width, r.Height, radius);
            }
        }

        public virtual void DrawHelp(Graphics g, RenderingEngine e, DrawableBase parent)
        {
            e.HelpService.Draw(g, this, parent, e);
        }

        public virtual void DrawSelection(Graphics g, Engine e, DrawableBase parent)
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





        public virtual Brush GetMainBrush(RectangleF rectangle, RenderingEngine e)
        {
            if (e.PerformanceMode == PerformanceMode.HighQuality)
            {
                var brush = new LinearGradientBrush(rectangle, Color.Black, Color.Black, 90, true);

                ColorBlend cb = new ColorBlend();

                cb.Positions = new[] { 0, 0.2f, 0.5f, 1 };
                cb.Colors = new Color[]
                {
                    Color.FromArgb(120, 0, 0, 0), Color.FromArgb(80, 0, 0, 0), Color.FromArgb(80, 0, 0, 0),
                    Color.FromArgb(60, 30, 160, 255)
                };

                brush.InterpolationColors = cb;

                return brush;
            }

            return Brushes.SteelBlue;
        }

        public virtual Brush GetDebugBrush(RectangleF rectangle, RenderingEngine e)
        {
            if (e.PerformanceMode == PerformanceMode.HighQuality)
            {
                var brush = new LinearGradientBrush(rectangle, Color.Black, Color.Black, 90, true);

                ColorBlend cb = new ColorBlend();

                cb.Positions = new[] { 0, 0.2f, 0.5f, 1 };
                cb.Colors = new Color[] { Color.FromArgb(120, 0, 0, 0), Color.FromArgb(150, 255, 0, 0), Color.FromArgb(150, 255, 0, 0), Color.FromArgb(60, 30, 160, 255) };

                brush.InterpolationColors = cb;

                return brush;
            }

            return Brushes.IndianRed;
        }

        public virtual Brush GetDebugDataBrush(RectangleF rectangle, RenderingEngine e)
        {
            if (e.PerformanceMode == PerformanceMode.HighQuality)
            {
                var brush = new LinearGradientBrush(rectangle, Color.Black, Color.Black, 90, true);

                ColorBlend cb = new ColorBlend();

                cb.Positions = new[] { 0, 0.2f, 0.5f, 1 };
                cb.Colors = new Color[]
                {
                    Color.FromArgb(120, 0, 0, 0), Color.FromArgb(160, 180, 10, 0), Color.FromArgb(160, 180, 10, 0),
                    Color.FromArgb(60, 30, 160, 255)
                };

                brush.InterpolationColors = cb;

                return brush;
            }

            return Brushes.DarkRed;
        }

        public virtual Brush GetMainSelectionBrush(RectangleF rectangle, RenderingEngine e)
        {
            if (e.PerformanceMode == PerformanceMode.HighQuality)
            {
                var brush = new LinearGradientBrush(rectangle, Color.Black, Color.Black, 90, true);

                ColorBlend cb = new ColorBlend();

                cb.Positions = new[] { 0, 0.2f, 0.5f, 1 };
                cb.Colors = new Color[]
                {
                    Color.FromArgb(120, 0, 0, 0), Color.FromArgb(80, 30, 144, 255), Color.FromArgb(80, 30, 144, 255),
                    Color.FromArgb(60, 30, 160, 255)
                };

                brush.InterpolationColors = cb;

                return brush;
            }

            return new SolidBrush(Color.FromArgb(30, 144, 255));
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
                uiComponent.Update(e, this, lastUiComponent);
                uiComponent.UpdateMouseService(e);
                lastUiComponent = uiComponent;
            }

            if (HasVariableInputs)
            {
                _addInputPin.Update(e, this, lastUiComponent);
                _addInputPin.UpdateMouseService(e);
            }

            if (HasVariableOutputs)
            {
                _addOutputPin.Update(e, this, lastUiComponent);
                _addOutputPin.UpdateMouseService(e);
            }
        }

        public override void UpdateMouseUp(RenderingEngine e, DrawableBase parent, DrawableBase previous)
        {
            UIComponent lastUiComponent = null;
            foreach (var uiComponent in Components)
            {
                uiComponent.UpdateMouseUp(e, this, lastUiComponent);
                lastUiComponent = uiComponent;
            }

            InputOutputBase lastIo = null;
            foreach (var input in Inputs)
            {
                input.UpdateMouseUp(e, this, lastIo);
                lastIo = input;
            }

            if (HasVariableInputs)
            {
                _addInputPin.UpdateMouseUp(e, this, lastIo);
            }

            if (HasActionOutput)
            {
                _addOutputPin.UpdateMouseUp(e, this, _addInputPin);
            }

            if (e.SelectionService.Input == null && e.SelectionService.Output == null && !e.SelectionService.StartSelectionRectangle)
            {
                if (IsSelected && e.MouseService.LeftMouseDown)
                {
                    Move(e, parent, previous,true);
                }
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

            if (HasVariableInputs)
            {
                _addInputPin.UpdateMouseDown(e, this, lastIo);
            }
            if (HasVariableOutputs)
            {
                _addOutputPin.UpdateMouseDown(e, this, HasVariableInputs ? (DrawableBase)_addInputPin : lastIo);
            }

            base.UpdateMouseDown(e, parent, previous);
        }

        public override void UpdateMouseMove(RenderingEngine e, DrawableBase parent, DrawableBase previous)
        {
            //Console.WriteLine("IsSelected: {0} LeftDown:{1}", IsSelected, e.MouseService.LeftMouseDown);

            if (e.SelectionService.Input == null && e.SelectionService.Output == null)
            {
                if (IsSelected && e.MouseService.LeftMouseDown)
                {
                    Move(e, parent, previous, false);
                }
            }

            InputOutputBase lastIo = null;
            foreach (var input in Inputs)
            {
                input.UpdateMouseMove(e, this, lastIo);
                lastIo = input;
            }

            if (HasVariableInputs)
            {
                _addInputPin.UpdateMouseMove(e, this, lastIo);
            }

            if (HasVariableOutputs)
            {
                _addOutputPin.UpdateMouseMove(e, this, HasVariableInputs ? (DrawableBase)_addInputPin : lastIo);
            }
        }

        public void Move(RenderingEngine e, DrawableBase parent, DrawableBase previous, bool snapToGrid)
        {
            var p = Position;
            var m = e.TranslatedMousePosition;

            PointF shift = new PointF(p.X - e.SelectionService.MouseLeftDownMovePositionLast.X, p.Y - e.SelectionService.MouseLeftDownMovePositionLast.Y);
            var newPosition = new PointF(m.X + shift.X, m.Y + shift.Y);

            Position = newPosition;

            if (snapToGrid && e.Settings.SnapToGridEnabled)
            {
                Position = SnapToGrid(newPosition, e.Settings.SnapToGridWidth);
            }

            Cursor.Current = Cursors.SizeAll;
        }

        private PointF SnapToGrid(PointF p, int gridWidth)
        {
            int x = (int)Math.Round(p.X / gridWidth) * gridWidth;
            int y = (int)Math.Round(p.Y / gridWidth) * gridWidth;

            return new PointF(x ,y);
        }

        public void DrawSelection(Graphics g, Engine e, DrawableBase parent, Pen pen)
        {
            DrawRectangleF(g, pen, SelectionBounds);
        }

        // --------------------------------------------------------------------------------

        public void Process<T>(Processor p, ProcessingNode<T> node) where T : SwitchBase
        {
            p.CurrentNode = node as ProcessingNode<SwitchBase>;
            p.Step++;

            if (p.RenderingEngine.DebugMode)
            {
                OnProcessDebug(p, node);
            }
            OnProcess(p, node);
            DebugMode = false;
        }

        public void ProcessData<T>(Processor p, ProcessingNode<T> node) where T : SwitchBase
        {
            p.CurrentNode = node as ProcessingNode<SwitchBase>;
            p.Step++;

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
        protected virtual void OnInitializeMetaInformation(Engine engine) { }

        public void InitializeMetaInformation(Engine engine)
        {
            engine.SearchService.AddSearchDescription(new SearchDescription(UniqueName)); // Basic Search Setup
            OnInitializeMetaInformation(engine);
        }

        public void Initialize(Engine renderingEngine)
        {
            RenderingEngine = renderingEngine;
            OnInitialize(renderingEngine);

            if (HasVariableInputs)
            {
                int inputs = ExtraVariableInputs + MinVariableInputs;
                for (int i = 0; i < inputs; i++)
                {
                    AddInput(_addInputPin.Description);
                }
            }

            if (HasVariableOutputs)
            {
                int outputs = ExtraVariableOutputs + MinVariableOutputs;
                for (int i = 0; i < outputs; i++)
                {
                    AddOutput(_addOutputPin.Description);
                }
            }

            foreach (var inputBase in Inputs)
            {
                inputBase.Initialize(renderingEngine, this);
            }
            foreach (var outputBase in Outputs)
            {
                outputBase.Initialize(renderingEngine, this);
            }
            foreach (var component in Components)
            {
                component.Initialize(renderingEngine, this);
            }

            if (HasVariableInputs)
            {
                _addInputPin.Initialize(renderingEngine, this);
                _addInputPin.Click += AddInputPinOnClick;
                _addInputPin.KeyUp += AddInputPinOnKeyUp;
            }

            if (HasVariableOutputs)
            {
                _addOutputPin.Initialize(renderingEngine, this);
                _addOutputPin.Click += AddOutputPinOnClick;
                _addOutputPin.KeyUp += AddOutputPinOnKeyUp;
            }

            Group = OnSetGroup();
        }

        /// <summary>
        /// Use this to define all properties of a switch.
        /// Set a UniqueName which should be unique across all switches.
        /// Set a DisplayName which can whatever name you want to be displayed.
        /// </summary>
        /// <param name="renderingEngine"></param>
        protected virtual void OnInitialize(Engine renderingEngine)
        {

        }

        public virtual void OnRegisterEvents(ProcessingTree<SwitchBase> tree, Engine renderingEngine)
        {

        }

        // --------------------------------------------------------------------------------

        private void AddInputPinOnClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                AddInput(_addInputPin.Description);
                ExtraVariableInputs++;
            }
        }

        private void AddInputPinOnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                if (ExtraVariableInputs > 0)
                {
                    int index = InputsSet.Count - 1;
                    var io = InputsSet[index];
                    InputsSet.Remove(index);
                    RenderingEngine.CurrentProject.RemoveConnection(io);
                    ExtraVariableInputs--;
                }
            }
        }

        private void AddOutputPinOnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                if (ExtraVariableOutputs > 0)
                {
                    int index = OutputsSet.Count - 1;
                    var io = OutputsSet[index];
                    OutputsSet.Remove(index);
                    RenderingEngine.CurrentProject.RemoveConnection(io);
                    ExtraVariableOutputs--;
                }
            }
        }

        private void AddOutputPinOnClick(object sender, MouseEventArgs e)
        {
            AddOutput(_addOutputPin.Description);
            ExtraVariableOutputs++;
        }
    }
}
