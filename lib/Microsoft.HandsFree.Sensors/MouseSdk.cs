using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Microsoft.HandsFree.Sensors
{
    internal class MouseSdk : IGazeDataProvider
    {
        DispatcherTimer _timer;

        int _mouseMoveCount;

        event EventHandler<GazeEventArgs> IGazeDataProvider.GazeEvent
        {
            add
            {
                _gazeEvent += value;
            }
            remove
            {
                _gazeEvent -= value;
            }
        }
        event EventHandler<GazeEventArgs> _gazeEvent;

        public Sensors Sensor => Sensors.Mouse;

        void MouseTick(object sender, EventArgs e)
        {
            try
            {
                var window = Application.Current.MainWindow;
                var windowPoint = Mouse.GetPosition(window);
                var point = window.PointToScreen(windowPoint);

                var eventData = new GazeEventArgs(point.X, point.Y, Environment.TickCount, Fixation.Unknown, false);

                _gazeEvent?.Invoke(this, eventData);
            }
            catch
            {
                // A Win32Exception is sometimes thrown Mouse.GetPosition; this try-catch block just lessens the impact of that.
            }
        }

        void IGazeDataProvider.BeginAddCalibrationPoint(int x, int y)
        {
        }

        void IGazeDataProvider.EndAddCalibrationPoint()
        {
        }

        bool IGazeDataProvider.Detect()
        {
            return true;
        }

        bool IGazeDataProvider.Initialize()
        {
            Application.Current.MainWindow.MouseMove += HandleMouseMove;

            return true;
        }

        void HandleMouseMove(object sender, MouseEventArgs e)
        {
            _mouseMoveCount++;

            // A mouse move occurs for free when the application starts up, so wait until the
            // second one arrives before assuming there is a real moving mouse to listen to. This
            // prevents a stationary mouse (or no mouse) located over a button causing it to be
            // interpreted as a dwell.
            if (2 <= _mouseMoveCount)
            {
                Application.Current.MainWindow.MouseMove -= HandleMouseMove;

                _timer = new DispatcherTimer(TimeSpan.FromSeconds(1.0 / 15), DispatcherPriority.Normal, MouseTick, Dispatcher.CurrentDispatcher);
            }
        }

        Task<bool> IGazeDataProvider.CreateProfileAsync()
        {
            ((IGazeDataProvider)this).LaunchRecalibration();
            return Task.FromResult(true);
        }

        void IGazeDataProvider.LaunchRecalibration()
        {
        }

        void IGazeDataProvider.Terminate()
        {
            _timer?.Stop();
        }
    }
}
