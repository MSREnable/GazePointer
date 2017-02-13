namespace WinFormsSample
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Tobii.Gaze.Core;

    public partial class WinFormsSample : Form
    {
        private readonly EyeTrackingEngine _eyeTrackingEngine;
        private Point _gazePoint;
        private delegate void UpdateStateDelegate(EyeTrackingStateChangedEventArgs eyeTrackingStateChangedEventArgs);

        public WinFormsSample(EyeTrackingEngine eyeTrackingEngine)
        {
            InitializeComponent();

            _eyeTrackingEngine = eyeTrackingEngine;
            _eyeTrackingEngine.StateChanged += StateChanged;
            _eyeTrackingEngine.GazePoint += GazePoint;
            _eyeTrackingEngine.Initialize();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var local = PointToClient(_gazePoint);
            e.Graphics.FillEllipse(Brushes.Red, local.X - 25, local.Y - 25, 20, 20);
        }

        private delegate void Action();

        private void GazePoint(object sender, GazePointEventArgs gazePointEventArgs)
        {
            BeginInvoke(new Action(() =>
                {
                    var handle = Handle;
                    if (handle == null)
                    {
                        // window not created yet. never mind.
                        return;
                    }

                    var screenBounds = Screen.FromHandle(handle).Bounds;
                    _gazePoint = new Point(
                        (int)(screenBounds.Left + screenBounds.Width * gazePointEventArgs.X),
                        (int)(screenBounds.Top + screenBounds.Height * gazePointEventArgs.Y));

                    Invalidate();
                }));
        }

        private void StateChanged(object sender, EyeTrackingStateChangedEventArgs eyeTrackingStateChangedEventArgs)
        {
            // Forward state change to UI thread
            if (InvokeRequired)
            {
                var updateStateDelegate = new UpdateStateDelegate(UpdateState);
                Invoke(updateStateDelegate, new object[] { eyeTrackingStateChangedEventArgs });
            }
            else
            {
                UpdateState(eyeTrackingStateChangedEventArgs);
            }
        }

        private void UpdateState(EyeTrackingStateChangedEventArgs eyeTrackingStateChangedEventArgs)
        {
            if (!string.IsNullOrEmpty(eyeTrackingStateChangedEventArgs.ErrorMessage))
            {
                InfoMessage.Visible = false;
                ErrorMessagePanel.Visible = true;
                ErrorMessage.Text = eyeTrackingStateChangedEventArgs.ErrorMessage;
                Retry.Enabled = eyeTrackingStateChangedEventArgs.CanRetry;
                return;
            }

            ErrorMessagePanel.Visible = false;

            if (eyeTrackingStateChangedEventArgs.EyeTrackingState != EyeTrackingState.Tracking)
            {
                InfoMessage.Visible = true;
                InfoMessage.Text = "Connecting to eye tracker, please wait...";
            }
            else
            {
                InfoMessage.Visible = false;
            }
        }

        private void RetryClick(object sender, EventArgs e)
        {
            _eyeTrackingEngine.Retry();
        }

        private void SuppressErrorMessageClick(object sender, EventArgs e)
        {
            ErrorMessagePanel.Visible = false;
        }
    }
}
