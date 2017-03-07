using System;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.HandsFree.Sensors
{
    public enum Fixation
    {
        Unknown,
        True,
        False
    };

    public class GazeEventArgs : EventArgs
    {
        public Point Scaled;
        public Point Screen;
        public long Timestamp;
        public Fixation Fixation;

        public GazeEventArgs(double x, double y, long timestamp, Fixation fixation, bool scaled)
        {
            Timestamp = timestamp;
            Fixation = fixation;

            var screenRect = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            if (scaled)
            {
                Scaled = new Point(x, y);
                Screen = new Point(screenRect.Left + (Scaled.X * screenRect.Width),
                                   screenRect.Top + (Scaled.Y * screenRect.Height));
            }
            else
            {
                Screen = new Point(x, y);
                Scaled = new Point((Screen.X - screenRect.Left)/screenRect.Width,
                                   (Screen.Y - screenRect.Top)/screenRect.Height);
            }
        }

        public GazeEventArgs(double x, double y, GazeEventArgs ea, bool scaled) : 
            this(x, y, ea.Timestamp, ea.Fixation, scaled)
        {
        }


        public override string ToString()
        {
            return $"[{Scaled}, {Screen}], Fixation={Fixation}, Timestamp={Timestamp}";
        }

        public GazeEventArgs Clone()
        {
            return new GazeEventArgs(Scaled.X, Scaled.Y, this, true);
        }
    }

    public interface IGazeDataProvider
    {
        event EventHandler<GazeEventArgs> GazeEvent;
        Sensors Sensor { get; }
        bool Detect();
        bool Initialize();
        void Terminate();

        // TODO: This function needs to be removed and Initialize replaced with an InitializeAsync
        // that does this work everytime a gaze provider is initialized.
        /// <summary>
        /// Do the work necessary on first-run to ensure the system is correctly configured.
        /// TODO: This function needs to be removed when the codebase moves more over to a fully
        /// async model.
        /// </summary>
        /// <returns>true iff operation succeeds, if false the operation should be retried on
        /// next startup. If false is returned it is not guaranteed that the data provider will
        /// behave usefully.</returns>
        Task<bool> CreateProfileAsync();

        void LaunchRecalibration();
        void BeginAddCalibrationPoint(int x, int y);
        void EndAddCalibrationPoint();
    }
}
