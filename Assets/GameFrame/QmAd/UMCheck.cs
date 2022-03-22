using BayatGames.SaveGamePro;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public static class UMCheck 
{
    public class UmengSave
    {
        public Dictionary<string, Dictionary<string, int>> umengKeyLabel = new Dictionary<string, Dictionary<string, int>>();
        public Dictionary<string, int> umengKey = new Dictionary<string, int>();
    }

    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        //友盟统计日志初始化
        var localUmengSave = SaveGame.Load<UmengSave>("UmengSave.dat");
        if (localUmengSave != null)
        {
            Observable.TimeInterval(TimeSpan.FromSeconds(0.5f)).Subscribe(_ => {
                umengSave = localUmengSave;
                printJson2();
            });
            
        }
        
    }
    public static void printJson2()
    {
        Debug.Log("获取埋点日志:" + (umengSave == null));
        try
        {
            //string Contentjson = JsonConvert.SerializeObject(umengSave.umengKey);
            string Contentjson = LitJson.JsonMapper.ToJson(umengSave.umengKey);
            Debug.Log("key:" + Contentjson);
        }
        catch (Exception e)
        {
            Debug.LogError("key:" + e.Message);

        }
        Dictionary<string, Dictionary<string, int>> keyLabel1 = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, Dictionary<string, int>> keyLabel2 = new Dictionary<string, Dictionary<string, int>>();
        int i = 0;
        foreach (var item in umengSave.umengKeyLabel)
        {
            if (i < umengSave.umengKeyLabel.Count / 2)
            {
                keyLabel1.Add(item.Key, item.Value);
            }
            else
            {
                keyLabel2.Add(item.Key, item.Value);
            }
            i++;
        }
        //string Contentjson2 = JsonConvert.SerializeObject(umengSave.umengKeyLabel);
        string str1 = LitJson.JsonMapper.ToJson(keyLabel1);
        string str2 = LitJson.JsonMapper.ToJson(keyLabel2);

        Debug.Log("keylabel1:" + str1);
        Debug.Log("keylabel2:" + str2);

    }

    public static UmengSave umengSave = new UmengSave();
    public static void SaveUmengLocalData()
    {
        SaveGame.Save<UmengSave>("UmengSave.dat", umengSave);
    }

    public static void SaveUmeng(string key)
    {
        if (!umengSave.umengKey.ContainsKey(key))
        {
            umengSave.umengKey.Add(key, 1);
        }
        else
        {
            umengSave.umengKey[key]++;
        }
        SaveUmengLocalData();
    }

    public static void SaveUmeng(string key, string label)
    {
        if (!umengSave.umengKeyLabel.ContainsKey(key))
        {
            Dictionary<string, int> d = new Dictionary<string, int>();
            d.Add(label, 1);
            umengSave.umengKeyLabel.Add(key, d);
        }
        else
        {
            if (umengSave.umengKeyLabel[key].ContainsKey(label))
            {
                umengSave.umengKeyLabel[key][label]++;
            }
            else
            {
                umengSave.umengKeyLabel[key].Add(label, 1);
            }
        }
        SaveUmengLocalData();
    }

    public static void printJson()
    {
        string Contentjson = JsonConvert.SerializeObject(umengSave.umengKey);
        Debug.Log("key:" + Contentjson);
        Dictionary<string, Dictionary<string, int>> keyLabel1 = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, Dictionary<string, int>> keyLabel2 = new Dictionary<string, Dictionary<string, int>>();
        int i = 0;
        foreach (var item in umengSave.umengKeyLabel)
        {
            if (i< umengSave.umengKeyLabel.Count/2)
            {
                keyLabel1.Add(item.Key, item.Value);
            }else
            {
                keyLabel2.Add(item.Key, item.Value);
            }
            i++;
        }
        //string Contentjson2 = JsonConvert.SerializeObject(umengSave.umengKeyLabel);
        string str1 = JsonConvert.SerializeObject(keyLabel1);
        string str2 = JsonConvert.SerializeObject(keyLabel2);

        Debug.Log("keylabel1:" + str1);
        Debug.Log("keylabel2:" + str2);

    }

}
