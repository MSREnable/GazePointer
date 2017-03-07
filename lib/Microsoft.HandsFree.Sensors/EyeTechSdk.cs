using Microsoft.HandsFree.Sensors.Interop.EyeTech;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Microsoft.HandsFree.Sensors
{
    public class EyeTechSdk : IGazeDataProvider
    {
        /*
         * !!!NOTE!!!
         * This initial checkin of the EyeTechSdk is actually a copy of the MouseSdk.
         * This is by design to assist in bringup of the new plumbing. This file needs
         * to be redone to point to the EyeTechSensor.
         */

        int _device;

        long _running;
        SemaphoreSlim _stoppedSemaphore;

        event EventHandler<GazeEventArgs> IGazeDataProvider.GazeEvent
        {
            add
            {
                _gazeEvent += value;
            }
            remove
            {
                _gazeEvent -= value;
            }
        }
        event EventHandler<GazeEventArgs> _gazeEvent;

        public Sensors Sensor => Sensors.EyeTechSDK;

        void IGazeDataProvider.BeginAddCalibrationPoint(int x, int y)
        {
        }

        void IGazeDataProvider.EndAddCalibrationPoint()
        {
        }

        bool IGazeDataProvider.Detect()
        {
            try
            {
                _device = EyeTechApi.Initialize(@"C:\Temp\Setup.txt");
                EyeTechApi.Start(_device);
                EyeTechApi.Stop(_device);
            }
            catch
            {
                return false;
            }

            return true;
        }

        bool IGazeDataProvider.Initialize()
        {
            _running = 1;
            _stoppedSemaphore = new SemaphoreSlim(0);

            _device = EyeTechApi.Initialize(@"C:\Temp\Setup.txt");
            ThreadPool.QueueUserWorkItem((o) =>
                {
                    EyeTechApi.Start(_device);
                    while (Interlocked.Read(ref _running) != 0)
                    {
                        var frame = EyeTechApi.GetFrame(_device);

                        if (frame._isValid)
                        {
                            var e = new GazeEventArgs(frame._x / 100.0, frame._y / 100.0, Environment.TickCount, Fixation.Unknown, true);
                            _gazeEvent?.Invoke(this, e);
                        }
                    }
                    EyeTechApi.Stop(_device);
                    _stoppedSemaphore.Release();
                });

            return true;
        }

        Task<bool> IGazeDataProvider.CreateProfileAsync()
        {
            ((IGazeDataProvider)this).LaunchRecalibration();
            return Task.FromResult(true);
        }

        void IGazeDataProvider.LaunchRecalibration()
        {
            Interlocked.Exchange(ref _running, 0);
            _stoppedSemaphore.Wait();

            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var relativePath = @"EyeTechDS\QuickLINK_2_2.7.3.2\bin\Quick Glance.exe";
            var absolutePath = Path.Combine(programFiles, relativePath);
            var process = Process.Start(absolutePath);
            process.WaitForExit();

            ((IGazeDataProvider)this).Initialize();
        }

        void IGazeDataProvider.Terminate()
        {
        }
    }
}
