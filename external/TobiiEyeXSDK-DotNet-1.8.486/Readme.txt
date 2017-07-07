Tobii EyeX Software Development Kit for .NET
============================================

README

  This package contains everything a developer needs for building games and 
  applications with the Tobii EyeX Engine API and the Microsoft .NET Framework.
  
  The SDK includes framework components, supporting both WPF and Windows Forms, 
  that simplify the development of user interfaces with the EyeX interaction concepts. The 
  framework components are provided in the form of open source code with a 
  permissive license.

  Note that Tobii offers several SDK packages targeted at different programming
  languages and frameworks, so be sure to pick the one that fits your needs best.

CONTACT

  If you have problems, questions, ideas, or suggestions, please use the forums
  on the Tobii Developer Zone (link below). That's what they are for!

WEB SITE

  Visit the Tobii Developer Zone web site for the latest news and downloads:

  http://developer.tobii.com/

COMPATIBILITY

  This version of the EyeX SDK requires EyeX Engine version 1.0.0 or later.
  Specific features will require newer versions of the EyeX Engine as listed
  in the revision history below.

REVISION HISTORY

  2016-08-11
  Version 1.8:
    - Added engine state value: EyeXHost.ConfigurationStatus, and state changed event:
          EyeXHost.ConfigurationStatusChanged. Updated MinimalEngineStates sample to 
          to include this state.

  2016-03-14
  Version 1.7:
    - No changes.
	- Known issue: When opening the sample solution in Visual Studio 2015, errors 
	  may appear for the WPF projects, stating that "'InitializeComponent' does
	  not exist in the current context". This seams to be a known issue with
	  VS2015. It will still work to build and run the samples. If you want to 
	  remove the errors, for each project with an error, simply add a new empty 
	  line somewhere in the MainWindow.xaml and save the file.

  2015-11-19
  Version 1.6:
    - Removed all dependencies on the EyeXButton for activatable behavior. The
      samples using the activatable behavior now hook their own keyboard keys
      for activation, and send action commands to trigger activation and
      activation mode on.
    - Removed all dependencies on the EyeXButton for pannable behavior. The 
      samples using the pannable behavior now hooks their own keyboard keys
      for panning, and triggers panning begin and end commands.
    - Methods for triggering action commands have been added to the EyeXHost.
    - Rewrote and restructured parts of the Developer Guide, and added more 
      detailed inforation about action commands for the activatable and the
      pannable behaviors.

  2015-06-12
  Version 1.5:
    - Added engine state value: EyeXHost.GazeTracking, and state changed event:
	  EyeXHost.GazeTrackingChanged. Updated MinimalEngineStates sample to 
	  include this state. Requires EyeX Engine 1.4.0.
    - Added engine state value: EyeXHost.UserProfiles, and state changed event:
      EyeXHost.UserProfilesChanged. Updated MinimalEngineStates sample to 
      include this state. Requires EyeX Engine 1.3.0.
    - Added method: EyeXHost.SetCurrentUserProfile. Requires EyeX Engine 1.3.0.
    - Added MinimalProfile sample to demonstrate listing profiles and changing
      current user profile. Requires EyeX Engine 1.3.0.
	- New semantic behavior for UserPresence state: the user will be detected as
	  present in more cases than before. The user's eyes do not have to be open.
	- New enum value for the UserPresence state: 'Unknown'.
    - The EyeXHost has been updated to use new states that replace deprecated 
      states with the same state paths. The new states where introduced in EyeX
      Engine 1.3.0.
      
      New state                                 | Replaces deprecated state
      ----------------------------------------- | --------------------------------
      StatePaths.EyeTrackingScreenBounds        | StatePaths.ScreenBounds           
      StatePaths.EyeTrackingDisplaySize         | StatePaths.DisplaySize            
      StatePaths.EyeTrackingConfigurationStatus | StatePaths.ConfigurationStatus    
	  
      Two new states with new state paths have also been added to the API, but are
      not updated yet in the EyeXHost because of backward compatibility with
      pre-1.3.0 EyeX Engines.
      
      New state                                 | Old state
      ----------------------------------------- | ---------------------------------
      StatePaths.EngineInfoVersion              | StatePaths.EngineVersion   
      StatePaths.EyeTrackingCurrentProfileName  | StatePaths.ProfileName 
	
  2015-04-14
  Version 1.4:
    - Added panning documentation and samples.
    - EyeXFramework is now available as a binary.

  2015-01-15
  Version 1.3:
    - Added support to create a new calibration profile and
      to setup the display.
    - Added sender for the IDataStreamObserver.HandleEvent invocation.

  2014-12-16
  Version 1.2:
    - Added minimal sample and support for launching recalibration, 
      calibration testing and guest calibration programmatically.
      This function requires EyeX Engine 1.1.
    - New EyeX Engine API function: Environment.GetEyeXAvailability. This 
      function is implemented in the client library and works with all Engine 
      versions.
    - Added a check for engine availability to the MinimalEngineStates
      sample app.

  2014-11-20
  Version 1.1: No changes.

  2014-10-27
  Version 1.0:
    - Client library compatible with EyeX Engine 1.0.
    - Added minimal example: MinimalEyePositionDataStream.
    - Added normalized eye positions to EyePositionDataStream.
    - Added WPF example: UserPresenceWpf.
    - More strict dispose handling.
    - Setting context and environment to null to avoid double dispose.
    - Added support for retrieving user calibration profile name.
    - Added User Calibration Profile Name state to MinimalEngineStates. 
    - Fixed the key hooking in ActivatableButtonsForms.
    - Updated samples and documentation for the new direct click modes in 
      EyeX Interaction settings.

  2014-09-23
  Version 0.32: 
    - Client library compatible with both EyeX Engine 0.10 and 1.0.
    - Changed the way data streams are created: now using the  
      CreateGazePointDataStream, CreateFixationDataStream, and
      CreateEyePositionDataStream methods on the EyeXHost class instead of the 
      generic CreateDataStream method.
    - Fixed an issue in the WPF framework with updates not being handled 
      properly when the application has multiple top-level windows.

  2014-09-05
  Version 0.31: Updated package for Tobii EyeX Engine 0.10.0:
    - Client libraries updated with some breaking API changes (see below).
    - All samples are updated to the new client libraries.
    - Clients are required to call Shutdown before disposing a Context.

  2014-08-22
  Version 0.24:
  - Simplified framework for engine states.
  - Added new Windows Forms sample for the user presence state.
  - Dropped support for Visual Studio 2010.
  - Bug fix in EyeXFramework.Wpf: invalid cast exception.

  2014-06-19
  Version 0.23: Minor updates.
  
  2014-05-21
  Version 0.22: First version of the .NET SDK. Note that the EyeX Engine API is 
  still in a pre-beta stage and the API has not been finalized yet. Backward 
  compatibility is not guaranteed, but we will do our best to keep the API 
  compatibility breaks to a minimum.

EYEX ENGINE API CHANGES

  2014-10-22
  EyeX Engine 1.0
  - No actual API changes, but functional changes related to the Activatable 
    behavior, direct click and key bindings: 
       - If EyeX Interaction is disabled, no default keys are mapped to direct click.
       - ActivationFocus and Activated events are sent simultaneously if EyeX Button 
         interaction is configured in EyeX Interaction settings.

  2014-09-05
  EyeX Engine Developer Preview 0.10.0
  - Name changes:
      InteractionSystem => Environment
      InteractionSnapshot => Snapshot
      InteractionQuery => Query
      InteractionBehaviorType => BehaviorType
      InteractionContext => Context
      InteractionBoundsType => BoundsType
      PresenceData => UserPresence
      Interactor.Set[X]Behavior => Interactor.Create[X]Behavior
