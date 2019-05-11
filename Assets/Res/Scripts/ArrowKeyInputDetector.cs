using UnityEngine;
using System.Collections;

/// <summary>
/// 方向键操作
/// </summary>
public class ArrowKeyInputDetector : MonoBehaviour, IInputDetector
{
    public InputDirection? DetectInputDirection()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
            return InputDirection.Top;
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            return InputDirection.Bottom;
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            return InputDirection.Right;
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
            return InputDirection.Left;
        else
            return null;
    }
}
