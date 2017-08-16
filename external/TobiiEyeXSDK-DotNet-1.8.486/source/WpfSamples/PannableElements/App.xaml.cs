//-----------------------------------------------------------------------
// Copyright 2015 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

using System.Windows;
using EyeXFramework.Wpf;

namespace PannableElements
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly WpfEyeXHost _eyeXHost;

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
