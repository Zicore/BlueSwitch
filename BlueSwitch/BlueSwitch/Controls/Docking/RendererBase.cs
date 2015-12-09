using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using WeifenLuo.WinFormsUI.Docking;
using XnaGeometry;
using Point = System.Drawing.Point;

namespace BlueSwitch.Controls.Docking
{
    public partial class RendererBase : DockContent
    {
        public RendererBase()
        {
            InitializeComponent();
            TabText = "Renderer";
            DoubleBuffered = true;
            Load += OnLoad;
            RenderingEngine.Redraw += RenderingEngineOnRedraw;
            this.MouseWheel += renderView_MouseWheel;
            GotFocus += OnGotFocus;
            LostFocus += OnLostFocus;
            AllowDrop = true;
            RenderingEngine.ProcessorCompiler.CompileStart += ProcessorCompilerOnCompileStart;
            RenderingEngine.ProcessorCompiler.Finished += ProcessorCompilerOnFinished;
        }

        private void ProcessorCompilerOnFinished(object sender, EventArgs eventArgs)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action) (() =>
                {
                    this.AllowDrop = true;
                }));
            }
            else
            {
                this.AllowDrop = true;
            }
        }

        private void ProcessorCompilerOnCompileStart(object sender, EventArgs eventArgs)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() =>
                {
                    this.AllowDrop = false;
                }));
            }
            else
            {
                this.AllowDrop = false;
            }
        }

        private ContextMenuStrip contextMenuStrip;
        private IContainer components;
        private ToolStripMenuItem removeToolStripMenuItem;
        Pen focusPen = new Pen(Brushes.CornflowerBlue, 2.0f);
        Pen focusPenSimulation = new Pen(Brushes.OrangeRed, 2.0f);

        private void OnGotFocus(object sender, EventArgs e)
        {

        }

        private void OnLostFocus(object sender, EventArgs eventArgs)
        {

        }

        private void RenderingEngineOnRedraw(object sender, EventArgs e)
        {
            Invalidate(); // TODO: Cross Thread Handling
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            RenderingEngine.Update();
            Invalidate();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RenderingEngine RenderingEngine { get; set; } = new RenderingEngine();

        protected override void OnPaint(PaintEventArgs e)
        {
            RenderingEngine.Draw(e.Graphics, ClientRectangle);

            if (Focused)
            {
                RectangleF bounds = new RectangleF(2, 2, Bounds.Width - 4, Bounds.Height - 4);
                if (RenderingEngine.DesignMode)
                {
                    e.Graphics.DrawRectangle(focusPen, bounds.X, bounds.Y, bounds.Width, bounds.Height);
                }
                else
                {
                    e.Graphics.DrawRectangle(focusPenSimulation, bounds.X, bounds.Y, bounds.Width, bounds.Height);
                }
            }

            base.OnPaint(e);
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //if (!Focused)
            //{
            //    this.Activate();
            //}

            RenderingEngine.MouseService.UpdateMouseMove(e);
            base.OnMouseMove(e);
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!Focused)
            {
                this.Activate();
            }

            RenderingEngine.MouseService.Down(e);
            RenderingEngine.MouseService.UpdateMouseMove(e);
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            RenderingEngine.MouseService.Up(e);
            Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                    e.SuppressKeyPress = false;
                    break;
            }
            base.OnKeyDown(e);
            RenderingEngine.KeyboardService.OnKeyDown(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            RenderingEngine.KeyboardService.OnKeyPress(e);
            base.OnKeyPress(e);
            Invalidate();
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                    e.IsInputKey = true;
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            RenderingEngine.KeyboardService.OnKeyUp(e);
            base.OnKeyUp(e);
            Invalidate();
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Move;
            }

            base.OnDragEnter(e);

        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                TreeNode node = (TreeNode)e.Data.GetData(typeof(TreeNode));
                SwitchBase sourceComponent = node.Tag as SwitchBase;
                if (sourceComponent != null)
                {
                    AddComponent(sourceComponent);
                }
            }

            base.OnDragDrop(e);
        }


        public void Zoom(float zoom)
        {
            Point m = PointToClient(Cursor.Position);

            if (RenderingEngine.CurrentProject.Zoom + zoom > 0.2)
            {
                Vector2 center = new Vector2(ClientSize.Width / 2.0f, ClientSize.Height / 2.0f);
                Vector2 maus = new Vector2(m.X, m.Y);
                Vector2 abweichung = center - maus;

                abweichung = (abweichung * zoom * 0.3);

                RenderingEngine.CurrentProject.Zoom += zoom;

                RenderingEngine.CurrentProject.Translation = new PointF(RenderingEngine.CurrentProject.Translation.X + (float)abweichung.X, RenderingEngine.CurrentProject.Translation.Y + (float)abweichung.Y);
            }

            //renderModel.Zoom(zoom);
            Invalidate();
        }


        Boolean scrollingUp = true;
        int oldDelta = 0;
        void renderView_MouseWheel(object sender, MouseEventArgs e)
        {
            if (oldDelta > e.Delta)
            {
                scrollingUp = true;
            }
            else
            {
                scrollingUp = false;
            }

            if (scrollingUp)
            {
                Zoom(-0.05f);
            }
            else
            {
                Zoom(+0.05f);
            }
        }

        public void AddComponent(SwitchBase sourceComponent)
        {
            SwitchBase switchBase = Activator.CreateInstance(sourceComponent.GetType()) as SwitchBase;
            PointF pt = RenderingEngine.TranslatePoint(PointToClient(MousePosition)); // this.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y)); //.Divide(CurrentProject.Zoom, CurrentProject.Zoom).Add(-CurrentProject.Translation.X, -CurrentProject.Translation.Y);

            if (switchBase != null)
            {
                switchBase.Position = pt;
                RenderingEngine.CurrentProject.Add(RenderingEngine, switchBase);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            Invalidate();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenderingEngine.SelectionService.RemoveSelected();
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = RenderingEngine.PreventContextMenu;
        }
    }
}
