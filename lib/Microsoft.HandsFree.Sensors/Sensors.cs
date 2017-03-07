using System.ComponentModel;

namespace Microsoft.HandsFree.Sensors
{
    public enum Sensors
    {
        [Description("Tobii EyeX SDK")]
        TobiiEyeXSDK,
        [Description("Tobii Gaze SDK")]
        TobiiGazeSDK,
	    [Description("EyeTech SDK")]
        EyeTechSDK,
        [Description("Mouse")]
        Mouse,
        [Description("Log Playback")]
        LogPlayback,
        [Description("None")]
        None
    }
}
