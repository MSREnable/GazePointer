////////////////////////////////////////////////////////////////////////////////////////////////////
/// @file   QuickLink2_ptr_C.h
///
/// @brief This file contains definitions of function pointers for all
/// functions available through QuickLink2.dll. This is useful for explicit
/// linking.
/// 
/// @copyright 1996 - 2015, EyeTech Digital Systems
////////////////////////////////////////////////////////////////////////////////////////////////////
#ifndef QUICK_LINK_2_PTR_C_H
#define	QUICK_LINK_2_PTR_C_H

#include "QLTypes.h"

#ifdef __cplusplus
extern "C"
{
#endif

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLAPI_GetVersion_Ptr)(
    int bufferSize, 
    char* buffer);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLAPI_ExportSettings_Ptr)(
    QLSettingsId settings);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLAPI_ImportSettings_Ptr)(
    QLSettingsId settings);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLAPI_ImportLastSettings_Ptr)();

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDevice_Enumerate_Ptr)(
    int* numDevices, 
    QLDeviceId* deviceBuffer);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDevice_GetInfo_Ptr)(
    QLDeviceId device, 
    QLDeviceInfo* info);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDevice_GetStatus_Ptr)(
    QLDeviceId device, 
    QLDeviceStatus* status);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDevice_ExportSettings_Ptr)(
    QLDeviceId device, 
    QLSettingsId settings);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDevice_ImportSettings_Ptr)(
    QLDeviceOrGroupId deviceOrGroup, 
    QLSettingsId settings);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDevice_ImportLastSettings_Ptr)(
    QLDeviceOrGroupId deviceOrGroup);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDevice_IsSettingSupported_Ptr)(
    QLDeviceId device, 
    const char* settingName);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDevice_SetPassword_Ptr)(
    QLDeviceId device, 
    const char* password);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDevice_Start_Ptr)(
    QLDeviceOrGroupId deviceOrGroup);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDevice_Stop_Ptr)(
    QLDeviceOrGroupId deviceOrGroup);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDevice_Stop_All_Ptr)();

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDevice_SetIndicator_Ptr)(
    QLDeviceOrGroupId deviceOrGroup, 
    QLIndicatorType type, 
    QLIndicatorMode mode);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDevice_GetIndicator_Ptr)(
    QLDeviceId device, 
    QLIndicatorType type, 
    QLIndicatorMode* mode);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDevice_GetFrame_Ptr)(
    QLDeviceOrGroupId deviceOrGroup,
	int waitTime,
    QLFrameData* frame);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDevice_ApplyCalibration_Ptr)(
    QLDeviceId device, 
    QLCalibrationId calibration);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDevice_ApplyLastCalibration_Ptr)(
    QLDeviceId device);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDevice_CalibrateEyeRadius_Ptr)(
		QLDeviceId device,
		float distance,
		float* leftRadius,
		float* rightRadius);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDeviceGroup_Create_Ptr)(
		QLDeviceGroupId *deviceGroupId);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDeviceGroup_AddDevice_Ptr)(
        QLDeviceGroupId deviceGroup, 
        QLDeviceId deviceId);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDeviceGroup_RemoveDevice_Ptr)(
        QLDeviceGroupId deviceGroup, 
        QLDeviceId deviceId);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDeviceGroup_Enumerate_Ptr)(
        QLDeviceGroupId deviceGroup, 
        int *numDevices,
        QLDeviceId *deviceIdBuffer);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLDeviceGroup_GetFrame_Ptr)(
        QLDeviceGroupId deviceGroup,
        int waitTime, 
        int* numFrames,
        QLFrameData* frame);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_Load_Ptr)(
    const char* path, 
    QLSettingsId* settings);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_Save_Ptr)(
    const char* path, 
    QLSettingsId settings);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_Create_Ptr)(
    QLSettingsId source, 
    QLSettingsId* settings);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_AddSetting_Ptr)(
    QLSettingsId settings,
    const char* settingName);
	
typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_RemoveSetting_Ptr)(
    QLSettingsId settings,
    const char* settingName);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_SetValue_Ptr)(
    QLSettingsId settings,
    const char* settingName,
    QLSettingType settingType,
    const void* value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_SetValueInt_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    int value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_SetValueInt8_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    __int8 value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_SetValueInt16_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    __int16 value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_SetValueInt32_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    __int32 value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_SetValueInt64_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    __int64 value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_SetValueUInt_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    unsigned int value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_SetValueUInt8_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    unsigned __int8 value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_SetValueUInt16_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    unsigned __int16 value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_SetValueUInt32_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    unsigned __int32 value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_SetValueUInt64_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    unsigned __int64 value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_SetValueFloat_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    float value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_SetValueDouble_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    double value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_SetValueBool_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    bool value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_SetValueVoidPointer_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    void* value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_SetValueString_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    char * value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_GetValue_Ptr)(
    QLSettingsId settings,
    const char* settingName,
    QLSettingType settingType,
    int size,
    void* value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_GetValueInt_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    int* value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_GetValueInt8_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    __int8* value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_GetValueInt16_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    __int16* value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_GetValueInt32_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    __int32* value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_GetValueInt64_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    __int64* value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_GetValueUInt_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    unsigned int* value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_GetValueUInt8_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    unsigned __int8* value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_GetValueUInt16_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    unsigned __int16* value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_GetValueUInt32_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    unsigned __int32* value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_GetValueUInt64_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    unsigned __int64* value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_GetValueFloat_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    float* value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_GetValueDouble_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    double* value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_GetValueBool_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    bool* value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_GetValueVoidPointer_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    void** value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_GetValueString_Ptr)(
    QLSettingsId settings, 
    const char* settingName, 
    int size, 
    char* value);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLSettings_GetValueStringSize_Ptr)(
    QLSettingsId settings,
    const char* settingName,
    int* size);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLCalibration_Load_Ptr)(
    const char* path, 
    QLCalibrationId* calibration);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLCalibration_Save_Ptr)(
    const char* path, 
    QLCalibrationId calibration);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLCalibration_Create_Ptr)(
    QLCalibrationId source,
	QLCalibrationId* calibration);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLCalibration_Initialize_Ptr)(
    QLDeviceId device, 
    QLCalibrationId calibration, 
    QLCalibrationType type);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLCalibration_GetTargets_Ptr)(
    QLCalibrationId calibration, 
    int* numTargets,
    QLCalibrationTarget* targets);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLCalibration_Calibrate_Ptr)(
    QLCalibrationId calibration, 
    QLTargetId target, 
    int duration, 
    bool block);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLCalibration_GetScoring_Ptr)(
    QLCalibrationId calibration, 
	QLTargetId target, 
    QLEyeType eye, 
    QLCalibrationScore* score);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLCalibration_GetStatus_Ptr)(
    QLCalibrationId calibration,
    QLTargetId target,
    QLCalibrationStatus* calibrationStatus);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLCalibration_Finalize_Ptr)(
    QLCalibrationId calibration);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLCalibration_Cancel_Ptr)(
    QLCalibrationId calibration);

typedef QLError (QUICK_LINK_2_CALL_CONVEN* QLCalibration_AddBias_Ptr)(
    QLCalibrationId calibration, 
    QLEyeType eye, 
    float xOffset, 
    float yOffset);

#ifdef __cplusplus
}
#endif

#endif
