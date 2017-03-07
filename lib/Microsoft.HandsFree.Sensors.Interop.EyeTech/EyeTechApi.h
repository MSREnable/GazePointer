// Microsoft.HandsFree.Sensors.Interop.EyeTech.h

#pragma once

#include "EyeTechApiException.h"

using namespace System;
using namespace System::Diagnostics;
using namespace System::Runtime::InteropServices;

namespace Microsoft { namespace HandsFree { namespace Sensors { namespace Interop { namespace EyeTech {

	public value struct EyeTechFrame
	{
	public:
		bool _isValid;
		float _x;
		float _y;
	};

	public ref class EyeTechApi
	{
	public:

		static String^ GetVersion()
		{
			char sz[256];
			auto error = QLAPI_GetVersion(sizeof(sz), sz);
			auto str = gcnew String(sz);
			return str;
		}

		static QLDeviceId Initialize(String^ path)
		{
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
				if (qlerror != QL_ERROR_OK)
				{
					Debug::WriteLine(String::Format("QLDevice_Enumerate() failed with error code {0}", (int)qlerror));
					return 0;
				}

				// If no devices were found then return 0;
				else if (numDevices == 0)
				{
					Debug::WriteLine("No devices present.");
					return 0;
				}

				// There was only one or more devices connected then use the first.
				deviceId = deviceIds[0];

				// Create a blank settings container. QLSettings_Load() can create a
				// settings container but it won't if the file fails to load. By calling
				// QLSettings_Create() we ensure that a container is created regardless.
				qlerror = QLSettings_Create(0, &settingsId);

				// Load the file with the stored password.
				auto intPtr = Marshal::StringToHGlobalAnsi(path);
				char* pszPath = (char*)(void*)intPtr;
				qlerror = QLSettings_Load(pszPath, &settingsId);
				Marshal::FreeHGlobal(intPtr);				

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

				if (qlerror != QL_ERROR_OK)
				{
					Debug::WriteLine(String::Format("Error setting the password for the device. Error = {0}", (int)qlerror));
					return 0;
				}

				// The application defined password setting that is stored in the settings 
				// container will have no effect if imported to the device, but there may 
				// have been other settings in the file that was loaded which are Quick 
				// Link 2 settings. Try and import them to the device.
				QLDevice_ImportSettings(deviceId, settingsId);

				QLAPI_ImportSettings(settingsId);

				return deviceId;
			}
		}

		static void Start(QLDeviceId device)
		{
			auto error = QLDevice_Start(device);
			EyeTechApiException::ThrowIfError(error);
		}

		static void Stop(QLDeviceId device)
		{
			auto error = QLDevice_Stop(device);
			EyeTechApiException::ThrowIfError(error);
		}

		static EyeTechFrame GetFrame(QLDeviceId device)
		{
			QLFrameData frame;
			auto error = QLDevice_GetFrame(device, 10000, &frame);
			EyeTechApiException::ThrowIfError(error);

			EyeTechFrame externalFrame;
			externalFrame._isValid = frame.WeightedGazePoint.Valid;
			externalFrame._x = frame.WeightedGazePoint.x;
			externalFrame._y = frame.WeightedGazePoint.y;

			return externalFrame;
		}
	};

} } } } }
