//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

using System;

namespace WinFormsSample
{
    public class EyeTrackingStateChangedEventArgs : EventArgs
    {
        public EyeTrackingStateChangedEventArgs(EyeTrackingState eyeTrackingState, string errorMessage, bool canRetry)
        {
            EyeTrackingState = eyeTrackingState;
            ErrorMessage = errorMessage;
            CanRetry = canRetry;
        }

        /// <summary>
        /// Gets the EyeTrackingState.
        /// </summary>
        public EyeTrackingState EyeTrackingState { get; private set; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Gets a value indicating whether it's possible to retry the operation.
        /// </summary>
        public bool CanRetry { get; private set; }
    }
}
