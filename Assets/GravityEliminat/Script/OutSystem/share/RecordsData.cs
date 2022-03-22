using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordsData 
{
    public RecordsData(string k, string t, float r, int s)
    {
        key = k;
        time = t;
        rmb = r;
        state = s;
    }
    //key
    public string key;
    //时间
    public string time;
    //金额
    public float rmb;
    //状态
    public int state;

    public override bool Equals(object obj)
    {
        if (obj == null || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            RecordsData data = (RecordsData)obj;
            return (this.key == data.key && time == data.time && rmb == data.rmb && state == data.state);
        }
    }


    public override int GetHashCode()
    {
        return this.key.GetHashCode();
    }
}
