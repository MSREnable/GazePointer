using System;
using System.Threading;
using SMIEyeTrackingController;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Point = System.Windows.Point;
using System.Threading.Tasks;

namespace Microsoft.HandsFree.Sensors
{
    public class SMISdk : IGazeDataProvider
    {
        readonly TraceSource _trace = new TraceSource("GazeMouse", SourceLevels.All);

        public event EventHandler<GazeEventArgs> GazeEvent;

        Rectangle _screenRect;
        EyeTrackingController _eyeTracker;
        Thread _eyeTrackerReadThread;
        bool _terminating = false;
        int _sleepTime = 33;

        public Sensors Sensor { get { return Sensors.SMImyGaze; } }

        public bool Initialize()
        {
            try
            {
                _screenRect = Screen.PrimaryScreen.Bounds;
                _eyeTracker = new EyeTrackingController();
                int ret = _eyeTracker.iV_Connect();
                if (ret != (int)SMIEyeTrackingController.EyeTrackingController.RetCode.Success)
                {
                    return false;
                }

                _eyeTrackerReadThread = new Thread(new ThreadStart(GazeDataThreadProc));
                _eyeTrackerReadThread.Start();
            }
            catch (Exception ex)
            {
                _trace.TraceInformation(ex.Message);
                return false;
            }
            return true;
        }

        public void Terminate()
        {
            try
            {
                _eyeTracker.iV_Disconnect();
            }
            catch (Exception ex)
            {
                _trace.TraceInformation(ex.Message);
            }
        }

        public Task<bool> CreateProfileAsync()
        {
            LaunchRecalibration();
            return Task.FromResult(true);
        }

        public void LaunchRecalibration()
        {

        }

        public void BeginAddCalibrationPoint(int x, int y)
        {

        }

        public void EndAddCalibrationPoint()
        {

        }

        public void GazeDataThreadProc()
        {
            while (!_terminating)
            {
                EyeTrackingController.SampleStruct sampleData = new EyeTrackingController.SampleStruct();
                if (_eyeTracker.iV_GetSample(ref sampleData) != (int)EyeTrackingController.RetCode.Success)
                {
                    Thread.Sleep(_sleepTime);
                    continue;
                }

                EventHandler<GazeEventArgs> handler = GazeEvent;
                if (handler != null)
                {
                    GazeEventArgs gazeEventArgs = new GazeEventArgs(sampleData.leftEye.gazeX,
                                                                    sampleData.leftEye.gazeY,
                                                                    sampleData.timestamp / 1000,
                                                                    Fixation.Unknown,
                                                                    false);
                    handler(this, gazeEventArgs);
                }

                Thread.Sleep(_sleepTime);
            }
        }
    }
}
