//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace WpfCalibrationSample
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// View model interface for the calibration window.
    /// The view model decides what should be displayed in the calibration window 
    /// and acts on input from the calibration window.
    /// </summary>
    internal interface ICalibrationViewModel : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Gets the stage the view is currently in.
        /// </summary>
        Stage Stage { get; }

        /// <summary>
        /// Gets a command used for moving on to the next stage if possible.
        /// (Typically invoked when the user presses space.)
        /// </summary>
        ICommand ContinueCommand { get; }

        /// <summary>
        /// Gets a command used for moving to the Exiting stage. Do not pass go, do not collect $200. 
        /// (Typically invoked when the user presses escape.)
        /// </summary>
        ICommand ExitCommand { get; }

        /// <summary>
        /// Gets an error message that describes what went wrong in the Error stage.
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// Gets the positions of the detected eyes in the PositioningGuide stage.
        /// </summary>
        ObservableCollection<Point> EyePositions { get; }

        /// <summary>
        /// Gets the current positioning status in the PositioningGuide stage.
        /// </summary>
        PositioningStatus PositioningStatus { get; }

        /// <summary>
        /// Gets the position of the calibration dot in the Calibration stage.
        /// </summary>
        Point CalibrationDotPosition { get; }

        /// <summary>
        /// Starts collecting data for a calibration point. Call this method when the animation is finished.
        /// </summary>
        void CalibrationDotAnimationCompleted();
    }

    /// <summary>
    /// Modes that the view may be in.
    /// </summary>
    public enum Stage
    {
        /// <summary>
        /// Initializing.
        /// </summary>
        Initializing,

        /// <summary>
        /// Positioning guide.
        /// </summary>
        PositioningGuide,

        /// <summary>
        /// Calibration.
        /// </summary>
        Calibration,

        /// <summary>
        /// Calibration is being computed, please wait.
        /// </summary>
        ComputingCalibration,

        /// <summary>
        /// Calibration failed.
        /// </summary>
        CalibrationFailed,

        /// <summary>
        /// We're finished.
        /// </summary>
        Finished,

        /// <summary>
        /// There was an error.
        /// </summary>
        Error
    }

    /// <summary>
    /// Possible states for the positioning guide.
    /// </summary>
    public enum PositioningStatus
    {
        /// <summary>
        /// Too close.
        /// </summary>
        TooClose,

        /// <summary>
        /// Too far, or not detected.
        /// </summary>
        TooFarOrNotDetected,

        /// <summary>
        /// Fine.
        /// </summary>
        PositionOk
    }
}
