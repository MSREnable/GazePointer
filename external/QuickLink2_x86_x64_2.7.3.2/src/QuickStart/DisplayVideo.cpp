////////////////////////////////////////////////////////////////////////////////////////////////////
/// project:  QuickStart
/// file:	  DisplayVideo.cpp
///
/// Copyright (c) 1996 - 2012 EyeTech Digital Systems. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////
#include "QL2Utility.h"
#include "opencvUtility.h"
#include <opencv2/imgproc/imgproc_c.h>
#include "DisplayVideo.h"

DisplayExitCode DisplayVideo(QLDeviceId deviceId)
{
	// Create a local members.
    QLFrameData frame;
	QLError qlerror = QL_ERROR_OK;

	// Get a frame from the device. If there was an 
	// error getting the frame then return an error.
    if((qlerror = QLDevice_GetFrame(deviceId, 5000, &frame)) != QL_ERROR_OK)
	{
		printf_s("Error getting frame from device. Error = %d\n", qlerror);     
		return DEC_ERROR;
	}

	// Create some local pointers to OpenCV image objects.
    IplImage* ql2Image;
    IplImage* displayImage;

	// Create the OpenCV image objects and initialize the local pointers. 
	// The image from Quick Link 2 is 8 bit grey scale and the pixel data 
	// buffer is allocated in Quick Link 2 so only create an image header. 
	// The image that will be displayed has other colored things that are 
	// drawn on it so and its buffer is not created elsewhere so allocate 
	// three bytes per pixel.
	ql2Image = cvCreateImageHeader(cvSize(frame.ImageData.Width, frame.ImageData.Height), 8, 1);
    displayImage = cvCreateImage(cvSize(frame.ImageData.Width, frame.ImageData.Height), 8, 3);

	// Create an OpenCV window for displaying the image
	std::string windowName = "Quick Link 2 Image";
    cvNamedWindow(windowName.c_str(), 1);

	// Create a some local members that will be used for 
	// displaying text information on the display image.
    CvFont font;
    cvInitFont(&font,CV_FONT_HERSHEY_SIMPLEX|CV_FONT_ITALIC, 1, 1, 0, 1);
    const int textBufferSize = 256;
    char textBuffer[textBufferSize];
	int fontSpacing = 30;
	int fontSpacingMultiplier = 1;

	// Create some other variables
    bool success = true;
	int waitKeyReturnValue = 0;

	// Display the image and then get a new image. If a new image was retrieved 
	// successfully then loop through again until an image was not successfully 
	// retrieved or until the user preses esc.
    do
    {
		// Reset the font spacing multiplier. The font spacing multiplier 
		// determines the line on which the text will be displayed. 
		fontSpacingMultiplier = 1;

		// Assign the pixel data buffer pointer in the OpenCV image to the 
		// pixel data buffer in the Quick Link 2 frame data.
		ql2Image->imageData = (char*)frame.ImageData.PixelData;

		// Copy the grey scale image to the color image buffer so it can be displayed.
		if(ql2Image->imageData != 0)
			cvCvtColor(ql2Image, displayImage, CV_GRAY2RGB);

		// Place some instructions on the image for the user.
        sprintf_s(textBuffer, textBufferSize, "Press ENTER to continue");
        cvPutText(displayImage, textBuffer, cvPoint(0, fontSpacing * fontSpacingMultiplier++), &font, CV_RGB(255,0,0));
        sprintf_s(textBuffer, textBufferSize, "Press ESC to exit");
        cvPutText(displayImage, textBuffer, cvPoint(0, fontSpacing * fontSpacingMultiplier++), &font, CV_RGB(255,0,0));

		// If the left was was found then mark the pupil and the glints;
        if(frame.LeftEye.Found)
        {
			DrawCross(displayImage, 
            cvPoint((int)frame.LeftEye.Pupil.x, (int)frame.LeftEye.Pupil.y), 
            10, CV_RGB(0,255,0), 1);

            DrawCross(displayImage, 
            cvPoint((int)frame.LeftEye.Glint0.x, (int)frame.LeftEye.Glint0.y), 
            5, CV_RGB(0,255,0), 1);

            DrawCross(displayImage, 
            cvPoint((int)frame.LeftEye.Glint1.x, (int)frame.LeftEye.Glint1.y), 
            5, CV_RGB(0,255,0), 1);
        }

		// If the right was was found then mark the pupil and the glints;
        if(frame.RightEye.Found)
        {
            DrawCross(displayImage, 
            cvPoint((int)frame.RightEye.Pupil.x, (int)frame.RightEye.Pupil.y), 
            10, CV_RGB(255,0,0), 1);

            DrawCross(displayImage, 
            cvPoint((int)frame.RightEye.Glint0.x, (int)frame.RightEye.Glint0.y), 
            5, CV_RGB(255,0,0), 1);

            DrawCross(displayImage, 
            cvPoint((int)frame.RightEye.Glint1.x, (int)frame.RightEye.Glint1.y), 
            5, CV_RGB(255,0,0), 1);
        }

		// Display the image in the OpenCV window.
        cvShowImage(windowName.c_str(), displayImage);
        success = ((qlerror = QLDevice_GetFrame(deviceId, 10000, &(frame))) == QL_ERROR_OK);

		// Check for user input.
		waitKeyReturnValue = cvWaitKey(1);

	// if the user pressed escape or enter or if the image was not retrieved 
	// from the device successfully then quit the loop.
    } while((waitKeyReturnValue != cvWaitKeyEnter)  && (waitKeyReturnValue != cvWaitKeyEsc) && success);

	// Destroy the OpenCV window and memory.
    cvReleaseImageHeader(&(ql2Image));
    cvReleaseImage(&(displayImage));
    cvDestroyWindow(windowName.c_str());

	// If the user pressed escape then return the exit code.
	if(waitKeyReturnValue == cvWaitKeyEsc)
		return DEC_EXIT;

	// If the image was not retrieved successfully then return an error.
	if(!success)
	{
		printf_s("Error getting frame from device. Error = %d\n", qlerror);     
		return DEC_ERROR;
	}

	// Return OK.
	return DEC_OK;
}
