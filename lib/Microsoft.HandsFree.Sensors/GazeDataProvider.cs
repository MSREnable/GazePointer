using System;
using System.Diagnostics;

namespace Microsoft.HandsFree.Sensors
{
    public class GazeDataProvider
    {
        public static IGazeDataProvider InitializeGazeDataProvider(Settings settings)
        {
            if (settings == null)
            {
                settings = new Settings { Sensor = Detect() };
            }

            IGazeDataProvider gazeDataProvider;
            switch (settings.Sensor)
            {
                case Sensors.TobiiEyeXSDK:
                    gazeDataProvider = new TobiiEyeXSdk(settings.UseFixationStream);
                    break;
                case Sensors.EyeTechSDK:
                    gazeDataProvider = new EyeTechSdk();
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
                gazeDataProvider = new NullSdk();
                var ret = gazeDataProvider.Initialize();
                Debug.Assert(ret);               
            }
            return gazeDataProvider;
        }

        public static Sensors Detect()
        {
            /*
            IGazeDataProvider gazeDataProvider = new TobiiEyeXSdk(false);
            if (gazeDataProvider.Detect())
            {
                return Sensors.TobiiEyeXSDK;
            }

            */
            IGazeDataProvider gazeDataProvider = new EyeTechSdk();
            if (gazeDataProvider.Detect())
            {
                return Sensors.EyeTechSDK;
            }

            return Sensors.Mouse;
        }
    }
}
