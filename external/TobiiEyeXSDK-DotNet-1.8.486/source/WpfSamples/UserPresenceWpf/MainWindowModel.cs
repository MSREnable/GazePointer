//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace UserPresenceWpf
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using EyeXFramework;
    using EyeXFramework.Wpf;
    using Tobii.EyeX.Framework;

    /// <summary>
    /// The MainWindowModel retrieves the UserPresence state from the WpfEyeXHost,
    /// and sets up a listener for changes to the state. It exposes a property
    /// ImageSource, which changes depending on the UserPresence state.
    /// </summary>
    public class MainWindowModel : INotifyPropertyChanged, IDisposable
    {
        private readonly WpfEyeXHost _eyeXHost;
        private string _imageSource;
        private bool _isUserPresent;
        private bool _isTrackingGaze;
        private bool _isTrackingGazeSupported;

        public MainWindowModel()
        {
            IsUserPresent = false;
            IsTrackingGaze = false;
            IsTrackingGazeSupported = true;

            // Create and start the WpfEyeXHost. Starting the host means
            // that it will connect to the EyeX Engine and be ready to 
            // start receiving events and get the current values of
            // different engine states. In this sample we will be using
            // the UserPresence engine state.
            _eyeXHost = new WpfEyeXHost();

            // Register an status-changed event listener for UserPresence.
            // NOTE that the event listener must be unregistered too. This is taken care of in the Dispose(bool) method.
            _eyeXHost.UserPresenceChanged += EyeXHost_UserPresenceChanged; 
            _eyeXHost.GazeTrackingChanged += EyeXHost_GazeTrackingChanged;

            // Start the EyeX host.
            _eyeXHost.Start();

            // Wait until we're connected.
            if (_eyeXHost.WaitUntilConnected(TimeSpan.FromSeconds(5)))
            {
                // Make sure the EyeX Engine version is equal to or greater than 1.4.
                var engineVersion = _eyeXHost.GetEngineVersion().Result;
                if (engineVersion.Major != 1 || engineVersion.Major == 1 && engineVersion.Minor < 4)
                {
                    IsTrackingGazeSupported = false;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// A path to an image corresponding to the current UserPresence state.
        /// </summary>
        public string ImageSource
        {
            get { return _imageSource; }
        }

        /// <summary>
        /// Gets whether or not the user is present.
        /// </summary>
        public bool IsUserPresent
        {
            get { return _isUserPresent; }
            private set
            {
                _isUserPresent = value;
                _imageSource = _isUserPresent 
                    ? "/Images/present.png" 
                    : "/Images/not-present.png";

                // Notify of properties that have changed.
                OnPropertyChanged("IsUserPresent");
                OnPropertyChanged("ImageSource");
            }
        }

        /// <summary>
        /// Gets whether or not gaze is being tracked.
        /// </summary>
        public bool IsTrackingGaze
        {
            get { return _isTrackingGaze; }
            private set 
            {
                _isTrackingGaze = value;
                OnPropertyChanged("IsTrackingGaze");
            }
        }

        public bool IsTrackingGazeSupported
        {
            get { return _isTrackingGazeSupported; }
            set
            {
                _isTrackingGazeSupported = value;
                OnPropertyChanged("IsTrackingGazeSupported");
                OnPropertyChanged("IsTrackingGazeNotSupported");
            }
        }

        public bool IsTrackingGazeNotSupported
        {
            get { return !IsTrackingGazeSupported; }
        }

        /// <summary>
        /// Cleans up any resources being used.
        /// </summary>
        public void Dispose()
        {
            _eyeXHost.UserPresenceChanged -= EyeXHost_UserPresenceChanged;
            _eyeXHost.GazeTrackingChanged -= EyeXHost_GazeTrackingChanged;
            _eyeXHost.Dispose();
        }

        private void EyeXHost_UserPresenceChanged(object sender, EngineStateValue<UserPresence> value)
        {
            // State-changed events are received on a background thread.
            // But operations that affect the GUI must be executed on the main thread.
            RunOnMainThread(() =>
            {
                IsUserPresent = value.IsValid && value.Value == UserPresence.Present;
            });
        }

        private void EyeXHost_GazeTrackingChanged(object sender, EngineStateValue<GazeTracking> value)
        {
            // State-changed events are received on a background thread.
            // But operations that affect the GUI must be executed on the main thread.
            RunOnMainThread(() =>
            {
                IsTrackingGaze = value.IsValid && value.Value == GazeTracking.GazeTracked;
            });
        }

        private void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Marshals the given operation to the UI thread.
        /// </summary>
        /// <param name="action">The operation to be performed.</param>
        private static void RunOnMainThread(Action action)
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.BeginInvoke(action);
            }
        }
    }
}
