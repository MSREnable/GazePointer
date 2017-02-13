//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

using EyeXFramework;
using System.Windows.Forms;

namespace GazeAwareForms
{
    public partial class GazeAwareForm : Form
    {
        public GazeAwareForm()
        {
            InitializeComponent();

            // Add eye-gaze interaction behaviors to the panels on the form.
            // The panels should display a border when the user's gaze are on them.
            // Note that panel4 is nested inside panel2. This means that any time 
            // panel2 has the user's gaze, panel4 will too.
            Program.EyeXHost.Connect(behaviorMap1);
            behaviorMap1.Add(panel1, new GazeAwareBehavior(OnGaze));
            behaviorMap1.Add(panel2, new GazeAwareBehavior(OnGaze));
            behaviorMap1.Add(panel3, new GazeAwareBehavior(OnGaze) { DelayMilliseconds = 500 });
            behaviorMap1.Add(panel4, new GazeAwareBehavior(OnGaze));
        }

        private void OnGaze(object sender, GazeAwareEventArgs e)
        {
            var panel = sender as Panel;
            if (panel != null)
            {
                panel.BorderStyle = (e.HasGaze) ? BorderStyle.FixedSingle : BorderStyle.None;
            }
        }
    }
}
