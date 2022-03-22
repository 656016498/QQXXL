using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using Spine.Unity;
public enum Porp_Size
{
    小 = 0, 中 = 1, 大 = 2
}

//public enum PropTyep{
//    Horizontal,
    
//    Cross,
//    Circular,
//    Upright,
//    X,
//    Box,
//}

/// <summary>
/// 横消除道具
/// </summary>
public class EliminatBomb : Prop
{
    public bool isCreat = false;
    public Vector2 bombRang;
    public float[] Radius;
#if !Animal
    //Animator animator;
#else
 public   SkeletonAnimation skeleton;
#endif
    public bool des=false;//

    private void Awake()
    {
#if !Animal
        //animator = transform.GetChild(0).GetComponent<Animator>();
#else
        skeleton = transform.GetComponentInChildren<SkeletonAnimation>();
#endif
    }
    private void Start()
    {
        if (!isCreat)
        {
            des = true;
            object[] all = new object[2] { SizeType, Gear };
            Init(all);
        }
    }
    /// <summary>
    /// 检测范围
    /// </summary>
    /// <returns></returns>
    public override void Init(object[] obj = null)
    {

        base.Init(obj);
        if (obj!=null)
        {
            isCreat = true;
            SizeType = (Porp_Size)obj[0];
            Gear = (int)obj[1];
#if !Animal
            //transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PropSprite/" + PropType.ToString() + Gear);
            for (int i = 0; i < 3; i++)
            {
                transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
            }
            transform.GetChild(0).GetChild(Gear - 1).gameObject.SetActive(true);
#endif
        }
#if !Animal
        if (Gear == 3)
        {
            transform.Find("huohua").gameObject.SetActive(true);
        }
        else
        { 
            transform.Find("huohua").gameObject.SetActive(false);
        }
        //animator.SetInteger("Gear",Gear);

#else
        //transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PropSprite/" + PropType.ToString() + Gear);

     float time=   skeleton.AnimationState.SetAnimation(0, "Created", false).Animation.Duration;
        Observable.TimeInterval(System.TimeSpan.FromSeconds(time)).Subscribe(_ => { 
        
        skeleton.AnimationState.SetAnimation(0,"Activity4",true);

        });
#endif
    }


    public override Dictionary<int, List<Collider2D>> DetectionRange2()
    {
        CanFusion = false;
        float[] tempRange = PropManger.Instance.GetRang(PropType.ToString(), Gear, SizeType);
        Dictionary<int, List<Collider2D>> all = new Dictionary<int, List<Collider2D>>();

        switch (Gear)
        {
            case 1:

                Vector2 bombRang2 = new Vector2(tempRange[0], tempRange[1]);
                Collider2D[] tempC= Physics2D.OverlapBoxAll(transform.position, bombRang2, 0);
                for (int i = 0; i < tempC.Length; i++)
                {

                    if (tempC[i]==null)
                    {
                        continue;
                    }
                    if (tempC[i] == null)
                    {
                        continue;
                    }

                    float Distx = Vector3.Distance(tempC[i].transform.position, transform.position);

                    int p = (int)(Distx / 0.53f);
                    if (all.ContainsKey(p))
                    {
                        all[p].Add(tempC[i]);
                    }
                    else
                    {
                        List<Collider2D> colliders = new List<Collider2D>();
                        colliders.Add(tempC[i]);
                        all.Add(p, colliders);
                    }
                    //var targetX = Mathf.Abs(tempC[i].transform.position.x);
                    //var selfX = Mathf.Abs(transform.position.x);
                    //var Distx=0F;                    
                    //if (targetX > selfX)
                    //{

                    //    Distx = targetX - selfX;

                    //}
                    //else { 

                    //     Distx = selfX - targetX;

                    //}
                    //int p =(int)(Distx / 0.53f);
                    //if (all.ContainsKey(p))
                    //{
                    //    all[p].Add(tempC[i]);
                    //}
                    //else {

                    //    List<Collider2D> colliders = new List<Collider2D>();
                    //    colliders.Add(tempC[i]);
                    //    all.Add(p, colliders);

                    //}
                }
                return all;

            case 2:
                Vector2 bombRang3 = new Vector2(tempRange[0], tempRange[1]);
                Collider2D[] h = Physics2D.OverlapBoxAll(transform.position, bombRang3, 0);
                Collider2D[] y = Physics2D.OverlapBoxAll(transform.position, bombRang3, 90);
                List<Collider2D> temp = new List<Collider2D>();
                for (int i = 0; i < h.Length; i++)
                {
                    temp.Add(h[i]);
                }
                for (int i = 0; i < y.Length; i++)
                {
                    if (!temp.Contains(y[i]))
                    {
                        temp.Add(y[i]);
                    }
                }
                for (int i = 0; i < temp.Count; i++)
                {

                    if (temp[i] == null)
                    {
                        continue;
                    }


                    float Distx = Vector3.Distance(temp[i].transform.position,transform.position);

                    //}
                    //else
                    //{

                        //Distx = selfX - targetX;

                    //}
                    int p = (int)(Distx / 0.53f);
                    if (all.ContainsKey(p))
                    {
                        all[p].Add(temp[i]);
                    }
                    else
                    {

                        List<Collider2D> colliders = new List<Collider2D>();
                        colliders.Add(temp[i]);
                        all.Add(p, colliders);

                    }
                }



                return all;
            case 3:

                //return
                    Collider2D[] tempC3= Physics2D.OverlapCircleAll(transform.position, tempRange[0] /**0.3F)*/);

                for (int i = 0; i < tempC3.Length; i++)
                {

                    if (tempC3[i] == null)
                    {
                        continue;
                    }
                 
                      float Distx = Vector3.Distance(tempC3[i].transform.position, transform.position);

                    int p = (int)(Distx / 0.53f);
                    if (all.ContainsKey(p))
                    {
                        all[p].Add(tempC3[i]);
                    }
                    else
                    {
                        List<Collider2D> colliders = new List<Collider2D>();
                        colliders.Add(tempC3[i]);
                        all.Add(p, colliders);
                    }
                }
                return all;

            default:
                return null;
        }
        return null;
    }


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.GetComponent<Ball>()!=null)
    //    {
    //        Ball ba = collision.GetComponent<Ball>();
    //        if (ba.isEliminat) return;
    //        ba.Eliminat();
    //    }

    //}


    public override void SetReady(Transform prop)
    {
        base.SetReady(prop);
#if Animal
        prop.GetComponent<EliminatBomb>().skeleton.AnimationState.SetAnimation(0, "Triggered", true);
#endif
    }

    string prfreName;
     private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("EQEQW");
        if (collision.transform.GetComponent<Prop>()!=null)
        {
            if (!CanFusion) return;
#if !Animal
            //animator.SetTrigger("Exit");
#endif
            //transform.localScale=Vector3.oneL
            Fusion(this, collision.transform.GetComponent<Prop>());
        }
        //if (!canTiggle) return;
        
        //if (type == PropTyep.Circular) return;

        //if (collision != null && collision.transform.CompareTag("Prop"))
        //{
        //    EliminatBomb otherBomb = collision.transform.GetComponent<EliminatBomb>();
          
        //    if (otherBomb != null && otherBomb.type == type)
        //    {
        //        if (!otherBomb.canTiggle|| otherBomb==null)
        //        {
        //            return;
        //        }
        //        canTiggle = false;
        //        otherBomb.canTiggle = false;
        //        Debug.Log(transform.name);
        //        transform.DOMove(otherBomb.transform.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        //            {
        //                Porp_Size newSize = ComplereSize(SizeType, otherBomb.SizeType);
        //                switch ((PropTyep)((int)type + 1))
        //                {
        //                    case PropTyep.Cross:
        //                        prfreName = Pool.CrossProp;
        //                        break;
        //                    case PropTyep.Circular:
        //                        prfreName = Pool.BombProp;
        //                        break;
        //                    default:
        //                        break;
        //                }
        //                EliminatBomb ElimiProp = Pool.Instance.Spawn(Pool.Prop_PoolName, prfreName).GetComponent<EliminatBomb>();
        //                ElimiProp.SizeType = newSize;
        //                ElimiProp.transform.position = transform.position;
        //                float M = GetSize(ElimiProp.SizeType);

        //                ElimiProp.transform.localScale = Vector3.one * M;
        //                canTiggle = true;
        //                if (otherBomb !=null)
        //                {
        //                    otherBomb.canTiggle = true;
        //                    otherBomb.Eliminat();
        //                    Eliminat();
        //                }
        //            });
        //        }
        //    }
    }



    public override void OnClick()
    {

        base.OnClick();

        if (!canTiggle) return;
        canTiggle = false;

#if !Animal
        //animator.SetTrigger("Exit");
        float effectSize = PropManger.Instance.GetEffectSize(PropType.ToString(), Gear, SizeType);
        //{
           
        //});
        //HB
        
        transform.DOBlendableScaleBy(Vector3.one*0.15f, 0.15f).SetEase(Ease.InBack).OnComplete(()=> {
            //{
            StartCoroutine(PropCollectBall());
            switch (Gear)
                {
                    case 1:
                        AudioMgr.Instance.PlaySFX("红橙技能--横竖消");
                        Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.HengBomb, transform.position, effectSize);
                        break;

                    case 2:
                        AudioMgr.Instance.PlaySFX("红色技能--十字消除");
                        Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.ShiZhiBoom, transform.position, effectSize);

                        break;
                    case 3:
                        AudioMgr.Instance.PlaySFX("红色圆形爆炸");
                        Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.CircularBomb, transform.position, effectSize);

                        break;
                    default:
                        break;
              
            }
           
            //GameManager.Instance.ElimintNoColorlBall();
        });
    //});
       
    
#else
        float time =skeleton.AnimationState.SetAnimation(0, "Attack3", false).Animation.Duration;
        Observable.TimeInterval(System.TimeSpan.FromSeconds(time)).Subscribe(_ => {
            PropCollectBall();
            //DetectionRange();
            GameManager.Instance.ElimintNoColorlBall();


        });
        Observable.TimeInterval(System.TimeSpan.FromSeconds(time/2)).Subscribe(_ => {
            switch (Gear)  
        {
            case 1:
                AudioMgr.Instance.PlaySFX("红橙技能--横竖消");

                break;
            case 2:
                AudioMgr.Instance.PlaySFX("红色技能--十字消除");

                break;
            case 3:
                AudioMgr.Instance.PlaySFX("红色圆形爆炸");

                break;
            default:
                break;
        }
        });

#endif

        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.2F)).Subscribe(_ => {

            if (!GameManager.Instance.OverGame)
            {
                //CameraManager.Instance.SetShake(shakeLevel+((int)Gear*1.5f),(shakeTimes)+(0.1f* (int)Gear));
                CameraManager.Instance.SetShake(PropManger.Instance.GetShackLevel(PropType.ToString(), Gear, SizeType), PropManger.Instance.GetShackTime(PropType.ToString(), Gear, SizeType));
            }

        });
       

        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.25F)).Subscribe(_ =>
        {
            if (!GameManager.Instance.OverGame)
            {
                //MoneyFly.instance.PropPlay(3, transform.position, UIManager.Instance.GetBase<GamePanel>().largeCashBtn.transform.GetChild(2).position, null);
                UIRoot.Instance.HideMask();
            }
        });

    }

    public override void Eliminat()
    {
        if (des)
        {
            des = false;
            if (PropManger.Instance.allProp.Contains(this))
            {
                PropManger.Instance.allProp.Remove(this);
            }
            Destroy(gameObject);
        }
        else
        {
            base.Eliminat();
        }
    }
}
