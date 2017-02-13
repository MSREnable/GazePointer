//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace ActivatableElements
{
    using System.Windows;
    using EyeXFramework.Wpf;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private WpfEyeXHost _eyeXHost;

        public WpfEyeXHost EyeXHost
        {
            get { return _eyeXHost; }
        }

        public App()
        {
            _eyeXHost = new WpfEyeXHost();
            _eyeXHost.Start();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _eyeXHost.Dispose();
        }
    }
}
