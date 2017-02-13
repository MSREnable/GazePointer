using System;
using System.Diagnostics;

namespace Microsoft.HandsFree.Sensors
{
    public class GazeDataProvider
    {
        public static IGazeDataProvider InitializeGazeDataProvider(Settings settings = null)
        {

            if (settings == null)
            {
                settings = new Settings { Sensor = Sensors.TobiiEyeXSDK, UseFixationStream = false };
            }

            IGazeDataProvider gazeDataProvider;
            switch (settings.Sensor)
            {
                case Sensors.TobiiEyeXSDK:
                    gazeDataProvider = new TobiiEyeXSdk(settings.UseFixationStream);
                    break;
                case Sensors.TobiiGazeSDK:
                    gazeDataProvider = new TobiiGazeSdk();
                    break;
                case Sensors.Mouse:
                    gazeDataProvider = new MouseSdk();
                    break;
                case Sensors.LogPlayback:
                    gazeDataProvider = new LogPlaybackSdk(new LoggingSettings().GazeDataLogFile);
                    break;
                case Sensors.None:
                    gazeDataProvider = new NullSdk();
                    break;
                default:
                    throw new ArgumentException("Unknown sensor type");
            }

            if (!gazeDataProvider.Initialize())
            {
                gazeDataProvider = new MouseSdk();
                var ret = gazeDataProvider.Initialize();
                Debug.Assert(ret);               
            }
            return gazeDataProvider;
        }
    }
}
