//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace WpfCalibrationSample
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// View model used for testing the calibration window.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Class used for testing the window.")]
    internal sealed class TestingViewModel : ICalibrationViewModel
    {
        private int _testCase;

        public TestingViewModel(Action exitAction)
        {
            EyePositions = new ObservableCollection<Point>();
            _testCase = 8;

            ContinueCommand = new ActionCommand(() =>
                {
                    _testCase++;
                    Init();
                });

            ExitCommand = new ActionCommand(exitAction);

            Init();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Invoked from Xaml.")]
        public static TestingViewModel Instance
        {
            get { return new TestingViewModel(new Action(() => { })); }
        }

        public Stage Stage { get; private set; }

        public ICommand ContinueCommand { get; private set; }

        public ICommand ExitCommand { get; private set; }

        public ObservableCollection<Point> EyePositions { get; private set; }

        public PositioningStatus PositioningStatus { get; private set; }

        public Point CalibrationDotPosition { get; private set; }

        public string ErrorMessage { get; private set; }

        public void Dispose()
        {
            // do nothing
        }

        public void CalibrationDotAnimationCompleted()
        {
            if (_testCase == 2)
            {
                _testCase++;
                Init();
            }
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Init()
        {
            switch (_testCase)
            {
                case 0:
                    Stage = WpfCalibrationSample.Stage.Initializing;
                    OnPropertyChanged();
                    break;

                case 1:
                    Stage = WpfCalibrationSample.Stage.PositioningGuide;
                    EyePositions.Clear();
                    EyePositions.Add(new Point(0, 0));
                    EyePositions.Add(new Point(1, 1));
                    OnPropertyChanged();
                    break;

                case 2:
                    Stage = WpfCalibrationSample.Stage.Calibration;
                    OnPropertyChanged();
                    CalibrationDotPosition = new Point(0.1, 0.1);
                    OnPropertyChanged("CalibrationDotPosition");
                    break;

                case 3:
                    Stage = WpfCalibrationSample.Stage.Calibration;
                    OnPropertyChanged();
                    CalibrationDotPosition = new Point(0, 0);
                    OnPropertyChanged("CalibrationDotPosition");
                    break;

                case 4:
                    Stage = WpfCalibrationSample.Stage.Calibration;
                    OnPropertyChanged();
                    CalibrationDotPosition = new Point(1, 1);
                    OnPropertyChanged("CalibrationDotPosition");
                    break;

                case 5:
                    Stage = WpfCalibrationSample.Stage.Calibration;
                    OnPropertyChanged();
                    CalibrationDotPosition = new Point(0.5, 0.5);
                    OnPropertyChanged("CalibrationDotPosition");
                    break;

                case 6:
                    Stage = WpfCalibrationSample.Stage.ComputingCalibration;
                    OnPropertyChanged();
                    break;

                case 7:
                    Stage = WpfCalibrationSample.Stage.Finished;
                    OnPropertyChanged();
                    break;

                case 8:
                    Stage = WpfCalibrationSample.Stage.Error;
                    ErrorMessage = "There has been a failure on the internets!";
                    OnPropertyChanged();
                    break;
            }
        }
    }
}
