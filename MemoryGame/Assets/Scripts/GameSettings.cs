using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    private static string s_DifficultyLevel = "DifficultyLevel";
    public static int DifficultyLevel
    {
        get => PlayerPrefs.GetInt(s_DifficultyLevel, 3);
        set => PlayerPrefs.SetInt(s_DifficultyLevel, value);
    }
    private static string s_DevMode = "DevMode";
    public static int DevMode
    {
        get => PlayerPrefs.GetInt(s_DevMode, 0);
        set
        {
            if (value == 0 || value == 1)
            {
                PlayerPrefs.SetInt(s_DevMode, value);
            }
        }
    }
    
}
