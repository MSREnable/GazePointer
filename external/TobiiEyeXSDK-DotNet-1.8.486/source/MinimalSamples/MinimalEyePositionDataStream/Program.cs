//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace MinimalEyePositionDataStream
{
    using EyeXFramework;
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            using (var eyeXHost = new EyeXHost())
            {
                eyeXHost.Start();

                using (var stream = eyeXHost.CreateEyePositionDataStream())
                {
                    stream.Next += (s, e) =>
                    {
                        Console.SetCursorPosition(0, 0);

                        // Output information about the left eye.
                        Console.WriteLine("LEFT EYE");
                        Console.WriteLine("========");
                        Console.WriteLine("3D Position: ({0:0.0}, {1:0.0}, {2:0.0})                   ", 
                            e.LeftEye.X, e.LeftEye.Y, e.LeftEye.Z);
                        Console.WriteLine("Normalized : ({0:0.0}, {1:0.0}, {2:0.0})                   ", 
                            e.LeftEyeNormalized.X, e.LeftEyeNormalized.Y, e.LeftEyeNormalized.Z);

                        // Output information about the right eye.
                        Console.WriteLine();
                        Console.WriteLine("RIGHT EYE");
                        Console.WriteLine("=========");
                        Console.WriteLine("3D Position: {0:0.0}, {1:0.0}, {2:0.0}                   ", 
                            e.RightEye.X, e.RightEye.Y, e.RightEye.Z);
                        Console.WriteLine("Normalized : {0:0.0}, {1:0.0}, {2:0.0}                   ", 
                            e.RightEyeNormalized.X, e.RightEyeNormalized.Y, e.RightEyeNormalized.Z);
                    };

                    Console.SetCursorPosition(0, 12);
                    Console.WriteLine("");
                    Console.WriteLine("The 3D position consists of X,Y,Z coordinates expressed in millimeters");
                    Console.WriteLine("in relation to the center of the screen where the eye tracker is mounted.");
                    Console.WriteLine("\n");
                    Console.WriteLine("The normalized coordinates are expressed in relation to the track box,");
                    Console.WriteLine("i.e. the volume in which the eye tracker is theoretically able to track eyes.");
                    Console.WriteLine("- (0,0,0) represents the upper, right corner closest to the eye tracker.");
                    Console.WriteLine("- (1,1,1) represents the lower, left corner furthest away from the eye tracker.");
                    Console.WriteLine();
                    Console.WriteLine("---------------------------------------------------------");
                    Console.WriteLine("Listening for eye position data, press any key to exit...");

                    Console.In.Read();
                }
            }
        }
    }
}
