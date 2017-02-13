using System;
using System.IO;

namespace Microsoft.HandsFree.Settings.Serialization
{
    /// <summary>
    /// Helper methods for implementing settings.
    /// </summary>
    public static class SettingsDirectory
    {
        static SettingsDirectory()
        {
            if (!Directory.Exists(DefaultSettingsFolder))
            {
                Directory.CreateDirectory(DefaultSettingsFolder);
            }
        }

        /// <summary>
        /// The default location for storing settings.
        /// </summary>
        public static string DefaultSettingsFolder
        {
            get
            {
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(appDataFolder, "Microsoft", "MSREnable");
            }
        }

        /// <summary>
        /// Obtain a file path in the default settings folder.
        /// </summary>
        /// <param name="basename"></param>
        /// <returns></returns>
        public static string GetDefaultSettingsFilePath(string basename)
        {
            return Path.Combine(DefaultSettingsFolder, basename);
        }
    }
}
