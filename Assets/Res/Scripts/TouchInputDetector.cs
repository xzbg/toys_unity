using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 手机触屏操作
/// </summary>
public class TouchInputDetector : MonoBehaviour, IInputDetector
{
    private TouchState state = TouchState.SwipeNotStarted;
    private Vector2 startPoint;
    private DateTime timeSwipeStarted;
    private TimeSpan maxSwipeDuration = TimeSpan.FromSeconds(1);
    private TimeSpan minSwipeDuration = TimeSpan.FromMilliseconds(100);

    // 触屏开始
    private void TouchStart(Vector2 touchPostion)
    {
        timeSwipeStarted = DateTime.Now;
        state = TouchState.SwipeStarted;
        startPoint = touchPostion;
    }

    // 触屏结束
    public InputDirection? TouchEnd(Vector2 touchPostion)
    {
        TimeSpan timeDifference = DateTime.Now - timeSwipeStarted;
        Debug.LogFormat("timeDifference={0},maxSwipeDuration={1},minSwipeDuration={2}", timeDifference.TotalMilliseconds, maxSwipeDuration.TotalMilliseconds, minSwipeDuration.TotalMilliseconds);
        if (timeDifference <= maxSwipeDuration && timeDifference >= minSwipeDuration)
        {
            Vector2 differenceVector = touchPostion - startPoint;
            if (differenceVector.magnitude <= 100)
                return null;
            float angle = Vector2.Angle(differenceVector, Vector2.right);
            Vector3 cross = Vector3.Cross(differenceVector, Vector2.right);
            if (cross.z > 0)
                angle = 360 - angle;
            if ((angle >= 315 && angle < 360) || (angle >= 0 && angle <= 45))
                return InputDirection.Right;
            else if (angle > 45 && angle <= 135)
                return InputDirection.Top;
            else if (angle > 135 && angle <= 225)
                return InputDirection.Left;
            else
                return InputDirection.Bottom;
        }
        return null;
    }


    public InputDirection? DetectInputDirection()
    {
        if (state == TouchState.SwipeNotStarted)
        {
            if (Application.isMobilePlatform && Input.touchCount > 0)
            {
                TouchStart(Input.GetTouch(0).position);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                TouchStart(Input.mousePosition);
            }
        }
        else if (state == TouchState.SwipeStarted)
        {
            InputDirection? input = null;
            if (Application.isMobilePlatform && Input.touchCount > 0)
            {
                input = TouchEnd(Input.GetTouch(0).position);
                state = TouchState.SwipeNotStarted;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                input = TouchEnd(Input.mousePosition);
                state = TouchState.SwipeNotStarted;
            }
            return input;
        }
        return null;
    }
}
