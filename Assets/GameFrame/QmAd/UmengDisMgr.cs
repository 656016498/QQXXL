using BayatGames.SaveGamePro;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 友盟打点区分管理
/// </summary>
/// 人数统计
/// 次数统计
public class UmengDisMgr
{
    private static UmengDisMgr mInstance;

    public static UmengDisMgr Instance
    {
        get
        {
            if(mInstance==null)
            {
                mInstance = new UmengDisMgr();
            }
            return mInstance;
        }
    }

    private static string LocalKey = "GAME_UMENGDISMGR_KEY";
    private static string Localkey2 = "GAME_UMENGDISMGR_KEY2";

    private Dictionary<string, bool> mDic_onPeople;
    private Dictionary<string, int> mDic_OnNumber;
    public UmengDisMgr()
    {
        LoadData();
    }
    

    /// <summary>
    /// 人数统计
    /// </summary>
    /// <param name="key"></param>
    /// <param name="table"></param>
    public void CountOnPeoples(string key,string table=null)
    {
        if (key ==null)
        {
            Debug.Log("埋点key为null");
            return;
        }
        var dicKey = key + table;
        var mbool = mDic_onPeople.ContainsKey(dicKey);
        if (mbool) return;

        mDic_onPeople.Add(dicKey, true);
        Debug.Log("埋点:打点人数:" + key+"_"+table);
        if (table==null)
        {
            AdControl.Instance.SendEvent(key);
        }
        else
        {
            AdControl.Instance.SendEvent(key, table);
        }
        

        SaveData();
    }
    /// <summary>
    /// 次数统计
    /// </summary>
    /// <param name="key"></param>
    /// <param name="table"></param>
    public void CountOnNumber(string key,string table=null)
    {
        if (key==null)
        {
            Debug.Log("埋点key为null");
            return;
        }
        if (mDic_OnNumber.ContainsKey(key))
        {
             var mvalue = mDic_OnNumber[key] + 1;
            mDic_OnNumber[key] = mvalue;
        }
        else
        {
            mDic_OnNumber.Add(key, 1);
        }

        Debug.Log("埋点:打点次数:" + key + "_" + table);
        if (table == null)
        {
            AdControl.Instance.SendEvent(key);
        }
        else
        {
            AdControl.Instance.SendEvent(key, table);
        }
        

        SaveData();
    }


    private void LoadData()
    {
        mDic_onPeople = SaveGame.Load<Dictionary<string, bool>>(LocalKey);
        if(mDic_onPeople==null)
        {
            mDic_onPeople = new Dictionary<string, bool>();
        }
        mDic_OnNumber = SaveGame.Load<Dictionary<string, int>>(Localkey2);
        if(mDic_OnNumber==null)
        {
            mDic_OnNumber = new Dictionary<string, int>();
        }
        SaveData();
    }

    private void SaveData()
    {
        if (mDic_onPeople == null) return;

        SaveGame.Save(LocalKey, mDic_onPeople);
        SaveGame.Save(Localkey2, mDic_OnNumber);
    }
}

