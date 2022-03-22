//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AddressableAssets;

//public class CheckConfigUpdate : MonoBehaviour
//{
//    // Start is called before the first frame update
//    void Start()
//    {
//        StartCoroutine(Check());
//    }
    
//    IEnumerator Check()
//    {
//        var sizeHandle = Addressables.GetDownloadSizeAsync("Config");
//        yield return sizeHandle;
//        long totalDownloadSize = sizeHandle.Result;
//        Debug.Log("更新Size："+totalDownloadSize);
//        if (totalDownloadSize > 0)
//        {
//            var downloadHandle = Addressables.DownloadDependenciesAsync("Config", Addressables.MergeMode.Union);
//            while (!downloadHandle.IsDone)
//            {
//                float percent = downloadHandle.PercentComplete;
//                Debug.Log($"已经下载：{(int)(totalDownloadSize * percent)}/{totalDownloadSize}");
//            }
//        }
//        //更新完成，加载配置进入游戏。
//        var c = ConfigMgr.Instance;
//    }
    
//}
