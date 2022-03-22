using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerPrefs扩展
/// </summary>
public static class PlayerPrefsExtension
{    
    public static int GetPlayerPrefsInt(this string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key,defaultValue);
    }
    public static void SetPlayerPrefsInt(this string key,int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static string GetPlayerPrefsString(this string key,string defaultValue=null)
    {
        return PlayerPrefs.GetString(key,defaultValue);
    }

    public static void SetPlayerPrefsString(this string key,string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public static float GetPlayerPrefsFloat(this string key,float defaultValue=0)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    public static void SetPlayerPrefsFloat(this string key,float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public static void DelateKey(this string key)
    {
        PlayerPrefs.DeleteKey(key);
    }


}
