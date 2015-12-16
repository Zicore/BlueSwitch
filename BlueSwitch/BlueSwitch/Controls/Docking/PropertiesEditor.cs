using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Components.UI;
using WeifenLuo.WinFormsUI.Docking;

namespace BlueSwitch.Controls.Docking
{
    public partial class PropertiesEditor : DockContent
    {
        public RenderingEngine RenderingEngine { get; set; }

        public PropertiesEditor(RenderingEngine renderingEngine)
        {
            RenderingEngine = renderingEngine;
            RenderingEngine.DebugValueUpdated += RenderingEngineOnDebugValueUpdated;
            
            InitializeComponent();
        }

        private void RenderingEngineOnDebugValueUpdated(object sender, EventArgs eventArgs)
        {
            UpdateValues();
            Invalidate();
        }
        
        float scale = 1.5f;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.TranslateTransform(2,0);
            e.Graphics.ScaleTransform(scale, scale);
            
            int count = 0;

            foreach (var keyValue in availableValues)
            {
                var item = keyValue.Value;
                item.Text = ToValue(keyValue.Key,RenderingEngine.DebugValues[item.Key]);
                item.Position = new PointF(item.Position.X,count * 14);
                item.Size = new SizeF(Size.Width / scale - 2, item.Size.Height);
                item.Draw(e.Graphics, RenderingEngine, null);
                count++;
            }
        }

        private Dictionary<string, DebugTextEdit> availableValues = new Dictionary<string, DebugTextEdit>();

        public void UpdateValues()
        {
            foreach (var debugValue in RenderingEngine.DebugValues)
            {
                if (!availableValues.ContainsKey(debugValue.Key))
                {
                    var item = new DebugTextEdit
                    {
                        Text = ToValue(debugValue.Key, debugValue.Value),
                        Size = new SizeF(20, 14),
                        Key = debugValue.Key
                    };
                    item.Initialize(RenderingEngine, null);
                    availableValues.Add(item.Key, item);
                }
            }
        }

        public string ToValue(String key, object value)
        {
            return $"{key}: {value}";
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            Invalidate();
        }
    }
}
