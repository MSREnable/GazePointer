////////////////////////////////////////////////////////////////////////////////////////////////////
/// project:  QuickStart
/// file:	  Calibrate.cpp
///
/// Copyright (c) 1996 - 2012 EyeTech Digital Systems. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////
#include "QL2Utility.h"
#include "Calibrate.h"
#include "opencvUtility.h"

void DrawTarget(IplImage* image, CvPoint center, int radius, CvScalar color)
{
    DrawCross(image, center, radius, color, 2);
    cvCircle(image, center, radius, CV_RGB(50, 50, 50), radius);
};

void DisplayCalibration(
    IplImage* displayImage, 
    const QLCalibrationTarget* targets, 
    const QLCalibrationScore* leftScores, 
    const QLCalibrationScore* rightScores,
    int numTargets,
    const char * windowName)
{
    int tx = 0;
    int ty = 0;

    //Clear the calibration window
    memset(displayImage->imageData, 128, displayImage->imageSize);

    //Loop through each target.
    printf_s("\nCalibration Scores\n");
	for (int i = 0; i < numTargets; i++)
    {
        // Draw the target positions
        tx = (int)targets[i].x * displayImage->width / 100;
        ty = (int)targets[i].y * displayImage->height / 100;
        DrawTarget(displayImage, cvPoint(tx, ty),  20, CV_RGB(50, 50, 50));

        // Draw the left 
        tx = (int)(targets[i].x + leftScores[i].x) * displayImage->width / 100;
        ty = (int)(targets[i].y + leftScores[i].y) * displayImage->height / 100;
        DrawCross(displayImage, cvPoint(tx, ty), 10, CV_RGB(0, 255, 255), 1);

        // Draw the right 
        tx = (int)(targets[i].x + rightScores[i].x) * displayImage->width / 100;
        ty = (int)(targets[i].y + rightScores[i].y) * displayImage->height / 100;
        DrawCross(displayImage, cvPoint(tx, ty), 10, CV_RGB(255, 0, 0), 1);

	    cvShowImage(windowName, displayImage);

        printf_s("%2.2f %2.2f ", leftScores[i].score, rightScores[i].score);
    }
    printf_s("\n");


};

bool CalibrateTarget(
    QLCalibrationId calibrationId,
    QLCalibrationTarget target,
    IplImage* displayImage,
    const char* windowName)
{
    // Loop on this target until eye data was successfully collected for each
    // eye.
    QLCalibrationStatus status = QL_CALIBRATION_STATUS_OK;
    do
    {
	    //Clear the calibration window
	    memset(displayImage->imageData, 128, displayImage->imageSize);

	    // Display the cleared image buffer in the calibration window.
	    cvShowImage(windowName, displayImage);

        // Wait for a little bit so show the blanked screen.
        if(cvWaitKeyEsc == cvWaitKey(100))
            return false;

        // The target positions are in percentage of the area to be tracked so
        // we need to scale to the calibration window size.
        int tx = (int)target.x * displayImage->width / 100;
        int ty = (int)target.y * displayImage->height / 100;

	    // Draw a target to the image buffer
	    DrawTarget(displayImage, cvPoint(tx, ty),  20, CV_RGB(0, 255, 0));
    		
	    // Display the image buffer in the calibration window.
	    cvShowImage(windowName, displayImage);

	    // Wait a little bit so the user can see the target before we calibrate.
	    cvWaitKey(250);

        // Calibrate the target for 1000 ms. This can be done two ways; blocking and
        // non-blocking. For blocking set the block variable to true. for
        // non-blocking set it to false. 
	    bool block = false;
        QLCalibration_Calibrate(calibrationId, target.targetId, 1500, block);

        // When non-blocking is used, the status of the target needs to be
        // polled to determine when it has finished. During the polling we can
        // do other things like querying user input as we do here to see if
        // the user wants to quit the calibration.
        int keyPressed = 0;
        while(!block &&
            ((keyPressed = cvWaitKey(10)) != cvWaitKeyEsc) &&
            (QLCalibration_GetStatus(calibrationId, target.targetId, &status) == QL_ERROR_OK) &&
            (status == QL_CALIBRATION_STATUS_CALIBRATING));

        // If the user terminated the calibration early then return false.
        if(keyPressed == cvWaitKeyEsc)
            return false;

        // Get the status of the target.
        QLCalibration_GetStatus(calibrationId, target.targetId, &status);
    }while(status != QL_CALIBRATION_STATUS_OK);

    // Return true to indicate that the target has successfully been calibrated
    return true;
};

bool AutoCalibrate(
    QLDeviceId deviceId, 
    QLCalibrationType calibrationType,  
    QLCalibrationId* calibrationId)
{
    QLError qlerror = QL_ERROR_OK;

    // Initialize the calibration using the inputed data.
    qlerror = QLCalibration_Initialize(deviceId, *calibrationId, calibrationType);

    // If the calibrationId was not valid then create a new calibration
    // container and use it.
    if(qlerror == QL_ERROR_INVALID_CALIBRATION_ID)
    {
        QLCalibration_Create(0, calibrationId);
        qlerror = QLCalibration_Initialize(deviceId, *calibrationId, calibrationType);
    }

    // If the initialization failed then print an error and return false.
    if(qlerror == QL_ERROR_INVALID_DEVICE_ID)
    {
        printf_s("QLCalibration_Initialize() failed with error code %d.\n", qlerror);
        return false;
    }

    // Create a buffer for the targets. This just needs to be large enough to
    // hold the targets. 
    const int bufferSize = 20;
    int numTargets = bufferSize;
    QLCalibrationTarget targets[bufferSize];

    // Get the targets. After the call, numTargets will contain the number of
    // actual targets. 
    qlerror = QLCalibration_GetTargets(*calibrationId, &numTargets, targets);

    // If the buffer was not large enough then print an error and return false.
    if(qlerror == QL_ERROR_BUFFER_TOO_SMALL)
    {
        printf_s(
            "The target buffer is too small. It should be at least %d bytes.\n", 
            numTargets * sizeof(QLCalibrationTarget));
        return false;
    }
    

    // Use OpenCV to create a window for doing the calibration. The calibration
    // will only be valid over the area of this window. If the entire screen
    // area is to be calibrated then this window should be set to the screen
    // size. 
    int windowWidth = 1024;
    int windowHeight = 768;
    const char* windowName = "Calibration Window";
    IplImage* displayImage = cvCreateImage(cvSize(windowWidth, windowHeight), 8, 3);
    cvNamedWindow(windowName, CV_WINDOW_AUTOSIZE);
	cvMoveWindow(windowName, 0, 0);
	cvResizeWindow(windowName, windowWidth, windowHeight);

    //Clear the calibration window
    memset(displayImage->imageData, 128, displayImage->imageSize);

    // Create a font for printing to the window.
    CvFont font;
    cvInitFont(&font,CV_FONT_HERSHEY_SIMPLEX, .5, .5, 0, 1);

    // Print a message to the image.
    int lineSize = 20;
    int lineIndent = 10;
    cvPutText(
        displayImage, 
        "The calibration is dependant on the location of the calibration area.",
        cvPoint(lineIndent, lineSize * 1),
        &font,
        CV_RGB(255, 255, 255));
    cvPutText(
        displayImage, 
        "Move the window to the area of the screen you would like calibrate.",
        cvPoint(lineIndent, lineSize * 2),
        &font,
        CV_RGB(255, 255, 255));
    cvPutText(
        displayImage, 
        "Press ENTER when ready.",
        cvPoint(lineIndent, lineSize * 3),
        &font,
        CV_RGB(255, 255, 255));
    cvPutText(
        displayImage, 
        "Press ESC at any time to terminate the calibration",
        cvPoint(lineIndent, lineSize * 4),
        &font,
        CV_RGB(255, 255, 255));

    // Display the image to the window.
    cvShowImage(windowName, displayImage);

    // Wait for the user to place the window and press a key.
    if(cvWaitKey() == cvWaitKeyEsc)
    {
        QLCalibration_Cancel(*calibrationId);
	    cvReleaseImage(&displayImage);
	    cvDestroyWindow(windowName);
        return false;
    }

    // Loop through each target and calibrate them
	for (int i = 0; i < numTargets; i++) 
	{
        // Calibrate each target. If the user terminates the calibration then
        // return false.
        if(!CalibrateTarget(
            *calibrationId, 
            targets[i],
            displayImage,
            windowName))
        {
            QLCalibration_Cancel(*calibrationId);
			cvReleaseImage(&displayImage);
			cvDestroyWindow(windowName);
            return false;
        }
            
    }

    // Get the scores and display them. If the user wants to improve the
    // calibration then recalibrate the worst target and loop through again.
    int keyPress = 0;
    do
    { 
        // When all calibration targets have been successfully calibrated then get
        // the scoring. Scores can only be calculated once all targets have been
        // calibrated.
        QLCalibrationScore leftScores[bufferSize];
        QLCalibrationScore rightScores[bufferSize];
	    for (int i = 0; i < numTargets; i++) 
	    {
            QLCalibration_GetScoring(
                *calibrationId, 
                targets[i].targetId, 
                QL_EYE_TYPE_LEFT, 
                leftScores + i);

            QLCalibration_GetScoring(
                *calibrationId, 
                targets[i].targetId, 
                QL_EYE_TYPE_RIGHT, 
                rightScores + i);
        }

        // Display the calibration results graphically.
        DisplayCalibration(displayImage, targets, leftScores, rightScores, numTargets, windowName);

        // Wait for user input to determine what to do.
        keyPress = cvWaitKey();

        // If the user wants to improve the calibration then determine which
        // target has the largest score and recalibrate it.
        if(keyPress == 'i')
        {
            float highestScore = 0;
            int highestIndex = 0;
	        for (int i = 0; i < numTargets; i++) 
	        {
                if(leftScores[i].score > highestScore)
                {
                    highestScore = leftScores[i].score;
                    highestIndex = i;
                }

                if(rightScores[i].score > highestScore)
                {
                    highestScore = rightScores[i].score;
                    highestIndex = i;
                }
            }

            // Calibrate the target. If the user terminates the calibration then
            // return false.
            if(!CalibrateTarget(
                *calibrationId, 
                targets[highestIndex],
                displayImage,
                windowName))
            {
                QLCalibration_Cancel(*calibrationId);
			    cvReleaseImage(&displayImage);
			    cvDestroyWindow(windowName);
                return false;
            }
        }

    }while(keyPress == 'i');

    // If the user would not like to save the calibration then cancel the
    // calibration and return false.
    if(keyPress == cvWaitKeyEsc)
    {
        QLCalibration_Cancel(*calibrationId);
		cvReleaseImage(&displayImage);
		cvDestroyWindow(windowName);
        return false;
    }


    QLCalibration_Finalize(*calibrationId);
	cvReleaseImage(&displayImage);
	cvDestroyWindow(windowName);
    return true;
}
