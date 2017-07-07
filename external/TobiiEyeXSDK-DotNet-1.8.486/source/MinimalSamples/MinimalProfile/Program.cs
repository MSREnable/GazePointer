//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace MinimalProfile
{
    using System;
    using System.Collections.Generic;
    using EyeXFramework;

    public class Program
    {
        public static void Main(string[] args)
        {
            using (var eyeXHost = new EyeXHost())
            {
                Run(eyeXHost);
            }

            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Press ANY key to quit");
            Console.ReadKey(true);
        }

        private static void Run(EyeXHost eyeXHost)
        {
            Console.CursorVisible = false;
            Console.WriteLine("AVAILABLE EYEX PROFILES");
            Console.WriteLine("=======================");
            Console.WriteLine();

            // Start the EyeX host.
            Version engineVersion;
            if (!StartHost(eyeXHost, out engineVersion))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Could not connect to EyeX Engine.");
                return;
            }

            // Too old EyeX Engine installed?
            if (engineVersion.Major != 1 || engineVersion.Major == 1 && engineVersion.Minor < 4)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("This sample requires EyeX Engine 1.4.");
                return;
            }

            // Create key bindings from the profiles.
            var keyBindings = CreateKeyBindings(eyeXHost.UserProfiles);
            foreach (var keyBinding in keyBindings)
            {
                Console.WriteLine("[{0}] {1}", keyBinding.Key, keyBinding.Value);
            }

            Console.WriteLine();
            Console.WriteLine("Select a profile to set it as the current one.");
            Console.WriteLine("Press ESC to abort.");

            // Read input from the user.
            var result = Console.ReadKey(true);
            if (result.Key != ConsoleKey.Escape)
            {
                if (keyBindings.ContainsKey(result.Key))
                {
                    // Change the profile.
                    var profile = keyBindings[result.Key];
                    eyeXHost.SetCurrentUserProfile(profile);
                }
            }
        }

        /// <summary>
        /// Starts the EyeX host and waits for all profiles to become available.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="version">The engine version.</param>
        private static bool StartHost(EyeXHost host, out Version version)
        {
            // Start the host.
            host.Start();

            // Wait for the client to connect.
            if (!host.WaitUntilConnected(TimeSpan.FromSeconds(5)))
            {
                version = null;
                return false;
            }

            // Get the engine version number.
            version = host.GetEngineVersion().Result;
            return true;
        }

        /// <summary>
        /// Combines the provided profiles with a sequence of <see cref="ConsoleKey"/>.
        /// </summary>
        /// <param name="profiles">The profiles to create key bindings for.</param>
        /// <returns>A dictionary of profile key bindings.</returns>
        private static IDictionary<ConsoleKey, string> CreateKeyBindings(EngineStateValue<string[]> profiles)
        {
            var bindings = new Dictionary<ConsoleKey, string>();
            if (profiles.IsValid)
            {
                const int startIndex = (int)ConsoleKey.F1;
                var maxIndex = startIndex + profiles.Value.Length;
                for (var index = startIndex; index < maxIndex; index++)
                {
                    bindings.Add((ConsoleKey)index, profiles.Value[index - startIndex]);
                }

            }
            return bindings;
        }
    }
}
