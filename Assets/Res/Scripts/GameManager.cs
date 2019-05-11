using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public IInputDetector touchIput;
    public IInputDetector arrowKeyInput;
    // Use this for initialization
    void Start()
    {
        touchIput = gameObject.AddComponent<TouchInputDetector>();
        arrowKeyInput = gameObject.AddComponent<ArrowKeyInputDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (AppConst.gameState == GameState.ON)
        {
            InputDirection? input = touchIput.DetectInputDirection();
            if (input.HasValue)
            {
                Debug.Log(input.Value);
                return;
            }
            input = arrowKeyInput.DetectInputDirection();
            if (input.HasValue)
            {
                Debug.Log(input.Value);
            }
        }
    }

}
