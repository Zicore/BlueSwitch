using System;
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
using Matrix = System.Drawing.Drawing2D.Matrix;

namespace BlueSwitch.Base.Components.Base
{
    public enum PerformanceMode
    {
        HighQuality = 1,
        Balanced = 2,
        HighPerformance = 4
    }

    public class RenderingEngine : Engine
    {
        public override event EventHandler DebugValueUpdated;

        protected Timer _tickerProvider = new Timer { Interval = 100, Enabled = true };

        protected Logger _log = LogManager.GetCurrentClassLogger();

        protected static Brush _selectionRectangleBrush = new SolidBrush(Color.FromArgb(120, 50, 50, 200));

        [JsonIgnore]
        public bool PreventContextMenu { get; protected set; } = false;

        [JsonIgnore]
        public EngineSettings Settings { get; set; } = new EngineSettings();

        [JsonIgnore]
        protected static Font FontInfo = new Font(new FontFamily("Calibri"), 30, FontStyle.Bold);
        
        protected static Pen _linePen = new Pen(Color.FromArgb(200, 30, 30, 30), 4.0f) { LineJoin = LineJoin.Round, EndCap = LineCap.Round, StartCap = LineCap.Round };

        

        public RenderingEngine() : base()
        {
            LoadSettings();

            MouseService = new MouseService(this);
            KeyboardService = new KeyboardService(this);
            SelectionService = new SelectionService(this);

            SelectionService.Completed += SelectionServiceOnCompleted;
            SelectionService.InComplete += SelectionServiceOnInComplete;

            MouseService.MouseMove += MouseServiceOnMouseMove;
            MouseService.MouseDown += MouseServiceOnMouseDown;
            MouseService.MouseUp += MouseServiceOnMouseUp;

            _tickerProvider.Tick += TickerProviderOnTick;

            ProcessorCompiler.CompileStart += ProcessorCompilerOnCompileStart;
            ProcessorCompiler.Finished += ProcessorCompilerOnFinished;
        }

        private void TickerProviderOnTick(object sender, EventArgs e)
        {
            EventManager.Run(EventTypeBase.TimerTick);
        }

        public void LoadSettings()
        {
            try
            {
                Settings = JsonSerializable.Load<EngineSettings>(EngineSettings.SettingsFilePath);
            }
            catch
            {
                Settings = new EngineSettings();
                Settings.Save(EngineSettings.SettingsFilePath);
            }
        }

        public void SaveSettings()
        {
            Settings.Save(EngineSettings.SettingsFilePath);
        }

        public List<SwitchBase> AvailableSwitchesForSearch
        {
            get
            {
                var list = new List<SwitchBase>(AvailableSwitches);
                //list.AddRange(CurrentProject.Variables);
                return list;
            }
        }

        public MouseService MouseService { get; set; }
        public KeyboardService KeyboardService { get; set; }
        public SelectionService SelectionService { get; set; }


        [JsonIgnore]
        public PointF TranslatedMousePosition
        {
            get { return new PointF(MouseService.Position.X / CurrentProject.Zoom - CurrentProject.Translation.X, MouseService.Position.Y / CurrentProject.Zoom - CurrentProject.Translation.Y); }
        }

        public PointF TranslatePoint(PointF mouse)
        {
            return new PointF(mouse.X / CurrentProject.Zoom - CurrentProject.Translation.X, mouse.Y / CurrentProject.Zoom - CurrentProject.Translation.Y);
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

                    //Kontextmenü Injecten

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
            
            var mat = new Matrix();
            mat.Translate(CurrentProject.Translation.X, CurrentProject.Translation.Y);
            mat.Scale(CurrentProject.Zoom, CurrentProject.Zoom, MatrixOrder.Append);
            g.Transform = mat;

            if (Settings.DrawGrid)
            {
                DrawGrid(g, viewport);
            }

            if (CurrentProject.Ready)
            {
                foreach (var connection in CurrentProject.Connections)
                {
                    var p1 =
                        connection.FromInputOutput.InputOutput.GetTranslationCenter(connection.FromInputOutput.Origin);
                    var p2 = connection.ToInputOutput.InputOutput.GetTranslationCenter(connection.ToInputOutput.Origin);
                    connection.Draw(this, g);

                    //DrawConnection(g, connection.ToInputOutput.InputOutput.Signature.Pen, _linePen, p1, p2);
                }

                if (SelectionService.InputOutputAvailable)
                {
                    var io = SelectionService.SelectedInputOutput;
                    if (io != null)
                    {
                        var p1 = SelectionService.DestinationConnectionPosition;
                        var p2 = io.InputOutput.GetTranslationCenter(io.Origin);
                        var pen = io.InputOutput.Signature.Pen;
                        if (!io.IsInput)
                        {
                            Connection.Draw(this, g, pen, p1, p2);
                        }
                        else
                        {
                            Connection.Draw(this, g, pen, p2, p1);
                        }
                    }
                }

                foreach (var item in CurrentProject.Items)
                {
                    item.Draw(g, this, null);
                }

                foreach (var item in CurrentProject.Items)
                {
                    item.DrawHelp(g, this, null);
                }
            }

            // Translated Rectangle Debug Modus
            //if (MouseService.LeftMouseDown)
            //{
            //    var rect = SelectionService.SelectionRectangleTranslated;
            //    g.DrawRectangle(Pens.DimGray, rect.X, rect.Y, rect.Width, rect.Height);
            //    g.FillRectangle(Brushes.PaleVioletRed, rect);
            //}

            g.Transform = transform;

            // Selection nach Rückgängig machen der Transformation, so ist die Box wieder an der richtigen Position
            DrawSelectionRectangle(g);

            DrawRenderingInfo(g, viewport);
        }

        private PointF SnapToGrid(PointF p, int gridWidth)
        {
            int x = (int)Math.Round(p.X / gridWidth) * gridWidth;
            int y = (int)Math.Round(p.Y / gridWidth) * gridWidth;

            return new PointF(x, y);
        }

        public void DrawGrid(Graphics g, RectangleF viewport)
        {
            float zoom = CurrentProject.Zoom;
            var pen = new Pen(Brushes.DarkGray, 2 / zoom);
            var penBlack = new Pen(Brushes.DimGray, 1.5f / zoom);

            int bigGridWidth = 200;
            float bigGridWithBy2 = bigGridWidth * 0.5f;
            int subGridWidthDivisor = 20;
            float gridWidth = bigGridWidth / (float)subGridWidthDivisor;

            RectangleF grid = new RectangleF(0, 0, bigGridWidth, bigGridWidth);

            PointF pBeforeSnap = new PointF(CurrentProject.Translation.X, CurrentProject.Translation.Y);
            PointF p = SnapToGrid(pBeforeSnap, bigGridWidth);

            float xoff = Math.Abs(p.X - pBeforeSnap.X) + bigGridWithBy2;
            float yoff = Math.Abs(p.Y - pBeforeSnap.Y) + bigGridWithBy2;
            viewport = new RectangleF(-p.X - bigGridWithBy2, -p.Y - bigGridWithBy2, xoff + ( viewport.Width) / zoom, yoff + (viewport.Height+ yoff) / zoom);


            int maxGridX = (int)Math.Floor(viewport.Width / grid.Width) + 1;
            int maxGridY = (int)Math.Floor(viewport.Height / grid.Height) + 1;

            int subGridX = (int)Math.Floor(gridWidth * maxGridX);
            int subGridY = (int)Math.Floor(gridWidth * maxGridY);

            GraphicsPath pathSubGrid = new GraphicsPath();
            GraphicsPath pathGrid = new GraphicsPath();

            if (Settings.DrawSubGrid)
            {
                for (int i = -1; i < subGridX; i++)
                {
                    var x = (i*(grid.Width/gridWidth)) + viewport.X;
                    if (i%subGridWidthDivisor != 0)
                    {
                        pathSubGrid.AddLine(x, viewport.Top, x, viewport.Bottom);
                        pathSubGrid.CloseFigure();
                    }
                }

                for (int i = -1; i < subGridY; i++)
                {
                    var y = i*(grid.Height/gridWidth) + viewport.Y;
                    if (i%subGridWidthDivisor != 0)
                    {
                        pathSubGrid.AddLine(viewport.Left, y, viewport.Right, y);
                        pathSubGrid.CloseFigure();
                    }
                }

                g.DrawPath(pen, pathSubGrid);
            }

            for (int i = -1; i < maxGridX; i++)
            {
                var x = i * grid.Width + viewport.X;
                pathGrid.AddLine(x, viewport.Top, x, viewport.Bottom);
                pathGrid.CloseFigure();
            }

            for (int i = -1; i < maxGridY; i++)
            {
                var y = i * grid.Height + viewport.Y;
                pathGrid.AddLine(viewport.Left, y, viewport.Right, y);
                pathGrid.CloseFigure();
            }
            g.DrawPath(penBlack, pathGrid);
        }
        
        public void Zoom(RectangleF clientSize,float zoom)
        {
            PointF m = MouseService.Position;

            if (CurrentProject.Zoom + zoom > 0.4)
            {
                Vector2 mausOld = new Vector2(m.X, m.Y) / CurrentProject.Zoom;
                Vector2 mausNew = new Vector2(m.X, m.Y) / (CurrentProject.Zoom + zoom);

                Vector2 translation = new Vector2(CurrentProject.Translation.X, CurrentProject.Translation.Y);
                Vector2 centerOld = new Vector2(clientSize.Width * 0.5f,clientSize.Height * 0.5f) / CurrentProject.Zoom;
                Vector2 centerNew = new Vector2(clientSize.Width * 0.5f, clientSize.Height * 0.5f) / (CurrentProject.Zoom + zoom);

                Vector2 abweichung = (mausNew - mausOld);

                CurrentProject.Zoom += zoom;

                PointF p = new PointF((float)translation.X + (float)abweichung.X, (float)translation.Y + (float)abweichung.Y);

                CurrentProject.Translation = p;
            }

            RequestRedraw();
        }

        public void DrawRenderingInfo(Graphics g, RectangleF viewport)
        {
            if (DesignMode)
            {
                var p = new PointF(viewport.Width - 196, 4);
                var p2 = new PointF(viewport.Width - 194, 5);
                g.DrawString("DESIGN ⏸", FontInfo, Brushes.CornflowerBlue, p2);
                g.DrawString("DESIGN ⏸", FontInfo, Brushes.Black, p);
            }
            else
            {
                var p = new PointF(viewport.Width - 286, 4);
                var p2 = new PointF(viewport.Width - 284, 5);
                g.DrawString("SIMULATION ⏵", FontInfo, Brushes.OrangeRed, p2);
                g.DrawString("SIMULATION ⏵", FontInfo, Brushes.Black, p);
            }
        }

        public void DrawSelectionRectangle(Graphics g)
        {
            if (MouseService.LeftMouseDown && SelectionService.StartSelectionRectangle)
            {
                var rect = SelectionService.SelectionRectangle;
                g.DrawRectangle(Pens.DimGray, rect.X, rect.Y, rect.Width, rect.Height);
                g.FillRectangle(_selectionRectangleBrush, rect);

            }
        }
        
        public static void DrawRect(Graphics g, PointF p)
        {
            g.FillRectangle(Brushes.Red, new RectangleF(new PointF(p.X - 1f, p.Y - 1f), new SizeF(3, 3)));
        }

        public void UpdateMouseMove(MouseEventArgs e)
        {
            if (CurrentProject.Ready)
            {
                if (!SelectionService.StartSelectionRectangle && MouseService.LeftMouseDown)
                {
                    foreach (var item in CurrentProject.Items)
                    {
                        item.UpdateMouseMove(this, null, null);
                    }

                    SelectionService.MouseLeftDownMovePositionLast = TranslatedMousePosition;
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

                PointF shift = new PointF(mouse.X - SelectionService.MouseRightDownMoveTranslationPositionLast.X, mouse.Y - SelectionService.MouseRightDownMoveTranslationPositionLast.Y);

                PointF newTranslation = new PointF(translation.X + shift.X / CurrentProject.Zoom, translation.Y + shift.Y / CurrentProject.Zoom);

                var distance = Vector2.Distance(new Vector2(newTranslation.X, newTranslation.Y), new Vector2(CurrentProject.Translation.X, CurrentProject.Translation.Y));

                CurrentProject.Translation = newTranslation;

                PreventContextMenu = distance > 0.01f || (!SelectionService.SelectedItemsAvailable && !SelectionService.SelectedItemsConnectionAvailable); // Wenn die Distanz beider Punkte > als angegeben ist, soll kein Kontext Menü angezeigt werden.

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
            return new PointF(p.X - CurrentProject.Translation.X, p.Y - CurrentProject.Translation.Y);
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

        public void Save(string fileName)
        {
            CurrentProject.FilePath = fileName;
            CurrentProject.Save(fileName);
        }
    }
}
