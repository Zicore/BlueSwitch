using System;
using System.Drawing;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Utils;

namespace BlueSwitch.Base.Services
{
    public class MouseService
    {
        public RenderingEngine RenderingEngine { get; set; }

        public MouseService(RenderingEngine renderingEngine)
        {
            RenderingEngine = renderingEngine;
        }

        public virtual PointF Position { get; set; }

        public bool LeftMouseDown { get; set; }
        public bool RightMouseDown { get; set; }
        public bool MiddleMouseDown { get; set; }

        public event EventHandler<MouseEventArgs> MouseMove;
        public event EventHandler<MouseEventArgs> MouseClick;
        public event EventHandler<MouseEventArgs> MouseUp;
        public event EventHandler<MouseEventArgs> MouseDown;

        protected virtual void OnMouseMove(MouseEventArgs e)
        {
            MouseMove?.Invoke(this, e);
        }

        protected virtual void OnMouseClick(MouseEventArgs e)
        {
            MouseClick?.Invoke(this, e);
        }

        protected virtual void OnMouseUp(MouseEventArgs e)
        {
            MouseUp?.Invoke(this, e);
        }

        protected virtual void OnMouseDown(MouseEventArgs e)
        {
            MouseDown?.Invoke(this, e);
        }

        public virtual bool IsButtonDown(MouseEventArgs e, MouseButtons button)
        {
            return (e.Button == button);
        }

        public virtual bool IsOver(DrawableBase drawable, PointF translation)
        {
            return MathUtils.RectangleIntersects(drawable.GetTranslatedRectangle(translation), RenderingEngine.TranslatedMousePosition);
        }

        public void UpdateMouseMove(MouseEventArgs e)
        {
            Position = e.Location;
            OnMouseMove(e);
        }

        public void Click(MouseEventArgs e)
        {
            OnMouseClick(e);
        }

        public void Up(MouseEventArgs e)
        {
            OnMouseUp(e);

            if (e.Button == MouseButtons.Left)
            {
                LeftMouseDown = false;
            }
            if (e.Button == MouseButtons.Right)
            {
                RightMouseDown = false;
            }
            if (e.Button == MouseButtons.Middle)
            {
                MiddleMouseDown = false;
            }
        }

        public void Down(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                LeftMouseDown = true;
            }
            if (e.Button == MouseButtons.Right)
            {
                RightMouseDown = true;
            }
            if (e.Button == MouseButtons.Middle)
            {
                MiddleMouseDown = true;
            }
            OnMouseDown(e);
        }
    }
}
