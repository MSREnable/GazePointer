# GazePointer
## Overview
GazePointer is an SDK for Hands-Free interaction with a Windows PC. The SDK provides an abstraction layer for interacting with a number of eye trackers. The current implementation provides support for Tobii EyeX based trackers, but other trackers can be added easily. 

The SDK has been tested using a Surface Pro 4 pc with a Tobii PCEye Mini eye tracker. Additionally, mouse emulation is provided for those who want to experiment but don't yet have an eye tracker.

A test application is provided for example usage and verification of the code. The library is presented as a NuGet package for easy consumption.

## Usage
The most straightforward usage model is to attach the GazePointer to the Window object in the Loaded event, and to detach the GazePointer in the OnClosing event.

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GazePointer.Attach(this, null, null, AppSettings.Instance.Mouse);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            GazePointer.DetachAll();
        }
