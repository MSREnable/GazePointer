using Microsoft.HandsFree.MVVM;
using Microsoft.HandsFree.Sensors;
using Microsoft.HandsFree.Settings;
using Microsoft.HandsFree.Settings.Serialization;

namespace Microsoft.HandsFree.Mouse
{
    public class Settings : NotifyingObject
    {
        public Sensors.Settings Sensor { get { return _sensor; } set { SetProperty(ref _sensor, value); } }
        Sensors.Settings _sensor;

        public Filters.Settings Filter { get { return _filter; } set { SetProperty(ref _filter, value); } }
        Filters.Settings _filter;

        public MouseEmulationSettings MouseEmulation { get { return _mouseEmulation; } set { SetProperty(ref _mouseEmulation, value); } }
        MouseEmulationSettings _mouseEmulation;

        public LoggingSettings Logging { get { return _logging; } set { SetProperty(ref _logging, value); } }
        LoggingSettings _logging;

        [SettingDescription("Track Active Window Only")]
        public bool TrackActiveWindowOnly { get { return _trackActiveWindowOnly; } set { SetProperty(ref _trackActiveWindowOnly, value); } }
        bool _trackActiveWindowOnly = true;

        [SettingDescription("Max Track Count", 0, 1000)]
        public int MaxTrackCount
        {
            get { return _maxTrackCount; }
            set { SetProperty(ref _maxTrackCount, value); }
        }
        int _maxTrackCount = 60;

        [SettingDescription("Show Gaze Cursor")]
        public bool ShowCursor
        {
            get { return _showCursor; }
            set { SetProperty(ref _showCursor, value); }
        }
        bool _showCursor = true;

        [SettingDescription("Show Cursor Tracks")]
        public bool ShowCursorTracks
        {
            get { return _showCursorTracks; }
            set { SetProperty(ref _showCursorTracks, value); }
        }
        bool _showCursorTracks;

        public Settings()
        {
            CreateDefaultSettings();
        }

        private void CreateDefaultSettings()
        {
            Sensor = new Sensors.Settings();
            Filter = new Filters.Settings();
            MouseEmulation = new MouseEmulationSettings();
            Logging = new LoggingSettings();
        }
    }
}
