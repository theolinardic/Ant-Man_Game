using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraSplit : MonoBehaviour
{
    [Range(-1, 1)]
    public float splitScreenPosition = 0;

    public Camera leftCam, rightCam;

    public Image splitScreenBar;

    float leftCamRectW, rightCamRectX, rightCamRectW;

    bool shouldMove;
    float newPos, currentPos, lerpValue = 0;
    int i = 0;
    // Camera Positions

    // Middle:
    // splitscreenBarPosition = 0
    // leftCam.rect = 0, 0, 0.5, 1
    // rightCam.rect = 0.5, 0, 0.5, 1

    // Left Cam all the way to the Right:
    // splitScreenBarPosition = 1
    // leftCam.rect = 0, 0, 0.71, 1
    // rightCam.rect = 0.71, 0, 0.295, 1

    // Left Cam all the way to the Left:
    // splitScreenBarPosition = -1
    // leftCam.rect = 0, 0, 0.295, 1
    // rightCam.rect = 0.29, 0, 0.71, 1

    // Update is called once per frame
    void Update()
    {
        // tempPosition finds the value of the current split screen bar position in between its maximum and minimum
        // with the formula (x - a) / (b - a)    x = splitScreenPos, a = minimum screenBarPos, b = maximum screenBarPos
        var tempPositionCam1 = ((splitScreenPosition - (-1)) / (1 - (-1)));

        // leftCamRectW finds the width that the left camera should be based on the tempPosition value
        // with the formula (-(a) * c) + (b * c) + a      a = minimum length value, b = maximum length value, c = tempPosition
        leftCamRectW = (-0.295f * tempPositionCam1) + (0.71f * tempPositionCam1) + (0.295f);

        // bar needs to be between -400 and 400 so we multiply the splitScreenPosition by 400 to find the x position it should be at
        splitScreenBar.transform.localPosition = new Vector3(splitScreenPosition * 400f, 0, 0);

        rightCamRectX = (-0.29f * tempPositionCam1) + (0.71f * tempPositionCam1) + (0.29f);

        rightCamRectW = (-0.71f * tempPositionCam1) + (0.295f * tempPositionCam1) + (0.71f);

        // Update left camera rect based on new found leftCamRectW
        leftCam.rect = new Rect(0, 0, leftCamRectW, 1);
        //Update right camera rect based on new found rightCamRectX and rightCamRectW
        rightCam.rect = new Rect(rightCamRectX, 0, rightCamRectW, 1);

        if (shouldMove)
        {
            /*   if(lerpValue == 0)
               {
                   lerpValue = 0.01f;
                   currentPos = splitScreenPosition;
               }
               if(lerpValue < 1)
               {
                   splitScreenPosition = Mathf.Lerp(currentPos, newPos, lerpValue);
                   lerpValue += (Time.deltaTime * 0.1f);
               }
               if(lerpValue > 1)
               {
                   shouldMove = false;
                   lerpValue = 0;
               }
            */

            if (newPos < splitScreenPosition)
            {
                if (splitScreenPosition > newPos)
                {
                    splitScreenPosition -= 0.001f;
                }
                else
                {
                    shouldMove = false;
                }
            }
            else
            {
                if (splitScreenPosition < newPos)
                {
                    splitScreenPosition += 0.001f;
                }
            }
        }

    }

    public void moveSplitCam(float newPosition)
    {
        newPos = newPosition;
        lerpValue = 0;
        shouldMove = true;
    }
}
