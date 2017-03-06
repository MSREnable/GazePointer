using Microsoft.HandsFree.Mouse;
using System.ComponentModel;
using System.Windows;
using Microsoft.HandsFree.MVVM;

namespace GazePointerTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow
    {
        // ReSharper disable once NotAccessedField.Local
        private GazeMouse _mouse;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            Closing += MainWindow_OnClosing;
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            GazeMouse.DetachAll();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _mouse = GazeMouse.Attach(this, null, null, AppSettings.Instance.Mouse);
        }

        #region Button Handlers

        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            Settings();
        }

        #endregion

        #region Actions
        void ExitApplicationAction(object o)
        {
            AppSettings.Store.Save();

            var app = (App)Application.Current;
            app.Shutdown();
        }

        static void CalibrateAction(object o)
        {
            GazeMouse.LaunchRecalibration();
        }
        #endregion

        private void Settings()
        {
            // Launch settings window
            var settingsWindow = new SettingsWindow
            {
                Owner = this,
                Quit = new RelayCommand(ExitApplicationAction),
                Recalibrate = new RelayCommand(CalibrateAction),
            };
            settingsWindow.ShowDialog();
        }
    }
}
