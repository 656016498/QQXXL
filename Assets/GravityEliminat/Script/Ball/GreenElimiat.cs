using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class GreenElimiat : Prop
{
    //public Porp_Size SizeType;
    //public override int Eliminat()
    //{
    //    return 0;
    //}
    public float Radius;

    public override  void Init(object[] obj) {
        base.Init(obj);
        if (obj != null)
        {
            //isCreat = true;
            SizeType = (Porp_Size)obj[0];
            Gear = (int)obj[1];
            //transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PropSprite/" + PropType.ToString() + Gear);
        }
    }

    public override void InitReady()
    {
        //base.InitReady();
        
    }

    public override void OnClick()
    {
        base.OnClick();
        if (!canTiggle) return;
        canTiggle = false;
        PropCollectBall();
     
        GameManager.Instance.ElimintNoColorlBall();
        if (!GameManager.Instance.OverGame)
        {
            //CameraManager.Instance.SetShake(shakeLevel * ((int)Gear), (shakeTimes));
            CameraManager.Instance.SetShake(PropManger.Instance.GetShackLevel(PropType.ToString(), Gear, SizeType), PropManger.Instance.GetShackTime(PropType.ToString(), Gear, SizeType));

        }
    }

    public override Collider2D[] DetectionRange()
    {

        float[] tempRange = PropManger.Instance.GetChildRang(PropType.ToString(), Gear, SizeType);
        float effectSize = PropManger.Instance.GetEffectSize(PropType.ToString(), Gear, SizeType);
        Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.GreenBombEffect, transform.position, effectSize);
        return Physics2D.OverlapCircleAll(transform.position, tempRange[0]);

    }

    
}
