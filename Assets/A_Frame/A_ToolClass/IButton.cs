using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;
using DG.Tweening;
public class IButton :Button
{
    bool CanClick;
    // Start is called before the first frame update
    protected override void Awake()
    {
        onClick.AddListener(() => {


            AudioMgr.Instance.PlaySFX("onClick");

            if (transform.GetComponent<Image>() != null)
            {
                transform.GetComponent<Image>().raycastTarget = false;
            }
            Observable.TimeInterval(System.TimeSpan.FromSeconds(0.2F)).Subscribe(_ =>
            {
                if (transform.GetComponent<Image>() != null)
                {
                    transform.GetComponent<Image>().raycastTarget = true;
                }
            });
        });
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        transform.DOScale(Vector3.one * 0.8F, 0.1F);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        transform.DOScale(Vector3.one * 1.1f, 0.1F).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOScale(Vector3.one * 0.9F, 0.1F).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.DOScale(Vector3.one * 1, 0.05F).SetEase(Ease.Linear);
            });
        });

    }
}
