// ----------------------------------------------------------------------------
//
// (C) Copyright 2014, Visual Interaction GmbH 
//
// All rights reserved. This work contains unpublished proprietary 
// information of Visual Interaction GmbH and is copy protected by law. 
// (see accompanying file eula.pdf)
//
// ----------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace SMIEyeTrackingController
{

    public class EyeTrackingController
    {

#if (x86) 
        // use for 32 bit
        const string dllName = "myGazeAPI.dll";

#elif (x64)
        //use for 64 bit
        const string dllName = "myGazeAPI64.dll";
#else
        // use for 32 bit
        const string dllName = "myGazeAPI.dll";
#warning myGazeAPI library might be wrong. 32bit dll will be loaded. 
#endif


        // API Struct definition. See the manual for further description. 
        public enum RetCode
        {
            Success = 1,
            NoValidData = 2,
            CalibrationAborted = 3,
            ServerIsRunning = 4,
            CalibrationInProgress = 5,
            WindowIsOpen = 11,
            WindowIsClosed = 12,

            CouldNotConnect = 100,
            NotConnected = 101,
            NotCalibrated = 102,
            NotValidated = 103,
            EyetrackingApplicationNotRunning = 104,
            WrongCommunicationParameter = 105,
            WrongDevice = 111,
            WrongParameter = 112,
            WrongCalibrationMethod = 113,
            CalibrationTimeout = 114,
            TrackingNotStable = 115,
            InsufficientBufferSize = 116,
            CreateSocket = 121,
            ConnectSocket = 122,
            BindSocket = 123,
            DeleteSocket = 124,
            NoResponseFromIViewx = 131,
            InvalidIViewxVersion = 132,
            WrongIViewxVersion = 133,
            AccessToFile = 171,
            SocketConnection = 181,
            EmptyDataBuffer = 191,
            RecordingDataBuffer = 192,
            FullDataBuffer = 193,
            IViewxIsNotReady = 194,
            PausedDataBuffer = 195,
            IViewxNotFound = 201,
            IViewxPathNotFound = 202,
            IViewxAccessDenied = 203,
            IViewxAccessIncomplete = 204,
            IViewxOutOfMemory = 205,
            MultipleDevices = 206,
            CameraNotFound = 211,
            WrongCamera = 212,
            WrongCameraPort = 213,
            Usb2CameraPort = 214,
            Usb3CameraPort = 215,
            CouldNotOpenPort = 220,
            CouldNotClosePort = 221,
            AoiAccess = 222,
            AoiNotDefined = 223,
            FeatureNotLicensed = 250,
            DeprecatedFunction = 300,
            Initialization = 400,
            FuncNotLoaded = 401,
        };

        public enum CalibrationStatusEnum 
		{
			unknownCalibrationStatus = 0, 
			noCalibration = 1, 
			validCalibration = 2, 
			performingCalibration = 3 
		};


		public enum ETDevice 
		{
			myGaze = 2
		};

		public struct SystemInfoStruct
		{
			public int samplerate;
			public int iV_MajorVersion;
			public int iV_MinorVersion;
			public int iV_Buildnumber;
			public int API_MajorVersion;
			public int API_MinorVersion;
			public int API_Buildnumber;
			public int iV_ETSystem;
		};

		public struct CalibrationPointStruct
		{
			public int number;
			public int positionX;
			public int positionY;
		};


		public struct EyeDataStruct
		{
			public double gazeX;
			public double gazeY;
			public double diam;
			public double eyePositionX;
			public double eyePositionY;
			public double eyePositionZ;
		};


		public struct SampleStruct
		{
			public Int64 timestamp;
			public EyeDataStruct leftEye;
			public EyeDataStruct rightEye;
		};

		public struct EventStruct
		{
			public char eventType;
			public char eye;
			public Int64 startTime;
			public Int64 endTime;
			public Int64 duration;
			public double positionX;
			public double positionY;
		};

		public struct EyePositionStruct
		{
			public int validity; 
			public double relativePositionX; 
			public double relativePositionY; 
			public double relativePositionZ; 
			public double positionRatingX;
			public double positionRatingY;
			public double positionRatingZ;
		};

		public struct TrackingStatusStruct 
		{
			public Int64 timestamp; 
			EyePositionStruct leftEye; 
			EyePositionStruct rightEye; 
			EyePositionStruct total; 
		};

		public struct AccuracyStruct
		{
			public double deviationLX;
			public double deviationLY;
			public double deviationRX;
			public double deviationRY;
		};

	   [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct CalibrationStruct
		{
			public int method;				        
			public int visualization;			    
			public int displayDevice;				
			public int speed;					    
			public int autoAccept;			        
			public int foregroundColor;	            
			public int backgroundColor;	            
			public int targetShape;		            
			public int targetSize;		            
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string targetFilename;
		};

		public struct MonitorAttachedGeometryStruct
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string setupName;
			public int stimX;
			public int stimY;
			public int redStimDistHeight;
			public int redStimDistDepth;
			public int redInclAngle;
		};

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct ImageStruct
		{
			public int imageHeight;
			public int imageWidth;
			public int imageSize;
			public IntPtr imageBuffer;
		};

		public struct DateStruct
		{
			public int day;
			public int month;
			public int year;
		};


        // Kernel Function definition 
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);


        // API Function definition. See the manual for further description. 
        [DllImport(dllName, EntryPoint = "iV_AbortCalibration")]
		private static extern int Unmanaged_AbortCalibration();

		[DllImport(dllName, EntryPoint = "iV_AcceptCalibrationPoint")]
		private static extern int Unmanaged_AcceptCalibrationPoint();

		[DllImport(dllName, EntryPoint = "iV_Calibrate")]
		private static extern int Unmanaged_Calibrate();
		
		[DllImport(dllName, EntryPoint = "iV_ChangeCalibrationPoint")]
		private static extern int Unmanaged_ChangeCalibrationPoint(int number, int x, int y);

		[DllImport(dllName, EntryPoint = "iV_Connect")]
		private static extern int Unmanaged_Connect();

		[DllImport(dllName, EntryPoint = "iV_ContinueEyetracking")]
		private static extern int Unmanaged_ContinueEyetracking();

		[DllImport(dllName, EntryPoint = "iV_DeleteMonitorAttachedGeometry")]
		private static extern int Unmanaged_DeleteMonitorAttachedGeometry(StringBuilder name);

		[DllImport(dllName, EntryPoint = "iV_DisableProcessorHighPerformanceMode")]
		private static extern int Unmanaged_DisableProcessorHighPerformanceMode();

		[DllImport(dllName, EntryPoint = "iV_Disconnect")]
		private static extern int Unmanaged_Disconnect();

		[DllImport(dllName, EntryPoint = "iV_EnableProcessorHighPerformanceMode")]
		private static extern int Unmanaged_EnableProcessorHighPerformanceMode();

		[DllImport(dllName, EntryPoint = "iV_GetAccuracy")]
		private static extern int Unmanaged_GetAccuracy(ref AccuracyStruct accuracy);

		[DllImport(dllName, EntryPoint = "iV_GetAccuracyImage")]
		private static extern int Unmanaged_GetAccuracyImage(ref ImageStruct image);

		[DllImport(dllName, EntryPoint = "iV_GetCalibrationParameter")]
		private static extern int Unmanaged_GetCalibrationParameter(ref CalibrationStruct calibrationParameter);
		
		[DllImport(dllName, EntryPoint = "iV_GetCalibrationPoint")]
		private static extern int Unmanaged_GetCalibrationPoint(int calibrationPointNumber, ref CalibrationPointStruct point);

		[DllImport(dllName, EntryPoint = "iV_GetCalibrationStatus")]
		private static extern int Unmanaged_GetCalibrationStatus(ref CalibrationStatusEnum status);

		[DllImport(dllName, EntryPoint = "iV_GetCurrentCalibrationPoint")]
		private static extern int Unmanaged_GetCurrentCalibrationPoint(ref CalibrationPointStruct point);

		[DllImport(dllName, EntryPoint = "iV_GetCurrentMonitorAttachedGeometry")]
		private static extern int Unmanaged_GetCurrentMonitorAttachedGeometry(ref MonitorAttachedGeometryStruct geometry);
		
		[DllImport(dllName, EntryPoint = "iV_GetCurrentTimestamp")]
		private static extern int Unmanaged_GetCurrentTimestamp(ref Int64 timestamp);

		[DllImport(dllName, EntryPoint = "iV_GetEvent")]
		private static extern int Unmanaged_GetEvent(ref EventStruct eventData);
			
		[DllImport(dllName, EntryPoint = "iV_GetFeatureKey")]
		private static extern int Unmanaged_GetFeatureKey(ref Int64 featureKey);

		[DllImport(dllName, EntryPoint = "iV_GetGeometryProfiles")]
		private static extern int Unmanaged_GetGeometryProfiles(int maxSize, ref StringBuilder profiles);
		
		[DllImport(dllName, EntryPoint = "iV_GetLicenseDueDate")]
		private static extern int Unmanaged_GetLicenseDueDate(ref DateStruct expiryDate);

		[DllImport(dllName, EntryPoint = "iV_GetMonitorAttachedGeometry")]
		private static extern int Unmanaged_GetMonitorAttachedGeometry(StringBuilder profile, ref MonitorAttachedGeometryStruct geometry);

		[DllImport(dllName, EntryPoint = "iV_GetSample")]
		private static extern int Unmanaged_GetSample(ref SampleStruct sample);
		
		[DllImport(dllName, EntryPoint = "iV_GetSerialNumber")]
		private static extern int Unmanaged_GetSerialNumber(ref StringBuilder serialNumber);

		[DllImport(dllName, EntryPoint = "iV_GetSystemInfo")]
		private static extern int Unmanaged_GetSystemInfo(ref SystemInfoStruct systemInfo);
		
		[DllImport(dllName, EntryPoint = "iV_GetTrackingMonitor")]
		private static extern int Unmanaged_GetTrackingMonitor(ref ImageStruct image);

		[DllImport(dllName, EntryPoint = "iV_GetTrackingStatus")]
		private static extern int Unmanaged_GetTrackingStatus(ref TrackingStatusStruct trackingStatus);

		[DllImport(dllName, EntryPoint = "iV_HideAccuracyMonitor")]
		private static extern int Unmanaged_HideAccuracyMonitor();

		[DllImport(dllName, EntryPoint = "iV_HideTrackingMonitor")]
		private static extern int Unmanaged_HideTrackingMonitor();

		[DllImport(dllName, EntryPoint = "iV_IsConnected")]
		private static extern int Unmanaged_IsConnected();
		
		[DllImport(dllName, EntryPoint = "iV_IsTrackingStable")]
		private static extern int Unmanaged_IsTrackingStable();

		[DllImport(dllName, EntryPoint = "iV_LoadCalibration")]
		private static extern int Unmanaged_LoadCalibration(StringBuilder name);

		[DllImport(dllName, EntryPoint = "iV_PauseEyetracking")]
		private static extern int Unmanaged_PauseEyetracking();

		[DllImport(dllName, EntryPoint = "iV_Quit")]
		private static extern int Unmanaged_Quit();

		[DllImport(dllName, EntryPoint = "iV_ResetCalibrationPoints")]
		private static extern int Unmanaged_ResetCalibrationPoints();

		[DllImport(dllName, EntryPoint = "iV_SaveCalibration")]
		private static extern int Unmanaged_SaveCalibration(StringBuilder  name);

		[DllImport(dllName, CallingConvention = CallingConvention.StdCall, EntryPoint = "iV_SetCalibrationCallback")]
		private static extern int Unmanaged_SetCalibrationCallback(MulticastDelegate calibrationPointCallbackFunction);

		[DllImport(dllName, EntryPoint = "iV_SetConnectionTimeout")]
		private static extern int Unmanaged_SetConnectionTimeout(int time);

		[DllImport(dllName, EntryPoint = "iV_SetGeometryProfile")]
		private static extern int Unmanaged_SetGeometryProfile(StringBuilder profile);
		
		[DllImport(dllName, CallingConvention = CallingConvention.StdCall, EntryPoint = "iV_SetEventCallback")]
		private static extern void Unmanaged_SetEventCallback(MulticastDelegate eventCallbackFunction);

		[DllImport(dllName, EntryPoint = "iV_SetEventDetectionParameter")]
		private static extern int Unmanaged_SetEventDetectionParameter(int minDuration, int maxDispersion);

		[DllImport(dllName, EntryPoint = "iV_SetLicense")]
		private static extern int Unmanaged_SetLicense(StringBuilder licenseKey);
		
		[DllImport(dllName, CallingConvention = CallingConvention.StdCall, EntryPoint = "iV_SetSampleCallback")]
		private static extern void Unmanaged_SetSampleCallback(MulticastDelegate sampleCallbackFunction);

		[DllImport(dllName, CallingConvention = CallingConvention.StdCall, EntryPoint = "iV_SetTrackingMonitorCallback")]
		private static extern void Unmanaged_SetTrackingMonitorCallback(MulticastDelegate trackingMonitorCallbackFunction);

		[DllImport(dllName, EntryPoint = "iV_SetTrackingParameter")]
		private static extern int Unmanaged_SetTrackingParameter(int eye, int parameter, int reserved);

		[DllImport(dllName, EntryPoint = "iV_SetupCalibration")]
		private static extern int Unmanaged_SetupCalibration(ref CalibrationStruct calibrationParameter);

		[DllImport(dllName, EntryPoint = "iV_SetupMonitorAttachedGeometry")]
		private static extern int Unmanaged_SetupMonitorAttachedGeometry(ref MonitorAttachedGeometryStruct geometry);

		[DllImport(dllName, EntryPoint = "iV_ShowAccuracyMonitor")]
		private static extern int Unmanaged_ShowAccuracyMonitor();

		[DllImport(dllName, EntryPoint = "iV_ShowTrackingMonitor")]
		private static extern int Unmanaged_ShowTrackingMonitor();

		[DllImport(dllName, EntryPoint = "iV_Start")]
		private static extern int Unmanaged_Start();

		[DllImport(dllName, EntryPoint = "iV_Validate")]
		private static extern int Unmanaged_Validate();	

		
		public int iV_AbortCalibration()
		{
			return Unmanaged_AbortCalibration();
		}

		public int iV_AcceptCalibrationPoint()
		{
			return Unmanaged_AcceptCalibrationPoint();
		}

		public int iV_Calibrate()
		{
			return Unmanaged_Calibrate();
		}

		public int iV_ChangeCalibrationPoint(int number, int x, int y)
		{
			return Unmanaged_ChangeCalibrationPoint(number, x, y);
		}

		public int iV_Connect()
		{
			return Unmanaged_Connect();
		}

		public int iV_ContinueEyetracking()
		{
			return Unmanaged_ContinueEyetracking();
		}

		public int iV_DeleteMonitorAttachedGeometry(StringBuilder name)
		{
			return Unmanaged_DeleteMonitorAttachedGeometry(name);
		}

		public int iV_DisableProcessorHighPerformanceMode()
		{
			return Unmanaged_DisableProcessorHighPerformanceMode();
		}

		public int iV_Disconnect()
		{
			return Unmanaged_Disconnect();
		}

		public int iV_EnableProcessorHighPerformanceMode()
		{
			return Unmanaged_EnableProcessorHighPerformanceMode();
		}

		public int iV_GetAccuracy(ref AccuracyStruct accuracyData)
		{
			return Unmanaged_GetAccuracy(ref accuracyData);
		}

		public int iV_GetAccuracyImage(ref ImageStruct image)
		{
			return Unmanaged_GetAccuracyImage(ref image);
		}

		public int iV_GetCalibrationParameter(ref CalibrationStruct calibrationData)
		{
			return Unmanaged_GetCalibrationParameter(ref calibrationData);
		}

		public int iV_GetCalibrationPoint(int calibrationPointNumber, ref CalibrationPointStruct point)
		{
			return Unmanaged_GetCalibrationPoint(calibrationPointNumber, ref point);
		}

		public int iV_GetCalibrationStatus(ref CalibrationStatusEnum status)
		{
			return Unmanaged_GetCalibrationStatus(ref status);
		}

		public int iV_GetCurrentCalibrationPoint(ref CalibrationPointStruct point)
		{
			return Unmanaged_GetCurrentCalibrationPoint(ref point);
		}

		public int iV_GetCurrentMonitorAttachedGeometry(ref MonitorAttachedGeometryStruct geometry)
		{
			return Unmanaged_GetCurrentMonitorAttachedGeometry(ref geometry);
		}

		public int iV_GetCurrentTimestamp(ref Int64 timestamp)
		{
			return Unmanaged_GetCurrentTimestamp(ref timestamp);
		}

		public int iV_GetEvent(ref EventStruct eventData)
		{
			return Unmanaged_GetEvent(ref eventData);
		}

		public int iV_GetFeatureKey(ref Int64 featureKey)
		{
			return Unmanaged_GetFeatureKey(ref featureKey);
		}
		
		public int iV_GetGeometryProfiles(int maxSize, ref StringBuilder profiles)
		{
			return Unmanaged_GetGeometryProfiles(maxSize, ref profiles);
		}

		public int iV_GetLicenseDueDate(ref DateStruct expiryDate)
		{
			return Unmanaged_GetLicenseDueDate(ref expiryDate);
		}

		public int iV_GetMonitorAttachedGeometry(StringBuilder profile, ref MonitorAttachedGeometryStruct geometry)
		{
			return Unmanaged_GetMonitorAttachedGeometry(profile, ref geometry);
		}

		public int iV_GetSample(ref SampleStruct sampleData)
		{
			return Unmanaged_GetSample(ref sampleData);
		}

		public int iV_GetSerialNumber(ref StringBuilder serialNumber)
		{
			return Unmanaged_GetSerialNumber(ref serialNumber);
		}

		public int iV_GetSystemInfo(ref SystemInfoStruct systemInfo)
		{
			return Unmanaged_GetSystemInfo(ref systemInfo);
		}

		public int iV_GetTrackingMonitor(ref ImageStruct image)
		{
			return Unmanaged_GetTrackingMonitor(ref image);
		}

		public int iV_GetTrackingStatus(ref TrackingStatusStruct trackingStatus)
		{
			return Unmanaged_GetTrackingStatus(ref trackingStatus);
		}

		public int iV_HideAccuracyMonitor()
		{
			return Unmanaged_HideAccuracyMonitor();
		}

		public int iV_HideTrackingMonitor()
		{
			return Unmanaged_HideTrackingMonitor();
		}

		public int iV_IsConnected()
		{
			return Unmanaged_IsConnected();
		}

		public int iV_IsTrackingStable()
		{
			return Unmanaged_IsTrackingStable();
		}
		
		public int iV_LoadCalibration(StringBuilder name)
		{
			return Unmanaged_LoadCalibration(name);
		}

		public int iV_PauseEyetracking()
		{
			return Unmanaged_PauseEyetracking();
		}

		public int iV_Quit()
		{
			return Unmanaged_Quit();
		}
					   
		public int iV_ResetCalibrationPoints()
		{
			return Unmanaged_ResetCalibrationPoints();
		}

		public int iV_SaveCalibration(StringBuilder name)
		{
			return Unmanaged_SaveCalibration(name);
		}

		public void iV_SetCalibrationCallback(MulticastDelegate calibrationPointCallbackFunction)
		{
			Unmanaged_SetCalibrationCallback(calibrationPointCallbackFunction);
		}

		public void iV_SetConnectionTimeout(int time)
		{
			Unmanaged_SetConnectionTimeout(time);
		}
		
		public void iV_SetGeometryProfile(StringBuilder profile)
		{
			Unmanaged_SetGeometryProfile(profile);
		}

		public void iV_SetEventCallback(MulticastDelegate eventCallbackFunciton)
		{
			Unmanaged_SetEventCallback(eventCallbackFunciton);
		}

		public int iV_SetEventDetectionParameter(int minDuration, int maxDispersion)
		{
			return Unmanaged_SetEventDetectionParameter(minDuration, maxDispersion);
		}

		public int iV_SetLicense(StringBuilder key)
		{
			return Unmanaged_SetLicense(key);
		}

		public void iV_SetSampleCallback(MulticastDelegate sampleCallbackFunction)
		{
			Unmanaged_SetSampleCallback(sampleCallbackFunction);
		}

		public void iV_SetTrackingMonitorCallback(MulticastDelegate trackingMonitorCallbackFunction)
		{
			Unmanaged_SetTrackingMonitorCallback(trackingMonitorCallbackFunction);
		}
		
		public void iV_SetTrackingParameter(int eye, int parameter, int reserved)
		{
			Unmanaged_SetTrackingParameter(eye, parameter, reserved);
		}
		
		public int iV_SetupCalibration(ref CalibrationStruct calibrationParameter)
		{
			return Unmanaged_SetupCalibration(ref calibrationParameter);
		}

		public int iV_SetupMonitorAttachedGeometry(ref MonitorAttachedGeometryStruct geometry)
		{
			return Unmanaged_SetupMonitorAttachedGeometry(ref geometry);
		}

		public int iV_ShowAccuracyMonitor()
		{
			return Unmanaged_ShowAccuracyMonitor();
		}

		public int iV_ShowTrackingMonitor()
		{
			return Unmanaged_ShowTrackingMonitor();
		}

		public int iV_Start()
		{
			return Unmanaged_Start();
		}

		public int iV_Validate()
		{
			return Unmanaged_Validate();
		}
	}
}

