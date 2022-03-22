using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class EnterGame : MonoBehaviour
{
    public SliderControl mSliderControl;
    public Image mMask;
    public GameObject TopAni;

    private AsyncOperation LoadInfo;
    float nowProgress = 0;      //现在的加载进度

    private IEnumerator Start()
    {
        GameADControl.Instance.ShowSplash();
        UmengDisMgr.Instance.CountOnPeoples("FirstEnterLoading");

        StartCoroutine("LoadScense");

        mMask.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(0.5f);        
        mMask.DOColor(new Color(0, 0, 0, 0), 0.5f).SetEase(Ease.Linear);
        TopAni.SetActive(true);
        yield return new WaitForSeconds(0.5f);//黑色遮罩渐变退出                
        StartCoroutine("LoadAni");
    }


    private IEnumerator LoadScense()
    {      
        var delay = new WaitForSeconds(0.01f);
        mSliderControl.SetSlider(0);
        LoadInfo= SceneManager.LoadSceneAsync("MainScene");
        LoadInfo.allowSceneActivation = false; 
        nowProgress = 0;
        while (!LoadInfo.isDone)
        {
            nowProgress = LoadInfo.progress;
            if (nowProgress>=0.9f)
            {
                break;
            }
        }
        yield return null;

    }

    private IEnumerator LoadAni()
    {
        var delay = new WaitForSeconds(0.01f);
        var delay2 = new WaitForSeconds(0.1f);
        float nowValue = 0;
        float onceAdd = 0.01f;
        while (nowValue<1)
        {
            nowValue += onceAdd;
            mSliderControl.SetSlider(nowValue);
            yield return nowValue < (nowProgress<0.9f?nowProgress:1) ? delay : delay2;
        }
        while (nowProgress<0.9f)
        {
            yield return delay;
        }
        mMask.DOColor(new Color(0, 0, 0, 1), 0.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.5f);
        if (LoadInfo!=null)
        {
            LoadInfo.allowSceneActivation = true;
        }       
        yield return null;
    }
}
