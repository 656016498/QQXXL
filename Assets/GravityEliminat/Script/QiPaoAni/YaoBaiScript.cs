using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class YaoBaiScript : MonoBehaviour
{
    private RectTransform t;
    public float time = 1;
    public Vector3 InitY;
    public float addValue;
    
    private void Start()
    {
        InitY = transform.localPosition;
        t = transform.GetComponent<RectTransform>();
        //toRig();
        ToUp();
        Debug.Log("开始摇摆");
    }

    void toRig()
    {
        t.DOLocalRotate(new Vector3(0, 0, 10), time).OnComplete(()=> {
            toLef();
        });
    }

    void toLef()
    {
        t.DOLocalRotate(new Vector3(0, 0, -10), time).OnComplete(() => {
            toRig();
        });
    }

    void ToUp()
    {
        t.DOLocalMove(new Vector3(InitY.x,InitY.y+ addValue, InitY.z), 3).SetEase(Ease.OutBack).OnComplete(() => {
            ToDown();
        });
    }

    void ToDown()
    {
         t.DOLocalMove(InitY, 3).OnComplete(() => {
             ToUp();
        });
    }
}
