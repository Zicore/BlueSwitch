﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.IO;
using BlueSwitch.Base.Processing;
using BlueSwitch.Base.Services;
using BlueSwitch.Base.Trigger;
using BlueSwitch.Base.Trigger.Types;
using Newtonsoft.Json;
using NLog;
using XnaGeometry;
using Zicore.Settings.Json;
using Matrix = System.Drawing.Drawing2D.Matrix;

namespace BlueSwitch.Base.Components.Base
{
    public class RenderingEngine
    {
        Timer _tickerProvider = new Timer { Interval = 100, Enabled = true};
        
        private Logger _log = LogManager.GetCurrentClassLogger();

        [JsonIgnore]
        public bool DebugMode
        {
            get { return _debugMode; }
            set
            {
                if (value != _debugMode)
                {
                    _debugMode = value;
                    OnDebugModeChanged();
                }
            }
        }

        [JsonIgnore]
        public bool PreventContextMenu { get; set; } = false;

        [JsonIgnore]
        public bool DesignMode { get; set; } = true;

        [JsonIgnore]
        protected static Font FontInfo = new Font(new FontFamily("Calibri"), 30, FontStyle.Bold);


        [JsonIgnore]
        public TimeSpan DebugTime { get; set; } = new TimeSpan(0,0,0,0,300);

        public BlueSwitchProject CurrentProject { get; set; } = new BlueSwitchProject(); // empty


        [JsonIgnore]
        public PointF TranslatedMousePosition
        {
            get {  return new PointF(MouseService.Position.X / CurrentProject.Zoom - CurrentProject.Translation.X, MouseService.Position.Y/ CurrentProject.Zoom - CurrentProject.Translation.Y );}
        }
        
        public PointF TranslatePoint(PointF mouse)
        {
            return new PointF(mouse.X / CurrentProject.Zoom - CurrentProject.Translation.X, mouse.Y / CurrentProject.Zoom - CurrentProject.Translation.Y);
        }

        public MouseService MouseService { get; set; }
        public KeyboardService KeyboardService { get; set; }
        public SelectionService SelectionService { get; set; }
        public ReflectionService ReflectionService { get; set; } = new ReflectionService();

        public readonly List<SwitchBase> AvailableSwitches;
        private bool _debugMode = true;

        static Pen _linePen = new Pen(Color.FromArgb(200,30,30,30), 4.0f) { LineJoin = LineJoin.Round, EndCap = LineCap.Round, StartCap = LineCap.Round};

        public EventManager EventManager { get; }
        public ProcessorCompiler ProcessorCompiler { get; }

        public event EventHandler DebugModeChanged;
        public event EventHandler ProjectLoaded;

        public RenderingEngine()
        {
            ProcessorCompiler = new ProcessorCompiler(this);
            EventManager = new EventManager(this);
            MouseService = new MouseService(this);
            KeyboardService = new KeyboardService(this);

            MouseService.MouseMove += MouseServiceOnMouseMove;
            MouseService.MouseDown += MouseServiceOnMouseDown;
            MouseService.MouseUp += MouseServiceOnMouseUp;

            SelectionService = new SelectionService(this);
            SelectionService.Completed += SelectionServiceOnCompleted;
            SelectionService.InComplete += SelectionServiceOnInComplete;

            _tickerProvider.Tick += TickerProviderOnTick;

            ProcessorCompiler.CompileStart += ProcessorCompilerOnCompileStart;
            ProcessorCompiler.Finished += ProcessorCompilerOnFinished;

            AvailableSwitches = ReflectionService.LoadAddons(this);
        }

        private void ProcessorCompilerOnFinished(object sender, EventArgs eventArgs)
        {
            DesignMode = true;
        }

        private void ProcessorCompilerOnCompileStart(object sender, EventArgs eventArgs)
        {
            DesignMode = false;
        }

        private void TickerProviderOnTick(object sender, EventArgs e)
        {
            EventManager.Run(EventTypeBase.TimerTick);
        }

        public event EventHandler Redraw;

        public void RequestRedraw()
        {
            Redraw?.Invoke(this, EventArgs.Empty);
        }

        private void MouseServiceOnMouseUp(object sender, MouseEventArgs e)
        {
            UpdateMouseUp(e);
        }

        private void MouseServiceOnMouseDown(object sender, MouseEventArgs e)
        {
            Update();
            UpdateMouseDown(e);
        }

        private void MouseServiceOnMouseMove(object sender, MouseEventArgs e)
        {
            Update();
            UpdateMouseMove(e);
        }

        private void SelectionServiceOnInComplete(object sender, EventArgs eventArgs)
        {
            if (DesignMode)
            {
            var selected = SelectionService.SelectedInputOutput;
            if (selected != null)
            {
                var selector =
                    CurrentProject.Connections.FirstOrDefault(
                        x =>
                            x.FromInputOutput.InputOutput == selected.InputOutput ||
                            x.ToInputOutput.InputOutput == selected.InputOutput);
                if (selector != null)
                {
                    CurrentProject.RemoveConnection(selector);
                }
            }
        }
    }

        private void AddOrReplaceConnection()
        {
            if (DesignMode)
            {
                var connection = new Connection(SelectionService.Input, SelectionService.Output);
                var selector =
                    CurrentProject.Connections.FirstOrDefault(
                        x => x.FromInputOutput.InputOutput == connection.FromInputOutput.InputOutput);
                if (selector != null)
                {
                    CurrentProject.RemoveConnection(selector);
                }

                CurrentProject.AddConnection(connection);
            }
        }

        private void SelectionServiceOnCompleted(object sender, EventArgs eventArgs)
        {
            AddOrReplaceConnection();
        }
        
        public void Draw(Graphics g, RectangleF viewport)
        {
            var transform = g.Transform;

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            DrawRenderingInfo(g, viewport);

            Matrix mat = new Matrix();
            mat.Translate(CurrentProject.Translation.X, CurrentProject.Translation.Y);
            mat.Scale(CurrentProject.Zoom, CurrentProject.Zoom, MatrixOrder.Append);
            //mat.Translate(CurrentProject.Translation.X - ((CurrentProject.Zoom * ClientSize.Width) - ClientSize.Width), CurrentProject.Translation.Y - ((CurrentProject.Zoom * ClientSize.Height) - ClientSize.Height)); 
            g.Transform = mat;


            if (CurrentProject.Ready)
            {
                foreach (var connection in CurrentProject.Connections)
                {
                    var p1 =
                        connection.FromInputOutput.InputOutput.GetTranslationCenter(connection.FromInputOutput.Origin);
                    var p2 = connection.ToInputOutput.InputOutput.GetTranslationCenter(connection.ToInputOutput.Origin);

                    DrawConnection(g, connection.ToInputOutput.InputOutput.Signature.Pen, _linePen, p1, p2);
                }

                if (SelectionService.InputOutputAvailable)
                {
                    var io = SelectionService.SelectedInputOutput;
                    if (io != null)
                    {
                        var p1 = TranslatedMousePosition;
                        var p2 = io.InputOutput.GetTranslationCenter(io.Origin);
                        DrawConnection(g, io.InputOutput.Signature.Pen, _linePen, p1, p2);
                    }
                }

                foreach (var item in CurrentProject.Items)
                {
                    item.Draw(g, this, null);
                }
            }

            g.Transform = transform;
        }

        public void DrawGrid(Graphics g, RectangleF viewport)
        {
            int maxGrid = (int)((CurrentProject.Zoom/500)*1000);

            float stepX = viewport.Width/maxGrid;
            float stepY = viewport.Height / maxGrid;

            for (int i = 1; i < maxGrid; i++)
            {
                var x = i * stepX;

                g.DrawLine(Pens.Black,x,viewport.Top, x, viewport.Bottom);
            }

            for (int i = 1; i < maxGrid; i++)
            {
                var y = i * stepY;

                g.DrawLine(Pens.Black, viewport.Left, y, viewport.Right, y);
            }
        }

        public void DrawRenderingInfo(Graphics g, RectangleF viewport)
        {
            if (DesignMode)
            {
                var p = new PointF(viewport.Width - 152, 4);
                var p2 = new PointF(viewport.Width - 150, 5);
                g.DrawString("DESIGN", FontInfo, Brushes.CornflowerBlue, p2);
                g.DrawString("DESIGN", FontInfo, Brushes.Black, p);
            }
            else
            {
                var p = new PointF(viewport.Width - 236, 4);
                var p2 = new PointF(viewport.Width - 234, 5);
                g.DrawString("SIMULATION", FontInfo, Brushes.OrangeRed, p2);
                g.DrawString("SIMULATION", FontInfo, Brushes.Black, p);
            }
        }

        public void LoadProject(String filePath)
        {
            CurrentProject = new BlueSwitchProject();
            try
            {
                CurrentProject = JsonSerializable.Load<BlueSwitchProject>(filePath);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            CurrentProject.Initialize(this);
            OnProjectLoaded();
        }

        public void Stop()
        {
            ProcessorCompiler.Stop(CurrentProject);
        }

        public void Compile()
        {
            ProcessorCompiler.Compile(CurrentProject);
        }

        public void Start()
        {
            EventManager.Run(EventTypeBase.Start);
        }

        public void CompileAndStart()
        {
            Stop();
            Compile();
            Start();
        }

        // Mittelpunkt
        //public void DrawConnection(Graphics g, Pen pen, Pen pen2, PointF p1, PointF p2)
        //{
        //    Vector2 v1 = new Vector2(p1.X, p1.Y);
        //    Vector2 v2 = new Vector2(p2.X, p2.Y);

        //    var distance = Vector2.Distance(v1, v2) * 0.05;

        //    var mid1 = Vector2.Lerp(v1, v2, 0.25);
        //    var mid2 = Vector2.Lerp(v1, v2, 0.75);

        //    mid1 = mid1 - Perpendicular(Vector2.Normalize(mid1)) * distance;
        //    mid2 = mid2 + Perpendicular(Vector2.Normalize(mid2)) * distance;

        //    PointF b1 = new PointF((float)mid1.X, (float)mid1.Y);
        //    PointF b2 = new PointF((float)mid2.X, (float)mid2.Y);


        //    g.DrawCurve(pen2, new PointF[] { p1, b1, b2, p2 });
        //    g.DrawCurve(pen, new PointF[] { p1, b1, b2, p2 });

        //    if (DebugMode)
        //    {
        //        DrawRect(g, p1);
        //        DrawRect(g, b1);
        //        DrawRect(g, b2);
        //        DrawRect(g, p2);
        //    }
        //}

        // Abhängig von X Achse
        public void DrawConnection(Graphics g, Pen pen, Pen pen2, PointF p1, PointF p2)
        {
            Vector2 v1 = new Vector2(p1.X, p1.Y);
            Vector2 v2 = new Vector2(p2.X, p2.Y);

            float overhangX = (Math.Max(p1.X, p2.X) - Math.Min(p1.X, p2.X)) * 0.85f;
            
            overhangX = Math.Min(overhangX, 100);
            overhangX = Math.Max(30, overhangX);

            float overhangY = (Math.Max(p1.Y, p2.Y) - Math.Min(p1.Y, p2.Y)) * 0.25f;

            overhangY = Math.Min(overhangY, 20);
            overhangY = Math.Max(2, overhangY);

            PointF b1 = new PointF(p1.X - overhangX, p1.Y - overhangY);
            PointF b2 = new PointF(p2.X + overhangX, p2.Y + overhangY);
            
            g.DrawBezier(pen2,p1,b1,b2,p2);
            g.DrawBezier(pen, p1, b1, b2, p2);
        }

        public static void DrawRect(Graphics g,PointF p)
        {
            g.FillRectangle(Brushes.Red, new RectangleF(new PointF(p.X - 1f,p.Y - 1f), new SizeF(3,3)));
        }

        public static Vector2 Perpendicular(Vector2 vector)
        {
            return new Vector2(vector.Y, -vector.X);
        }

        public static PointF Add(PointF p1, PointF p2)
        {
            return new PointF(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static PointF Substract(PointF p1, PointF p2)
        {
            return new PointF(p1.X - p2.X, p1.Y - p2.Y);
        }

        public void Update()
        {
            if (CurrentProject.Ready)
            {
                foreach (var item in CurrentProject.Items)
                {
                    item.Update(this, null, null);
                    item.UpdateMouseService(this);
                }
            }
        }

        public void UpdateMouseMove(MouseEventArgs e)
        {
            if (CurrentProject.Ready)
            {
                foreach (var item in CurrentProject.Items)
                {
                    item.UpdateMouseMove(this, null, null);
                }
            }

            TranslateViewPort();
        }

        public void TranslateViewPort()
        {
            if (MouseService.RightMouseDown)
            {
                var translation = CurrentProject.Translation;
                var mouse = MouseService.Position;

                PointF shift = new PointF(mouse.X - SelectionService.MouseRightDownMoveTranslationPositionLast.X, mouse.Y  - SelectionService.MouseRightDownMoveTranslationPositionLast.Y);

                PointF newTranslation = new PointF(translation.X + shift.X / CurrentProject.Zoom, translation.Y + shift.Y / CurrentProject.Zoom);

                var distance = Vector2.Distance(new Vector2(newTranslation.X, newTranslation.Y), new Vector2(CurrentProject.Translation.X, CurrentProject.Translation.Y));
                
                CurrentProject.Translation = newTranslation;

                PreventContextMenu = distance > 0.005f; // Wenn die Distanz beider Punkte > also angegeben ist, soll kein Kontext Menü angezeigt werden.

                Cursor.Current = Cursors.Hand;

                SelectionService.MouseRightDownMoveTranslationPositionLast = mouse;
            }
        }

        public void UpdateMouseDown(MouseEventArgs e)
        {
            if (CurrentProject.Ready)
            {
                foreach (var item in CurrentProject.Items)
                {
                    item.UpdateMouseDown(this, null, null);
                }
            }
        }

        public void UpdateMouseUp(MouseEventArgs e)
        {
            if (CurrentProject.Ready)
            {
                foreach (var item in CurrentProject.Items)
                {
                    item.UpdateMouseUp(this, null, null);
                }
            }
        }
        
        public PointF GetTranslation(PointF p)
        {
            return new PointF(p.X - CurrentProject.Translation.X , p.Y - CurrentProject.Translation.Y);
        }

        protected virtual void OnDebugModeChanged()
        {
            DebugModeChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnProjectLoaded()
        {
            ProjectLoaded?.Invoke(this, EventArgs.Empty);
        }
    }
}
