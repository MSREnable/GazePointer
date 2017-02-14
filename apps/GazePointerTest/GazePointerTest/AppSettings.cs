using Microsoft.HandsFree.Mouse;
using Microsoft.HandsFree.MVVM;
using Microsoft.HandsFree.Settings.Serialization;
using System;
using System.Reflection;
using System.Xml.Serialization;

// TODO: Clarify the use of this class.
namespace GazePointerTest
{
    public class AppSettings : NotifyingObject
    {
        static readonly SettingsStore<AppSettings> store = LoadSettings();

        public static readonly AppSettings Instance = store.Settings;

        public static readonly SettingsStore<AppSettings> Store = store;

        static SettingsStore<AppSettings> LoadSettings()
        {
            var path = SettingsDirectory.GetDefaultSettingsFilePath(DefaultSettingsFile);
            var store = SettingsStore<AppSettings>.Create(path);

            return store;
        }

        public Settings Mouse { get { return _mouse; } set { SetProperty(ref _mouse, value); } }
        Settings _mouse;

        [XmlIgnore]
        public string ApplicationVersion => _applicationVersion;
        static readonly string _applicationVersion = GetApplicationVersion();
        // TODO: The following code really belongs in some Hands Free branding assembly that contains code common to all Enable products.
        static string GetApplicationVersion()
        {
            var number = Assembly.GetEntryAssembly().GetName().Version;
            var branch = ((AssemblyInformationalVersionAttribute)Attribute.GetCustomAttribute(Assembly.GetEntryAssembly(), typeof(AssemblyInformationalVersionAttribute)))?.InformationalVersion;
            var version = $"{branch}-{number}";
            return version;
        }

        public static string DefaultSettingsFile => SettingsDirectory.GetDefaultSettingsFilePath("GazePointerTest.config");

        public AppSettings()
        {
            Mouse = new Settings();
        }
    }
}
