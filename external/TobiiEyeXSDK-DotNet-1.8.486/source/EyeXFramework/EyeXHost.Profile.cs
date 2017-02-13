//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

using Tobii.EyeX.Client;

namespace EyeXFramework
{
    using System;

    public partial class EyeXHost
    {
        private readonly EngineStateAccessor<string> _userProfileNameStateAccessor;
        private readonly EnumerableStateAccessor<string> _userProfilesStateAccessor;

        /// <summary>
        /// Event raised when the user profile name has changed.
        /// </summary>
        public event EventHandler<EngineStateValue<string>> UserProfileNameChanged
        {
            add { _userProfileNameStateAccessor.Changed += value; }
            remove { _userProfileNameStateAccessor.Changed -= value; }
        }

        /// <summary>
        /// Event raised when the list of availale user profiles changed.
        /// </summary>
        public event EventHandler<EngineStateValue<string[]>> UserProfilesChanged
        {
            add { _userProfilesStateAccessor.Changed += value; }
            remove { _userProfilesStateAccessor.Changed -= value; }
        }

        /// <summary>
        /// Gets the engine state: User profile name.
        /// </summary>
        public EngineStateValue<string> UserProfileName
        {
            get { return _userProfileNameStateAccessor.GetCurrentValue(); }
        }

        /// <summary>
        /// Gets the engine state: User profiles.
        /// </summary>
        public EngineStateValue<string[]> UserProfiles
        {
            get { return _userProfilesStateAccessor.GetCurrentValue(); }
        }

        /// <summary>
        /// Sets the current user profile.
        /// </summary>
        /// <param name="profileName">The name of the profile to set as the current one.</param>
        public void SetCurrentUserProfile(string profileName)
        {
            SetCurrentUserProfile(profileName, null);
        }

        /// <summary>
        /// Sets the current user profile.
        /// </summary>
        /// <param name="profileName">The name of the profile to set as the current one.</param>
        /// <param name="callback">The callback invoked when the response from the server arrives. Can be null.</param>
        private void SetCurrentUserProfile(string profileName, Action callback)
        {
            EnsureStarted();

            // Create the callback handler.
            var handler = callback == null 
                ? (AsyncDataHandler) null 
                : data => callback();

            // Set the current profile.
            _context.SetCurrentProfile(profileName, handler);
        }
    }
}
