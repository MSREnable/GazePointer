//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace ActivatableNotesForms
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using EyeXFramework;

    public partial class NoteControl : UserControl
    {
        public NoteControl()
        {
            InitializeComponent();

            // Make this control direct clickable using the Activatable behavior.
            Program.EyeXHost.Connect(behaviorMap1);
            behaviorMap1.Add(this, new ActivatableBehavior(OnActivated));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle;
            rect.Inflate(-15, -15);
            var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            e.Graphics.DrawString(Text, Font, Brushes.Black, rect, format);

            rect = ClientRectangle;
            rect.Width--;
            rect.Height--;
            e.Graphics.DrawRectangle(Pens.Gray, rect);
        }

        private void OnActivated(object sender, EventArgs e)
        {
            Parent.Controls.Remove(this);
            Dispose();
        }
    }
}
