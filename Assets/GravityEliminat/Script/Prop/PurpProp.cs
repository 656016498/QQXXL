using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
public class PurpProp : Prop
{
   
    public List<Vector3> balls = new List<Vector3>();
    public float Radius;
    public bool CanClick;
    //public int Gear=1;
    public override void Init(object[] obj = null)
    {
        base.Init(obj);
        CanClick = true; Gear = 1;
        if (obj != null)
        {
            SizeType = (Porp_Size)obj[0];
            Gear = (int)obj[1];
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PropSprite/" + PropType.ToString() + Gear);
        }
    }

    public override void OnClick()
    {
        if (!CanClick) return;
        base.OnClick();
        if (!canTiggle) return;
        canTiggle = false;
        FlyEffect();
        AudioMgr.Instance.PlaySFX("点击紫色技能");
        //Pool.Instance.
        Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.YellowProp, transform.position);

        if (!GameManager.Instance.OverGame)
        {
            Observable.TimeInterval(System.TimeSpan.FromSeconds(0.8F)).Subscribe(_ =>
        {

            UIRoot.Instance.HideMask();

        });
        }
    }
    public void FlyEffect()
    {
       
        balls.Clear();

        Pool.Instance.Despawn(Pool.Prop_PoolName, transform);

        CanClick = false;
        for (int i = 0; i < PropManger.Instance.GetCubeNum(PropType.ToString(),Gear); i++)
        {
            int p = i;
            balls.Add(RomdRang());
            Transform PROP = Pool.Instance.Spawn(Pool.Prop_PoolName, "PurpleFly");
            int index = GetColor();
            PROP.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PropSprite/qqqc_"+ GameManager.Instance.level.ballWeights[index].colorType);
            PROP.localScale = Vector3.one * 0.7F;
            DynamicMgr.Instance.FlyEffectCurveByGG(transform.position, balls[p], PROP,30, 0.5F,0.5f,15,() =>
            {
                
                Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(balls[p],PropManger.Instance.GetRang(PropType.ToString(),Gear, SizeType)[0]);
                foreach (var item in collider2Ds)
                {
                    if (item.GetComponent<ColorBall>() != null)
                    {
                        item.GetComponent<ColorBall>().ChangeColor(GameManager.Instance.level.ballWeights[index].colorType,1);
                        
                        

                        
                    }
                }
                if (!GameManager.Instance.OverGame)
                {
                    //CameraManager.Instance.SetShake(shakeLevel+((int)Gear*1.5f),(shakeTimes)+(0.1f* (int)Gear));
                    CameraManager.Instance.SetShake(PropManger.Instance.GetShackLevel(PropType.ToString(), Gear, SizeType), PropManger.Instance.GetShackTime(PropType.ToString(), Gear, SizeType));

                }
                AudioMgr.Instance.PlaySFX("紫色技能--染色");
                Pool.Instance.Despawn(Pool.Prop_PoolName, PROP);

            });
        }
    }

    public int GetColor()
    {
        int k = Random.Range(0,GameManager.Instance.level.ballWeights.Count);
        if (GameManager.Instance.level.ballWeights[k].ballType == BallType.ColorBall)
        {
            return k;
        }
        else {
            return GetColor();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<Prop>()!=null)
        {
            Fusion(this,collision.transform.GetComponent<Prop>());
        }
    }

}
