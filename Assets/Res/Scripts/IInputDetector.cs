public interface IInputDetector
{
    InputDirection? DetectInputDirection();
}

public enum InputDirection
{
    Left, Right, Top, Bottom
}

public enum TouchState
{
    SwipeWait,
    SwipeStarted
}