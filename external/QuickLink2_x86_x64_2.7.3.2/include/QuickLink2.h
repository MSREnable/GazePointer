////////////////////////////////////////////////////////////////////////////////////////////////////
/// @file   QuickLink2.h
///
/// @brief  This file declares the functions available through QuickLink2.dll.
/// 
/// @copyright 1996 - 2015, EyeTech Digital Systems
////////////////////////////////////////////////////////////////////////////////////////////////////
#ifndef QUICK_LINK_2_H
#define	QUICK_LINK_2_H

#include "QLTypes.h"

#ifdef __cplusplus
extern "C"
{
#endif


////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLAPI_GetVersion( int bufferSize, char* buffer);
///
/// @ingroup API
/// @brief Get the version of the API.
/// 
/// This function retrieves the version string of the API.
///
/// @param [in] bufferSize The size of the output buffer in bytes.
/// @param [out] buffer    A pointer to a char buffer that receives the version string.
///
/// @return The success of the function.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLAPI_GetVersion(
        int bufferSize, 
        char* buffer);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLAPI_ExportSettings( QLSettingsId settings);
///
/// @ingroup API
/// @brief Export the system level API settings that are currently being used by the API.
/// 
/// This function exports the current system level API settings values to the specified settings
/// container. It will only export the values for setting that have been added to the settings
/// container. To add a setting to the settings container use the function
/// QLSettings_AddSetting() or the function QLSettings_SetValue().
///
/// @param [in] settings The ID of the settings container that will receive the exported values
///                      from the API. This ID is obtained by calling either the function
///                      QLSettings_Create() or the function QLSettings_Load().
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the settings
/// values were successfully exported.
/// 
/// @see Settings.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLAPI_ExportSettings(
        QLSettingsId settings);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLAPI_ImportSettings( QLSettingsId settings);
///
/// @ingroup API
/// @brief Import settings values to the system level of the API.
/// 
/// This function imports settings values to the system level of the API from
/// the specified settings container. The new settings values will take
/// effect immediately. The settings values that are imported are also cached
/// for later use. The cached settings values can be recalled by calling the 
/// function QLAPI_ImportLastSettings().
/// 
/// Not all settings are supported at the system level of the API. Only
/// settings values that are supported will be imported from the settings
/// container. All other settings in the container will be ignored.
///
/// @param [in] settings The ID of the settings container that will supply the
///                      settings values that will be imported to the API.
///                      This ID is obtained by calling either the function
///                      QLSettings_Create() or the function QLSettings_Load().
///
/// @return The success of the function. If the return value is QL_ERROR_OK
/// then the settings values were successfully imported to API.
/// 
/// @see Settings.
/// @see QLAPI_ImportLastSettings()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLAPI_ImportSettings(
        QLSettingsId settings);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLAPI_ImportLastSettings();
///
/// @ingroup API
/// @brief Import settings values to the system level of the API.
/// 
/// This function imports the last settings values that were imported to the
/// system level of the API using the function QLAPI_ImportSettings(). The new
/// settings values will take effect immediately.
/// 
/// Not all settings are supported at the system level of the API. Only
/// settings values that are supported will be imported from the settings
/// container. All other settings in the container will be ignored.
///
/// @return The success of the function. If the return value is QL_ERROR_OK
/// then the settings values were successfully imported to API.
/// 
/// @see Settings.
/// @see QLAPI_ImportSettings()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLAPI_ImportLastSettings();

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDevice_Enumerate( int* numDevices,
/// QLDeviceId* deviceBuffer);
///
/// @ingroup Device
/// @brief Enumerate the bus to find connected EyeTech devices.
/// 
/// This function creates an ID for each EyeTech device connected to the system. The IDs are used
/// in other functions to reference a specific device.
///
/// @param [in,out] numDevices A pointer to an integer containing the size of deviceBuffer. When
///                            the function returns the integer contains the number of devices
///                            found on the bus.
/// @param [out] deviceBuffer  A pointer to a QLDeviceId buffer that will receive the IDs of the
///                            devices that are connected to the system.
///
/// @return The success of the function. If the return value is QL_ERROR_BUFFER_TOO_SMALL then
/// the device buffer was too small to contain all the devices that are attached to the computer.
/// numDevices will contain the number of devices detected and the deviceBuffer should be resized
/// to be at least as large this large before this function is called again. If the return value
/// is QL_ERROR_OK then the bus was successfully enumerated and the device buffer contains IDs
/// for all the EyeTech devices that were detected.
/// 
/// @example /src/QuickStart/Initialize.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_Enumerate(
        int* numDevices, 
        QLDeviceId* deviceBuffer);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDevice_GetInfo( QLDeviceId device, QLDeviceInfo* info);
///
/// @ingroup Device
/// @brief Get device specific information for a device.
///
/// @param [in] device The ID of the device from which to get information. This ID is obtained
///                    by calling the function QLDevice_Enumerate().
/// @param [out] info  A pointer to a QLDeviceInfo object. This object will receive the
///                    information about the device.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the info
/// parameter contains the information about the device.
///
/// @example /src/QuickStart/Initialize.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_GetInfo(
        QLDeviceId device, 
        QLDeviceInfo* info);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDevice_GetStatus( QLDeviceId device,
/// QLDeviceStatus* status);
///
/// @ingroup Device
/// @brief Get the status of a device.
/// 
/// This function causes momentary slowness of the device while trying to determine the status.
/// It can greatly affect frame rates and should not be called when capture times are critical.
///
/// @param [in] device  The ID of the device from which to get the status. This ID is obtained by
///                     calling the function QLDevice_Enumerate().
/// @param [out] status A pointer to a QLDeviceStatus object. This object will receive the status
///                     of the device.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the status
/// parameter contains the status of the device.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_GetStatus(
        QLDeviceId device, 
        QLDeviceStatus* status);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDevice_ExportSettings( QLDeviceId device,
/// QLSettingsId settings);
///
/// @ingroup Device
/// @brief Export the active settings values that are being used by a device.
/// 
/// This function exports the settings values that are currently being used by the specified
/// device. The values are exported to the specified settings container. It will only export the
/// values for setting that have been added to the settings container. To add a setting to the
/// settings container use the function QLSettings_AddSetting() or the function
/// QLSettings_SetValue().
/// 
/// Not all settings are supported by all devices. To determine if a setting is supported by a
/// particular device then use the function QLDevice_IsSettingSupported().
///
/// @param [in] device   The ID of the device from which to export the settings. This ID is
///                      obtained by calling the function QLDevice_Enumerate().
/// @param [in] settings The ID of the settings container that will receive the exported values
///                      from the device. This ID is obtained by calling either the function
///                      QLSettings_Create() or the function QLSettings_Load().
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the settings
/// values were successfully exported from the device.
/// 
/// @see Settings.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_ExportSettings(
        QLDeviceId device, 
        QLSettingsId settings);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDevice_ImportSettings( QLDeviceOrGroupId deviceOrGroup,
/// QLSettingsId settings);
///
/// @ingroup Device
/// @brief Import settings values for a device and make them active.
/// 
/// This function imports settings values for the specified device from the
/// specified settings container. This operation is synchronous with the
/// device. The new settings values will take effect after the device has
/// finished processing its current frame, resulting in a latency of one
/// frame.
/// 
/// Not all settings are supported by all devices. Only settings values that
/// are supported by the specified device will be imported from the settings
/// container. All other settings in the settings container will be ignored.
/// To determine if a setting is supported by a particular device then use
/// the function QLDevice_IsSettingSupported().
/// 
/// The settings values that are exported using this function are also
/// cached. The settings in the cache can be recalled at any time by calling
/// the function QLDevice_ImportLastSettings(). This also works after the
/// library has been unloaded and reloaded.
/// 
/// Calling this function using a device group will cause the settings to be
/// imported to all devices in the group.
///
/// @param [in] deviceOrGroup The ID of the device or device group in which to
///                           import the settings. This ID is obtained by
///                           calling either the function
///                           QLDevice_Enumerate() or the function
///                           QLDeviceGroup_Create().
/// @param [in] settings      The ID of the settings container that will supply
///                           the settings values that will be imported to
///                           the device or group. This ID is obtained by
///                           calling either the function
///                           QLSettings_Create() or the function
///                           QLSettings_Load().
///
/// @return The success of the function. If the return value is QL_ERROR_OK
/// then the settings values were successfully imported to the device.
/// 
/// @see Settings.
/// @see QLDevice_ImportLastSettings()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_ImportSettings(
        QLDeviceOrGroupId deviceOrGroup, 
        QLSettingsId settings);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDevice_ImportLastSettings( QLDeviceOrGroupId deviceOrGroup);
///
/// @ingroup Device
/// @brief Import the most recent settings values that have been applied to
/// the device and make them active.
/// 
/// This function imports the last settings values that have been imported to
/// any device that is the same type as the specified device. Each time
/// settings values are imported to a device using the function
/// QLDevice_ImportSettings(), a copy of the settings values are saved to a
/// file for later use. This can be useful for loading the same settings that
/// were used the last time the tracker was used or an for application to use
/// the last settings that were used by another application.
/// 
/// This operation is synchronous with the device. The new settings values
/// will take effect after the device has finished processing its current
/// frame, resulting in a latency of one frame.
/// 
/// Calling this function using a device group will cause the last settings
/// to be imported to all devices in the group.
///
/// @param [in] deviceOrGroup The ID of the device or device group in which to
///                           import the settings. This ID is obtained by
///                           calling either the function
///                           QLDevice_Enumerate() or the function
///                           QLDeviceGroup_Create().
///
/// @return The success of the function. If the return value is QL_ERROR_OK
/// then the settings values were successfully imported to the device.
/// 
/// @see Settings.
/// @see QLDevice_ImportSettings()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_ImportLastSettings(
        QLDeviceOrGroupId deviceOrGroup);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDevice_IsSettingSupported( QLDeviceId device,
/// const char* settingName);
///
/// @ingroup Device
/// @brief Determine if a setting is supported by a device.
/// 
/// This function determines if a setting is supported by a given device. Not all settings are
/// supported by all devices.
///
/// @param [in] device      The ID of the device to check for setting support. This ID is
///                         obtained by calling the function QLDevice_Enumerate().
/// @param [in] settingName A pointer to a NULL-terminated string containing the name of the
///                         setting to check for in the device.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the setting is
/// supported by the specified device.
/// 
/// @see Settings.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_IsSettingSupported(
        QLDeviceId device, 
        const char* settingName);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDevice_SetPassword( QLDeviceId device,
/// const char* password);
///
/// @ingroup Device
/// @brief Set the password for a device.
/// 
/// This function sets the password for the specified device. The password is usually found on a
/// label affixed to the device. If the password is not known for a device then contact EyeTech
/// Digital Systems to obtain it. The password must be set for most functions to work properly.
/// If the password is not set then the functions QLDevice_Start(), QLDevice_GetFrame(),
/// QLDevice_GetStatus() and QLCalibration_Initialize() will return the value
/// QL_ERROR_INVALID_PASSWORD.
///
/// @param [in] device   The ID of the device for which to set the password. This ID is obtained
///                      by calling the function QLDevice_Enumerate().
/// @param [in] password A pointer to a NULL-terminated string containing the password for the
///                      device.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the password was
/// valid and stored in the device.
/// 
/// @example /src/QuickStart/Initialize.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_SetPassword(
        QLDeviceId device, 
        const char* password);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDevice_Start( QLDeviceOrGroupId deviceOrGroup);
///
/// @ingroup Device
/// @brief Start the device.
/// 
/// This function causes the device to start running. Once running, the device will grab and
/// process frames as fast as the current settings allow. Calling this function on a device that
/// has already been started will not restart the device. QLDevice_Stop() must be called before a
/// device can be restarted.
/// 
/// This function creates one or more threads. Do not call it from any function that warns against
/// creating threads from within it such as dllmain().
/// 
/// Calling this function using a device group will cause all devices in the group to start.
///
/// @param [in] deviceOrGroup The ID of the device or device group to start. This ID is obtained by
///                           calling either the function QLDevice_Enumerate() or the function
///                           QLDeviceGroup_Create().
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the device/group
/// was started successfully.
/// 
/// @example /src/QuickStart/Main.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_Start(
        QLDeviceOrGroupId deviceOrGroup);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDevice_Stop( QLDeviceOrGroupId deviceOrGroup);
///
/// @ingroup Device
/// @brief Stop the device.
/// 
/// This function causes the device to stop running. Before unloading the library, this function
/// must be called for each device that has been started using the function QLDevice_Start().
/// Undefined behavior could result if the library is unloaded before a device is stopped.
/// 
/// It is best if this function is called before a process has been signaled to exit. This will
/// give a device sufficient time to clean up its memory and close its thread before the process
/// exit procedure unloads the library automatically.
/// 
/// This function kills one or more threads. Do not call it from any function that warns against
/// destroying threads from within it such as dllmain().
/// 
/// Calling this function using a device group will cause all devices in the group to stop.
///
/// @param [in] deviceOrGroup The ID of the device or device group to stop. This ID is obtained by
///                           calling either the function QLDevice_Enumerate() or the function
///                           QLDeviceGroup_Create().
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the device/group
/// was stopped successfully and all system resources used by the device(s) have been closed.
///
/// @example /src/QuickStart/Main.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_Stop(
        QLDeviceOrGroupId deviceOrGroup);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDevice_Stop_All();
///
/// @ingroup Device
/// @brief Stop all devices.
/// 
/// This function causes all devices to stop running. Before unloading the library, each device
/// that has been started by using the function QLDevice_Start() must be stopped. This can be
/// done by calling QLDevice_Stop() for each device that has been started, or this function could
/// be called to stop all devices. Undefined behavior could result if the library is unloaded
/// before a device is stopped.
/// 
/// It is best if each device is stopped before a process has been signaled to exit. This will
/// give a device sufficient time to clean up its memory and close its thread before the process
/// exit procedure unloads the library automatically.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then all devices were
/// stopped successfully and all system resources used by the devices have been closed.
///
/// @example /src/QuickStart/QL2Utility.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_Stop_All();

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDevice_SetIndicator( QLDeviceOrGroupId deviceOrGroup,
/// QLIndicatorType type, QLIndicatorMode mode);
///
/// @ingroup Device
/// @brief Set the indicator mode for the device.
/// 
/// This function sets the mode for the indicator lights on the front of the device.
/// 
/// Calling this function using a device group will sets the mode for the indicator lights on the
/// front of all devices in the group.
///
/// @param [in] deviceOrGroup The ID of the device or device group whose indicator lights should be
///                           set. This ID is obtained by calling either the function
///                           QLDevice_Enumerate() or the function QLDeviceGroup_Create().
/// @param [in] type          The type of indicator to set.
/// @param [in] mode          The mode to set the indicator.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the indicator
/// was set to the desired mode for the specified device(s).
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_SetIndicator(
        QLDeviceOrGroupId deviceOrGroup, 
        QLIndicatorType type, 
        QLIndicatorMode mode);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDevice_GetIndicator( QLDeviceId device,
/// QLIndicatorType type, QLIndicatorMode* mode);
///
/// @ingroup Device
/// @brief Get the current indicator mode for the device.
/// 
/// This function gets the current mode for the indicator lights on the front of the device.
///
/// @param [in] device The ID of the device from which to get the indicator mode. This ID is
///                    obtained by calling the function QLDevice_Enumerate().
/// @param [in] type   The type of indicator to get.
/// @param [out] mode  A pointer to a QLIndicatorMode object. This object will receive the mode
///                    of the indicator type for the specified device.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the indicator
/// mode was retrieved from the specified device.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_GetIndicator(
        QLDeviceId device, 
        QLIndicatorType type, 
        QLIndicatorMode* mode);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDevice_GetFrame( QLDeviceOrGroupId deviceOrGroup,
/// int waitTime, QLFrameData* frame);
///
/// @ingroup Device
/// @brief Get a frame from the device
/// 
/// This function gets the most recent frame from the device. It is a blocking function which
/// will wait for a frame to become available if there is not one ready. Waiting does not use CPU
/// time.
/// 
/// Only the most recent frame can be retrieved from the device. To ensure that no frames are
/// dropped this function needs to be called at least as fast as the frame rate of the device. A
/// good way to use this function that will help ensure the retrieval of every frame is to use it
/// in a loop. The blocking nature of this function will ensure that the loop will only run as
/// fast as the device can produce frames. If other processing in the loop does not exceed the
/// time that it takes for the device to make another frame available then this function will
/// sync the loop to the approximate frame rate of the device.
/// 
/// Calling this function using a device group will get a frame from only one device in the
/// group. If there are no devices that are tracking eyes then the image that is returned rotates
/// between all devices in the group. Each device will be selected for 3 seconds before rotating
/// to the next device. The order of rotation is based on the order in which the devices were
/// added to the group. If a device in the group is tracking eyes then that device becomes the
/// selected device. It will stay the selected device until the eyes are lost. Once a device
/// begins tracking eyes and becomes the selected device, eye tracking is dissabled on all other
/// devices in the group until the selected device loses the eyes. If two devices find eyes at
/// the same time then the device that has eyes closest to the center of the image becomes the
/// selected device.
///
/// @param [in] deviceOrGroup The ID of the device or device group from which to get the most
///                           recent frame. This ID is obtained by calling either the function
///                           QLDevice_Enumerate() or the function QLDeviceGroup_Create().
/// @param [in] waitTime      The maximum time in milliseconds to wait for a new frame.
/// @param [out] frame        A pointer to a QLFrameData object. This object receives the data for
///                           the most recent frame.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the most recent
/// frame was successfully retrieved from the device or group.
/// 
/// @see QLDeviceGroup_AddDevice()
/// 
/// @example /src/QuickStart/Main.cpp
/// @example /src/QuickStart/DisplayVideo.cpp
/// @example /src/QuickStart/OpencvUtility.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_GetFrame(
        QLDeviceOrGroupId deviceOrGroup,
        int waitTime, 
        QLFrameData* frame);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDevice_ApplyCalibration( QLDeviceId device,
/// QLCalibrationId calibration);
///
/// @ingroup Device
/// @brief Apply a calibration to a device.
/// 
/// This function applies a calibration to a device. If a calibration is not
/// applied to a device then the gaze point cannot be calculated. By
/// applying a calibration object that has not been calibrated it is possible
/// to clear a calibration from a device so it will be un-calibrated.
/// 
/// The calibration that is applied using this method is also cached. The
/// cached calibration can be applied at any time by calling the function
/// QLDevice_ApplyLastCalibration(). This can even be used after the library
/// has been unloaded and loaded again or between different applications.
/// 
/// For greatest accuracy a device should be calibrated for each user, for
/// each physical setup of the device and monitor.
///
/// @param [in] device      The ID of the device for which to apply the
///                         calibration. This ID is obtained by calling the
///                         function QLDevice_Enumerate().
/// @param [in] calibration The ID of the calibration object to apply. This
///                         ID is obtained by calling either the function
///                         QLCalibration_Create() or the function
///                         QLCalibration_Load().
///
/// @return The success of the function. If the return value is QL_ERROR_OK
/// then the calibration was successfully applied to the device.
/// 
/// @see Calibration.
/// @see QLDevice_ApplyLastCalibration()
///
/// @example /src/QuickStart/Main.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_ApplyCalibration(
        QLDeviceId device, 
        QLCalibrationId calibration);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDevice_ApplyLastCalibration( QLDeviceId device);
///
/// @ingroup Device
/// @brief Apply a cached calibration to a device.
/// 
/// This function applies the last calibration that was applied to the device
/// using the function QLDevice_ApplyCalibration(). This can be used to load
/// the last calibration that was applied even if it was done in another
/// application. Care needs to be taken to ensure that the cached calibration
/// covers the same area that is desired. If there is a doubt then the cached
/// calibration should not be used.
///
/// @param [in] device The ID of the device for which to apply the
///                    calibration. This ID is obtained by calling the
///                    function QLDevice_Enumerate().
///
/// @return The success of the function. If the return value is QL_ERROR_OK
/// then the calibration was successfully applied to the device.
/// 
/// @see Calibration.
/// @see QLDevice_ApplyCalibration()
///
/// @example /src/QuickStart/Main.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_ApplyLastCalibration(
        QLDeviceId device);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDevice_CalibrateEyeRadius( QLDeviceId device,
/// float distance, float* leftRadius, float* rightRadius);
///
/// @ingroup Device
/// @brief Calibrate the radius for each eye.
/// 
/// This function uses the measured distance to the user in order to calculate the radius of the
/// cornea for each eye. Using the radii returned by this function as the values for the settings
/// QL_SETTING_DEVICE_IMAGE_PROCESSING_EYE_RADIUS_LEFT and
/// QL_SETTING_DEVICE_IMAGE_PROCESSING_EYE_RADIUS_RIGHT will result in greater accuracy of the
/// distance output for each frame.
///
/// @param [in] device       The ID of the device from which to calibrate the eye radius.
/// @param [in] distance     The actual measured distance in centimeters that the user is away
///                          from the device at the time this function is called.
/// @param [out] leftRadius  A pointer to a float that will receive the radius in centimeters of
///                          the cornea of the left eye.
/// @param [out] rightRadius A pointer to a float that will receive the radius in centimeters of
///                          the cornea of the right eye.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the radius of
/// the left and right eyes were successful.
/// 
/// @see QL_SETTING_DEVICE_IMAGE_PROCESSING_EYE_RADIUS_LEFT.
/// @see QL_SETTING_DEVICE_IMAGE_PROCESSING_EYE_RADIUS_RIGHT.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN
	QLDevice_CalibrateEyeRadius(
		QLDeviceId device,
		float distance,
		float* leftRadius,
		float* rightRadius);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDeviceGroup_Create( QLDeviceGroupId* deviceGroup);
///
/// @ingroup Device
/// @brief Create a new device group and get its ID.
/// 
/// This function creates a new device group. A device group is used to perform actions on a
/// number of devices at once instead of having to do it for each device individually. Many
/// functions that accept a device ID can also be used with a group ID. See the function's
/// documentation for verificaion.
///
/// @param [out] deviceGroup A pointer to a QLDeviceGroupId object. This object will receive the
///                          ID of the new device group.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the device group
/// was created successfully.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDeviceGroup_Create(
        QLDeviceGroupId* deviceGroup);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDeviceGroup_AddDevice( QLDeviceGroupId deviceGroup,
/// QLDeviceId device);
///
/// @ingroup Device
/// @brief Add a device to a device group.
/// 
/// This function adds a device to the given device group. The order of the devices in the group
/// is based on the order in which they were added to the group. Adding a device that is already
/// in the group will cause it to be removed from its previous position and added to the end of
/// the device list.
///
/// @param [in] deviceGroup The ID of the device group to which the device should be added.
/// @param [in] device      The ID of the device that will be added to the device group.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the device was
/// successfully added to the device group. QL_ERROR_OK will also be returned if the device was
/// already in the group.
/// 
/// @see QLDeviceGroup_Create()
/// @see QLDeviceGroup_RemoveDevice()
/// @see QLDevice_Enumerate()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDeviceGroup_AddDevice(
        QLDeviceGroupId deviceGroup, 
        QLDeviceId device);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDeviceGroup_RemoveDevice( QLDeviceGroupId deviceGroup,
/// QLDeviceId device);
///
/// @ingroup Device
/// @brief Remove a device from a device group.
/// 
/// This function removes a device from the given device group.
///
/// @param [in] deviceGroup The ID of the device group from which the device should be removed.
/// @param [in] device      The ID of the device that will be removed from the device group.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the device was
/// successfully removed from the device group. QL_ERROR_OK will also be returned if the device
/// was already missing from the group.
/// 
/// @see QLDeviceGroup_Create()
/// @see QLDeviceGroup_AddDevice()
/// @see QLDevice_Enumerate()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDeviceGroup_RemoveDevice(
        QLDeviceGroupId deviceGroup, 
        QLDeviceId device);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDeviceGroup_Enumerate( QLDeviceGroupId deviceGroup,
/// int* numDevices, QLDeviceId* deviceBuffer);
///
/// @ingroup Device
/// @brief Enumerate the device group.
/// 
/// This function enumerates the device group and retrieves a list of device IDs that are part of
/// the group. The order of the devices in the buffer is based on the order in which the devices
/// were added to the group.
///
/// @param [in] deviceGroup    The ID of the device group to enumerate.
/// @param [in,out] numDevices A pointer to an integer containing the size of deviceBuffer. When
///                            the function returns the integer contains the number of devices
///                            found on the bus.
/// @param [out] deviceBuffer  A pointer to a QLDeviceId buffer that will receive the IDs of the
///                            devices that are in the device group.
///
/// @return The success of the function. If the return value is QL_ERROR_BUFFER_TOO_SMALL then
/// the device buffer was too small to contain all the devices that are in the group. numDevices
/// will contain the number of devices in the group and the deviceBuffer should be resized to be
/// at least this large before this function is called again. If the return value is QL_ERROR_OK
/// then the group was successfully enumerated and the device buffer contains IDs for all the
/// devices that are in the group.
///
/// @see QLDeviceGroup_AddDevice()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDeviceGroup_Enumerate(
        QLDeviceGroupId deviceGroup, 
        int* numDevices,
        QLDeviceId* deviceBuffer);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLDeviceGroup_GetFrame( QLDeviceGroupId deviceGroup,
/// int waitTime, int* numFrames, QLFrameData* frame);
///
/// @ingroup Device
/// @brief Get frames from all devices in the group.
/// 
/// This function gets the most recent frame from each device in the device group. It is a
/// blocking function which will wait for a frame to become available on each device before
/// returning. Waiting does not use CPU time. The ordering of the frames will be based on the
/// the order in which the devices were added to the group. The order can be recalled by
/// enumerating the group with the function QLDeviceGroup_Enumerate().
///
/// @param [in] deviceGroup   The ID of the device group whose devices will be queried for frames.
/// @param [in] waitTime      The maximum time in milliseconds to wait for a new frame from a
///                           single device. This time is used for blocking each device which
///                           means that the total time the function could possibly block can
///                           be larger than this time.
/// @param [in,out] numFrames A pointer to an integer containing the size of frame buffer. This
///                           should be at least as large as the number of devices in the
///                           group. When the function returns the integer contains the number
///                           of frames used in the frame buffer and will equal the number of
///                           devices in the group.
/// @param [out] frame        A pointer to a QLFrameData buffer that will receive the frames from
///                           the devices in the group.
///
/// @return The success of the function. If the return value is QL_ERROR_BUFFER_TOO_SMALL then
/// the frame buffer was too small to contain the frames from all the devices in the group.
/// numFrames will contain the number of devices in the group and the frame buffer should be
/// resized to be at least this large before this function is called again. If the return value
/// is QL_ERROR_OK then the frames were successfully collected from all devices in the group.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLDeviceGroup_GetFrame(
        QLDeviceGroupId deviceGroup,
        int waitTime, 
        int* numFrames,
        QLFrameData* frame);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_Load( const char* path,
/// QLSettingsId* settings);
///
/// @ingroup Settings
/// @brief Load settings from a file into a settings container.
/// 
/// This function loads settings from a file into a settings container. If there are settings in
/// the file that are currently in the settings container then the values from the file will
/// overwrite the current values. If a setting is in the file multiple times with different
/// values, the last entry takes precedence.
///
/// @param [in] path         A NULL-terminated string containing the full pathname of the settings
///                          file.
/// @param [in,out] settings A pointer to a QLSettingsId object. If the QLSettingsId object refers
///                          to a valid settings container then that settings container will
///                          receive the loaded settings. If the QLSettingsId object does not
///                          refer to a valid settings container then a new settings container
///                          will be created and this object will receive its ID.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the settings
/// were successfully loaded.
/// 
/// @example /src/QuickStart/Initialize.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_Load(
        const char* path, 
        QLSettingsId* settings);


////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_Save( const char* path,
/// QLSettingsId settings);
///
/// @ingroup Settings
/// @brief Save settings from a settings container to a file.
/// 
/// This function saves the settings of a settings container to a file. If the file already
/// exists then its contents are modified. Only the values of the settings that are in the
/// settings container are changed. New settings are added to the end of the file. If the file
/// did not previously exist then a new file is created.
///
/// @param [in] path     A NULL-terminated string containing the full pathname of the settings
///                      file.
/// @param [in] settings The ID of the settings container containing the settings to save.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the settings
/// were successfully saved.
/// 
/// @example /src/QuickStart/Initialize.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_Save(
        const char* path, 
        QLSettingsId settings);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_Create( QLSettingsId source,
/// QLSettingsId* settings);
///
/// @ingroup Settings
/// @brief Create a new settings container.
/// 
/// This function creates a new settings container. If the source ID references a valid settings
/// container then its contents will be copied into the newly created settings container. If the
/// source ID does not reference a valid settings container then the new settings container will
/// be empty.
/// 
/// If settings ID pointer references a QLSettingsId that has already been created then the container
/// associated with this ID will be cleared first.
///
/// @param [in] source       The ID of the settings container from which to copy settings. This ID
///                          is obtained by calling either this function or the function
///                          QLSettings_Load(). To create an empty settings container, set this
///                          value to zero.
/// @param [in,out] settings A pointer to a QLSettingsId object. This object will receive the ID
///                          of the new settings container.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the new settings
/// container was successfully created.
///
/// @example /src/QuickStart/Initialize.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_Create(
        QLSettingsId source, 
        QLSettingsId* settings);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_AddSetting( QLSettingsId settings,
/// const char* settingName);
///
/// @ingroup Settings
/// @brief Add a setting to a settings container.
/// 
/// This function adds a setting to a settings container and gives it an initial value of zero.
/// If the setting already exists in the settings container then its value remains unchanged.
///
/// @param [in] settings    The ID of the settings container that will receive the new setting.
///                         This ID is obtained by calling either the function
///                         QLSettings_Create() or the function QLSettings_Load().
/// @param [in] settingName A NULL terminated string containing the name of the setting to add to
///                         the settings container.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the setting was
/// successfully added to the settings container or the setting was already in the settings
/// container.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_AddSetting(
        QLSettingsId settings,
        const char* settingName);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_RemoveSetting( QLSettingsId settings,
/// const char* settingName);
///
/// @ingroup Settings
/// @brief Remove a setting from a settings container.
/// 
/// This function removes a setting from a settings container.
///
/// @param [in] settings    The ID of the settings container from which the setting will be
///                         removed. This ID is obtained by calling either the function
///                         QLSettings_Create() or the function QLSettings_Load().
/// @param [in] settingName A NULL terminated string containing the name of the setting that will
///                         be removed from the settings container.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the setting was
/// successfully removed from the settings container. If the return value is QL_ERROR_NOT_FOUND
/// then the setting was not in the container.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_RemoveSetting(
        QLSettingsId settings,
        const char* settingName);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_SetValue( QLSettingsId settings,
/// const char* settingName, QLSettingType settingType, const void* value);
///
/// @ingroup Settings
/// @brief Set the value of a setting in a settings container.
/// 
/// This function sets the value of a setting in a settings container. If the setting already
/// existed in the settings container then its value is updated. If the setting did not already
/// exist in the settings container then it is added with the specified value.
///
/// @param [in] settings    The ID of the settings container that either contains the setting to
///                         be altered or will receive the new setting if it did not exist
///                         previously. This ID is obtained by calling either the function
///                         QLSettings_Create() or the function QLSettings_Load().
/// @param [in] settingName A NULL terminated string containing the name of the setting that will
///                         be altered in, or added to, the settings container.
/// @param [in] settingType The type of data that was passed in. This tells the API how to
///                         interpret the data pointed to by "value".
/// @param [in] value       A pointer to an object which contains the value to set the setting.
///                         The object type must match the type indicated by "settingType". For
///                         the type QL_SETTING_TYPE_STRING this must be a NULL terminated string
///                         containing the desired value.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the setting
/// value was successfully updated or the setting was successfully added to the settings
/// container.
///
/// @example /src/QuickStart/Initialize.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_SetValue(
        QLSettingsId settings,
        const char* settingName,
        QLSettingType settingType,
        const void* value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_SetValueInt( QLSettingsId settings,
/// const char* settingName, int value);
///
/// @ingroup Settings
/// @brief Set the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_INT. This is a wrapper for QLSettings_SetValue().
/// 
/// @see QLSettings_SetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_SetValueInt(
		QLSettingsId settings, 
		const char* settingName, 
		int value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_SetValueInt8( QLSettingsId settings,
/// const char* settingName, __int8 value);
///
/// @ingroup Settings
/// @brief Set the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_INT8. This is a wrapper for QLSettings_SetValue().
/// 
/// @see QLSettings_SetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_SetValueInt8(
		QLSettingsId settings, 
		const char* settingName, 
		__int8 value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_SetValueInt16( QLSettingsId settings,
/// const char* settingName, __int16 value);
///
/// @ingroup Settings
/// @brief Set the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_INT16. This is a wrapper for QLSettings_SetValue().
/// 
/// @see QLSettings_SetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_SetValueInt16(
		QLSettingsId settings, 
		const char* settingName, 
		__int16 value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_SetValueInt32( QLSettingsId settings,
/// const char* settingName, __int32 value);
///
/// @ingroup Settings
/// @brief Set the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_INT32. This is a wrapper for QLSettings_SetValue().
/// 
/// @see QLSettings_SetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_SetValueInt32(
		QLSettingsId settings, 
		const char* settingName, 
		__int32 value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_SetValueInt64( QLSettingsId settings,
/// const char* settingName, __int64 value);
///
/// @ingroup Settings
/// @brief Set the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_INT64. This is a wrapper for QLSettings_SetValue().
/// 
/// @see QLSettings_SetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_SetValueInt64(
		QLSettingsId settings, 
		const char* settingName, 
		__int64 value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_SetValueUInt( QLSettingsId settings,
/// const char* settingName, unsigned int value);
///
/// @ingroup Settings
/// @brief Set the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_UINT. This is a wrapper for QLSettings_SetValue().
/// 
/// @see QLSettings_SetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_SetValueUInt(
		QLSettingsId settings, 
		const char* settingName, 
		unsigned int value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_SetValueUInt8( QLSettingsId settings,
/// const char* settingName, unsigned __int8 value);
///
/// @ingroup Settings
/// @brief Set the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_UINT8. This is a wrapper for QLSettings_SetValue().
/// 
/// @see QLSettings_SetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_SetValueUInt8(
		QLSettingsId settings, 
		const char* settingName, 
		unsigned __int8 value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_SetValueUInt16( QLSettingsId settings,
/// const char* settingName, unsigned __int16 value);
///
/// @ingroup Settings
/// @brief Set the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_UINT16. This is a wrapper for QLSettings_SetValue().
/// 
/// @see QLSettings_SetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_SetValueUInt16(
		QLSettingsId settings, 
		const char* settingName, 
		unsigned __int16 value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_SetValueUInt32( QLSettingsId settings,
/// const char* settingName, unsigned __int32 value);
///
/// @ingroup Settings
/// @brief Set the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_UINT32. This is a wrapper for QLSettings_SetValue().
/// 
/// @see QLSettings_SetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_SetValueUInt32(
		QLSettingsId settings, 
		const char* settingName, 
		unsigned __int32 value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_SetValueUInt64( QLSettingsId settings,
/// const char* settingName, unsigned __int64 value);
///
/// @ingroup Settings
/// @brief Set the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_UINT64. This is a wrapper for QLSettings_SetValue().
/// 
/// @see QLSettings_SetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_SetValueUInt64(
		QLSettingsId settings, 
		const char* settingName, 
		unsigned __int64 value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_SetValueFloat( QLSettingsId settings,
/// const char* settingName, float value);
///
/// @ingroup Settings
/// @brief Set the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_FLOAT. This is a wrapper for QLSettings_SetValue().
/// 
/// @see QLSettings_SetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_SetValueFloat(
		QLSettingsId settings, 
		const char* settingName, 
		float value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_SetValueDouble( QLSettingsId settings,
/// const char* settingName, double value);
///
/// @ingroup Settings
/// @brief Set the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_DOUBLE. This is a wrapper for QLSettings_SetValue().
/// 
/// @see QLSettings_SetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_SetValueDouble(
		QLSettingsId settings, 
		const char* settingName, 
		double value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_SetValueBool( QLSettingsId settings,
/// const char* settingName, bool value);
///
/// @ingroup Settings
/// @brief Set the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_BOOL. This is a wrapper for QLSettings_SetValue().
/// 
/// @see QLSettings_SetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_SetValueBool(
		QLSettingsId settings, 
		const char* settingName, 
		bool value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_SetValueVoidPointer( QLSettingsId settings,
/// const char* settingName, void* value);
///
/// @ingroup Settings
/// @brief Set the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_VOID_POINTER. This is a wrapper for QLSettings_SetValue().
/// 
/// @see QLSettings_SetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_SetValueVoidPointer(
		QLSettingsId settings, 
		const char* settingName, 
		void* value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_SetValueString( QLSettingsId settings,
/// const char* settingName, char* value);
///
/// @ingroup Settings
/// @brief Set the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_STRING. This is a wrapper for QLSettings_SetValue().
/// 
/// @see QLSettings_SetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_SetValueString(
		QLSettingsId settings, 
		const char* settingName, 
		char* value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_GetValue( QLSettingsId settings,
/// const char* settingName, QLSettingType settingType, int size, void* value);
///
/// @ingroup Settings
/// @brief Get the value of a setting in a settings container.
/// 
/// This function gets the value of a setting in a settings container if the setting exists.
///
/// @param [in] settings    The ID of the settings container from which to get the value. This ID
///                         is obtained by calling either the function QLSettings_Create() or the
///                         function QLSettings_Load().
/// @param [in] settingName A NULL terminated string containing the name of the setting whose
///                         value will be retrieved.
/// @param [in] settingType The type of data object that was passed in. This tells the API how to
///                         interpret the object pointed to by "value".
/// @param [in] size        If the type is QL_SETTING_TYPE_STRING then this is the size of the
///                         buffer pointed to by "value". For other types this is not used.
/// @param [out] value      A pointer to an object that will receive the value of the setting.
///                         The object type must match the type indicated by "settingType". For
///                         the type QL_SETTING_TYPE_STRING this must be a char array at least as
///                         large as "size".
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the setting
/// value was successfully retrieved.
/// 
/// @example /src/QuickStart/Initialize.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValue(
        QLSettingsId settings,
        const char* settingName,
        QLSettingType settingType,
        int size,
        void* value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_GetValueInt( QLSettingsId settings,
/// const char* settingName, int* value);
///
/// @ingroup Settings
/// @brief Get the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_INT. This is a wrapper for QLSettings_GetValue().
/// 
/// @see QLSettings_GetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_GetValueInt(
		QLSettingsId settings, 
		const char* settingName, 
		int* value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_GetValueInt8( QLSettingsId settings,
/// const char* settingName, __int8* value);
///
/// @ingroup Settings
/// @brief Get the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_INT8. This is a wrapper for QLSettings_GetValue().
/// 
/// @see QLSettings_GetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_GetValueInt8(
		QLSettingsId settings, 
		const char* settingName, 
		__int8* value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_GetValueInt16( QLSettingsId settings,
/// const char* settingName, __int16* value);
///
/// @ingroup Settings
/// @brief Get the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_INT16. This is a wrapper for QLSettings_GetValue().
/// 
/// @see QLSettings_GetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_GetValueInt16(
		QLSettingsId settings, 
		const char* settingName, 
		__int16* value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_GetValueInt32( QLSettingsId settings,
/// const char* settingName, __int32* value);
///
/// @ingroup Settings
/// @brief Get the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_INT32. This is a wrapper for QLSettings_GetValue().
/// 
/// @see QLSettings_GetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_GetValueInt32(
		QLSettingsId settings, 
		const char* settingName, 
		__int32* value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_GetValueInt64( QLSettingsId settings,
/// const char* settingName, __int64* value);
///
/// @ingroup Settings
/// @brief Get the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_INT64. This is a wrapper for QLSettings_GetValue().
/// 
/// @see QLSettings_GetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_GetValueInt64(
		QLSettingsId settings, 
		const char* settingName, 
		__int64* value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_GetValueUInt( QLSettingsId settings,
/// const char* settingName, unsigned int* value);
///
/// @ingroup Settings
/// @brief Get the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_UINT. This is a wrapper for QLSettings_GetValue().
/// 
/// @see QLSettings_GetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_GetValueUInt(
		QLSettingsId settings, 
		const char* settingName, 
		unsigned int* value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_GetValueUInt8( QLSettingsId settings,
/// const char* settingName, unsigned __int8* value);
///
/// @ingroup Settings
/// @brief Get the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_UINT8. This is a wrapper for QLSettings_GetValue().
/// 
/// @see QLSettings_GetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_GetValueUInt8(
		QLSettingsId settings, 
		const char* settingName, 
		unsigned __int8* value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_GetValueUInt16( QLSettingsId settings,
/// const char* settingName, unsigned __int16* value);
///
/// @ingroup Settings
/// @brief Get the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_UINT16. This is a wrapper for QLSettings_GetValue().
/// 
/// @see QLSettings_GetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_GetValueUInt16(
		QLSettingsId settings, 
		const char* settingName, 
		unsigned __int16* value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_GetValueUInt32( QLSettingsId settings,
/// const char* settingName, unsigned __int32* value);
///
/// @ingroup Settings
/// @brief Get the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_UINT32. This is a wrapper for QLSettings_GetValue().
/// 
/// @see QLSettings_GetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_GetValueUInt32(
		QLSettingsId settings, 
		const char* settingName, 
		unsigned __int32* value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_GetValueUInt64( QLSettingsId settings,
/// const char* settingName, unsigned __int64* value);
///
/// @ingroup Settings
/// @brief Get the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_UINT64. This is a wrapper for QLSettings_GetValue().
/// 
/// @see QLSettings_GetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_GetValueUInt64(
		QLSettingsId settings, 
		const char* settingName, 
		unsigned __int64* value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_GetValueFloat( QLSettingsId settings,
/// const char* settingName, float* value);
///
/// @ingroup Settings
/// @brief Get the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_FLOAT. This is a wrapper for QLSettings_GetValue().
/// 
/// @see QLSettings_GetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_GetValueFloat(
		QLSettingsId settings, 
		const char* settingName, 
		float* value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_GetValueDouble( QLSettingsId settings,
/// const char* settingName, double* value);
///
/// @ingroup Settings
/// @brief Get the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_DOUBLE. This is a wrapper for QLSettings_GetValue().
/// 
/// @see QLSettings_GetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_GetValueDouble(
		QLSettingsId settings, 
		const char* settingName, 
		double* value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_GetValueBool( QLSettingsId settings,
/// const char* settingName, bool* value);
///
/// @ingroup Settings
/// @brief Get the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_BOOL. This is a wrapper for QLSettings_GetValue().
/// 
/// @see QLSettings_GetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_GetValueBool(
		QLSettingsId settings, 
		const char* settingName, 
		bool* value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_GetValueVoidPointer( QLSettingsId settings,
/// const char* settingName, void** value);
///
/// @ingroup Settings
/// @brief Get the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_VOID_POINTER. This is a wrapper for QLSettings_GetValue().
/// 
/// @see QLSettings_SetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_GetValueVoidPointer(
		QLSettingsId settings, 
		const char* settingName, 
		void** value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_GetValueString( QLSettingsId settings,
/// const char* settingName, int size, char* value);
///
/// @ingroup Settings
/// @brief Get the value of a setting in a settings container using the setting type
/// QL_SETTING_TYPE_STRING. This is a wrapper for QLSettings_GetValue().
/// 
/// @see QLSettings_GetValue()
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
	QLSettings_GetValueString(
		QLSettingsId settings,
		const char* settingName, 
		int size, 
		char* value);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @}
////////////////////////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLSettings_GetValueStringSize( QLSettingsId settings,
/// const char* settingName, int* size);
///
/// @ingroup Settings
/// @brief Get the string size of the setting value.
/// 
/// This function gets the size of the string, including the terminating NULL character, for the
/// specified setting's value. A buffer would have to be at least as large as the value returned
/// by "size" to successfully get a value of type QL_SETTING_TYPE_STRING using the function
/// QLSettings_GetValue().
///
/// @param [in] settings    The ID of the settings container from which to get the value's string
///                         length. This ID is obtained by calling either the function
///                         QLSettings_Create() or the function QLSettings_Load().
/// @param [in] settingName A NULL terminated string containing the name of the setting whose
///                         value's string length will be retrieved.
/// @param [out] size       A pointer to an object that will receive the string length of the
///                         value.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the string
/// length for the setting value was successfully retrieved.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValueStringSize(
        QLSettingsId settings,
        const char* settingName,
        int* size);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLCalibration_Load( const char* path,
/// QLCalibrationId* calibration);
///
/// @ingroup Calibration
/// @brief Load calibration data from a file.
/// 
/// This function loads calibration data from a file into a calibration container. The file must
/// have been created by calling the function QLCalibration_Save().
///
/// @param [in] path            A NULL-terminated string containing the full pathname of the
///                             calibration file.
/// @param [in,out] calibration A pointer to a QLCalibrationId object. If the QLCalibrationId
///                             object refers to a valid calibration container then that
///                             calibration container will receive the loaded calibration. If the
///                             QLCalibrationId object does not refer to a valid calibration
///                             container then a new calibration container will be created and
///                             this object will receive its ID.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the calibration
/// was successfully loaded.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_Load(
        const char* path, 
        QLCalibrationId* calibration);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLCalibration_Save( const char* path,
/// QLCalibrationId calibration);
///
/// @ingroup Calibration
/// @brief Save calibration data to a file.
/// 
/// This function saves calibration data to a file. The calibration data can later be loaded by
/// calling the function QLCalibration_Load().
///
/// @param [in] path        A NULL-terminated string containing the full pathname of the
///                         calibration file.
/// @param [in] calibration The ID of the calibration container whose data will be saved. This ID
///                         is obtained by calling either the function QLCalibration_Create() or
///                         the function QLCalibration_Load().
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the calibration
/// was successfully saved.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_Save(
        const char* path, 
        QLCalibrationId calibration);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLCalibration_Create( QLCalibrationId source,
/// QLCalibrationId* calibration);
///
/// @ingroup Calibration
/// @brief Create a new calibration container.
/// 
/// This function creates a new calibration container. If the source ID references a valid
/// calibration container then its data will be copied into the newly created calibration
/// container. If the source ID does not reference a valid settings container then the new
/// settings container will be empty.
///
/// @param [in] source       The ID of the calibration container from which to copy calibration
///                          data. This ID is obtained by calling either this function or the
///                          function QLCalibration_Load(). To create an empty settings container,
///                          set this value to zero.
/// @param [out] calibration A pointer to a QLCalibrationId object. This object will receive the
///                          ID of the newly created calibration container.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the new
/// calibration container was successfully created.
/// 
/// @example /src/QuickStart/Calibrate.cpp
/// @example /src/QuickStart/OpencvUtility.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_Create(
        QLCalibrationId source,
        QLCalibrationId* calibration);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLCalibration_Initialize( QLDeviceId device,
/// QLCalibrationId calibration, QLCalibrationType type);
///
/// @ingroup Calibration
/// @brief Initialize a calibration container.
/// 
/// This function initializes a calibration container which makes it ready to receive new
/// calibration data from a device. Any calibration data previously in the container is stored in
/// temporary memory until QLCalibration_Finalize() is called.
///
/// @param [in] device      The ID of the device from which to receive calibration data. This ID
///                         is obtained by calling the function QLDevice_Enumerate().
/// @param [in] calibration The ID of the calibration container that will receive the new
///                         calibration data. This ID is obtained by calling either the function
///                         QLCalibration_Create() or the function QLCalibration_Load().
/// @param [in] type        The type of calibration to perform.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the calibration
/// container was successfully initialized and is now ready to receive new calibration data.
/// 
/// @see Device
/// 
/// @example /src/QuickStart/Calibrate.cpp
/// @example /src/QuickStart/OpencvUtility.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_Initialize(
        QLDeviceId device, 
        QLCalibrationId calibration, 
        QLCalibrationType type);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLCalibration_GetTargets( QLCalibrationId calibration,
/// int* numTargets, QLCalibrationTarget* targets);
///
/// @ingroup Calibration
/// @brief Get the positions for the calibration targets.
/// 
/// This function gets the locations and IDs of the targets for the calibration. It can be called
/// to retrieve target positions from a calibration container that is calibrating or one that has
/// already been finalized.
///
/// @param [in] calibration    The ID of a calibration container from which to get the target
///                            data. This ID is obtained by calling either the function
///                            QLCalibration_Create() or the function QLCalibration_Load().
/// @param [in,out] numTargets A pointer to an integer containing the size of the target buffer
///                            pointed to by "targets". When the function returns this contains
///                            the number of targets for the calibration.
/// @param [out] targets       A pointer to a QLCalibrationTarget buffer that will receive the
///                            data for the calibration points.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the target
/// positions were successfully retrieved.
///
/// @example /src/QuickStart/Calibrate.cpp
/// @example /src/QuickStart/OpencvUtility.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_GetTargets(
        QLCalibrationId calibration, 
        int* numTargets,
        QLCalibrationTarget* targets);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLCalibration_Calibrate( QLCalibrationId calibration,
/// QLTargetId target, int duration, bool block);
///
/// @ingroup Calibration
/// @brief Calibrate a target.
/// 
/// This function causes the calibration container to collect data from a device for a specific
/// target location. It should be called at least once for each target that was received by
/// calling the function QLCalibration_GetTargets(). If this function is called multiple times
/// for the same target and not all targets have been calibrated then the data collected by the
/// last call will overwrite previous data. If this function is called multiple times for the
/// same target and all targets have been calibrated then the data collected by the last call
/// will only overwrite the previous data if it improves the calibration overall.
/// 
/// This function must be called for each target after a calibration has been initialized but
/// before it has been finalized.
///
/// @param [in] calibration The ID of a calibration container which has been initialized and is
///                         ready to receive calibration data. This ID is obtained by calling
///                         either the function QLCalibration_Create() or the function
///                         QLCalibration_Load().
/// @param [in] target      The ID of a target that the user is looking at. This ID is obtained
///                         by calling the function QLCalibration_GetTargets(). Usually there
///                         should be a target drawn on the screen at the location referenced by
///                         this target before calling this function.
/// @param [in] duration    The length of time that the API will collect calibration data. For
///                         best results the user should be looking at the target position the
///                         entire time.
/// @param [in] block       A flag to determine whether this function should block. If this is
///                         true then the function will block and not return until the API is
///                         done collecting data for the calibration point. If this is false then
///                         this function will return immediately and the status of the data
///                         collection can be determined by calling the function
///                         QLCalibration_GetStatus().
///
/// @return The success of the function. If the function blocks then a return value of
/// QL_ERROR_OK means that the duration has finished and that data was gathered for the target.
/// If the function does not block then a return value of QL_ERROR_OK means that calibration data
/// collection was successfully started for the target.
///
/// @example /src/QuickStart/Calibrate.cpp
/// @example /src/QuickStart/OpencvUtility.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_Calibrate(
        QLCalibrationId calibration, 
        QLTargetId target, 
        int duration, 
        bool block);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLCalibration_GetScoring( QLCalibrationId calibration,
/// QLTargetId target, QLEyeType eye, QLCalibrationScore* score);
///
/// @ingroup Calibration
/// @brief Get the scoring of a calibration target.
/// 
/// This function gets the the score of a calibration target for a particular eye. The eye must
/// have been detected at least once for each target of the calibration in order to produce a
/// score.
/// 
/// This function can be called before or after a calibration has been finalized.
///
/// @param [in] calibration The ID of a calibration container whose data will be used to
///                         calculate a score. This ID is obtained by calling either the function
///                         QLCalibration_Create() or the function QLCalibration_Load().
/// @param [in] target      The ID of a target whose score will be retrieved. This ID is obtained
///                         by calling the function QLCalibration_GetTargets().
/// @param [in] eye         The eye whose score should be retrieved.
/// @param [out] score      A pointer to a QLCalibrationScore object that will receive the score.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the score was
/// successfully retrieved. If the return value is QL_ERROR_INTERNAL_ERROR then use the function
/// QLCalibration_GetStatus()
/// to get extended error information().
/// 
/// @example /src/QuickStart/Calibrate.cpp
/// @example /src/QuickStart/OpencvUtility.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_GetScoring(
        QLCalibrationId calibration, 
        QLTargetId target, 
        QLEyeType eye, 
        QLCalibrationScore* score);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLCalibration_GetStatus( QLCalibrationId calibration,
/// QLTargetId target, QLCalibrationStatus* calibrationStatus);
///
/// @ingroup Calibration
/// @brief Get the status of a calibration target.
/// 
/// This function retrieves the status of a calibration target for a given calibration container.
/// It can be called before or after a calibration has been finalized.
///
/// @param [in] calibration        The ID of a calibration container whose data will be used to
///                                determine a status for the target. This ID is obtained by
///                                calling either the function QLCalibration_Create() or the
///                                function QLCalibration_Load().
/// @param [in] target             The ID of a target whose status will be retrieved. This ID is
///                                obtained by calling the function QLCalibration_GetTargets().
/// @param [out] calibrationStatus A pointer to a QLCalibrationStatus object that will receive
///                                the status.
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the calibration
/// status for the target was successfully retrieved.
/// 
/// @example /src/QuickStart/Calibrate.cpp
/// @example /src/QuickStart/OpencvUtility.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_GetStatus(
        QLCalibrationId calibration,
        QLTargetId target,
        QLCalibrationStatus* calibrationStatus);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLCalibration_Finalize( QLCalibrationId calibration);
///
/// @ingroup Calibration
/// @brief Finalize a completed calibration.
/// 
/// This function finalizes a calibration after it is complete. It should be called when
/// calibration data has been successfully collected at each target position and the target
/// scores meet the user requirements.
/// 
/// This function clears any previous calibration data that was stored in the container and
/// replaces it with the new calibration data.
///
/// @param [in] calibration The ID of a calibration container to finalize. This ID is obtained by
///                         calling either the function QLCalibration_Create() or the function
///                         QLCalibration_Load().
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the calibration
/// was successfully finalized.
///
/// @example /src/QuickStart/Calibrate.cpp
/// @example /src/QuickStart/OpencvUtility.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_Finalize(
        QLCalibrationId calibration);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLCalibration_Cancel( QLCalibrationId calibration);
///
/// @ingroup Calibration
/// @brief Cancel a calibration that is in process.
/// 
/// This function cancels a calibration. Recently collected calibration data is cleared and the
/// calibration container is restored to the state it was in before QLCalibration_Initialize()
/// was called.
///
/// @param [in] calibration The ID of a calibration container to cancel. This ID is obtained by
///                         calling either the function QLCalibration_Create() or the function
///                         QLCalibration_Load().
///
/// @return The success of the function. If the return value is QL_ERROR_OK then the calibration
/// was successfully canceled.
/// 
/// @example /src/QuickStart/Calibrate.cpp
/// @example /src/QuickStart/OpencvUtility.cpp.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_Cancel(
        QLCalibrationId calibration);

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @fn QLError QUICK_LINK_2_CALL_CONVEN QLCalibration_AddBias( QLCalibrationId calibration,
/// QLEyeType eye, float xOffset, float yOffset);
///
/// @ingroup Calibration
/// @brief Add a bias to the data in the calibration container.
///
/// @param [in] calibration The ID of a calibration container to which the
///                         bias will be added. This ID is obtained by
///                         calling either the function
///                         QLCalibration_Create() or the function
///                         QLCalibration_Load().
/// @param [in] eye         The eye to which the bias should be added.
/// @param [in] xOffset     The percent of the screen in the x direction that
///                         the bias should affect the gaze point. Negative
///                         values will cause the resulting gaze point to be
///                         left of the current position. Positive values
///                         will cause the resulting gaze point to be right
///                         of the current position.
/// @param [in] yOffset     The percent of the screen in the Y direction that
///                         the bias should affect the gaze point. Negative
///                         values will cause the resulting gaze point to be
///                         above the current position. Positive values will
///                         cause the resulting gaze point to be below the
///                         current position.
///
/// @return The success of the function. If the return value is QL_ERROR_OK
/// then the bias was successfully added to the calibration container.
////////////////////////////////////////////////////////////////////////////////////////////////////
QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_AddBias(
        QLCalibrationId calibration, 
        QLEyeType eye, 
        float xOffset, 
        float yOffset);

#ifdef __cplusplus
}
#endif

#endif
