////////////////////////////////////////////////////////////////////////////////////////////////////
/// project:  QuickStart
/// file:	  opencvUtility.h
///
/// Copyright (c) 1996 - 2012 EyeTech Digital Systems. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////
#ifndef QL2_OPENCV_UTILITY_H
#define	QL2_OPENCV_UTILITY_H

#include <opencv2/highgui/highgui.hpp>

const int cvWaitKeyEsc = 27;
const int cvWaitKeyEnter = 13;

void DrawCross(IplImage* image, CvPoint center, int size, CvScalar color, int thickness);

#endif

