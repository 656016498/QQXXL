using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static  class XDebug 
{
    static bool  Open = true;
    public static void Log(string info) {

        if (Open)
        {
            Debug.Log(info);
        }
    }

    public static void LogError(string info)
    {

        //#if UNITY_EDITOR
        if (Open)
        {

            Debug.LogError(info);
        }
    }
    public static void LogWarning(string info)
    {
        if (Open)
        {
            Debug.LogWarning(info);

        }
    }
}
