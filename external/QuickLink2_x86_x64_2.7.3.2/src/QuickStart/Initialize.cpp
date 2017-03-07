////////////////////////////////////////////////////////////////////////////////////////////////////
/// project:  QuickStart
/// file:	  Initialize.cpp
///
/// Copyright (c) 1996 - 2012 EyeTech Digital Systems. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////
#include "Initialize.h"
#include "QL2Utility.h"
#include <stdio.h>
#include <string>


QLDeviceId QL2Initialize(const char* path)
{
    QLError qlerror = QL_ERROR_OK;
    QLDeviceId deviceId = 0;
    QLDeviceInfo deviceInfo;
    QLSettingsId settingsId;

    // Enumerate the bus to find out which eye trackers are connected to the
    // computer. 
    const int bufferSize = 100;
    QLDeviceId deviceIds[bufferSize];
    int numDevices = bufferSize;
    qlerror = QLDevice_Enumerate(&numDevices, deviceIds);

    // If the enumeration failed then return 0
    if(qlerror != QL_ERROR_OK) 
    {
        printf_s("QLDevice_Enumerate() failed with error code %d\n", qlerror);
        return 0;
    }

    // If no devices were found then return 0;
    else if(numDevices == 0)
    {
        printf_s("No devices present.\n");
        return 0;
    }

    // If there was only one device connected then use it without prompting the
    // user.
    else if(numDevices == 1)
        deviceId = deviceIds[0];

    // If there is more than one device then ask which one to use.
    else if(numDevices > 1)
    {
        printf_s("QLDevice_Enumerate() found %d devices\n\n", numDevices);

        // Get the information for each eye tracker and print it to the screen.
        for(int i = 0; i < numDevices; i++)
        {
            QLDevice_GetInfo(deviceIds[i], &deviceInfo);
            printf_s("Device %d\n", i);
            printf_s("\tdeviceInfo.modelName = %s\n", deviceInfo.modelName);
            printf_s("\tdeviceInfo.serialNumber = %s\n\n", deviceInfo.serialNumber);
        }

        // Ask which device to use
        int deviceToUse = -1;

        printf_s("Which device would you like to use? ");
        deviceToUse = getchar() - (int)'0';
        _flushall();

        // Check to make sure the user input is valid. If it is not valid then
        // return 0.
        if((deviceToUse < 0) || (deviceToUse >= numDevices))
        {
            printf_s("Invalid device.\n\n", deviceToUse);
            return 0;
        }

        // If the device is valid then select it as the device to use.
        else
            deviceId = deviceIds[deviceToUse];
    }

    // Create a blank settings container. QLSettings_Load() can create a
    // settings container but it won't if the file fails to load. By calling
    // QLSettings_Create() we ensure that a container is created regardless.
    qlerror = QLSettings_Create(0, &settingsId);

    // Load the file with the stored password.
    qlerror = QLSettings_Load(path, &settingsId);

    // Get the device info so we can access the serial number.
    QLDevice_GetInfo(deviceId, &deviceInfo);

    // Create an application defined setting name using the serial number. The
    // settings containers can be used to hold settings other than the
    // QuickLink2 defined setting. Using it to store the password for future
    // use as we are doing here is a good example. 
    std::string serialNumberName = "SN_";
    serialNumberName += deviceInfo.serialNumber;

    // Create a buffer for getting the stored password.
    const int passwordBufferSize = 128;
    char password[passwordBufferSize];
    password[0] = 0; 

    // Check for the password in the settings file.
    int stringSize = passwordBufferSize;
    QLSettings_GetValue(
        settingsId, 
        serialNumberName.c_str(), 
        QL_SETTING_TYPE_STRING, 
        stringSize, 
        password);

    // Try setting the password for the device.
    qlerror = QLDevice_SetPassword(deviceId, password);

    // If setting the password failed then get the password from the user and
    // try again.
    if(qlerror == QL_ERROR_INVALID_PASSWORD)
    {
        printf_s("What is the password for the device? ");
        gets_s(password, bufferSize);

        // Set the password for the device.
        qlerror = QLDevice_SetPassword(deviceId, password);

        // If the password is not correct then print an error and return 0.
        if(qlerror != QL_ERROR_OK)
        {
            printf_s("Invalid password. Error = %d\n", qlerror);
            return 0;
        }

        // Set the password for the device in the settings container.
        QLSettings_SetValue(
            settingsId, 
            serialNumberName.c_str(), 
            QL_SETTING_TYPE_STRING, 
            password);

        // Save the settings container to file.
        QLSettings_Save(path, settingsId);
    }
	else if(qlerror != QL_ERROR_OK)
	{
        printf_s("Error setting the password for the device. Error = %d\n", qlerror);
        return 0;
	}

	// The application defined password setting that is stored in the settings 
	// container will have no effect if imported to the device, but there may 
	// have been other settings in the file that was loaded which are Quick 
	// Link 2 settings. Try and import them to the device.
	QLDevice_ImportSettings(deviceId, settingsId);

	QLAPI_ImportSettings(settingsId);

    return deviceId;
};