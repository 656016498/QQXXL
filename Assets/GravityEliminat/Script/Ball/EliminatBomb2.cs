using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;


/// <summary>
/// 横消除道具
/// </summary>
public class EliminatBomb2 : Prop
{
    Animator animator;
    public Vector2 bombRang;
    public float Radius;
    public Vector2[] box;
    private void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
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
        animator.SetInteger("Gear", Gear);

    }
    /// <summary>
    /// 检测范围
    /// </summary>
    /// <returns></returns>
    /// 

    public override Dictionary<int, List<Collider2D>> DetectionRange2()
    {
        CanFusion = false;
        float[] tempRange = PropManger.Instance.GetRang(PropType.ToString(), Gear, SizeType);
        Dictionary<int, List<Collider2D>> all = new Dictionary<int, List<Collider2D>>();
        float effectSize = PropManger.Instance.GetEffectSize(PropType.ToString(), Gear, SizeType);

        switch (Gear)
        {
            case 1:
                Transform t = Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.ShuBoom, transform.position, effectSize);
                t.localEulerAngles = new Vector3(0, 0, 90);
                Vector2 bombRang2 = new Vector2(tempRange[0], tempRange[1]);
                Collider2D[] tempC = Physics2D.OverlapBoxAll(transform.position, bombRang2, 90);
                for (int i = 0; i < tempC.Length; i++)
                {

                    if (tempC[i] == null)
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


                    float Distx = Vector3.Distance(temp[i].transform.position, transform.position);

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
                Collider2D[] tempC3 = Physics2D.OverlapCircleAll(transform.position, tempRange[0] /**0.3F)*/);

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

    public override Collider2D[] DetectionRange()
    {
        CanFusion = false;
        float[] tempRange = PropManger.Instance.GetRang(PropType.ToString(), Gear, SizeType);
        float effectSize = PropManger.Instance.GetEffectSize(PropType.ToString(), Gear, SizeType);
        switch (Gear)
        {

            case 1:
                AudioMgr.Instance.PlaySFX("红橙技能--横竖消");

                Transform t = Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.ShuBoom, transform.position, effectSize);
                t.localEulerAngles = new Vector3(0, 0, 90);
                Vector2 bombRang2 = new Vector2(tempRange[0], tempRange[1]);
                return Physics2D.OverlapBoxAll(transform.position, bombRang2, 90);

            case 2:
                AudioMgr.Instance.PlaySFX("红色技能--十字消除");
                Transform t1 = Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.CrossBomb, transform.position, effectSize);
                
                //t1.localEulerAngles = new Vector3(0, 0, 0);
                Vector2 bombRang3 = new Vector2(tempRange[0], tempRange[1]);

                Collider2D[] h = Physics2D.OverlapBoxAll(transform.position, bombRang3, 45);
                Collider2D[] y = Physics2D.OverlapBoxAll(transform.position, bombRang3, -45);
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
                return temp.ToArray();

            case 3:
                AudioMgr.Instance.PlaySFX("橙色正方形爆炸");

                Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.BoxBomb, transform.position, effectSize);
                return Physics2D.OverlapBoxAll(transform.position,new Vector2 (tempRange[0], tempRange[0]) , 0);


            default:
                return null;
        }
    }


    string prfreName;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<Prop>()!=null)
        {
            if (!CanFusion) return;
          
            Fusion(this, collision.transform.GetComponent<Prop>());
        }
    }
    public override void OnClick()
    {
        base.OnClick();
        if (!canTiggle) return;
        canTiggle = false;
        //PropCollectBall();
        StartCoroutine(PropCollectBall());
        GameManager.Instance.ElimintNoColorlBall();
        if (!GameManager.Instance.OverGame)
        {
            CameraManager.Instance.SetShake(PropManger.Instance.GetShackLevel(PropType.ToString(),Gear,SizeType), PropManger.Instance.GetShackTime(PropType.ToString(), Gear, SizeType));
        }
        //炸弹动画关闭
        animator.SetTrigger("Exit");

        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.5F)).Subscribe(_ =>
            {
            if (!GameManager.Instance.OverGame)
            {
                UIRoot.Instance.HideMask();
                }
            });
        
    }
}
