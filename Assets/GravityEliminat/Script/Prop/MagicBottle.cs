using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class MagicBottle : Prop,CanClick
{
    public override void Init(object[] obj = null)
    {
        base.Init(obj);
        CanFusion = false;
        canTiggle = false;
        transform.GetComponent<CircleCollider2D>().enabled = true;
        //transform.gameObject.layer= LayerMask.NameToLayer("UI");
        //transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("UI");
        //transform.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("UI");
        //transform.GetComponent<SpriteRenderer>().sortingOrder = 50;
        //transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("UI");
        //transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 50;
        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.5F)).Subscribe(_ => { 
        
        transform.GetChild(1).gameObject.SetActive(true);

        });
    }

    public override void OnClick()
    {
        base.OnClick();

        if (GameManager.Instance.CurrentLevel == DataManager.Instance.data.UnlockLevel)
        {
            UmengDisMgr.Instance.CountOnNumber("dj_magic_use", DataManager.Instance.data.UnlockLevel.ToString());
        }
        AudioMgr.Instance.PlaySFX("使用蓄力技能");
        GameManager.Instance.EMostOfSameColor(transform.position);
        Pool.Instance.Despawn(Pool.Prop_PoolName, transform);
        if (!GameManager.Instance.OverGame)
        {
            GameManager.Instance.magicBottles.Remove(this);
        }
       
        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.3F)).Subscribe(_ => {
            UIRoot.Instance.HideMask();
        });

    }
    public override void InitReady()
    {
        base.InitReady();
    }






}
