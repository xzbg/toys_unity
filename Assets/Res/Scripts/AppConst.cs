using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum GameState
{
    ON,
    OFF
}

public class AppConst
{
    // 游戏状态
    public static GameState gameState = GameState.OFF;
}
