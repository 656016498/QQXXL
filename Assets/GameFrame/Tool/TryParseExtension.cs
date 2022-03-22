using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using LitJson;
using System;

public static class TryParseExtension
{
    public static int TryParseInt(this string str)
    {
        int.TryParse(str, out int i);
        return i;
    }


    public static bool TryParseBool(this string str)
    {
        bool.TryParse(str, out bool b);
        return b;
    }

    public static bool TryParseIntToBool(this string str)
    {
        var mint = str.TryParseInt();
        return mint == 0 ? false : true;
    }

    public static double TryParseDouble(this string str)
    {
        double.TryParse(str, out double d);
        return d;
    }

    public static float TryParseFloat(this string str)
    {       
        var mBool = float.TryParse(str, out float f);
        if (mBool == false)
        {            
            str = str.AutoCheckString();
            float.TryParse(str, out f);
        }
        return f;       
    }

    //自动检查纠正字符串
    public static string AutoCheckString(this string str)
    {
        XDebug.Log("字符串错误,正在尝试纠正字符串...");
        if (str.Contains("\""))
        {
            str = str.Replace("\"", "");
            XDebug.Log("纠正成功,多余字符:\"");
        }
        return str;
    }


    /// <summary>
    /// 保留几位小数
    /// </summary>
    /// <param name="value"></param>
    /// <param name="keepN">小数点后几位</param>
    /// <returns></returns>
    public static float GetFloat(this float value, int keepN)
    {
        var mstr = value.ToString(string.Format("f{0}", keepN));
        float.TryParse(mstr, out float mfloat);
        return mfloat;
    }
    public static float GetFloat(this string mstr)
    {
        float.TryParse(mstr, out float mfloat);
        return mfloat;
    }

    /// <summary>
    /// 获取小数
    /// </summary>
    /// <param name="md"></param>
    /// <param name="ratio">比例</param>
    /// <returns></returns>
    public static float GetFloat(this double md, int ratio)
    {
        var value = md / ratio;
        return GetFloat(value.ToString()).GetFloat(2);
    }

    public static float ConvertFloat(double md, int ratio)
    {
        return md.GetFloat(ratio);
    }

    public static float ConvertFloat(int mint, int ratio)
    {
        return mint.GetRationValue(ratio);
    }


    /// <summary>
    /// 获取比例后的数值
    /// </summary>
    /// <param name="mInt"></param>
    /// <param name="ratio"></param>
    /// <returns></returns>
    public static float GetRationValue(this int mInt, int ratio)
    {
        var value = mInt / (float)ratio;
        return GetFloat(value.ToString()).GetFloat(2);
    }

    public static float IntToFloat(this int mInt)
    {
        return mInt * 1.0f;
    }
}
