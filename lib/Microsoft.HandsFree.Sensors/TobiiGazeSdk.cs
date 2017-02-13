using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Tobii.Gaze.Core;

namespace Microsoft.HandsFree.Sensors
{
    public class TobiiGazeSdk : IGazeDataProvider
    {
        private bool _disposed;
        private DispatcherTimer _trackerEnumTimer;
        private IEyeTracker _eyeTracker = null;
        private Thread _trackingThread = null;

        public event EventHandler<GazeEventArgs> GazeEvent;

        public Sensors Sensor { get { return Sensors.TobiiGazeSDK; } }

        public bool Initialize()
        {
            _trackerEnumTimer = new DispatcherTimer();
            _trackerEnumTimer.Tick += new EventHandler(TrackerEnumTimer_Tick);
            _trackerEnumTimer.Interval = new TimeSpan(0, 0, 5);

            if (!InitializeEyeTracker())
            {
                // if we can't find the tracker, poll every five seconds looking for the timer
                _trackerEnumTimer.Start();
                return false;
            }
            return true;
        }

        public void Terminate()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<bool> CreateProfileAsync()
        {
            LaunchRecalibration();
            return Task.FromResult(true);
        }

        public void LaunchRecalibration()
        {
            SendKeys.SendWait("+^{F9}");
        }

        public void BeginAddCalibrationPoint(int x, int y)
        {

        }

        public void EndAddCalibrationPoint()
        {

        }

        private bool InitializeEyeTracker()
        {
            Uri url = new EyeTrackerCoreLibrary().GetConnectedEyeTracker();
            if (url == null)
            {
                return false;
            }

            _eyeTracker = new EyeTracker(url);
            if (_eyeTracker == null)
            {
                return false;
            }

            _eyeTracker.EyeTrackerError += OnEyeTrackerError;
            _eyeTracker.GazeData += OnEyeTrackerGazeData;
            _trackingThread = new Thread(new ThreadStart(EyeTrackerEventLoop));
            _trackingThread.Start();

            if (!_eyeTracker.Connected)
            {
                _eyeTracker.Connect();
                _eyeTracker.StartTracking();
            }

            return true;
        }

        private void EyeTrackerEventLoop()
        {
            try
            {
                _eyeTracker.RunEventLoop();
            }
            catch (EyeTrackerException ex)
            {
                Debug.WriteLine("ERROR: {0}", ex.Message);
            }
        }

        private void OnEyeTrackerError(object sender, EyeTrackerErrorEventArgs e)
        {
            Debug.WriteLine("ERROR: " + e.Message);
            _trackerEnumTimer.Start();
        }

        private void OnEyeTrackerGazeData(object sender, GazeDataEventArgs e)
        {
            EventHandler<GazeEventArgs> handler = GazeEvent;
            if (handler != null)
            {
                Point2D gazePoint;
                var gazeData = e.GazeData;

                switch (gazeData.TrackingStatus)
                {
                    case TrackingStatus.BothEyesTracked:
                        gazePoint = new Point2D((gazeData.Left.GazePointOnDisplayNormalized.X + gazeData.Right.GazePointOnDisplayNormalized.X) / 2.0,
                            (gazeData.Left.GazePointOnDisplayNormalized.Y + gazeData.Right.GazePointOnDisplayNormalized.Y) / 2.0);
                        break;

                    case TrackingStatus.OnlyLeftEyeTracked:
                    case TrackingStatus.OneEyeTrackedProbablyLeft:
                        gazePoint = gazeData.Left.GazePointOnDisplayNormalized;
                        break;

                    case TrackingStatus.OnlyRightEyeTracked:
                    case TrackingStatus.OneEyeTrackedProbablyRight:
                        gazePoint = gazeData.Right.GazePointOnDisplayNormalized;
                        break;

                    default:
                        return;
                }

                GazeEventArgs gazeEventArgs = new GazeEventArgs(gazePoint.X, gazePoint.Y, gazeData.Timestamp / 1000, Fixation.Unknown, true);
                handler(this, gazeEventArgs);
            }
        }

        private void TrackerEnumTimer_Tick(object sender, EventArgs e)
        {
            if (InitializeEyeTracker())
            {
                _trackerEnumTimer.Stop();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_eyeTracker != null)
                    {
                        _eyeTracker.BreakEventLoop();
                        _trackingThread.Join();
                        _eyeTracker.Dispose();
                    }
                }
                _disposed = true;
            }
        }

        ~TobiiGazeSdk()
        {
            Dispose(false);
        }

    }
}
