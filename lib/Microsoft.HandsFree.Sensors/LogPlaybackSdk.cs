using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Microsoft.HandsFree.Sensors
{
    public class LogPlaybackSdk : IGazeDataProvider
    {
        private string _logFile;
        private bool _terminating = false;
        private Thread _gazeDataThread = null;
        private StreamReader _gazeDataReader = null;


        public event EventHandler<GazeEventArgs> GazeEvent;

        public Sensors Sensor {  get { return Sensors.LogPlayback; } }

        public LogPlaybackSdk(string logFile)
        {
            _logFile = logFile;
        }

        public bool Initialize()
        {
            if (!File.Exists(_logFile))
            {
                return false;
            }

            _gazeDataReader = new StreamReader(_logFile);
            _gazeDataThread = new Thread(new ThreadStart(GazeDataReaderProc));
            _gazeDataThread.Start();
            return true;
        }

        public void Terminate()
        {
            _terminating = true;
            _gazeDataThread.Join();
            _gazeDataReader.Close();
            _gazeDataReader = null;
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

        private GazeEventArgs ParseGazeEventArgs(string line)
        {
            string[] parts = line.Split(',');
            if (parts.Length != 3)
            {
                return null;
            }

            double x = double.Parse(parts[0]);
            double y = double.Parse(parts[1]);
            long t = long.Parse(parts[2]);

            GazeEventArgs ea = new GazeEventArgs(x, y, t, Fixation.Unknown, true);
            return ea;
        }

        private void GazeDataReaderProc()
        {
            string line = null;
            GazeEventArgs ea = null;
            long curTimeStamp = 0;

            line = _gazeDataReader.ReadLine();
            if (line != null)
            {
                ea = ParseGazeEventArgs(line);
                if (ea != null)
                {
                    curTimeStamp = ea.Timestamp;
                }
            }

            Thread.Sleep(2000);
            RaiseGazeEvent(ea);

            while ((!_terminating) && ((line = _gazeDataReader.ReadLine()) != null))
            {
                ea = ParseGazeEventArgs(line);
                if (ea == null)
                {
                    continue;
                }

                Thread.Sleep((int)(ea.Timestamp - curTimeStamp));
                curTimeStamp = ea.Timestamp;
                RaiseGazeEvent(ea);
            }
        }

        private void RaiseGazeEvent(GazeEventArgs ea)
        {
            EventHandler<GazeEventArgs> handler = GazeEvent;
            if (handler != null)
            {
                handler(this, ea);
            }
        }
    }
}
