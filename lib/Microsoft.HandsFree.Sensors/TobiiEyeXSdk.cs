using EyeXFramework;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using Tobii.EyeX.Framework;

namespace Microsoft.HandsFree.Sensors
{
    public class TobiiEyeXSdk : IGazeDataProvider
    {
        readonly TraceSource _trace = new TraceSource("TobiiEyeXSdk", SourceLevels.Information);
        private readonly EyeXHost _eyeXHost = new EyeXHost();

        public event EventHandler<GazeEventArgs> GazeEvent;

        private readonly bool _useFixationStream;

        private EyeTrackingDeviceStatus _deviceStatus;
        private EyeTrackingConfigurationStatus _configurationStatus;
        private SemaphoreSlim _semaphore;
        private readonly Dispatcher _dispatcher = System.Windows.Application.Current.Dispatcher;
        private bool _initialized;

        public Sensors Sensor { get { return Sensors.TobiiEyeXSDK; } }

        public TobiiEyeXSdk(bool useFixationStream)
        {
            _useFixationStream = useFixationStream;
        }

        public bool Detect()
        {
            return Initialize();
        }

        public bool Initialize()
        {
            if (_initialized)
            {
                return true;
            }

            if (_useFixationStream)
            {
                _eyeXHost.CreateFixationDataStream(FixationDataMode.Slow).Next += OnFixationPointNext;
            }
            else
            {
                _eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered).Next += OnGazePointNext;
            }

            _eyeXHost.Start();

            // TODO: Initialize needs to be replaced with an InitializeAsync to void
            // having to do this callback, waiting and semaphore dance.

            _initialized = false;

            var initialSemaphore = new SemaphoreSlim(0);
            var initialCallback = (EventHandler<EngineStateValue<EyeTrackingDeviceStatus>>)((s, e) =>
            {
                switch (e.Value)
                {
                    case EyeTrackingDeviceStatus.Configuring:
                    case EyeTrackingDeviceStatus.Initializing:
                    case EyeTrackingDeviceStatus.Tracking:
                    case EyeTrackingDeviceStatus.TrackingPaused:
                        _initialized = true;
                        initialSemaphore.Release();
                        break;

                    case EyeTrackingDeviceStatus.DeviceNotConnected:
                        initialSemaphore.Release();
                        break;
                }
            });
            _eyeXHost.EyeTrackingDeviceStatusChanged += initialCallback;

            // Wait a bit for the tracking engine to initialize the device status  
            initialSemaphore.Wait(1000);
            _eyeXHost.EyeTrackingDeviceStatusChanged -= initialCallback;

            return _initialized;
        }

        public void BeginAddCalibrationPoint(int x, int y)
        {

        }

        public void EndAddCalibrationPoint()
        {

        }

        void RaiseGazeEvent(double x, double y, double timestamp)
        {
            EventHandler<GazeEventArgs> handler = GazeEvent;
            if (handler != null)
            {
                GazeEventArgs gazeEventArgs = new GazeEventArgs(x, y, (long)timestamp, Fixation.Unknown, false);
                handler(this, gazeEventArgs);
            }
        }

        void OnFixationPointNext(object sender, FixationEventArgs e)
        {
            RaiseGazeEvent(e.X, e.Y, e.Timestamp);
        }

        public void Terminate()
        {
            if (_useFixationStream)
            {
                _eyeXHost.CreateFixationDataStream(FixationDataMode.Slow).Next -= OnFixationPointNext;
            }
            else
            {
                _eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered).Next -= OnGazePointNext;
            }

            _eyeXHost.Dispose();
        }

        void OnGazePointNext(object sender, GazePointEventArgs e)
        {
            RaiseGazeEvent(e.X, e.Y, e.Timestamp);
        }

        static bool IsSuccessDeviceStatus(EyeTrackingDeviceStatus status)
        {
            switch (status)
            {
                case EyeTrackingDeviceStatus.Initializing:
                case EyeTrackingDeviceStatus.Configuring:
                case EyeTrackingDeviceStatus.Tracking:
                case EyeTrackingDeviceStatus.TrackingPaused:
                    return true;

                default:
                    return false;
            }
        }

        static bool IsFailedDeviceStatus(EyeTrackingDeviceStatus status)
        {
            return status == EyeTrackingDeviceStatus.DeviceNotConnected;
        }

        static bool IsStableDeviceStatus(EyeTrackingDeviceStatus status)
        {
            return IsSuccessDeviceStatus(status) || IsFailedDeviceStatus(status);
        }

        async Task WaitForConfigurationToCompleteAsync()
        {
            // Wait up to about 1s for device to start configuring.
            var isProgressing = true;
            while (isProgressing && _deviceStatus != EyeTrackingDeviceStatus.Configuring)
            {
                Debug.WriteLine("Waiting for device to start configuring");

                isProgressing = await _semaphore.WaitAsync(TimeSpan.FromSeconds(5));
            }

            // Wait for configuration to complete.
            while (_deviceStatus == EyeTrackingDeviceStatus.Configuring)
            {
                Debug.WriteLine("Waiting for device to stop configuring");

                await _semaphore.WaitAsync(TimeSpan.FromSeconds(60));
            }
        }

        static EyeTrackingDeviceStatus GetDeviceStatus(EngineStateValue<EyeTrackingDeviceStatus> status)
        {
            return status.IsValid ? status.Value : default(EyeTrackingDeviceStatus);
        }

        static EyeTrackingConfigurationStatus GetConfigurationStatus(EngineStateValue<EyeTrackingConfigurationStatus> status)
        {
            return status.IsValid ? status.Value : default(EyeTrackingConfigurationStatus);
        }

        void DeviceStatusHandler(object sender, EngineStateValue<EyeTrackingDeviceStatus> e)
        {
            _dispatcher.Invoke(() =>
            {
                _deviceStatus = GetDeviceStatus(e);
                _semaphore.Release();
            });
        }

        void ConfigurationStatusHandler(object sender, EngineStateValue<EyeTrackingConfigurationStatus> e)
        {
            _dispatcher.Invoke(() =>
            {
                _configurationStatus = GetConfigurationStatus(e);
                _semaphore.Release();
            });
        }

        void StartStatusListening()
        {
            _semaphore = new SemaphoreSlim(0);

            // Get initial status values.
            _deviceStatus = GetDeviceStatus(_eyeXHost.EyeTrackingDeviceStatus);
            _configurationStatus = GetConfigurationStatus(_eyeXHost.ConfigurationStatus);

            // Start listening for changes.
            _eyeXHost.EyeTrackingDeviceStatusChanged += DeviceStatusHandler;
            _eyeXHost.ConfigurationStatusChanged += ConfigurationStatusHandler;
        }

        void StopStatusListening()
        {
            _eyeXHost.EyeTrackingDeviceStatusChanged -= DeviceStatusHandler;
            _eyeXHost.ConfigurationStatusChanged -= ConfigurationStatusHandler;

            _semaphore?.Dispose();
            _semaphore = null;
        }

        public async Task<bool> CreateProfileAsync()
        {
            StartStatusListening();

            // Wait for a good configuration status.
            while (_configurationStatus == default(EyeTrackingConfigurationStatus))
            {
                await _semaphore.WaitAsync();
            }

            // Wait for device to become configured.
            var validConfiguration = false;
            for (var iteration = 0; iteration < 10 && !validConfiguration; iteration++)
            {
                switch (_configurationStatus)
                {
                    case EyeTrackingConfigurationStatus.Valid:
                        validConfiguration = true;
                        break;

                    case EyeTrackingConfigurationStatus.InvalidMonitorConfiguration:
                        _eyeXHost.LaunchDisplaySetup();
                        await WaitForConfigurationToCompleteAsync();
                        break;

                    case EyeTrackingConfigurationStatus.InvalidCalibration:
                        _eyeXHost.LaunchProfileCreation();
                        await WaitForConfigurationToCompleteAsync();
                        break;

                    default:
                        await Task.Delay(TimeSpan.FromSeconds(5));
                        break;
                }
            }

            StopStatusListening();

            return validConfiguration;
        }

        public void LaunchRecalibration()
        {
            {
                var start = new ThreadStart(() =>
                {
                    Thread.Sleep(5000);
                    SendKeys.SendWait("{ENTER}");
                });
                var thread = new Thread(start);
                thread.Start();
            }

            {
                var start = new ThreadStart(() => _eyeXHost.LaunchRecalibration());
                var thread = new Thread(start);
                thread.Start();
            }
        }
    }

}
