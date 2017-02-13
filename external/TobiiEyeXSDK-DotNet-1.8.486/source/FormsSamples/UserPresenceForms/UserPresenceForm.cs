//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace UserPresenceForms
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using EyeXFramework;
    using EyeXFramework.Forms;
    using Tobii.EyeX.Framework;

    public partial class UserPresenceForm : Form
    {
        private readonly FormsEyeXHost _eyeXHost;

        public UserPresenceForm()
        {
            // Initialize components.
            InitializeComponent();

            // Create the EyeX host.
            _eyeXHost = new FormsEyeXHost();
        }

        protected override void OnLoad(EventArgs e)
        {
            // Register an status-changed event listener for user presence.
            // NOTE that the event listener must be unregistered too. This is taken care of in the Dispose(bool) method.
            _eyeXHost.UserPresenceChanged += EyeXHost_UserPresenceChanged;
            _eyeXHost.GazeTrackingChanged += EyeXHost_GazeTrackingChanged;

            // Start the EyeX host.
            _eyeXHost.Start();

            // Wait until we're connected.
            if (_eyeXHost.WaitUntilConnected(TimeSpan.FromSeconds(5)))
            {
                // Make sure the EyeX Engine version is equal to or greater than 1.4.
                var engineVersion = _eyeXHost.GetEngineVersion().Result;
                if (engineVersion.Major != 1 || engineVersion.Major == 1 && engineVersion.Minor < 4)
                {
                    _labelHasGazeTracking.ForeColor = Color.Red;
                    _labelHasGazeTracking.Text = "Requires EyeX Engine 1.4";
                }
            }
            else
            {
                MessageBox.Show("Could not connect to EyeX engine.");
            }
        }

        private void EyeXHost_GazeTrackingChanged(object sender, EngineStateValue<GazeTracking> e)
        {
            // State-changed events are received on a background thread.
            // But operations that affect the GUI must be executed on the main thread.
            // We use BeginInvoke to marshal the call to the main thread.

            if (Created)
            {
                BeginInvoke(new Action(() =>
                {
                    if (e.IsValid && e.Value == GazeTracking.GazeTracked)
                    {
                        _labelHasGazeTracking.Text = "Yes";
                    }
                    else
                    {
                        _labelHasGazeTracking.Text = "No";
                    }
                }));
            }
        }

        private void EyeXHost_UserPresenceChanged(object sender, EngineStateValue<UserPresence> e)
        {
            // State-changed events are received on a background thread.
            // But operations that affect the GUI must be executed on the main thread.
            // We use BeginInvoke to marshal the call to the main thread.

            if (Created)
            {
                BeginInvoke(new Action(() => UpdateUserPresence(e)));
            }
        }

        private void UpdateUserPresence(EngineStateValue<UserPresence> value)
        {
            if (value.IsValid &&
                value.Value == UserPresence.Present)
            {
                pictureBox1.ImageLocation = "Images/present.png";
                _labelHasUserPresence.Text = "Yes";
            }
            else
            {
                pictureBox1.ImageLocation = "Images/not-present.png";
                _labelHasUserPresence.Text = "No";
            }
        }
    }
}
