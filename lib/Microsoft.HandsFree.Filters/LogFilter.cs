using Microsoft.HandsFree.Sensors;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Microsoft.HandsFree.Filters
{
    public class LogFilter :  IFilter
    {
        private Point _dummyStatsPoint;
        private int _maxLogEntries;
        private List<GazeEventArgs> _logEntries;
        public Settings Settings = new Settings();
        // TODO: Make this follow the settings pattern.
        public LoggingSettings LoggingSettings = new LoggingSettings();

        public void Initialize()
        {
            _dummyStatsPoint = new Point();
            _logEntries = new List<GazeEventArgs>();
            _maxLogEntries = LoggingSettings.LogDuration * 30 * 60; // N minutes at 30fps
        }

        public void Terminate()
        {
            StreamWriter logFile = new StreamWriter(LoggingSettings.GazeDataLogFile, false);
            foreach (GazeEventArgs entry in _logEntries)
            {
                logFile.WriteLine($"{entry.Scaled.X}, {entry.Scaled.Y}, {entry.Timestamp}");
            }
            logFile.Close();
        }

        public GazeEventArgs Update(GazeEventArgs gazeArgs)
        {
            _logEntries.Add(gazeArgs);
            if (_logEntries.Count > _maxLogEntries)
            {
                _logEntries.RemoveAt(0);
            }
            return gazeArgs.Clone();
        }

        public Point StandardDeviationOriginal()
        {
            return _dummyStatsPoint;
        }

        public Point StandardDeviationFiltered()
        {
            return _dummyStatsPoint;
        }
    }
}
