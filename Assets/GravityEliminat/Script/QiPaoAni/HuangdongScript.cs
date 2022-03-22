using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HuangdongScript : MonoBehaviour
{

    private RectTransform t;

    private void Start()
    {
       
        t = transform.GetComponent<RectTransform>();
        //toRig();
        t.DOShakeScale(6f, 0.15f, 2, 20).SetLoops(-1, LoopType.Yoyo);

        
    }


    void toRig()
    {
        t.DOScale(new Vector3(0.8f, 0.9f, 1), 1f).OnComplete(() =>
        {
            toLef();
        });
    }

    void toLef()
    {
        t.DOScale(new Vector3(0.9f, 0.8f, 1), 1f).OnComplete(() =>
        {
            toNor();
        });
    }

    void toNor()
    {
        t.DOScale(new Vector3(1f, 1f, 1), 0.5f).OnComplete(() =>
        {
            toRig();
        });
    }
}
