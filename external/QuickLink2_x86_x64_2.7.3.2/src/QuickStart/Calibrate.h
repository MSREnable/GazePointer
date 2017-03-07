////////////////////////////////////////////////////////////////////////////////////////////////////
/// project:  QuickStart
/// file:	  Calibrate.h
///
/// Copyright (c) 1996 - 2012 EyeTech Digital Systems. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////
#ifndef QL2_CALIBRATE_H
#define	QL2_CALIBRATE_H

#include <QLTypes.h>

bool AutoCalibrate(
    QLDeviceId deviceId, 
    QLCalibrationType calibrationType,  
    QLCalibrationId* calibrationId);

#endif