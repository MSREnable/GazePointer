//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

using Tobii.EyeX.Client.Interop;
using Tobii.EyeX.Framework;

namespace MinimalEngineStates
{
    using System;
    using EyeXFramework;
    using Tobii.EyeX.Client;

    public static class Program
    {
        public static void Main(string[] args)
        {
            switch (EyeXHost.EyeXAvailability)
            {
                case EyeXAvailability.NotAvailable:
                    Console.WriteLine("This sample requires the EyeX Engine, but it isn't available.");
                    Console.WriteLine("Please install the EyeX Engine and try again.");
                    return;

                case EyeXAvailability.NotRunning:
                    Console.WriteLine("This sample requires the EyeX Engine, but it isn't rnning.");
                    Console.WriteLine("Please make sure that the EyeX Engine is started.");
                    break;
            }

            using (var eyeXHost = new EyeXHost())
            {
                // Listen to state-changed events.
                eyeXHost.ScreenBoundsChanged += (s, e) => Console.WriteLine("Screen Bounds in pixels (state-changed event): {0}", e);
                eyeXHost.DisplaySizeChanged += (s, e) => Console.WriteLine("Display Size in millimeters (state-changed event): {0}", e);
                eyeXHost.EyeTrackingDeviceStatusChanged += (s, e) => Console.WriteLine("Eye tracking device status (state-changed event): {0}", e);
                eyeXHost.UserPresenceChanged += (s, e) => Console.WriteLine("User presence (state-changed event): {0}", e);
                eyeXHost.UserProfileNameChanged += (s, e) => Console.WriteLine("Active profile name (state-changed event): {0}", e);
                eyeXHost.ConfigurationStatusChanged += (s, e) => Console.WriteLine("Configuration status is : {0}", e);

                // This state-changed event required EyeX Engine 1.4.
                eyeXHost.UserProfilesChanged += (s, e) => Console.WriteLine("User profile names (state-changed event): {0}", e);
                eyeXHost.GazeTrackingChanged += (s, e) => Console.WriteLine("Gaze tracking (state-changed event): {0}", e);

                // Start the EyeX host.
                eyeXHost.Start();
                eyeXHost.WaitUntilConnected(TimeSpan.FromSeconds(5));

                // First, let's display the current engine state. Because we're still in the 
                // process of connecting to the engine, we might not get any valid state 
                // information at this point.
                Console.WriteLine("Screen Bounds in pixels (initial value): {0}", eyeXHost.ScreenBounds);
                Console.WriteLine("Display Size in millimeters (initial value): {0}", eyeXHost.DisplaySize);
                Console.WriteLine("Eye tracking device status (initial value): {0}", eyeXHost.EyeTrackingDeviceStatus);
                Console.WriteLine("User presence (initial value): {0}", eyeXHost.UserPresence);
                Console.WriteLine("Active profile name (initial value): {0}", eyeXHost.UserProfileName);
                Console.WriteLine("Configuration status (initial value): {0}", eyeXHost.ConfigurationStatus);

                // This state requires EyeX Engine 1.4.
                Console.WriteLine("User profile names (initial value): {0}", eyeXHost.UserProfiles);
                Console.WriteLine("Gaze tracking (initial value): {0}", eyeXHost.GazeTracking);

                // Wait for the user to exit the application.
                Console.WriteLine("Listening for state changes, press any key to exit...");
                Console.ReadKey(true);
            }
        }
    }
}
