using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathfExtension
{
    /// <summary>
    /// 获取两个数字字符串之间的最大值
    /// </summary>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <returns></returns>
    public static string Max(this string s1,string s2)
    {
        return s1.TryParseInt() > s2.TryParseInt() ? s1 : s2;
    }

    /// <summary>
    /// 取两个数字字符串之间的最小值
    /// </summary>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <returns></returns>
    public static string Min(this string s1,string s2)
    {
        return s1.TryParseInt() < s2.TryParseInt() ? s1 : s2;
    }    


    /// <summary>
    /// 保留小数点后一位
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double Value_F1(this double value)
    {
        return string.Format("{0}:F1", value).TryParseDouble(); 
    }
}
