using System;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Base;

namespace BlueSwitch.Base.Services
{
    public class KeyboardService
    {
        public RenderingEngine RenderingEngine { get; set; }

        public KeyboardService(RenderingEngine renderingEngine)
        {
            RenderingEngine = renderingEngine;
            _keyboardCaretTimer.Tick += KeyboardCaretTimerOnTick;
            _keyboardCaretTimer.Enabled = true;
        }

        private void KeyboardCaretTimerOnTick(object sender, EventArgs e)
        {
            OnCaretTick();
        }

        public event EventHandler<KeyEventArgs> KeyDown;
        public event EventHandler<KeyEventArgs> KeyUp;
        public event EventHandler<KeyPressEventArgs> KeyPress;
        public event EventHandler CaretTick;

        public virtual void OnKeyUp(KeyEventArgs e)
        {
            KeyUp?.Invoke(this, e);
        }

        public virtual void OnKeyDown(KeyEventArgs e)
        {
            KeyDown?.Invoke(this, e);
        }

        public virtual void OnKeyPress(KeyPressEventArgs e)
        {
            KeyPress?.Invoke(this, e);
        }

        readonly Timer _keyboardCaretTimer = new Timer { Interval = 500};

        protected virtual void OnCaretTick()
        {
            CaretTick?.Invoke(this, EventArgs.Empty);
        }
    }
}
