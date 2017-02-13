//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace WpfCalibrationSample
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Tobii.Gaze.Core;

    /// <summary>
    /// View model for the calibration window.
    /// <para>
    /// These are the basic steps of a calibration procedure:
    ///   1) Call eyeTracker.StartCalibration(...)
    ///   2) For each calibration point:
    ///       - amimate a calibration point stimulus on the screen
    ///       - call eyeTracker.AddCalibrationPointAsync(...) with the calibration point in normalized coordinates and a completion callback
    ///       - in calibration point completed callback:
    ///          - if last calibration point, call eyeTracker.ComputeAndSetCalibrationAsync(...) with a completion callback
    ///          - else move to next point
    ///   3) In compute and set calibration completed callback, call eyeTracker.StopCalibrationAsync(...)
    /// See also the Developer's Guide for more information about calibration.
    /// </para><para>
    /// This sample also includes a positioning guide to help the user position themselves within the track box of the eye tracker.
    /// </para>
    /// </summary>
    internal sealed class CalibrationViewModel : ICalibrationViewModel
    {
        private const double CalibrationNearLimit = 0.3;
        private const double CalibrationFarLimit = 0.7;

        /// <summary>
        /// Calibration points specified in the ADCS coordinate system. (0, 0) means top left of the display, (1, 1) is bottom right.
        /// </summary>
        private static readonly Point[] CalibrationPoints = new Point[]
        {
            new Point(0.5, 0.5),
            new Point(0.9, 0.1),
            new Point(0.9, 0.9),
            new Point(0.1, 0.9),
            new Point(0.1, 0.1)
        };

        private Dispatcher _dispatcher;
        private Action _exitAction;
        private IEyeTracker _tracker;
        private int _currentCalibrationPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalibrationViewModel"/> class.
        /// </summary>
        /// <param name="dispatcher">Dispatcher used for marshaling operations to the main thread.</param>
        /// <param name="eyeTrackerUrl">Eye tracker URL.</param>
        /// <param name="exitAction">Delegate invoked to exit the application.</param>
        public CalibrationViewModel(Dispatcher dispatcher, string eyeTrackerUrl, Action exitAction)
        {
            _dispatcher = dispatcher;
            _exitAction = exitAction;
            Stage = WpfCalibrationSample.Stage.Initializing;
            ContinueCommand = new ActionCommand(Continue);
            ExitCommand = new ActionCommand(exitAction);
            EyePositions = new ObservableCollection<Point>();

            Uri url;
            if (eyeTrackerUrl == "--auto")
            {
                url = new EyeTrackerCoreLibrary().GetConnectedEyeTracker();
                if (url == null)
                {
                    Stage = WpfCalibrationSample.Stage.Error;
                    ErrorMessage = "No eye tracker found.";
                    return;
                }
            }
            else
            {
                try
                {
                    url = new Uri(eyeTrackerUrl);
                }
                catch (UriFormatException)
                {
                    Stage = WpfCalibrationSample.Stage.Error;
                    ErrorMessage = "Invalid eye tracker URL.";
                    return;
                }
            }

            InitializeEyeTracker(url);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the stage the view is currently in.
        /// </summary>
        public Stage Stage { get; private set; }

        /// <summary>
        /// Gets a command used for moving on to the next stage if possible.
        /// (Typically invoked when the user presses space.)
        /// </summary>
        public ICommand ContinueCommand { get; private set; }

        /// <summary>
        /// Gets a command used for moving to the Exiting stage. Do not pass go, do not collect $200. 
        /// (Typically invoked when the user presses escape.)
        /// </summary>
        public ICommand ExitCommand { get; private set; }

        /// <summary>
        /// Gets an error message that describes what went wrong in the Error stage.
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Gets the positions of the detected eyes in the PositioningGuide stage.
        /// </summary>
        public ObservableCollection<Point> EyePositions { get; private set; }

        /// <summary>
        /// Gets the current positioning status in the PositioningGuide stage.
        /// </summary>
        public PositioningStatus PositioningStatus { get; private set; }

        /// <summary>
        /// Gets the position of the calibration dot in the Calibration stage.
        /// </summary>
        public Point CalibrationDotPosition
        {
            get
            {
                return CalibrationPoints[_currentCalibrationPoint];
            }
        }

        /// <summary>
        /// Starts collecting data for a calibration point. Call this method when the animation is finished.
        /// </summary>
        public void CalibrationDotAnimationCompleted()
        {
            Trace.WriteLine(string.Format("Adding calibration point {0}", _currentCalibrationPoint + 1));

            // when the animation has completed, we call calibration_add_point (once), which will in turn 
            // call OnAddCalibrationPointCompleted.
            _tracker.AddCalibrationPointAsync(ToPoint2D(CalibrationDotPosition), OnAddCalibrationPointCompleted);
        }

        public void Dispose()
        {
            if (_tracker != null)
            {
                _tracker.Dispose();
                _tracker = null;
            }
        }

        private static Point2D ToPoint2D(Point p)
        {
            return new Point2D(p.X, p.Y);
        }

        private static Point ToPoint(Point2D p)
        {
            return new Point(p.X, p.Y);
        }

        private void Continue()
        {
            switch (Stage)
            {
                case WpfCalibrationSample.Stage.PositioningGuide:
                    // proceed to start calibration
                    StartCalibration();
                    break;

                case WpfCalibrationSample.Stage.CalibrationFailed:
                    // go back to the positioning guide
                    StartPositioningGuide();
                    break;

                case WpfCalibrationSample.Stage.Finished:
                case WpfCalibrationSample.Stage.Error:
                    // continue from terminal states means exit the application
                    _exitAction();
                    break;

                default:
                    // ignore.
                    break;
            }
        }

        private void StartPositioningGuide()
        {
            Trace.WriteLine("Starting positioning guide.");

            Stage = WpfCalibrationSample.Stage.PositioningGuide;
            OnPropertyChanged("Stage");
        }

        private void StartCalibration()
        {
            Trace.WriteLine("Starting calibration.");

            Stage = WpfCalibrationSample.Stage.Calibration;
            _currentCalibrationPoint = 0;
            OnPropertyChanged("Stage");
            OnPropertyChanged("CalibrationDotPosition"); // triggers the animation -- the view should call CalibrationDotAnimationCompleted when it finishes.
        }

        private void InitializeEyeTracker(Uri url)
        {
            Trace.WriteLine("Initializing eye tracker " + url.ToString());

            try
            {
                _tracker = new EyeTracker(url);
            }
            catch (EyeTrackerException ex)
            {
                HandleError(ex.Message);
                return;
            }

            _tracker.EyeTrackerError += OnEyeTrackerError;
            _tracker.GazeData += OnGazeData;
            _tracker.RunEventLoopOnInternalThread(OnGenericOperationCompleted);
            _tracker.ConnectAsync(OnConnectCompleted);
        }

        private void OnGazeData(object sender, GazeDataEventArgs e)
        {
            // mirror the x coordinate to make the visualization make sense.
            var left = new Point2D(1 - e.GazeData.Left.EyePositionInTrackBoxNormalized.X, e.GazeData.Left.EyePositionInTrackBoxNormalized.Y);
            var right = new Point2D(1 - e.GazeData.Right.EyePositionInTrackBoxNormalized.X, e.GazeData.Right.EyePositionInTrackBoxNormalized.Y);
            var z = 1.1;

            switch (e.GazeData.TrackingStatus)
            {
                case TrackingStatus.BothEyesTracked:
                    z = (e.GazeData.Left.EyePositionInTrackBoxNormalized.Z + e.GazeData.Right.EyePositionInTrackBoxNormalized.Z) / 2;
                    break;

                case TrackingStatus.OnlyLeftEyeTracked:
                    z = e.GazeData.Left.EyePositionInTrackBoxNormalized.Z;
                    right = new Point2D(double.NaN, double.NaN);
                    break;

                case TrackingStatus.OnlyRightEyeTracked:
                    z = e.GazeData.Right.EyePositionInTrackBoxNormalized.Z;
                    left = new Point2D(double.NaN, double.NaN);
                    break;

                default:
                    left = right = new Point2D(double.NaN, double.NaN);
                    break;
            }

            _dispatcher.BeginInvoke(new Action(() =>
                {
                    SetEyePositions(left, right, z);
                }));
        }

        private void SetEyePositions(Point2D left, Point2D right, double z)
        {
            EyePositions.Clear();

            if (!double.IsNaN(left.X))
            {
                EyePositions.Add(ToPoint(left));
            }

            if (!double.IsNaN(right.X))
            {
                EyePositions.Add(ToPoint(right));
            }

            if (z < CalibrationNearLimit)
            {
                PositioningStatus = WpfCalibrationSample.PositioningStatus.TooClose;
            }
            else if (z <= CalibrationFarLimit)
            {
                PositioningStatus = WpfCalibrationSample.PositioningStatus.PositionOk;
            }
            else
            {
                PositioningStatus = WpfCalibrationSample.PositioningStatus.TooFarOrNotDetected;
            }

            OnPropertyChanged("PositioningStatus");
        }

        private void OnEyeTrackerError(object sender, EyeTrackerErrorEventArgs e)
        {
            Trace.WriteLine("The eye tracker reported a spurious error.");
            HandleError(e.Message);
        }

        private void OnConnectCompleted(ErrorCode errorCode)
        {
            Trace.WriteLine("Connect completed.");
            if (errorCode != ErrorCode.Success)
            {
                HandleError(Tobii.Gaze.Core.ErrorMessage.GetErrorMessage(errorCode));
                return;
            }

            _tracker.StartCalibrationAsync(OnStartCalibrationCompleted);
        }

        private void OnStartCalibrationCompleted(ErrorCode errorCode)
        {
            Trace.WriteLine("Start calibration completed.");
            if (errorCode != ErrorCode.Success)
            {
                HandleError(Tobii.Gaze.Core.ErrorMessage.GetErrorMessage(errorCode));
                return;
            }

            _dispatcher.Invoke(new Action(StartPositioningGuide));

            _tracker.StartTrackingAsync(OnGenericOperationCompleted);
        }

        private void OnAddCalibrationPointCompleted(ErrorCode errorCode)
        {
            Trace.WriteLine("Add calibration point completed.");
            if (errorCode != ErrorCode.Success)
            {
                HandleError(Tobii.Gaze.Core.ErrorMessage.GetErrorMessage(errorCode));
                return;
            }

            if (_currentCalibrationPoint + 1 < CalibrationPoints.Length)
            {
                // next point, please
                _dispatcher.Invoke(new Action(() =>
                {
                    _currentCalibrationPoint++;
                    OnPropertyChanged("CalibrationDotPosition");
                }));
            }
            else
            {
                // done: move to the next stage.
                _dispatcher.Invoke(new Action(() =>
                {
                    Stage = WpfCalibrationSample.Stage.ComputingCalibration;
                    OnPropertyChanged("Stage");
                }));

                Trace.WriteLine("Computing and setting calibration.");
                _tracker.ComputeAndSetCalibrationAsync(OnComputeAndSetCalibrationCompleted);
            }
        }

        private void OnComputeAndSetCalibrationCompleted(ErrorCode errorCode)
        {
            Trace.WriteLine("Compute and set calibration completed.");
            if (errorCode != ErrorCode.Success)
            {
                if (errorCode == ErrorCode.FirmwareOperationFailed)
                {
                    // the calibration process failed. try again.
                    _dispatcher.Invoke(new Action(() =>
                    {
                        Stage = WpfCalibrationSample.Stage.CalibrationFailed;
                        OnPropertyChanged("Stage");
                    }));
                }
                else
                {
                    HandleError(Tobii.Gaze.Core.ErrorMessage.GetErrorMessage(errorCode));
                }

                return;
            }

            _tracker.StopCalibrationAsync(OnStopCalibrationCompleted);
        }

        private void OnStopCalibrationCompleted(ErrorCode errorCode)
        {
            Trace.WriteLine("Stop calibration completed.");
            if (errorCode != ErrorCode.Success)
            {
                HandleError(Tobii.Gaze.Core.ErrorMessage.GetErrorMessage(errorCode));
                return;
            }

            _dispatcher.Invoke(new Action(() =>
            {
                Stage = WpfCalibrationSample.Stage.Finished;
                OnPropertyChanged("Stage");
            }));
        }

        private void OnGenericOperationCompleted(ErrorCode errorCode)
        {
            Trace.WriteLine("Operation completed.");
            if (errorCode != ErrorCode.Success)
            {
                HandleError(Tobii.Gaze.Core.ErrorMessage.GetErrorMessage(errorCode));
            }
        }

        private void HandleError(string message)
        {
            Trace.WriteLine("Error: " + message);

            var action = new Action(() =>
                {
                    Stage = WpfCalibrationSample.Stage.Error;
                    ErrorMessage = message;
                    OnPropertyChanged("Stage");
                });

            if (_dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                _dispatcher.BeginInvoke(action);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
