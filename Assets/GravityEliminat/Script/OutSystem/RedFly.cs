using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class RedFly : MonoBehaviour
{
    public static RedFly instance;

    [SerializeField]
    private GameObject mRes;
    [SerializeField]
    private Transform mParent;


    private const float mRandomArea = 0.4f;

    private List<Transform> mlist = new List<Transform>(30);

    private void Awake()
    {
        instance = this;
    }

    public void Play(int m, Vector3 startPoint, Vector3 endPoint, Action Oncomplete)
    {
        this.transform.SetAsLastSibling();
        StartCoroutine(IE_play(m, startPoint, endPoint, Oncomplete));
    }

    private IEnumerator IE_play(int m, Vector3 startPoint, Vector3 endPoint, Action Oncomplete)
    {
        WaitForSeconds delay = new WaitForSeconds(0.05f);
        int showEffect = 3;
        for (int i = 0; i < m; i++)
        {
            var index = i;
            Transform mtrans = null;
            if (mlist.Count == 0)
            {
                var newObj = MonoBehaviour.Instantiate(mRes, mParent, false);
                mtrans = newObj.transform;
            }
            else
            {
                mtrans = mlist[0];
                mlist.RemoveAt(0);
            }
            mtrans.gameObject.SetActive(true);
            mtrans.position = startPoint;
            //随机扩散
            var centerPoint = mtrans.position + new Vector3(Random.Range(-mRandomArea, mRandomArea), Random.Range(-mRandomArea, mRandomArea));
            mtrans.DOMove(centerPoint, 0.3f).OnComplete(() =>
            {
                mtrans.DOMove(endPoint, 0.8f).SetEase(Ease.InBack)
                   .OnComplete(() =>
                   {
                       if (!mlist.Contains(mtrans))
                       {
                           mlist.Add(mtrans);
                           mtrans.gameObject.SetActive(false);
                           showEffect -= 1;
                           if (showEffect<=1)
                           {
                              
                               showEffect = 3;
                           }
                       }
                       Oncomplete.Run();
                   });
            });

            yield return delay;
        }

        //Oncomplete.Run();

        yield return null;
    }
}
