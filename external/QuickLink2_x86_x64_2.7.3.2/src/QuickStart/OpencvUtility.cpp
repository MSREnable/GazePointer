////////////////////////////////////////////////////////////////////////////////////////////////////
/// project:  QuickStart
/// file:	  opencvUtility.cpp
///
/// Copyright (c) 1996 - 2012 EyeTech Digital Systems. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////
#include "opencvUtility.h"

void DrawCross(IplImage* image, CvPoint center, int size, CvScalar color, int thickness)
{
        cvLine( 
            image, 
            cvPoint( center.x - size, center.y - size ), 
            cvPoint( center.x + size, center.y + size ), 
            color, 
            thickness, 
            0);

        cvLine( 
            image, 
            cvPoint( center.x + size, center.y - size ),
            cvPoint( center.x - size, center.y + size ),
            color, 
            thickness, 
            0 );
};
