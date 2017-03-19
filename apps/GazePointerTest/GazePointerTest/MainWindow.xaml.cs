using System.ComponentModel;
using System.Windows;
using Microsoft.HandsFree.MVVM;
using System.Windows.Controls;
using Microsoft.HandsFree.GazePointer;

namespace GazePointerTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow
    {
        // ReSharper disable once NotAccessedField.Local
        private GazePointer _pointer;

        int buttonCount;

        int anotherCount;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            Closing += MainWindow_OnClosing;
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            GazePointer.DetachAll();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _pointer = GazePointer.Attach(this, null, null, AppSettings.Instance.Mouse);
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
            GazePointer.LaunchRecalibration();
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

        void SetText(TextBlock block, ref int count)
        {
            string text;

            count++;
            switch(count)
            {
                default:
                    text = $"You have pressed this button {count} times";
                    break;
            }

            block.Text = text;
        }

        void OnTestButton(object sender, RoutedEventArgs e)
        {
            SetText(ButtonTextBlock, ref buttonCount);
        }

        private void OnAnotherButton(object sender, RoutedEventArgs e)
        {
            SetText(AnotherTextBlock, ref anotherCount);
        }
    }
}
