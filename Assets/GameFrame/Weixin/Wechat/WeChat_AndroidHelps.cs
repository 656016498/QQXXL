using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeChat_AndroidHelps : MonoBehaviour
{
    public static WeChat_AndroidHelps Instance;

    [RuntimeInitializeOnLoadMethod]
    private static void AutoInit()
    {
        GameObject obj = new GameObject("WeChat_AndroidHelps");
        obj.AddComponent<WeChat_AndroidHelps>();
        DontDestroyOnLoad(obj);
    }

    private void Awake()
    {
        Instance = this;
    }


    public delegate void Callback(string message);
    

    public Callback CompleteCallback;

    public Callback FaildCallback;

    public void OnCompleteCallback(string message)
    {
        Debug.Log("安卓回调:" + message);
        if (CompleteCallback!=null)
        {
            CompleteCallback(message);
        }
        
        //MyGameInfo.isVedioing = false;
        //CheckRun.isBanPlay = false;
    }

    public void OnFaildCallback(string message)
    {
        Debug.Log("安卓回调:" + message);
        if (FaildCallback!=null)
        {
            FaildCallback(message);
        }
       
    }

}
