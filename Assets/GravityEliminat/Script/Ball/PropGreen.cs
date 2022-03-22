using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class PropGreen : Prop
{
    //public Vector2 bombRang;
    public float Radius;
    public List<Vector3> ballsPoint = new List<Vector3>();
    public List<Collider2D> willEli = new List<Collider2D>();
    

    public override void OnClick()
    {
        base.OnClick();
        if (!canTiggle) return;
        ballsPoint.Clear();
         canTiggle = false;
        for (int i = 0; i < PropManger.Instance.GetCubeNum(PropType.ToString(), (int)Gear); i++)
        {
            //Ball ball =
            //if (ball != null)
            //{
                ballsPoint.Add(RomdRang());
            //}
        }
        Eliminat();
        FlyEffect();
        //GameManager.Instance.ElimintNoColorlBall();
      
            //CameraManager.Instance.SetShake(shakeLevel * ((int)SizeType + 1), shakeTimes);


            Observable.TimeInterval(System.TimeSpan.FromSeconds(0.8F)).Subscribe(_ =>
            {
                if (!GameManager.Instance.OverGame)
                {
                    UIRoot.Instance.HideMask();
                }

            });

    }
    public override void Init(object[] obj = null)
    {
        base.Init(obj);
        if (obj != null)
        {
            SizeType = (Porp_Size)obj[0];
            Gear = (int)obj[1];
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PropSprite/" + PropType.ToString() + Gear);
        }
    }
    float effectSize1;
    public override Collider2D[] DetectionRange()
    {

        //ballsPoint.Clear();
        float[] tempRange = PropManger.Instance.GetRang(PropType.ToString(), Gear, SizeType);
        effectSize1 = PropManger.Instance.GetEffectSize(PropType.ToString(), Gear, SizeType);
      
        
        return Physics2D.OverlapCircleAll(transform.position, tempRange[0]);
    }


    public void FlyEffect() {

        willEli.Clear();
        AudioMgr.Instance.PlaySFX("绿色技能--飞行");
        for (int i = 0; i < ballsPoint.Count; i++)
        {
            //if (balls[i] == null)
            //{
            //    XDebug.LogError("绿色炸弹未检测到球");
            //    break;
            //}
            //else
            //{
                int p = i;
                Transform PROP = Pool.Instance.Spawn(Pool.Prop_PoolName, "xvqc_Green" + Gear);
                PROP.localScale = Vector3.one * 0.7F;

            float[] tempRange = PropManger.Instance.GetChildRang(PropType.ToString(), Gear, SizeType);
            float effectSize = PropManger.Instance.GetEffectSize(PropType.ToString(), Gear, SizeType);
     
            Collider2D[] ds= Physics2D.OverlapCircleAll(ballsPoint[p], tempRange[0]);

            foreach (var item in ds)
            {
                if (!willEli.Contains(item))
                {
                    willEli.Add(item);
                }
            }

            DynamicMgr.Instance.FlyEffectCurveByGG(transform.position, ballsPoint[p], PROP, 15, 0.5F, 0.5f, 15, () =>
                 {
                     Pool.Instance.Despawn(Pool.Prop_PoolName, PROP);
                     //Transform t = Pool.Instance.Spawn(Pool.Prop_PoolName, Pool.GreenBomb);
                     //t.transform.position = ballsPoint[p];
                     //t.localScale = Vector3.one * GetSize(SizeType);
                     //object[] all = new object[2] { SizeType, Gear };
                     //t.transform.GetComponent<GreenElimiat>().Init(all);
                     //t.transform.GetComponent<GreenElimiat>().OnClick();
                     //if
                     if (p== ballsPoint.Count-1)
                     {
                         AudioMgr.Instance.PlaySFX("绿色技能--爆炸");
                         GameManager.Instance.RangeElimination(willEli.ToArray());
                        
                         //for (int i = 0; i < ballsPoint.Count; i++)
                         //{
                             foreach (var item in ballsPoint)
                             {
                                 Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.GreenBombEffect, item, effectSize);
                                 //Debug.LogError("绿色特效位置" + item);
                         }

                         //}
                         CameraManager.Instance.SetShake(PropManger.Instance.GetShackLevel(PropType.ToString(), Gear, SizeType), PropManger.Instance.GetShackTime(PropType.ToString(), Gear, SizeType));

                         //Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.GreenBombEffect, transform.position, effectSize);
                     }

                 });
            //}
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.transform.GetComponent<Prop>() != null)
        {
            Fusion(this, collision.transform.GetComponent<Prop>());
        }

    }

}
