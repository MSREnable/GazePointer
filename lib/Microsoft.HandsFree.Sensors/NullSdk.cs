using System;
using System.Threading.Tasks;

namespace Microsoft.HandsFree.Sensors
{
    public class NullSdk : IGazeDataProvider
    {
        public event EventHandler<GazeEventArgs> GazeEvent { add { } remove { } }

        public Sensors Sensor => Sensors.None;

        public void BeginAddCalibrationPoint(int x, int y)
        {
        }

        public void EndAddCalibrationPoint()
        {
        }

        public bool Detect()
        {
            return false;
        }

        public bool Initialize()
        {
            return true;
        }

        public Task<bool> CreateProfileAsync()
        {
            LaunchRecalibration();
            return Task.FromResult(true);
        }

        public void LaunchRecalibration()
        {
        }

        public void Terminate()
        {
        }
    }
}
