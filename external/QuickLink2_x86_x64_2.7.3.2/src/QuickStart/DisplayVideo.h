////////////////////////////////////////////////////////////////////////////////////////////////////
/// project:  QuickStart
/// file:	  DisplayVideo.h
///
/// Copyright (c) 1996 - 2012 EyeTech Digital Systems. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////
#ifndef QL2_DISPLAY_VIDEO_H
#define	QL2_DISPLAY_VIDEO_H

#include <QLTypes.h>

typedef enum
{
	DEC_OK,
	DEC_EXIT,
	DEC_ERROR
} DisplayExitCode;

DisplayExitCode DisplayVideo(QLDeviceId deviceId);

#endif