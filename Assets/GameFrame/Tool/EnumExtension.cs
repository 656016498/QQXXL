using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumExtension 
{
    /// <summary>
    /// 获取枚举类型的长度
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public static int GetEnumLength(Type e)
    {
        return System.Enum.GetNames(e).Length;
    }    
}
