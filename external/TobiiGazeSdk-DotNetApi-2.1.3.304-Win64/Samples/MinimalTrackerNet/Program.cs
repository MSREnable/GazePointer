//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

using System;
using System.Threading;
using Tobii.Gaze.Core;

namespace MinimalTrackerNet
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            Uri url = null;
            if (args.Length == 1)
            {
                if (args[0] == "--auto")
                {
                    url = new EyeTrackerCoreLibrary().GetConnectedEyeTracker();
                    if (url == null)
                    {
                        Console.WriteLine("No eye tracker found");
                        return;
                    }
                }
                else if (args[0] == "--list")
                {
                    ListConnectedEyeTrackers();
                    return;
                }
                else
                {
                    url = new Uri(args[0]);
                }
            }
            else
            {
                Console.WriteLine("usage: MinimalTrackerNet {url|--auto|--list}");
                return;
            }

            IEyeTracker tracker = null;
            Thread thread = null;
            try
            {
                Console.WriteLine("TobiiGazeCore DLL version: {0}", CoreLibraryVersion());
                Console.WriteLine("Creating eye tracker with url {0}.", url);
                tracker = new EyeTracker(url);

                tracker.EyeTrackerError += EyeTrackerError;
                tracker.GazeData += EyeTrackerGazeData;

                thread = CreateAndRunEventLoopThread(tracker);

                Console.WriteLine("Connecting...");
                tracker.Connect();
                Console.WriteLine("Connected");

                // Good habit to start by retrieving device info to check that communication works.
                PrintDeviceInfo(tracker);

                Console.WriteLine("Start tracking...");
                tracker.StartTracking();
                Console.WriteLine("Tracking started");

                // Let eye tracker track for 20 s.
                Thread.Sleep(20000);

                Console.WriteLine("Stop tracking...");
                tracker.StopTracking();
                Console.WriteLine("Stopped tracking");

                Console.WriteLine("Disconnecting...");
                tracker.Disconnect();
                Console.WriteLine("Disconnected");
            }
            catch (EyeTrackerException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (thread != null)
                {
                    tracker.BreakEventLoop();
                    thread.Join();
                }

                if (tracker != null)
                {
                    tracker.Dispose();
                }
            }
        }

        private static void ListConnectedEyeTrackers()
        {
            Console.WriteLine("Connected eye trackers:");
            foreach (var url in new EyeTrackerCoreLibrary().GetConnectedEyeTrackers())
            {
                Console.WriteLine(url);
            }
        }

        private static string CoreLibraryVersion()
        {
            using (var lib = new EyeTrackerCoreLibrary())
            {
                return lib.LibraryVersion();
            }
        }

        private static Thread CreateAndRunEventLoopThread(IEyeTracker tracker)
        {
            var thread = new Thread(() =>
            {
                try
                {
                    tracker.RunEventLoop();
                }
                catch (EyeTrackerException ex)
                {
                    Console.WriteLine("An error occurred in the eye tracker event loop: " + ex.Message);
                }

                Console.WriteLine("Leaving the event loop.");
            });

            thread.Start();

            return thread;
        }

        private static void PrintDeviceInfo(IEyeTracker tracker)
        {
            var info = tracker.GetDeviceInfo();
            Console.WriteLine("Serial number: {0}", info.SerialNumber);

            var trackBox = tracker.GetTrackBox();
            Console.WriteLine("Track box front upper left ({0}, {1}, {2})", trackBox.FrontUpperLeftPoint.X, trackBox.FrontUpperLeftPoint.Y, trackBox.FrontUpperLeftPoint.Z);
        }

        private static void EyeTrackerGazeData(object sender, GazeDataEventArgs e)
        {
            var gazeData = e.GazeData;
            Console.Write("{0} ", gazeData.Timestamp / 1e6); // in seconds
            Console.Write("{0} ", gazeData.TrackingStatus);

            if (gazeData.TrackingStatus == TrackingStatus.BothEyesTracked ||
                gazeData.TrackingStatus == TrackingStatus.OnlyLeftEyeTracked ||
                gazeData.TrackingStatus == TrackingStatus.OneEyeTrackedProbablyLeft)
            {
                Console.Write("[{0:N4},{1:N4}] ", gazeData.Left.GazePointOnDisplayNormalized.X, gazeData.Left.GazePointOnDisplayNormalized.Y);
            }
            else
            {
                Console.Write("[-,-] ");
            }

            if (gazeData.TrackingStatus == TrackingStatus.BothEyesTracked ||
                gazeData.TrackingStatus == TrackingStatus.OnlyRightEyeTracked ||
                gazeData.TrackingStatus == TrackingStatus.OneEyeTrackedProbablyRight)
            {
                Console.Write("[{0:N4},{1:N4}] ", gazeData.Right.GazePointOnDisplayNormalized.X, gazeData.Right.GazePointOnDisplayNormalized.Y);
            }
            else
            {
                Console.Write("[-,-] ");
            }

            Console.WriteLine();
        }

        private static void EyeTrackerError(object sender, EyeTrackerErrorEventArgs e)
        {
            Console.WriteLine("ERROR: " + e.Message);
        }

        private Program()
        {
        }
    }
}