﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum GameState
{
    READY,
    RUN,
    STOP
}

public class AppConst
{
    // 游戏状态
    public static GameState gameState = GameState.STOP;
    public readonly static int Rows = 4;
    public readonly static int Columns = 4;
    public static readonly float AnimationDuration = 0.05f;
}
