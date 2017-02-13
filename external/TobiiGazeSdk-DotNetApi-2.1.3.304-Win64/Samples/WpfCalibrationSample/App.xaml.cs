//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace WpfCalibrationSample
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Class has app scope.")]
    public partial class App : Application
    {
        private ICalibrationViewModel _viewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var url = "--auto";
            if (e.Args.Length == 1)
            {
                url = e.Args[0];
            }

            //_viewModel = new TestingViewModel(new Action(ExitAction));
            _viewModel = new CalibrationViewModel(Dispatcher, url, new Action(ExitAction));
            MainWindow = new MainWindow { DataContext = _viewModel, Visibility = Visibility.Visible };
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _viewModel.Dispose();

            base.OnExit(e);
        }

        private void ExitAction()
        {
            MainWindow.Close();
        }
    }
}
