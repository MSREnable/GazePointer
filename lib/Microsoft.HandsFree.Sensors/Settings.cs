using Microsoft.HandsFree.MVVM;
using Microsoft.HandsFree.Settings.Serialization;

namespace Microsoft.HandsFree.Sensors
{
    public class Settings : NotifyingObject
    {
        [SettingDescription("Sensor Type\n(may only be changed using touch and requires restart)")]
        public Sensors Sensor
        {
            get { return _sensor; }
            set { SetProperty(ref _sensor, value); }
        }
        Sensors _sensor = Sensors.TobiiEyeXSDK;

        // This is not a filter settings. But a feature of the sensor SDK if it supports it
        // Right now, only the Tobii SDK supports
        [SettingDescription("Use Fixation Stream")]
        public bool UseFixationStream
        {
            get { return _useFixationStream; }
            set { SetProperty(ref _useFixationStream, value); }
        }
        bool _useFixationStream;
    }
}
