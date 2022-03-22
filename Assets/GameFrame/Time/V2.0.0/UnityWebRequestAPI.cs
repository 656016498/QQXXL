using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace GameTime
{
    public class UnityWebRequestAPI : MonoBehaviour
    {

        private static UnityWebRequestAPI mInstance;
        public static UnityWebRequestAPI Instance
        {
            get
            {
                if (mInstance == null)
                {
                    var newobj = new GameObject("UnityWebRequestAPI");
                    mInstance = newobj.AddComponent<UnityWebRequestAPI>();
                    DontDestroyOnLoad(newobj);
                }
                return mInstance;
            }
        }


        public void GetRequest(string url, Action<bool, string> callback)
        {
            StartCoroutine(MGetRequest(url, callback));
        }


        IEnumerator MGetRequest(string uri, Action<bool, string> callback)
        {
            XDebug.Log("请求数据");

            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                if (webRequest.isNetworkError)
                {
                    XDebug.Log(pages[page] + ": Error: " + webRequest.error);
                    callback(false, webRequest.error);
                }
                else
                {
                    XDebug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    callback(true, webRequest.downloadHandler.text);
                }
            }
        }
    }
}