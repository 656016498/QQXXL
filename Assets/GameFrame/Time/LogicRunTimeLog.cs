using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 对Dictory的拓展
/// </summary>
public static class DictionaryExtension
{


    /// <summary>
    /// 尝试根据Key得到Value,得到的话直接返回value,没有则返回null
    /// this Dictionary<Tkey,Tvalue> dict 这个字典表示我们要获取值的字典
    /// </summary>
    /// <typeparam name="Tkey"></typeparam>
    /// <typeparam name="Tvalue"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Tvalue TryGet<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key)
    {
        Tvalue value;
        dict.TryGetValue(key, out value);
        return value;
    }

    public static void AddData<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key, Tvalue value)
    {
        if (dict.ContainsKey(key))
        {
            dict[key] = value;
        }
        else
        {
            dict.Add(key, value);
        }
    }

    public static void RemoveKey<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dic, Tkey key)
    {
        if (dic.ContainsKey(key))
        {
            dic.Remove(key);
        }
    }

}
