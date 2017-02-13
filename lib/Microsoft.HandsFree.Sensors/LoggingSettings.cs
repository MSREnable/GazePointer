using Microsoft.HandsFree.MVVM;
using Microsoft.HandsFree.Settings.Serialization;
using System.IO;

namespace Microsoft.HandsFree.Sensors
{
    public class LoggingSettings : NotifyingObject
    {
        bool _logGazeData;
        [SettingDescription("Log Gaze Data")]
        public bool LogGazeData
        {
            get { return _logGazeData; }
            set { SetProperty(ref _logGazeData, value); }
        }

        string _gazeDataLogFile = Path.Combine(SettingsDirectory.DefaultSettingsFolder, "GazeLog.txt");
        [SettingDescription]
        public string GazeDataLogFile
        {
            get { return _gazeDataLogFile; }
            set { SetProperty(ref _gazeDataLogFile, value); }
        }

        int _logDuration = 10;
        [SettingDescription("Log Duration (minutes)", 1, 120, 5)]
        public int LogDuration
        {
            get { return _logDuration; }
            set { SetProperty(ref _logDuration, value); }
        }
    }

}
