////////////////////////////////////////////////////////////////////////////////////////////////////
/// @file   QLTypes.h
///
/// @brief This file contains the definitions of all the types, structures,
/// and enumerations available through QuickLink2.dll.
/// 
/// @copyright 1996 - 2015, EyeTech Digital Systems
////////////////////////////////////////////////////////////////////////////////////////////////////
#ifndef QUICK_LINK_2_TYPES_H
#define	QUICK_LINK_2_TYPES_H

#ifdef __cplusplus
extern "C"
{
#endif

#define QUICK_LINK_2_CALL_CONVEN __stdcall

#define STRING_LENGTH_128 128

#if (defined _WIN64) || (defined _M_X64)
////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def    QUICK_LINK_64_BIT
///
/// @brief This is defined when _WIN64 is defined. _WIN64 is defined by the Microsoft compiler when
/// targeting a 64 bit platform. Due to errors in some versions of Visual Studio, automatic
/// syntax highlighting shows _WIN64 as not defined even though it actually is defined at compile
/// time. To facilitate coding, a check for _M_X64 was added as well which fixes automatic syntax
/// highlighting. _M_X64 is defined when targeting an AMD64 processor.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QUICK_LINK_64_BIT

#elif defined _WIN32

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def    QUICK_LINK_32_BIT
///
/// @brief This is defined when _WIN64 is not defined and when _WIN32 is defined. 
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QUICK_LINK_32_BIT

#endif

#if !(defined QUICK_LINK_64_BIT) && !(defined QUICK_LINK_32_BIT)
#error QUICK_LINK_64_BIT or QUICK_LINK_32_BIT must be defined. Please check compiler settings.
#endif

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @ingroup Settings
/// @{
////////////////////////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_BANDWIDTH_MODE
///
/// @brief The bandwidth mode the device will use.
/// 
/// @see QLDeviceBandwidthMode.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_BANDWIDTH_MODE "DeviceBandwidthMode"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_BANDWIDTH_PERCENT_FULL
///
/// @brief The percentage of the bus bandwidth used by the device when searching for eyes. This
/// value is only used when the QL_SETTING_DEVICE_BANDWIDTH_MODE setting is set to
/// QL_DEVICE_BANDWIDTH_MODE_MANUAL. 
/// 
/// Possible values range from 1-100.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_BANDWIDTH_PERCENT_FULL "DeviceBandwidthPercentFull"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_BANDWIDTH_PERCENT_TRACKING
///
/// @brief The percentage of the bus bandwidth used by the device when at least one eye has been
/// found and is being tracked. This value is only used when the QL_SETTING_DEVICE_BANDWIDTH_MODE
/// setting is set to QL_DEVICE_BANDWIDTH_MODE_MANUAL. 
/// 
/// Possible values range from 1-100.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_BANDWIDTH_PERCENT_TRACKING "DeviceBandwidthPercentTracking"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_BINNING
///
/// @brief  The number of pixels to combine in the x and y direction.
/// 
/// The output pixel values are the average of the combined pixels. Possible values are 1, 2,
/// and 4. A value of 1 outputs one pixel of data for each pixel on the image sensor. A value of
/// 2 outputs one pixel of data for every 4 (2 X 2) pixels on the image sensor. A value of 4
/// outputs one pixel of data for every 16 (4 X 4) pixels on the image sensor.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_BINNING "DeviceBinning"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_CALIBRATE_ENABLED
///
/// @brief The flag for enabling/disabling calibration.
/// 
/// Possible values are true and false. If false then calibration data collection will be
/// disabled and new calibrations can not be performed.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_CALIBRATE_ENABLED "DeviceCalibrateEnabled"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_DISTANCE
///
/// @brief The approximate distance in centimeters from the user to the device.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_DISTANCE "DeviceDistance"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_EXPOSURE
///
/// @brief The exposure time in milliseconds for each frame. Possible values range from 1-50 ms.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_EXPOSURE "DeviceExposure"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_FLIP_X
///
/// @brief The horizontal direction of the image.
/// 
/// Possible values are true and false. A value of false will result in the right eye being
/// closest to the origin (0, 0) of the image. A value of true will mirror the image and cause
/// the left eye to be closest to the origin.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_FLIP_X "DeviceFlipX"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_FLIP_Y
///
/// @brief The vertical direction of the image.
/// 
/// Possible values are true and false. A value of false will result in the top of the head being
/// closest to the origin (0, 0) of the image. A value of true will cause the bottom of the face
/// to be closest to the origin of the image.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_FLIP_Y "DeviceFlipY"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_GAIN_MODE
///
/// @brief  The gain mode the device will use.
/// 
/// @see QLDeviceGainMode. 
/// @see QL_SETTING_DEVICE_GAIN_VALUE. 
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_GAIN_MODE "DeviceGainMode"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_GAIN_VALUE
///
/// @brief The gain value the device will use when the setting QL_SETTING_DEVICE_GAIN_MODE is set to
/// QL_DEVICE_GAIN_MODE_MANUAL.
/// 
/// @see DeviceGainMode. 
/// @see QL_SETTING_DEVICE_GAIN_MODE. 
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_GAIN_VALUE "DeviceGainValue"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_GAZE_POINT_FILTER_MODE
///
/// @brief The filter mode for the output gaze point.
/// 
/// @see QLDeviceGazePointFilterMode.
/// @see QL_SETTING_DEVICE_GAZE_POINT_FILTER_VALUE.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_GAZE_POINT_FILTER_MODE "DeviceGazePointFilterMode"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_GAZE_POINT_FILTER_VALUE
///
/// @brief The value used for the filtering mode.
/// 
/// @see QLDeviceGazePointFilterMode.
/// @see QL_SETTING_DEVICE_GAZE_POINT_FILTER_MODE.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_GAZE_POINT_FILTER_VALUE "DeviceGazePointFilterValue"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_IMAGE_PROCESSING_ENABLED
///
/// @brief The flag for enabling/disabling image processing.
/// 
/// Possible values are true and false. If false then the eyes will not be searched for and the
/// output eye information will not be valid.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_IMAGE_PROCESSING_ENABLED "DeviceImageProcessingEnabled"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_IMAGE_PROCESSING_EYES_TO_FIND
///
/// @brief The search setting for the image processing.
///
/// @see QLDeviceEyesToUse.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_IMAGE_PROCESSING_EYES_TO_FIND "DeviceImageProcessingEyesToUse"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_IMAGE_PROCESSING_EYE_RADIUS_LEFT;
///
/// @brief The radius of the cornea of the left eye in centimeters. This radius can be
/// calculated by calling the function QLDevice_CalibrateEyeRadius(). This radius will affect the
/// calculated distance value that is output for each frame.
/// 
/// @see QL_SETTING_DEVICE_IMAGE_PROCESSING_EYE_RADIUS_RIGHT.
/// @see QLFrameData.
/// @see QLDevice_CalibrateEyeRadius().
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_IMAGE_PROCESSING_EYE_RADIUS_LEFT "DeviceImageProcessingEyeRadiusLeft"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_IMAGE_PROCESSING_EYE_RADIUS_RIGHT
///
/// @brief The radius of the cornea of the right eye in centimeters. This radius can be
/// calculated by calling the function QLDevice_CalibrateEyeRadius(). This radius will affect the
/// calculated distance value that is output for each frame.
///
/// @see QL_SETTING_DEVICE_IMAGE_PROCESSING_EYE_RADIUS_LEFT.
/// @see QLFrameData.
/// @see QLDevice_CalibrateEyeRadius().
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_IMAGE_PROCESSING_EYE_RADIUS_RIGHT "DeviceImageProcessingEyeRadiusRight"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_INTERPOLATE_ENABLED
///
/// @brief The flag for enabling/disabling final gaze point interpolation.
/// 
/// Possible values are true and false. If false then the final gaze point will not be
/// interpolated and the output gaze point will not be valid.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_INTERPOLATE_ENABLED "DeviceInterpolateEnabled"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_IR_LIGHT_MODE
///
/// @brief The mode of operation for the IR lights of the device.
/// @see QLDeviceIRLightMode.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_IR_LIGHT_MODE "DeviceIRLightMode"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_LENS_FOCAL_LENGTH
/// 
/// @brief The focal length in centimeters of the lens.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_LENS_FOCAL_LENGTH "DeviceLensFocalLength"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_ROI_MOVE_THRESHOLD_PERCENT_X
///
/// @brief The horizontal distance in percentage of the image width that either eye can be from
/// the left or right edge of the region of interest before the region of interest will move and
/// try to re-center the eyes.
/// 
/// Possible values range from 1-50.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_ROI_MOVE_THRESHOLD_PERCENT_X "DeviceRoiMoveThresholdPercentX"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_ROI_MOVE_THRESHOLD_PERCENT_Y
///
/// @brief The vertical distance in percentage of the image height that either eye can be from
/// the top or bottom edge of the region of interest before the region of interest will move and
/// try to re-center the eyes.
/// 
/// Possible values range from 1-50.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_ROI_MOVE_THRESHOLD_PERCENT_Y "DeviceRoiMoveThresholdPercentY"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_ROI_SIZE_PERCENT_X
///
/// @brief The width of the region of interest in percentage of the horizontal sensor size when
/// the eyes are being tracked.
/// 
/// Possible values range from 1-100.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_ROI_SIZE_PERCENT_X "DeviceRoiSizePercentX"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_ROI_SIZE_PERCENT_Y
///
/// @brief The height of the region of interest in percentage of the vertical sensor size when
/// the eyes are being tracked.
/// 
/// Possible values range from 1-100.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_ROI_SIZE_PERCENT_Y "DeviceRoiSizePercentY"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @def QL_SETTING_DEVICE_DOWNSAMPLE_SCALE_FACTOR
///
/// @brief The scale at which to downsample the image before sending it over USB. The downsampled
/// image is actually 1/(this value) in both the x and y direction, so a value of 2 will downsample
/// the image to 1/2 the original image width and 1/2 the original image height. A value of 5 will
/// downsample the image to 1/5 the original width and 1/5 the original image height.
////////////////////////////////////////////////////////////////////////////////////////////////////
#define QL_SETTING_DEVICE_DOWNSAMPLE_SCALE_FACTOR "DeviceDownsampleScaleFactor"


////////////////////////////////////////////////////////////////////////////////////////////////////
/// @}
////////////////////////////////////////////////////////////////////////////////////////////////////

//------------------------------------------------------------------------------
// Type Defines
//------------------------------------------------------------------------------

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @typedef int QLDeviceId
///
/// @brief A unique identifier that is used to access a device.
/// 
/// Once QuickLink2.dll is loaded a QLDeviceId will always refer to the same device until
/// QuickLink2.dll is unloaded. This is true even if the device is disconnected from the computer
/// and then later reconnected. A device is not guaranteed to have the same QLDeviceId value from
/// one loading of QuickLink2.dll to the next.
///
/// @ingroup Device
/// 		 
/// @see QLDevice_Enumerate().
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef int QLDeviceId;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @typedef    int QLDeviceGroupId
///
/// @brief A unique identifier that is used to access a group of devices. 
///
/// @ingroup Device
/// 		 
/// @see QLDeviceGroup_Create().
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef int QLDeviceGroupId;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @typedef    int QLDeviceOrGroupId
///
/// @brief This indicates support of either a QLDeviceId or a QLDeviceGroupId. 
///
/// @ingroup Device
/// 		 
/// @see QLDeviceId.
/// @see QLDeviceGroupId.
/// @see QLDevice_Enumerate().
/// @see QLDeviceGroup_Create().
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef int QLDeviceOrGroupId;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @typedef    int QLSettingsId
///
/// @brief A unique identifier that is used to access a settings container. 
///
/// @ingroup Settings
/// 		 
/// @see QLSettings_Create().
/// @see QLSettings_Load().
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef int QLSettingsId;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @typedef    int QLCalibrationId
///
/// @brief A unique identifier that is used to access a calibration container. 
///
/// @ingroup Calibration
/// 		 
/// @see QLCalibration_Create().
/// @see QLCalibration_Load().
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef int QLCalibrationId;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @typedef    int QLTargetId
///
/// @brief A unique identifier that is used to access a target for a given calibration container.
///
/// @ingroup Calibration
/// 		 
/// @see QLCalibrationTarget.
/// @see QLCalibration_GetTargets().
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef int QLTargetId;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @enum QLCalibrationStatus
///
/// @brief Status values for a calibration target
/// 		 
/// @see QLCalibration_GetStatus().
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef enum
{
    /// The calibration target has data for both the left and right eyes.
    QL_CALIBRATION_STATUS_OK =            0,

    /// The calibration target is in the process of calibrating.
    QL_CALIBRATION_STATUS_CALIBRATING =   1,

    /// The calibration target has data for the right eye but not the left eye.
    QL_CALIBRATION_STATUS_NO_LEFT_DATA =  2,

    /// The calibration target has data for the left eye but not the right eye.
    QL_CALIBRATION_STATUS_NO_RIGHT_DATA = 3,

    // The calibration target does not have data for the left or right eye.
    QL_CALIBRATION_STATUS_NO_DATA =       4
} QLCalibrationStatus;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @enum QLCalibrationType
///
/// @brief Values that identify different calibration modes.
/// 		 
/// @see QLCalibration_Initialize().
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef enum
{
    /// A five point calibration.
    QL_CALIBRATION_TYPE_5 =  0,

    /// A nine point calibration.
    QL_CALIBRATION_TYPE_9 =  1,

    /// A sixteen point calibration.
    QL_CALIBRATION_TYPE_16 = 2
} QLCalibrationType;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @enum QLDeviceBandwidthMode
///
/// @brief Values that identify which bandwidth mode to use.
/// 		 
/// @see QL_SETTING_DEVICE_BANDWIDTH_MODE.
/// @see QL_SETTING_DEVICE_BANDWIDTH_PERCENT_FULL.
/// @see QL_SETTING_DEVICE_BANDWIDTH_PERCENT_TRACKING.
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef enum
{
    /// The device will automatically adjust the bandwidth value until the best value is found.
    QL_DEVICE_BANDWIDTH_MODE_AUTO	=  0,

    /// The device will use a fixed bandwidth value.
    QL_DEVICE_BANDWIDTH_MODE_MANUAL  =  1
} QLDeviceBandwidthMode;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @enum QLDeviceEyesToUse
///
/// @brief Values that identify which eyes the device should process.
/// 		 
/// @see QL_SETTING_DEVICE_IMAGE_PROCESSING_EYES_TO_FIND.
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef enum
{
    /// Only search for and process the left eye
    QL_DEVICE_EYES_TO_USE_LEFT			= 0,

    /// Only search for and process the right eye
    QL_DEVICE_EYES_TO_USE_RIGHT			= 1,

    /// Search for and process both eyes. If only one eye is found then use it for calculating the
    /// weighted gaze point. 
    QL_DEVICE_EYES_TO_USE_LEFT_OR_RIGHT	= 2,

    /// Search for and process both eyes. Both eyes must be found for calculating the weighted gaze
    /// point. 
    QL_DEVICE_EYES_TO_USE_LEFT_AND_RIGHT = 3
} QLDeviceEyesToUse;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @enum QLDeviceGainMode
///
/// @brief Values that identify which gain mode to use.
/// 		 
/// @see QL_SETTING_DEVICE_GAIN_MODE.
/// @see QL_SETTING_DEVICE_GAIN_VALUE.
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef enum
{
    /// The device will automatically adjust the vain value.
    QL_DEVICE_GAIN_MODE_AUTO	=  0,

    /// The device will use a fixed gain value.
    QL_DEVICE_GAIN_MODE_MANUAL  =  1
} QLDeviceGainMode;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @enum QLDeviceGazePointFilterMode
///
/// @brief Values that identify which gain mode to use.
///
/// @see QL_SETTING_DEVICE_GAZE_POINT_FILTER_MODE.
/// @see QL_SETTING_DEVICE_GAZE_POINT_FILTER_VALUE.
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef enum
{
    /// No gaze point filtering will be done.
	QL_DEVICE_GAZE_POINT_FILTER_NONE                    = 0,

    /// The median gaze point value over the last X number of frames, where X equals the value
    /// represented by the setting
    /// @ref QL_SETTING_DEVICE_GAZE_POINT_FILTER_VALUE. This will produce a gaze point latency time
    /// of about (X / fps / 2) seconds. 
	QL_DEVICE_GAZE_POINT_FILTER_MEDIAN_FRAMES           = 1,

    /// The median gaze point value over the last X number of frames, where X is the number of
    /// frames gathered over twice the amount of milliseconds represented by the setting
    /// @ref QL_SETTING_DEVICE_GAZE_POINT_FILTER_VALUE. This will produce a
    /// latency of about
    /// @ref QL_SETTING_DEVICE_GAZE_POINT_FILTER_VALUE milliseconds. 
    QL_DEVICE_GAZE_POINT_FILTER_MEDIAN_TIME             = 2,

    /// The heuristic filter uses different filtering strengths when the eye is moving and when it
    /// is fixating. When the eye is moving, very little filtering is done which results in very low
    /// latency. When the eye is fixating, large amounts of filtering are being done which greatly
    /// reduce the amount of jitter. Durring fixation, filtering is done over the last X number of
    /// frames where X equals the value represented by the setting
    /// @ref QL_SETTING_DEVICE_GAZE_POINT_FILTER_VALUE. 
	QL_DEVICE_GAZE_POINT_FILTER_HEURISTIC_FRAMES        = 3,

    /// The heuristic filter uses different filtering strengths when the eye is moving and when it
    /// is fixating. When the eye is moving, very little filtering is done which results in very low
    /// latency. When the eye is fixating, large amounts of filtering are being done which greatly
    /// reduce the amount of jitter. Durring fixation, filtering is done over the last X number of
    /// frames where X equals the number of frames gathered over twice the amount of milliseconds
    /// represented by the setting
    /// @ref QL_SETTING_DEVICE_GAZE_POINT_FILTER_VALUE. This produces aproximatly
    /// the same amount of latency durring fixation for all frame rates. 
	QL_DEVICE_GAZE_POINT_FILTER_HEURISTIC_TIME          = 4,

    /// The weighted previous frame mode filters the gaze point by summing the current weighted gaze
    /// point location and the previous weighted gazepoint location. The weights are based on the
    /// distance the current gaze point is away from the previous gaze point. The larger the
    /// distance, the greater the weight on the current gaze point. The smaller the distance, the
    /// greater the weight on the previous gaze point. This results in very low latency when the eye
    /// is moving and very low jitter when the eye is fixating. The value represented by the setting
    /// @ref QL_SETTING_DEVICE_GAZE_POINT_FILTER_VALUE affects the rate at which
    /// the weighting changes from the previous gaze point to the current gaze point. Possible
    /// values range between 0 and 200. 
    QL_DEVICE_GAZE_POINT_FILTER_WEIGHTED_PREVIOUS_FRAME = 5

} QLDeviceGazePointFilterMode;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @enum QLDeviceIRLightMode
///
/// @brief Values that identify which gain mode to use.
///
/// @see QL_SETTING_DEVICE_IR_LIGHT_MODE.
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef enum
{
    /// The IR lights on the device will be off. Use this when other IR light sources are being
    /// used. 
    QL_DEVICE_IR_LIGHT_MODE_OFF  =  0,

    /// The IR lights on the device will be on. This should be used for normal operation.
    QL_DEVICE_IR_LIGHT_MODE_AUTO	=  1
} QLDeviceIRLightMode;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @enum QLDeviceStatus
///
/// @brief Status values for a device.
///
/// @see QLDevice_GetStatus().
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef enum
{
    /// The device cannot be detected. It has probably been disconnected from the computer.
    QL_DEVICE_STATUS_UNAVAILABLE = 0,

    /// The device is stopped.
    QL_DEVICE_STATUS_STOPPED =     1,

    /// The device is in the process of being started.
    QL_DEVICE_STATUS_INITIALIZED = 2,

    /// The device is running.
    QL_DEVICE_STATUS_STARTED =   3
} QLDeviceStatus;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @enum   QLError
///
/// @brief  Error codes returned from Quick Link 2 functions.
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef enum
{
    /// The function successfully completed.
    QL_ERROR_OK =                               0,

    /// The input device ID is invalid.
    QL_ERROR_INVALID_DEVICE_ID =                1,
    
    /// The input settings ID is invalid.
    QL_ERROR_INVALID_SETTINGS_ID =              2,

    /// The input calibration ID is invalid.
    QL_ERROR_INVALID_CALIBRATION_ID =           3,

    /// The input target ID is invalid.
    QL_ERROR_INVALID_TARGET_ID =                4,

    /// The password for the device is not valid.
    QL_ERROR_INVALID_PASSWORD =                 5,

    /// The input path is invalid.
    QL_ERROR_INVALID_PATH =                     6,

    /// The input duration is invalid.
    QL_ERROR_INVALID_DURATION =                 7,

    /// The input pointer is NULL.
	QL_ERROR_INVALID_POINTER =                  8,

    /// The timeout time was reached.
    QL_ERROR_TIMEOUT_ELAPSED =                  9,

    /// An internal error occurred. Contact EyeTech for further help.
    QL_ERROR_INTERNAL_ERROR =                   10,

    /// The input buffer is is not large enough.
    QL_ERROR_BUFFER_TOO_SMALL =                 11,

    /// The calibration container has not been initialized. See QLCalibration_Initialize()
    QL_ERROR_CALIBRAION_NOT_INITIALIZED =       12,

    /// The device has not been started. See QLDevice_Start()
    QL_ERROR_DEVICE_NOT_STARTED =               13,

    /// The setting is not supported by the given device. See QLDevice_IsSettingSupported()
	QL_ERROR_NOT_SUPPORTED =			        14,

    /// The setting is not in the settings container.
	QL_ERROR_NOT_FOUND =    			        15,

	/// The API has detected that an unauthorized application is running on the computer. This occurs
	/// most often when a device is being used in a way that is prohibited by the original sales
	/// agreement of the device. For further information please contact EyeTech. 
	QL_ERROR_UNAUTHORIZED_APPLICATION_RUNNING = 16,

    /// The input device group ID is invalid.
    QL_ERROR_INVALID_DEVICE_GROUP_ID =          17
} QLError;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @enum QLEyeType
///
/// @brief Values that identify an eye.
///
/// @see QLCalibration_GetScoring().
/// @see QLCalibration_AddBias().
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef enum
{
    /// The left eye.
    QL_EYE_TYPE_LEFT =  0,

    /// The right eye.
    QL_EYE_TYPE_RIGHT = 1
} QLEyeType;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @enum   QLIndicatorMode
///
/// @brief  Values that identify different indicator modes.
///
/// @see QLDevice_SetIndicator().
/// @see QLDevice_GetIndicator().
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef enum
{
    /// The indicator will be off constantly.
    QL_INDICATOR_MODE_OFF =                       0,

    /// The indicator will be on constantly.
    QL_INDICATOR_MODE_ON =                        1,

    /// The indicator will show the current tracking status of the left eye.
    QL_INDICATOR_MODE_LEFT_EYE_STATUS =           2,

    /// The indicator will show the current tracking status of the right eye.
    QL_INDICATOR_MODE_RIGHT_EYE_STATUS =          3,

    /// The indicator will show the filtered current tracking status of the left eye. This will
    /// prevent the indicator from flickering if the eye was lost for only a couple of frames. 
    QL_INDICATOR_MODE_LEFT_EYE_STATUS_FILTERED =  4,

    /// The indicator will show the filtered current tracking status of the right eye. This will
    /// prevent the indicator from flickering if the eye was lost for only a couple of frames. 
    QL_INDICATOR_MODE_RIGHT_EYE_STATUS_FILTERED = 5
} QLIndicatorMode;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @enum QLIndicatorType
///
/// @brief Values that identify an indicator light.
///
/// @see QLDevice_SetIndicator().
/// @see QLDevice_GetIndicator().
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef enum
{
    /// The left indicator light.
    QL_INDICATOR_TYPE_LEFT =  0,

    /// The right indicator light.
    QL_INDICATOR_TYPE_RIGHT = 1
} QLIndicatorType;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @enum   QLSettingType
///
/// @brief  Values that identify what data type a pointer points to.
///
/// @see QLSettings_SetValue().
/// @see QLSettings_GetValue().
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef enum
{
    /// A c/c++ int type
	QL_SETTING_TYPE_INT =          0,

    /// An 8-bit wide integer type
	QL_SETTING_TYPE_INT8 =         1,

    /// A 16-bit wide integer type
	QL_SETTING_TYPE_INT16 =        2,

    /// A 32-bit wide integer type
	QL_SETTING_TYPE_INT32 =        3,

    /// A 64-bit wide integer type
	QL_SETTING_TYPE_INT64 =        4,

    /// A c/c++ unsigned int type
	QL_SETTING_TYPE_UINT =         5,

    /// An 8-bit wide unsigned integer type
	QL_SETTING_TYPE_UINT8 =        6,

    /// An 16-bit wide unsigned integer type
	QL_SETTING_TYPE_UINT16 =       7,

    /// An 32-bit wide unsigned integer type
	QL_SETTING_TYPE_UINT32 =       8,

    /// An 64-bit wide unsigned integer type
	QL_SETTING_TYPE_UINT64 =       9,

    /// A c/c++ float type
	QL_SETTING_TYPE_FLOAT =        10,

    /// A c/c++ double type
	QL_SETTING_TYPE_DOUBLE =       11,

    /// A c/c++ bool type
	QL_SETTING_TYPE_BOOL =         12,

    /// A c/c++ char* type
	QL_SETTING_TYPE_STRING =       13,

    /// A c/c++ void* type
	QL_SETTING_TYPE_VOID_POINTER = 14
} QLSettingType;


//------------------------------------------------------------------------------
// Structures
//------------------------------------------------------------------------------

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @struct   QLCalibrationTarget
///
/// @brief  Data that identifies a calibration target.
///
/// @see QLCalibration_GetTargets().
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef struct
{
    /// An identifying value used to reference the target. This is unique for each target in a given
    /// calibration container. 
    QLTargetId targetId;

    /// The x position of the target. This is in percentage of the horizontal area to be
    /// calibrated (0.-100.) 
    float x;
    
    /// The y position of the target. This is in percentage of the vertical area to be calibrated
    /// (0.- 100.) 
    float y;
} QLCalibrationTarget;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @struct   QLCalibrationScore
///
/// @brief  Values that identify the quality of a calibration at a target.
///
/// @see QLCalibration_GetScoring().
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef struct
{
    /// The x offset from the target position for the calibrated gaze point. This is in percentage
    /// of the horizontal area to be calibrated (0.-100.). Negative are to the left. Positive values
    /// are to the right. 
    float x;

    /// The y offset from the target position for the calibrated gaze point. This is in percentage
    /// of the vertical area to be calibrated (0.-100.). Negative are above. Positive values are
    /// below. 
    float y;

    /// The magnitude of the distance from the calibrated gaze point to the actual target position.
    /// This can be used to identify which target has the worst calibration. 
    float score;
} QLCalibrationScore;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @struct   QLDeviceInfo
///
/// @brief  Device specific information
///
/// @see QLDevice_GetInfo().
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef struct
{
    /// The actual sensor width in pixels.
	int sensorWidth;
	
    /// The actual sensor height in pixels.
    int sensorHeight;
	
    /// The unique serial number of the device.
    char serialNumber[STRING_LENGTH_128];

    /// The EyeTech model name of the camera.
	char modelName[STRING_LENGTH_128];
} QLDeviceInfo;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @struct   QLXYPairDouble
///
/// @brief  An x-y pair of type double.
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef struct
{
    double x;
    double y;
} QLXYPairDouble;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @struct   QLXYPairFloat
///
/// @brief  An x-y pair of type float.
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef struct
{
    float x;
    float y;
} QLXYPairFloat;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @struct QLRectInt
///
/// @brief Information describing a rectangle.
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef struct
{
    /// The x coordinate of the upper left corner of the rectangle.
    int x;

    /// The y coordinate of the upper left corner of the rectangle.
    int y;

    /// The width of the rectangle.
    int width;

    /// The height of the rectangle.
    int height;
} QLRectInt;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @struct   QLImageData
///
/// @brief  Information/data about an image 
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef struct
{
    /// The pixel data of the image. The data is single channel grey-scale with 8-bits per pixel.
    unsigned char* PixelData;

    /// The width in pixels of the image. This can be different than the sensorWidth of the device
    /// if binning is turned on. 
    int            Width;

    /// The height in pixels of the image. This can be different than the sensorHeight of the device
    /// if binning is turned on. 
    int            Height;

    /// The timestamp of the frame. It is the number of milliseconds from when the computer was
    /// started. 
    double         Timestamp;

    /// The gain value of the image.
    int            Gain;

    /// A number to identify a frame. Checking for non consecutive numbers from frame to frame can
    /// determine if a frame was lost. 
    long           FrameNumber;

    /// Information describing the location and size of the region of interest. If the device is not
    /// in region of interest mode then the x and y values will be zero and the width and height
    /// values will equal the width and height of the entire image. 
    QLRectInt      ROI;

    ///Image resolution scale factor
    float          ScaleFactor;

#if defined QUICK_LINK_64_BIT
    /// Reserved for future implementation
    void*          Reserved[13];
#elif defined QUICK_LINK_32_BIT
    /// Reserved for future implementation
    void*          Reserved[11];
#endif

} QLImageData;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @struct   QLEyeData
///
/// @brief  Eye specific data.
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef struct
{
    /// Indicates whether the eye is found in the image. If this is false then the rest of the data
    /// for the eye is not valid. 
	bool          Found;
	
    /// Indicates whether the eye is calibrated. If this is false then the GazePoint is not valid
    /// for the eye. 
    bool          Calibrated;
	
	/// The diameter of the pupil in millimeters. The resolution of this measurement is .02mm. If
	/// the eye is found and the pupil diameter is less than zero then the diameter could not be
	/// determined and it's value should not be used for that frame. 
    float         PupilDiameter;
    
    /// The position in pixels of the image of the center of the pupil.
    QLXYPairFloat Pupil;

    /// The position in pixels of the image of the center of one of the glints. Compare the x value
    /// to determine if this is the left of right glint. 
	QLXYPairFloat Glint0;

    /// The position in pixels of the image of the center of one of the glints. Compare the x value
    /// to determine if this is the left of right glint. 
    QLXYPairFloat Glint1;

    /// The position of the gaze point in percentage of the calibrated area. This value is not
    /// constrained to the calibrated area. 
	QLXYPairFloat GazePoint;

    /// Reserved for future implementation.
	void*         Reserved[16];
} QLEyeData;


////////////////////////////////////////////////////////////////////////////////////////////////////
/// @struct   QLWeightedGazePoint
///
/// @brief The averaged gaze point. These values intelligently average the left and right eye
/// based on which eyes were found and previous data depending on filtering settings.
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef struct
{
    /// Indicates whether the weighted gaze point is available this frame. If this is false then the
    /// rest of the data is invalid. 
	bool  Valid;

    /// The x position of the gaze point in percentage of the calibrated area.
    float x;

    /// The y position of the gaze point in percentage of the calibrated area.
    float y;

    /// The amount the left eye affected the weighted gaze point.
    float LeftWeight;

    /// The amount the right eye affected the weighted gaze point.
    float RightWeight;

    /// Reserved for future implementation.
	void*               Reserved[16];
} QLWeightedGazePoint;

////////////////////////////////////////////////////////////////////////////////////////////////////
/// @struct   QLFrameData
///
/// @brief The complete data available in each frame.
/// 	   
/// @see QLDevice_GetFrame().
////////////////////////////////////////////////////////////////////////////////////////////////////
typedef struct
{
    /// The image specific data.
	QLImageData         ImageData;

    /// Left eye specific data.
	QLEyeData           LeftEye;

    /// Right eye specific data.
	QLEyeData           RightEye;

    /// An intelligently averaged single gaze point that is the best representation of the user's
    /// gaze. 
    QLWeightedGazePoint WeightedGazePoint;

    /// The focus measurement of the eyes in the image. Higher values are better. Typical good
    /// values range between 15 and 19. 
    float               Focus;

    /// The distance from the device to the user in centimeters.
    float               Distance;

    /// The current bandwidth of the device.
    int                 Bandwidth;

    /// The ID of the device that is the source for the current frame.
    QLDeviceId          DeviceId;

#if defined QUICK_LINK_64_BIT
    /// Reserved for future implementation
    void*          Reserved[15];
#elif defined QUICK_LINK_32_BIT
    /// Reserved for future implementation
    void*          Reserved[14];
#endif

} QLFrameData;

#ifdef __cplusplus
}
#endif

#endif
