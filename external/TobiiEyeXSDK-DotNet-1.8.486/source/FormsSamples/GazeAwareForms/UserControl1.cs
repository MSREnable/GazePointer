//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace GazeAwareForms
{
    using EyeXFramework;
    using System.Windows.Forms;

    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();

            // Add eye-gaze interaction behaviors to the user control itself and 
            // to the panel inside it.
            // The controls should display a border when the user's gaze are on them.
            // Note that since panel1 is nested inside this UserControl, any time the 
            // panel1 has the user's gaze, the UserControl will too.
            Program.EyeXHost.Connect(behaviorMap1);
            behaviorMap1.Add(this, new GazeAwareBehavior(OnGaze));
            behaviorMap1.Add(panel1, new GazeAwareBehavior(OnGaze));
        }

        private void OnGaze(object sender, GazeAwareEventArgs e)
        {
            var container = sender as UserControl;
            if (container != null)
            {
                container.BorderStyle = (e.HasGaze) ? BorderStyle.FixedSingle : BorderStyle.None;
            }

            var panel = sender as Panel;
            if (panel != null)
            {
                panel.BorderStyle = (e.HasGaze) ? BorderStyle.FixedSingle : BorderStyle.None;
            }
        }
    }
}
