//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace WinFormsSample
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Tobii.Gaze.Core;

    /// <summary>
    /// Class representing the gaze point arguments.
    /// </summary>
    public class GazePointEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GazePointEventArgs"/> class.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        public GazePointEventArgs(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets the X-coordinate.
        /// </summary>
        public double X { get; private set; }

        /// <summary>
        /// Gets the Y-coordinate.
        /// </summary>
        public double Y { get; private set; }
    }

    public enum EyeTrackingState
    {
        NotInitialized,
        EyeTrackerNotFound,
        Connecting,
        StartingTracking,
        ConnectionFailed,
        Error,
        Tracking
    }

    /// <summary>
    /// The eye tracking engine provides gaze data from the currently setup eye tracker.
    /// It reads and validates the current eye tracker configuration,
    /// connects to and prepares the eye tracker for eyetracking and then
    /// provides gaze data until the eye tracker is disconnected or eyetracking engine is disposed. 
    /// </summary>
    public sealed class EyeTrackingEngine : IDisposable
    {
        public EventHandler<EyeTrackingStateChangedEventArgs> StateChanged;
        public EventHandler<GazePointEventArgs> GazePoint;

        private static readonly Dictionary<EyeTrackingState, string> ErrorMessages = new Dictionary<EyeTrackingState, string>
        {
            { EyeTrackingState.EyeTrackerNotFound, "No eye tracker could be found." },
            { EyeTrackingState.ConnectionFailed, "The connection to the eye tracker failed." },
            { EyeTrackingState.Error, "The eye tracker reported an error." },
        };

        private EyeTrackingState _state = EyeTrackingState.NotInitialized;
        private Uri _eyeTrackerUrl;
        private IEyeTracker _eyeTracker;
        private Thread _thread;

        /// <summary>
        /// Initializes a new instance of the <see cref="EyeTrackingEngine"/> class.
        /// Throws EyeTrackerException if not successful
        /// </summary>
        public EyeTrackingEngine()
        {
        }

        public EyeTrackingState State
        {
            get
            {
                return _state;
            }

            private set
            {
                if (_state != value)
                {
                    _state = value;
                    RaiseStateChanged();
                }
            }
        }

        /// <summary>
        /// Stop eye tracking and dispose eye tracking engine and Tobii EyeTracking
        /// </summary>
        public void Dispose()
        {
            Reset();
        }

        /// <summary>
        /// Initialize the eye tracker engine.
        /// State changes are notified to the client with the StateChanged event handler. 
        /// </summary>
        public void Initialize()
        {
            if (State != EyeTrackingState.NotInitialized)
            {
                throw new InvalidOperationException("EyeTrackingEngine can not be initialized when not in state NotInitialized");
            }

            InitializeEyeTrackerAndRunEventLoop();
        }

        /// <summary>
        ///  Retry to Initialize eye tracker engine. Should be called when user has manually
        /// changed the configuration or performed a calibration with the Tobii EyeTracking Control Panel.
        /// </summary>
        public void Retry()
        {
            Reset();
            Initialize();
        }

        private bool CanRetry
        {
            get
            {
                return State == EyeTrackingState.EyeTrackerNotFound ||
                    State == EyeTrackingState.ConnectionFailed ||
                    State == EyeTrackingState.Error;
            }
        }

        private string ErrorMessage
        {
            get { return ErrorMessages.ContainsKey(State) ? ErrorMessages[State] : string.Empty; }
        }

        private void InitializeEyeTrackerAndRunEventLoop()
        {
            if (State != EyeTrackingState.NotInitialized)
            {
                throw new InvalidOperationException("Can not initialize eye tracker and run event loop when not in state NotInitialized");
            }

            try
            {
                _eyeTrackerUrl = new EyeTrackerCoreLibrary().GetConnectedEyeTracker();
                if (_eyeTrackerUrl == null)
                {
                    State = EyeTrackingState.EyeTrackerNotFound;
                    return;
                }
            }
            catch (EyeTrackerException)
            {
                State = EyeTrackingState.Error;
                return;
            }

            try
            {
                _eyeTracker = new EyeTracker(_eyeTrackerUrl);
                _eyeTracker.EyeTrackerError += OnEyeTrackerError;
                _eyeTracker.GazeData += OnGazeData;

                CreateAndRunEventLoopThread();

                _eyeTracker.ConnectAsync(OnConnectFinished);

                State = EyeTrackingState.Connecting;
            }
            catch (EyeTrackerException)
            {
                State = EyeTrackingState.ConnectionFailed;
            }
        }

        private void CreateAndRunEventLoopThread()
        {
            if (_thread != null)
            {
                throw new InvalidOperationException("_thread parameter is already set");
            }

            _thread = new Thread(() =>
            {
                try
                {
                    _eyeTracker.RunEventLoop();
                }
                catch (EyeTrackerException)
                {
                    State = EyeTrackingState.Error;
                }
            });

            _thread.Start();
        }

        private void OnConnectFinished(ErrorCode errorCode)
        {
            if (errorCode != ErrorCode.Success)
            {
                State = EyeTrackingState.ConnectionFailed;
                return;
            }

            _eyeTracker.StartTrackingAsync(OnStartTrackingFinished);

        }

        private void OnStartTrackingFinished(ErrorCode errorCode)
        {
            if (errorCode != ErrorCode.Success)
            {
                State = EyeTrackingState.Error;
            }
            else
            {
                State = EyeTrackingState.Tracking;
            }
        }

        private void Reset()
        {
            if (_eyeTracker != null)
            {
                _eyeTracker.BreakEventLoop();
                if (_thread != null)
                {
                    _thread.Join();
                }

                _eyeTracker.EyeTrackerError -= OnEyeTrackerError;
                _eyeTracker.GazeData -= OnGazeData;
                _eyeTracker.Dispose();
                _eyeTracker = null;
            }

            if (_thread != null)
            {
                _thread.Abort();
                _thread = null;
            }

            State = EyeTrackingState.NotInitialized;
        }

        private void RaiseStateChanged()
        {
            var handler = StateChanged;

            if (handler != null)
            {
                handler(this, new EyeTrackingStateChangedEventArgs(State, ErrorMessage, CanRetry));
            }
        }

        private void RaiseGazePoint(Point2D point)
        {
            var handler = GazePoint;
            if (handler != null)
            {
                handler(this, new GazePointEventArgs(point.X, point.Y));
            }
        }

        private void OnEyeTrackerError(object sender, EyeTrackerErrorEventArgs eyeTrackerErrorEventArgs)
        {
            if (eyeTrackerErrorEventArgs.ErrorCode != ErrorCode.Success)
            {
                State = EyeTrackingState.ConnectionFailed;
            }
        }

        private void OnGazeData(object sender, GazeDataEventArgs gazeDataEventArgs)
        {
            var gazeData = gazeDataEventArgs.GazeData;

            if (gazeData.TrackingStatus == TrackingStatus.BothEyesTracked ||
                gazeData.TrackingStatus == TrackingStatus.OneEyeTrackedUnknownWhich)
            {
                var p = new Point2D(
                    (gazeData.Left.GazePointOnDisplayNormalized.X +
                        gazeData.Right.GazePointOnDisplayNormalized.X) / 2,
                    (gazeData.Left.GazePointOnDisplayNormalized.Y +
                        gazeData.Right.GazePointOnDisplayNormalized.Y) / 2);

                RaiseGazePoint(p);
            }
            else if (gazeData.TrackingStatus == TrackingStatus.OnlyLeftEyeTracked ||
                        gazeData.TrackingStatus == TrackingStatus.OneEyeTrackedProbablyLeft)
            {
                RaiseGazePoint(gazeData.Left.GazePointOnDisplayNormalized);
            }
            else if (gazeData.TrackingStatus == TrackingStatus.OnlyRightEyeTracked ||
                        gazeData.TrackingStatus == TrackingStatus.OneEyeTrackedProbablyRight)
            {
                RaiseGazePoint(gazeData.Right.GazePointOnDisplayNormalized);
            }
        }
    }
}
