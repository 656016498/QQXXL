using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIBase : GetOjbectBace
{

    public UIType type = UIType.Normal;
    protected CanvasGroup canvasGroup;
    Animator pageAnim;
    public virtual void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup==null)
        {
            canvasGroup=gameObject.AddComponent<CanvasGroup>();
        }
        if (transform.childCount > 1 && transform.GetChild(1).GetComponent<Animator>() != null)
        {
            pageAnim = transform.GetChild(1).GetComponent<Animator>();
        }
        else if (transform.GetChild(0).GetComponent<Animator>() != null)
        {
            pageAnim = transform.GetChild(0).GetComponent<Animator>();
        }
    }

    //public virtual void Start() { }

    public virtual void Show() {
        //canvasGroup.alpha = 1;
        if (!this.gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        canvasGroup.DOFade(1, 0.2f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        //播放弹窗动画
        if (pageAnim!=null)
        {
            pageAnim.Play("Open");
            AudioMgr.Instance.PlaySFX("弹出弹窗");
        }
    }

    public virtual void Show(object data) {

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true; 
        canvasGroup.blocksRaycasts = true;

    }

    public virtual void Refresh() { }

    public virtual void Hide() {

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        //弹窗关闭动画
        if (pageAnim!=null)
        {
            pageAnim.Play("Close");
        }
        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ =>
            {
                this.gameObject.SetActive(false);
            });
    }

 

}
