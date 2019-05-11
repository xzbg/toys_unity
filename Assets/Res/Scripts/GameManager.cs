using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    // 游戏主界面
    public MainUI mainUI;

    private IInputDetector touchIput;
    private IInputDetector arrowKeyInput;
    // Use this for initialization
    void Start()
    {
        touchIput = gameObject.AddComponent<TouchInputDetector>();
        arrowKeyInput = gameObject.AddComponent<ArrowKeyInputDetector>();
    }

    public void GameStart()
    {
        AppConst.gameState = GameState.ON;
        if (mainUI != null)
        {
            mainUI.GameStart();
        }
    }

    public void GameOver()
    {
        AppConst.gameState = GameState.OFF;
        if (mainUI != null)
        {
            Debug.Log("Game Over!");
            mainUI.GameOver();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (AppConst.gameState == GameState.ON)
        {
            InputDirection? input = touchIput.DetectInputDirection();
            if (input.HasValue)
            {
                if (mainUI != null) mainUI.DoMove(input.Value);
                return;
            }
            input = arrowKeyInput.DetectInputDirection();
            if (input.HasValue)
            {
                if (mainUI != null) mainUI.DoMove(input.Value);
            }
        }
    }

}
