////////////////////////////////////////////////////////////////////////////////////////////////////
/// project:  QuickStart
/// file:	  Main.cpp
///
/// Copyright (c) 1996 - 2012 EyeTech Digital Systems. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////

#include <stdio.h>
#include <conio.h>
#include "QL2Utility.h"
#include "Initialize.h"
#include "Calibrate.h"
#include "DisplayVideo.h"


int main(int argc, char* argv[])
{
	// Load the Quick Link 2 library. If if loaded successfully then print the library version.
	QL2Utility ql2Utility;
    if(ql2Utility.Load())
	{
		const int bufferSize = 32;
		char versionBuffer[bufferSize];
		versionBuffer[0] = 0;

		QLAPI_GetVersion(bufferSize, versionBuffer);
		printf_s("Quick Link 2 loaded. Version: %s\n", versionBuffer);
	}

	// Else if loading failed then print a message and return an error.
	else
    {
        printf_s("Could not load QuickLink2.dll.");
        return 1;
    }

    // Get the device ID for a device and initialize the device 
	// with the settings in the specified settings file.
    QLDeviceId deviceId = QL2Initialize("C:\\temp\\QL2Settings.txt");

	// If the device id is zero then a device was not successfully chosen by the user and we should
	// return an error. 
	if(deviceId == 0)
        return 2;

    // Start the device. If it failed then return
    if(QLDevice_Start(deviceId) != QL_ERROR_OK)
    {
        printf("Device not started successfully!\n");
        return 3;
    }

	// Display the live image.
	DisplayExitCode displayExitCode = DisplayVideo(deviceId);

	// If the user exited then return. All devices should be stopped and the library should be
	// unloaded before we return, but in our case this is all handled by the destructor of our
	// qlUtility object. 
	if(displayExitCode == DEC_EXIT)
        return 4;

	// If there was an error then display an error message and return. All devices should be
	// stopped and the library should be unloaded before we return, but in our case this is all
	// handled by the destructor of our qlUtility object. 
	else if(displayExitCode == DEC_ERROR)
	{
        printf_s("There was an error getting/displaying the image!");
        return 5;
	}

    // Calibrate the device.
    QLCalibrationId calibrationId = 0;
    if(AutoCalibrate(deviceId, QL_CALIBRATION_TYPE_16, &calibrationId))
    {
        printf_s("\nPress ESC to quit.\n\n");

        // If the calibration was successful then apply the calibration to the
        // device.
        QLDevice_ApplyCalibration(deviceId, calibrationId);

        // Display the gaze information until the user quits.
        QLFrameData frameData;
        while((_kbhit() == 0) || (_getch() != 27))
        {
            QLDevice_GetFrame(deviceId, 10000, &frameData);
            if(frameData.WeightedGazePoint.Valid)
                printf_s(
					"\rFrame#: %-10d    X:%6.2f%%    Y:%6.2f%%                                    ", 
					frameData.ImageData.FrameNumber,
                    frameData.WeightedGazePoint.x, 
                    frameData.WeightedGazePoint.y);
        }
    }

	// Else, the calibration was not succesfull. Print an error and return. All devices should be
	// stopped and the library should be unloaded before we return, but in our case this is all
	// handled by the destructor of our qlUtility object. 
    else
    {
        printf_s("The calibration did not finish successfully!");
        return 6;
    }

	// Stop all devices that may have been started. This step is not needed in this example because
	// the Unload() function of the ql2Utility object will also stop all devices. It is only placed
	// here for calrity and to demonstrate that in general all devices should be stopped before the
	// library is unloaded. 
    QLDevice_Stop_All();

	// Unload the library. This step is not needed in this example because the destructor of the
	// ql2Utility object will unload the library. It is only placed here for calrity. 
	ql2Utility.Unload();

	// return zero to indicate that the program exited without any errors.
	return 0;
}

