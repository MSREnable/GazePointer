////////////////////////////////////////////////////////////////////////////////////////////////////
/// project:  QuickStart
/// file:	  QL2Utility.cpp
///
/// Copyright (c) 1996 - 2012 EyeTech Digital Systems. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////
#include "QL2Utility.h"
#include <vector>
#include <tchar.h>

// The global handle to the Quick Link 2 dll. 
HMODULE	g_ql2Utility_ql2Module = 0;

// If eplicit linking is defined then define function pointers for each function in the API. 
#ifdef EXPLICIT_LINKING

#include <QuickLink2_ptr_C.h>

QLAPI_GetVersion_Ptr				pQLAPI_GetVersion = 0;
QLAPI_ExportSettings_Ptr			pQLAPI_ExportSettings = 0;
QLAPI_ImportSettings_Ptr			pQLAPI_ImportSettings = 0;
QLDevice_Enumerate_Ptr				pQLDevice_Enumerate = 0;
QLDevice_GetInfo_Ptr				pQLDevice_GetInfo = 0;
QLDevice_GetStatus_Ptr				pQLDevice_GetStatus = 0;
QLDevice_ExportSettings_Ptr			pQLDevice_ExportSettings = 0;
QLDevice_ImportSettings_Ptr			pQLDevice_ImportSettings = 0;
QLDevice_IsSettingSupported_Ptr		pQLDevice_IsSettingSupported = 0;
QLDevice_SetPassword_Ptr			pQLDevice_SetPassword = 0;
QLDevice_Start_Ptr					pQLDevice_Start = 0;
QLDevice_Stop_Ptr					pQLDevice_Stop = 0;
QLDevice_Stop_All_Ptr				pQLDevice_Stop_All = 0;
QLDevice_SetIndicator_Ptr			pQLDevice_SetIndicator = 0;
QLDevice_GetIndicator_Ptr			pQLDevice_GetIndicator = 0;
QLDevice_GetFrame_Ptr				pQLDevice_GetFrame = 0;
QLDevice_ApplyCalibration_Ptr		pQLDevice_ApplyCalibration = 0;
QLDevice_CalibrateEyeRadius_Ptr		pQLDevice_CalibrateEyeRadius = 0;
QLDeviceGroup_Create_Ptr            pQLDeviceGroup_Create = 0;
QLDeviceGroup_AddDevice_Ptr         pQLDeviceGroup_AddDevice = 0;
QLDeviceGroup_RemoveDevice_Ptr      pQLDeviceGroup_RemoveDevice = 0;
QLDeviceGroup_Enumerate_Ptr         pQLDeviceGroup_Enumerate = 0;
QLDeviceGroup_GetFrame_Ptr          pQLDeviceGroup_GetFrame = 0;
QLSettings_Load_Ptr					pQLSettings_Load = 0;
QLSettings_Save_Ptr					pQLSettings_Save = 0;
QLSettings_Create_Ptr				pQLSettings_Create = 0;
QLSettings_AddSetting_Ptr			pQLSettings_AddSetting = 0;
QLSettings_RemoveSetting_Ptr		pQLSettings_RemoveSetting = 0;
QLSettings_SetValue_Ptr				pQLSettings_SetValue = 0;
QLSettings_SetValueInt_Ptr			pQLSettings_SetValueInt = 0;
QLSettings_SetValueInt8_Ptr			pQLSettings_SetValueInt8 = 0;
QLSettings_SetValueInt16_Ptr		pQLSettings_SetValueInt16 = 0;
QLSettings_SetValueInt32_Ptr		pQLSettings_SetValueInt32 = 0;
QLSettings_SetValueInt64_Ptr		pQLSettings_SetValueInt64 = 0;
QLSettings_SetValueUInt_Ptr			pQLSettings_SetValueUInt = 0;
QLSettings_SetValueUInt8_Ptr		pQLSettings_SetValueUInt8 = 0;
QLSettings_SetValueUInt16_Ptr		pQLSettings_SetValueUInt16 = 0;
QLSettings_SetValueUInt32_Ptr		pQLSettings_SetValueUInt32 = 0;
QLSettings_SetValueUInt64_Ptr		pQLSettings_SetValueUInt64 = 0;
QLSettings_SetValueFloat_Ptr		pQLSettings_SetValueFloat = 0;
QLSettings_SetValueDouble_Ptr		pQLSettings_SetValueDouble = 0;
QLSettings_SetValueBool_Ptr			pQLSettings_SetValueBool = 0;
QLSettings_SetValueVoidPointer_Ptr	pQLSettings_SetValueVoidPointer = 0;
QLSettings_SetValueString_Ptr		pQLSettings_SetValueString = 0;
QLSettings_GetValue_Ptr				pQLSettings_GetValue = 0;
QLSettings_GetValueInt_Ptr			pQLSettings_GetValueInt = 0;
QLSettings_GetValueInt8_Ptr			pQLSettings_GetValueInt8 = 0;
QLSettings_GetValueInt16_Ptr		pQLSettings_GetValueInt16 = 0;
QLSettings_GetValueInt32_Ptr		pQLSettings_GetValueInt32 = 0;
QLSettings_GetValueInt64_Ptr		pQLSettings_GetValueInt64 = 0;
QLSettings_GetValueUInt_Ptr			pQLSettings_GetValueUInt = 0;
QLSettings_GetValueUInt8_Ptr		pQLSettings_GetValueUInt8 = 0;
QLSettings_GetValueUInt16_Ptr		pQLSettings_GetValueUInt16 = 0;
QLSettings_GetValueUInt32_Ptr		pQLSettings_GetValueUInt32 = 0;
QLSettings_GetValueUInt64_Ptr		pQLSettings_GetValueUInt64 = 0;
QLSettings_GetValueFloat_Ptr		pQLSettings_GetValueFloat = 0;
QLSettings_GetValueDouble_Ptr		pQLSettings_GetValueDouble = 0;
QLSettings_GetValueBool_Ptr			pQLSettings_GetValueBool = 0;
QLSettings_GetValueVoidPointer_Ptr	pQLSettings_GetValueVoidPointer = 0;
QLSettings_GetValueString_Ptr		pQLSettings_GetValueString = 0;
QLSettings_GetValueStringSize_Ptr	pQLSettings_GetValueStringSize = 0;
QLCalibration_Load_Ptr				pQLCalibration_Load = 0;
QLCalibration_Save_Ptr				pQLCalibration_Save = 0;
QLCalibration_Create_Ptr			pQLCalibration_Create = 0;
QLCalibration_Initialize_Ptr		pQLCalibration_Initialize = 0;
QLCalibration_GetTargets_Ptr		pQLCalibration_GetTargets = 0;
QLCalibration_Calibrate_Ptr			pQLCalibration_Calibrate = 0;
QLCalibration_GetScoring_Ptr		pQLCalibration_GetScoring = 0;
QLCalibration_GetStatus_Ptr			pQLCalibration_GetStatus = 0;
QLCalibration_Finalize_Ptr			pQLCalibration_Finalize = 0;
QLCalibration_Cancel_Ptr			pQLCalibration_Cancel = 0;
QLCalibration_AddBias_Ptr			pQLCalibration_AddBias = 0;

#endif

QL2Utility::QL2Utility()
{
}

QL2Utility::~QL2Utility()
{
    // Unload the library
	Unload();
}


bool QL2Utility::Load(const TCHAR* libraryPath)
{
#ifdef EXPLICIT_LINKING

	// Verify that the library is unloaded first if it is already loaded.
	Unload();

    // Load the library. If the input path is not zero then try to load it. otherwise just load the
    // library by name only. If something fails then return false. 
    if(((libraryPath != 0) && (0 == (g_ql2Utility_ql2Module = LoadLibrary(libraryPath)))) ||         
        (0 == (g_ql2Utility_ql2Module = LoadLibrary(_T("QuickLink2.dll")))))
		return false;

	std::vector<void*> functionPointers;

	// Load each function and push its address to the vector.
    functionPointers.push_back(pQLDevice_Enumerate = (QLDevice_Enumerate_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDevice_Enumerate"));
	functionPointers.push_back(pQLAPI_GetVersion = (QLAPI_GetVersion_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLAPI_GetVersion"));
	functionPointers.push_back(pQLAPI_ExportSettings = (QLAPI_ExportSettings_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLAPI_ExportSettings"));
	functionPointers.push_back(pQLAPI_ImportSettings = (QLAPI_ImportSettings_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLAPI_ImportSettings"));
	functionPointers.push_back(pQLDevice_Enumerate = (QLDevice_Enumerate_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDevice_Enumerate"));
	functionPointers.push_back(pQLDevice_GetInfo = (QLDevice_GetInfo_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDevice_GetInfo"));
	functionPointers.push_back(pQLDevice_GetStatus = (QLDevice_GetStatus_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDevice_GetStatus"));
	functionPointers.push_back(pQLDevice_ExportSettings = (QLDevice_ExportSettings_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDevice_ExportSettings"));
	functionPointers.push_back(pQLDevice_ImportSettings = (QLDevice_ImportSettings_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDevice_ImportSettings"));
	functionPointers.push_back(pQLDevice_IsSettingSupported = (QLDevice_IsSettingSupported_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDevice_IsSettingSupported"));
	functionPointers.push_back(pQLDevice_SetPassword = (QLDevice_SetPassword_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDevice_SetPassword"));
	functionPointers.push_back(pQLDevice_Start = (QLDevice_Start_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDevice_Start"));
	functionPointers.push_back(pQLDevice_Stop = (QLDevice_Stop_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDevice_Stop"));
	functionPointers.push_back(pQLDevice_Stop_All = (QLDevice_Stop_All_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDevice_Stop_All"));
	functionPointers.push_back(pQLDevice_SetIndicator = (QLDevice_SetIndicator_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDevice_SetIndicator"));
	functionPointers.push_back(pQLDevice_GetIndicator = (QLDevice_GetIndicator_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDevice_GetIndicator"));
	functionPointers.push_back(pQLDevice_GetFrame = (QLDevice_GetFrame_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDevice_GetFrame"));
	functionPointers.push_back(pQLDevice_ApplyCalibration = (QLDevice_ApplyCalibration_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDevice_ApplyCalibration"));
	functionPointers.push_back(pQLDevice_CalibrateEyeRadius = (QLDevice_CalibrateEyeRadius_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDevice_CalibrateEyeRadius"));
	functionPointers.push_back(pQLDeviceGroup_Create = (QLDeviceGroup_Create_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDeviceGroup_Create"));
	functionPointers.push_back(pQLDeviceGroup_AddDevice = (QLDeviceGroup_AddDevice_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDeviceGroup_AddDevice"));
	functionPointers.push_back(pQLDeviceGroup_RemoveDevice = (QLDeviceGroup_RemoveDevice_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDeviceGroup_RemoveDevice"));
	functionPointers.push_back(pQLDeviceGroup_Enumerate = (QLDeviceGroup_Enumerate_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDeviceGroup_Enumerate"));
	functionPointers.push_back(pQLDeviceGroup_GetFrame = (QLDeviceGroup_GetFrame_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLDeviceGroup_GetFrame"));
    functionPointers.push_back(pQLSettings_Load = (QLSettings_Load_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_Load"));
	functionPointers.push_back(pQLSettings_Save = (QLSettings_Save_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_Save"));
	functionPointers.push_back(pQLSettings_Create = (QLSettings_Create_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_Create"));
	functionPointers.push_back(pQLSettings_AddSetting = (QLSettings_AddSetting_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_AddSetting"));
	functionPointers.push_back(pQLSettings_RemoveSetting = (QLSettings_RemoveSetting_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_RemoveSetting"));
	functionPointers.push_back(pQLSettings_SetValue = (QLSettings_SetValue_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_SetValue"));
	functionPointers.push_back(pQLSettings_SetValueInt = (QLSettings_SetValueInt_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_SetValueInt"));
	functionPointers.push_back(pQLSettings_SetValueInt8 = (QLSettings_SetValueInt8_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_SetValueInt8"));
	functionPointers.push_back(pQLSettings_SetValueInt16 = (QLSettings_SetValueInt16_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_SetValueInt16"));
	functionPointers.push_back(pQLSettings_SetValueInt32 = (QLSettings_SetValueInt32_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_SetValueInt32"));
	functionPointers.push_back(pQLSettings_SetValueInt64 = (QLSettings_SetValueInt64_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_SetValueInt64"));
	functionPointers.push_back(pQLSettings_SetValueUInt = (QLSettings_SetValueUInt_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_SetValueUInt"));
	functionPointers.push_back(pQLSettings_SetValueUInt8 = (QLSettings_SetValueUInt8_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_SetValueUInt8"));
	functionPointers.push_back(pQLSettings_SetValueUInt16 = (QLSettings_SetValueUInt16_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_SetValueUInt16"));
	functionPointers.push_back(pQLSettings_SetValueUInt32 = (QLSettings_SetValueUInt32_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_SetValueUInt32"));
	functionPointers.push_back(pQLSettings_SetValueUInt64 = (QLSettings_SetValueUInt64_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_SetValueUInt64"));
	functionPointers.push_back(pQLSettings_SetValueFloat = (QLSettings_SetValueFloat_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_SetValueFloat"));
	functionPointers.push_back(pQLSettings_SetValueDouble = (QLSettings_SetValueDouble_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_SetValueDouble"));
	functionPointers.push_back(pQLSettings_SetValueBool = (QLSettings_SetValueBool_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_SetValueBool"));
	functionPointers.push_back(pQLSettings_SetValueVoidPointer = (QLSettings_SetValueVoidPointer_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_SetValueVoidPointer"));
	functionPointers.push_back(pQLSettings_SetValueString = (QLSettings_SetValueString_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_SetValueString"));
	functionPointers.push_back(pQLSettings_GetValue = (QLSettings_GetValue_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_GetValue"));
	functionPointers.push_back(pQLSettings_GetValueInt = (QLSettings_GetValueInt_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_GetValueInt"));
	functionPointers.push_back(pQLSettings_GetValueInt8 = (QLSettings_GetValueInt8_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_GetValueInt8"));
	functionPointers.push_back(pQLSettings_GetValueInt16 = (QLSettings_GetValueInt16_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_GetValueInt16"));
	functionPointers.push_back(pQLSettings_GetValueInt32 = (QLSettings_GetValueInt32_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_GetValueInt32"));
	functionPointers.push_back(pQLSettings_GetValueInt64 = (QLSettings_GetValueInt64_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_GetValueInt64"));
	functionPointers.push_back(pQLSettings_GetValueUInt = (QLSettings_GetValueUInt_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_GetValueUInt"));
	functionPointers.push_back(pQLSettings_GetValueUInt8 = (QLSettings_GetValueUInt8_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_GetValueUInt8"));
	functionPointers.push_back(pQLSettings_GetValueUInt16 = (QLSettings_GetValueUInt16_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_GetValueUInt16"));
	functionPointers.push_back(pQLSettings_GetValueUInt32 = (QLSettings_GetValueUInt32_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_GetValueUInt32"));
	functionPointers.push_back(pQLSettings_GetValueUInt64 = (QLSettings_GetValueUInt64_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_GetValueUInt64"));
	functionPointers.push_back(pQLSettings_GetValueFloat = (QLSettings_GetValueFloat_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_GetValueFloat"));
	functionPointers.push_back(pQLSettings_GetValueDouble = (QLSettings_GetValueDouble_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_GetValueDouble"));
	functionPointers.push_back(pQLSettings_GetValueBool = (QLSettings_GetValueBool_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_GetValueBool"));
	functionPointers.push_back(pQLSettings_GetValueVoidPointer = (QLSettings_GetValueVoidPointer_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_GetValueVoidPointer"));
	functionPointers.push_back(pQLSettings_GetValueString = (QLSettings_GetValueString_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_GetValueString"));
	functionPointers.push_back(pQLSettings_GetValueStringSize = (QLSettings_GetValueStringSize_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLSettings_GetValueStringSize"));
	functionPointers.push_back(pQLCalibration_Load = (QLCalibration_Load_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLCalibration_Load"));
	functionPointers.push_back(pQLCalibration_Save = (QLCalibration_Save_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLCalibration_Save"));
	functionPointers.push_back(pQLCalibration_Create = (QLCalibration_Create_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLCalibration_Create"));
	functionPointers.push_back(pQLCalibration_Initialize = (QLCalibration_Initialize_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLCalibration_Initialize"));
	functionPointers.push_back(pQLCalibration_GetTargets = (QLCalibration_GetTargets_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLCalibration_GetTargets"));
	functionPointers.push_back(pQLCalibration_Calibrate = (QLCalibration_Calibrate_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLCalibration_Calibrate"));
	functionPointers.push_back(pQLCalibration_GetScoring = (QLCalibration_GetScoring_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLCalibration_GetScoring"));
	functionPointers.push_back(pQLCalibration_GetStatus = (QLCalibration_GetStatus_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLCalibration_GetStatus"));
	functionPointers.push_back(pQLCalibration_Finalize = (QLCalibration_Finalize_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLCalibration_Finalize"));
	functionPointers.push_back(pQLCalibration_Cancel = (QLCalibration_Cancel_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLCalibration_Cancel"));
	functionPointers.push_back(pQLCalibration_AddBias = (QLCalibration_AddBias_Ptr)GetProcAddress(g_ql2Utility_ql2Module, "QLCalibration_AddBias"));

	// If any of the functions failed to load then unload the library and return false.
	for(size_t i = 0; i < functionPointers.size(); i++)
	{
		if(functionPointers[i] == 0)
		{
			Unload();
			return false;
		}
	}

#endif

	// Return true indicating that the library loaded properly.
    return true;
}

void QL2Utility::Unload()
{
    // As a safety measure, stop all devices that may have been started.
    QLDevice_Stop_All();

#ifdef EXPLICIT_LINKING

    // If the library is loaded then unload it and set the module handle to zero.
    if(g_ql2Utility_ql2Module != 0)
    {
        FreeLibrary(g_ql2Utility_ql2Module);
        g_ql2Utility_ql2Module = 0;
    }
#endif
}

bool QL2Utility::IsLoaded()
{
#ifdef EXPLICIT_LINKING
	// If the library module handle is zero then the library is not loaded.
	if(g_ql2Utility_ql2Module == 0)
		return false;
#endif

	// return true indicating that the library is loaded.
	return true;
}

// If explicit linking is defined then create functions that have the same call signature as the
// QuickLink2 functions and have them call the explicitly loaded function pointers. This will allow
// a program to seamlessly switch between explicit and implicit linking. The only difference is
// that if a program links explicitly and tries to call an API function before the library is
// loaded, the function will return QL_ERROR_INTERNAL_ERROR. 
#ifdef EXPLICIT_LINKING

QLError QUICK_LINK_2_CALL_CONVEN 
    QLAPI_GetVersion(
        int bufferSize, 
        char* buffer)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLAPI_GetVersion == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLAPI_GetVersion(
            bufferSize, 
            buffer);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLAPI_ExportSettings(
        QLSettingsId settings)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLAPI_ExportSettings == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLAPI_ExportSettings(
            settings);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLAPI_ImportSettings(
        QLSettingsId settings)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLAPI_ImportSettings == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLAPI_ImportSettings(
            settings);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_Enumerate(
        int* numDevices, 
        QLDeviceId* deviceBuffer)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDevice_Enumerate == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDevice_Enumerate(
            numDevices, 
            deviceBuffer);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_GetInfo(
        QLDeviceId device, 
        QLDeviceInfo* info)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDevice_GetInfo == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDevice_GetInfo(
            device, 
            info);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_GetStatus(
        QLDeviceId device, 
        QLDeviceStatus* status)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDevice_GetStatus == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDevice_GetStatus(
            device, 
            status);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_ExportSettings(
        QLDeviceId device, 
        QLSettingsId settings)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDevice_ExportSettings == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDevice_ExportSettings(
            device, 
            settings);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_ImportSettings(
        QLDeviceOrGroupId deviceOrGroup, 
        QLSettingsId settings)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDevice_ImportSettings == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDevice_ImportSettings(
            deviceOrGroup, 
            settings);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_IsSettingSupported(
        QLDeviceId device, 
        const char* settingName)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDevice_IsSettingSupported == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDevice_IsSettingSupported(
            device, 
            settingName);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_SetPassword(
        QLDeviceId device, 
        const char* password)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDevice_SetPassword == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDevice_SetPassword(
            device, 
            password);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_Start(
        QLDeviceOrGroupId deviceOrGroup)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDevice_Start == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDevice_Start(
            deviceOrGroup);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_Stop(
        QLDeviceOrGroupId deviceOrGroup)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDevice_Stop == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDevice_Stop(
            deviceOrGroup);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_Stop_All()
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDevice_Stop_All == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDevice_Stop_All();
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_SetIndicator(
        QLDeviceOrGroupId deviceOrGroup, 
        QLIndicatorType type, 
        QLIndicatorMode mode)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDevice_SetIndicator == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDevice_SetIndicator(
            deviceOrGroup, 
            type, 
            mode);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_GetIndicator(
        QLDeviceId device, 
        QLIndicatorType type, 
        QLIndicatorMode* mode)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDevice_GetIndicator == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDevice_GetIndicator(
            device, 
            type, 
            mode);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_GetFrame(
        QLDeviceOrGroupId deviceOrGroup,
        int waitTime, 
        QLFrameData* frame)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDevice_GetFrame == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDevice_GetFrame(
            deviceOrGroup,
            waitTime, 
            frame);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_ApplyCalibration(
        QLDeviceId device, 
        QLCalibrationId calibration)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDevice_ApplyCalibration == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDevice_ApplyCalibration(
            device, 
            calibration);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDevice_CalibrateEyeRadius(
		QLDeviceId device,
		float distance,
		float* leftRadius,
		float* rightRadius)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDevice_CalibrateEyeRadius == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDevice_CalibrateEyeRadius(
		    device,
		    distance,
		    leftRadius,
		    rightRadius);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDeviceGroup_Create(
        QLDeviceGroupId *deviceGroupId)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDeviceGroup_Create == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDeviceGroup_Create(
            deviceGroupId);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDeviceGroup_AddDevice(
        QLDeviceGroupId deviceGroup, 
        QLDeviceId deviceId)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDeviceGroup_AddDevice == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDeviceGroup_AddDevice(
            deviceGroup, 
            deviceId);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDeviceGroup_RemoveDevice(
        QLDeviceGroupId deviceGroup, 
        QLDeviceId deviceId)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDeviceGroup_RemoveDevice == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDeviceGroup_RemoveDevice(
            deviceGroup, 
            deviceId);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDeviceGroup_Enumerate(
        QLDeviceGroupId deviceGroup, 
        int *numDevices,
        QLDeviceId *deviceIdBuffer)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDeviceGroup_Enumerate == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDeviceGroup_Enumerate(
            deviceGroup, 
            numDevices,
            deviceIdBuffer);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLDeviceGroup_GetFrame(
        QLDeviceGroupId deviceGroup,
        int waitTime, 
        int *numFrames,
        QLFrameData* frames)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLDeviceGroup_GetFrame == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLDeviceGroup_GetFrame(
            deviceGroup,
            waitTime, 
            numFrames,
            frames);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_Load(
        const char* path, 
        QLSettingsId* settings)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_Load == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_Load(
            path, 
            settings);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_Save(
        const char* path, 
        QLSettingsId settings)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_Save == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_Save(
            path, 
            settings);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_Create(
        QLSettingsId source,
        QLSettingsId* settings)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_Create == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_Create(
            source,
            settings);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_AddSetting(
        QLSettingsId settings,
        const char* settingName)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_AddSetting == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_AddSetting(
            settings,
            settingName);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_RemoveSetting(
        QLSettingsId settings,
        const char* settingName)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_RemoveSetting == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_RemoveSetting(
            settings,
            settingName);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_SetValue(
        QLSettingsId settings,
        const char* settingName,
        QLSettingType settingType,
        const void * value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_SetValue == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_SetValue(
            settings,
            settingName,
            settingType,
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_SetValueInt(
        QLSettingsId settings, 
        const char* settingName, 
        int value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_SetValueInt == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_SetValueInt(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_SetValueInt8(
        QLSettingsId settings, 
        const char* settingName, 
        __int8 value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_SetValueInt8 == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_SetValueInt8(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_SetValueInt16(
        QLSettingsId settings, 
        const char* settingName, 
        __int16 value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_SetValueInt16 == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_SetValueInt16(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_SetValueInt32(
        QLSettingsId settings, 
        const char* settingName, 
        __int32 value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_SetValueInt32 == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_SetValueInt32(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_SetValueInt64(
        QLSettingsId settings, 
        const char* settingName, 
        __int64 value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_SetValueInt64 == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_SetValueInt64(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_SetValueUInt(
        QLSettingsId settings, 
        const char* settingName, 
        unsigned int value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_SetValueUInt == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_SetValueUInt(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_SetValueUInt8(
        QLSettingsId settings, 
        const char* settingName, 
        unsigned __int8 value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_SetValueUInt8 == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_SetValueUInt8(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_SetValueUInt16(
        QLSettingsId settings, 
        const char* settingName, 
        unsigned __int16 value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_SetValueUInt16 == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_SetValueUInt16(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_SetValueUInt32(
        QLSettingsId settings, 
        const char* settingName, 
        unsigned __int32 value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_SetValueUInt32 == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_SetValueUInt32(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_SetValueUInt64(
        QLSettingsId settings, 
        const char* settingName, 
        unsigned __int64 value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_SetValueUInt64 == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_SetValueUInt64(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_SetValueFloat(
        QLSettingsId settings, 
        const char* settingName, 
        float value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_SetValueFloat == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_SetValueFloat(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_SetValueDouble(
        QLSettingsId settings, 
        const char* settingName, 
        double value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_SetValueDouble == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_SetValueDouble(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_SetValueBool(
        QLSettingsId settings, 
        const char* settingName, 
        bool value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_SetValueBool == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_SetValueBool(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_SetValueVoidPointer(
        QLSettingsId settings, 
        const char* settingName, 
        void* value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_SetValueVoidPointer == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_SetValueVoidPointer(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_SetValueString(
        QLSettingsId settings, 
        const char* settingName, 
        char * value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_SetValueString == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_SetValueString(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValue(
        QLSettingsId settings,
        const char* settingName,
        QLSettingType settingType,
        int size,
        void * value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_GetValue == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_GetValue(
            settings,
            settingName,
            settingType,
            size,
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValueInt(
        QLSettingsId settings, 
        const char* settingName, 
        int* value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_GetValueInt == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_GetValueInt(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValueInt8(
        QLSettingsId settings, 
        const char* settingName, 
        __int8* value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_GetValueInt8 == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_GetValueInt8(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValueInt16(
        QLSettingsId settings, 
        const char* settingName, 
        __int16* value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_GetValueInt16 == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_GetValueInt16(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValueInt32(
        QLSettingsId settings, 
        const char* settingName, 
        __int32* value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_GetValueInt32 == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_GetValueInt32(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValueInt64(
        QLSettingsId settings, 
        const char* settingName, 
        __int64* value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_GetValueInt64 == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_GetValueInt64(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValueUInt(
        QLSettingsId settings, 
        const char* settingName, 
        unsigned int* value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_GetValueUInt == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_GetValueUInt(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValueUInt8(
        QLSettingsId settings, 
        const char* settingName, 
        unsigned __int8* value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_GetValueUInt8 == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_GetValueUInt8(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValueUInt16(
        QLSettingsId settings, 
        const char* settingName, 
        unsigned __int16* value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_GetValueUInt16 == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_GetValueUInt16(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValueUInt32(
        QLSettingsId settings, 
        const char* settingName, 
        unsigned __int32* value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_GetValueUInt32 == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_GetValueUInt32(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValueUInt64(
        QLSettingsId settings, 
        const char* settingName, 
        unsigned __int64* value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_GetValueUInt64 == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_GetValueUInt64(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValueFloat(
        QLSettingsId settings, 
        const char* settingName, 
        float* value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_GetValueFloat == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_GetValueFloat(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValueDouble(
        QLSettingsId settings, 
        const char* settingName, 
        double* value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_GetValueDouble == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_GetValueDouble(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValueBool(
        QLSettingsId settings, 
        const char* settingName, 
        bool* value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_GetValueBool == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_GetValueBool(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValueVoidPointer(
        QLSettingsId settings, 
        const char* settingName, 
        void** value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_GetValueVoidPointer == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_GetValueVoidPointer(
            settings, 
            settingName, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValueString(
        QLSettingsId settings, 
        const char* settingName, 
        int size, 
        char* value)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_GetValueString == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_GetValueString(
            settings, 
            settingName, 
            size, 
            value);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLSettings_GetValueStringSize(
        QLSettingsId settings,
        const char* settingName,
        int* size)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLSettings_GetValueStringSize == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLSettings_GetValueStringSize(
            settings,
            settingName,
            size);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_Load(
        const char* path, 
        QLCalibrationId* calibration)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLCalibration_Load == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLCalibration_Load(
            path, 
            calibration);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_Save(
        const char* path, 
        QLCalibrationId calibration)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLCalibration_Save == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLCalibration_Save(
            path, 
            calibration);
}
QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_Create(
        QLCalibrationId source,
        QLCalibrationId* calibration)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLCalibration_Create == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLCalibration_Create(
            source,
            calibration);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_Initialize(
        QLDeviceId device, 
        QLCalibrationId calibration, 
        QLCalibrationType type)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLCalibration_Initialize == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLCalibration_Initialize(
            device, 
            calibration, 
            type);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_GetTargets(
        QLCalibrationId calibration, 
        int* numTargets,
        QLCalibrationTarget* targets)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLCalibration_GetTargets == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLCalibration_GetTargets(
            calibration, 
            numTargets,
            targets);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_Calibrate(
        QLCalibrationId calibration, 
        QLTargetId target, 
        int duration, 
        bool block)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLCalibration_Calibrate == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLCalibration_Calibrate(
            calibration, 
            target, 
            duration, 
            block);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_GetScoring(
        QLCalibrationId calibration, 
		QLTargetId target, 
        QLEyeType eye, 
        QLCalibrationScore* score)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLCalibration_GetScoring == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLCalibration_GetScoring(
            calibration, 
		    target, 
            eye, 
            score);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_GetStatus(
        QLCalibrationId calibration,
        QLTargetId target, 
        QLCalibrationStatus* calibrationStatus)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLCalibration_GetStatus == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLCalibration_GetStatus(
            calibration,
            target, 
            calibrationStatus);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_Finalize(
        QLCalibrationId calibration)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLCalibration_Finalize == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLCalibration_Finalize(
            calibration);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_Cancel(
        QLCalibrationId calibration)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLCalibration_Cancel == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLCalibration_Cancel(
            calibration);
}

QLError QUICK_LINK_2_CALL_CONVEN 
    QLCalibration_AddBias(
        QLCalibrationId calibration, 
        QLEyeType eye, 
        float xOffset, 
        float yOffset)
{
    if((g_ql2Utility_ql2Module == 0) || (pQLCalibration_AddBias == 0))
        return QL_ERROR_INTERNAL_ERROR;
    else
        return pQLCalibration_AddBias(
            calibration, 
            eye, 
            xOffset, 
            yOffset);
}
#endif
