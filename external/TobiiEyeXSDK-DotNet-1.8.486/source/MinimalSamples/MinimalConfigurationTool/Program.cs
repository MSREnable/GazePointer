//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace MinimalConfigurationTool
{
    using System;
    using EyeXFramework;

    public class Program
    {
        public static void Main(string[] args)
        {
            using (var eyeXHost = new EyeXHost())
            {
                eyeXHost.Start();

                Console.WriteLine("EYEX CONFIGURATION TOOLS");
                Console.WriteLine("========================");
                Console.WriteLine();
                Console.WriteLine("T) Test calibration");
                Console.WriteLine("G) Guest calibration");
                Console.WriteLine("R) Recalibrate");
                Console.WriteLine("D) Display Setup");
                Console.WriteLine("P) Create Profile");

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.T:
                        eyeXHost.LaunchCalibrationTesting();
                        break;
                    case ConsoleKey.G:
                        eyeXHost.LaunchGuestCalibration();
                        break;
                    case ConsoleKey.R:
                        eyeXHost.LaunchRecalibration();
                        break;
                    case ConsoleKey.D:
                        eyeXHost.LaunchDisplaySetup();
                        break;
                    case ConsoleKey.P:
                        eyeXHost.LaunchProfileCreation();
                        break;
                }
            }
        }
    }
}
