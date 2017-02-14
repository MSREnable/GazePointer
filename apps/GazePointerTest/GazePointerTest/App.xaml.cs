using Microsoft.Shell;
using System;
using System.Reflection;
using System.Windows;
using System.Collections.Generic;

namespace GazePointerTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : ISingleInstanceApp
    {
        private const string Unique = "{28FDD04A-C6E4-429F-B943-8D4978CE6A79}";

        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
                var application = new App();

                application.InitializeComponent();

                application.Run();

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
            else
            {
                MessageBox.Show($"{Assembly.GetEntryAssembly().GetName().Name} is already running.");
            }
        }

        bool ISingleInstanceApp.SignalExternalCommandLineArgs(IList<string> args)
        {
            return true;
        }
    }
}
