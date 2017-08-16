//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace UserPresenceWpf
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindowModel _mainWindowModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _mainWindowModel = new MainWindowModel();
            MainWindow = new MainWindow()
            {
                Visibility = Visibility.Visible, 
                DataContext = _mainWindowModel
            };
        }

        /// <summary>
        /// We have to dispose the WpfEyeXHost on exit. This makes sure
        /// that all resources are cleaned up and that the connection to
        /// the EyeX Engine is closed. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _mainWindowModel.Dispose();
        }
    }
}
