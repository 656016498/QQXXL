using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


/// <summary>
/// 监听游戏运行状态
/// </summary>
public class ListenGameRunState : MonoBehaviour
{

    /// <summary>
    /// 游戏运行状态-是否处于前台
    /// </summary>
    public static ReactiveProperty<bool> GameStatue_RunForward = new ReactiveProperty<bool>(true);

    /// <summary>
    /// 自动启动
    /// </summary>
    //[RuntimeInitializeOnLoadMethod]    
    public static void AutoInit()
    {
        if (Application.isPlaying)
        {
            GameObject obj = new GameObject("ListenGameRunState");
            obj.AddComponent<ListenGameRunState>();
            DontDestroyOnLoad(obj);
        }      
    }

    private void OnApplicationPause(bool isShow)
    {
        GameStatue_RunForward.Value = !isShow;
    }
}
