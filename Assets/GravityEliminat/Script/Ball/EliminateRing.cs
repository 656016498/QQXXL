using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using UnityEditor;
public class EliminateRing: Prop, CanClick
{
    
    public int needSpswnNum;
    //List<Transform> transforms = new List<Transform>();
    public float Radius ;
    public float[] sizeNum;
    public List<Vector3> nowBalls;
    public List<Collider2D> all = new List<Collider2D>();
     public Dictionary<string, WillDown> temp = new Dictionary<string, WillDown>();
    //public CircleCollider2D[] circleCollider2Ds;
    //public
    //public bool can;

    public override void Init(object[] obj = null)
    {
        base.Init(obj);
        if (obj != null)
        {
            SizeType = (Porp_Size)obj[0];
            Gear = (int)obj[1];
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PropSprite/" + PropType.ToString() + Gear);
            //for (int i = 0; i < circleCollider2Ds.Length; i++)
            //{
            //    circleCollider2Ds[i].enabled = false;
            //}
        }
        needSpswnNum = 0;
        temp.Clear();
    }
    public override void OnClick()
    {
        all.Clear();
        nowBalls.Clear();
        if (!canTiggle) return;
        canTiggle = false;
        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.8F)).Subscribe(_ => {
            if (!GameManager.Instance.OverGame)
            {
                UIRoot.Instance.HideMask();
            }
        });
        Pool.Instance.Despawn(Pool.Prop_PoolName, transform);
        Transform elecriBll = Pool.Instance.Spawn(Pool.Effect_PoolName,Pool.ElecriBall);
        float effectSize = PropManger.Instance.GetEffectSize(PropType.ToString(), Gear, SizeType);
        float bombRang = PropManger.Instance.GetRang(PropType.ToString(), Gear, SizeType)[0];
        elecriBll.position = transform.position;
    
        for (int i = 0; i < PropManger.Instance.GetCubeNum(PropType.ToString(), Gear); i++)
        { 
            nowBalls.Add(RomdRang());
        }
        //line.positionCount = nowBalls.Count;
        //line.positionCount = nowBalls.Count*2+1;
        //int index = 0;

        for (int i = 0; i < nowBalls.Count; i++)
        {
            int p = i;
            Transform elecriLine = Pool.Instance.Spawn(Pool.Effect_PoolName, Pool.ElectricWire);
            LineRenderer line = elecriLine.GetComponent<LineRenderer>();
            elecriLine.SetParent(GameManager.Instance.allBallParent);
            line.positionCount = 2;
            Vector3 orgin = transform.position;
            line.SetPosition(0, transform.position);
            AudioMgr.Instance.PlaySFX("黄色技能--电流");
            DOTween.To(() => orgin, x => orgin = x, nowBalls[i], 0.3f).OnUpdate(() =>
            {
                line.SetPosition(1, orgin);
                
            }).OnComplete(()=> {
                line.positionCount = 0;
                Pool.Instance.SpawnEffect(Pool.Effect_PoolName,Pool.ElecriBomb,nowBalls[p], effectSize);
                //Debug.Log("电球范围消除");
                Collider2D[] collision2s = Physics2D.OverlapCircleAll(orgin, bombRang);

                foreach (var item in collision2s)
                {
                    if (!all.Contains(item))
                    {
                        all.Add(item);
                    }
                }
            
                Observable.TimeInterval(System.TimeSpan.FromSeconds(0.1F)).Subscribe(_ => {

                    Pool.Instance.Despawn(Pool.Effect_PoolName,elecriLine);
                   
                });
            });
        }

        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.4F)).Subscribe(_ => {

            if (!GameManager.Instance.OverGame)
            {
                //CameraManager.Instance.SetShake(shakeLevel+((int)Gear*1.5f),(shakeTimes)+(0.1f* (int)Gear));
                CameraManager.Instance.SetShake(PropManger.Instance.GetShackLevel(PropType.ToString(), Gear, SizeType), PropManger.Instance.GetShackTime(PropType.ToString(), Gear, SizeType));

            }
            GameManager.Instance.RangeElimination(all.ToArray());
            Pool.Instance.Despawn(Pool.Effect_PoolName, elecriBll);
            AudioMgr.Instance.PlaySFX("黄色技能--爆炸");
        });
       
        //if (Gear == 1)
        //{
        //    RingElimination(1,SizeType);
        //}
        //else if (Gear == 2)
        //{
        //    RingElimination(1,SizeType);
        //    RingElimination(2, SizeType);
        //}
        //else
        //{

        //    RingElimination(1, SizeType);

        //    RingElimination(2, SizeType);

        //    RingElimination(2, SizeType);

        //}

        //Eliminat();
        //AddBatch(temp, needSpswnNum);
        //GameManager.Instance.ElimintNoColorlBall();



    }
   


    public void RingElimination(int GEAR,Porp_Size SizeType)
    {
        //sizeNum = Gear * Gear;
        float[] tempRange = PropManger.Instance.GetRang(PropType.ToString(), GEAR, SizeType);
        float effectSize = PropManger.Instance.GetEffectSize(PropType.ToString(), GEAR, SizeType);
        Pool.Instance.SpawnEffect(Pool.Effect_PoolName,Pool.Yellow_LD,transform.position, effectSize);
        Collider2D[] collider2s = Physics2D.OverlapCircleAll(transform.position, tempRange[0]);
        
        foreach (var item in collider2s)
        {
            if (item  == null) continue;

            //Debug.Log(Vector3.Distance(transform.position, item.transform.position) + "EEE" + item.transform.GetComponent<SpriteRenderer>().bounds.size.x / 2);
            if (item.GetComponent<Ball>() != null && Vector3.Distance(transform.position, item.transform.position) + (item as CircleCollider2D).radius >=tempRange[0])
            {
                if (item.GetComponent<ColorBall>() != null)
                {
                    Ball tempBall = item.GetComponent<Ball>();
                    if (tempBall.ballType != BallType.ColorBall)
                    {
                        GameManager.Instance.ClickSpecailBall.Add(tempBall);
                    }
                    else
                    {
                        WillDown willDown = new WillDown(tempBall.ballType, tempBall.sort, tempBall.isFix, 1);
                        if (temp.ContainsKey(willDown.BallTypeString))
                        {
                            temp[willDown.BallTypeString].num++;
                        }
                        else
                        {
                            temp.Add(willDown.BallTypeString, willDown);
                        }
                        //Debug.LogError(transform + "   ");
                        needSpswnNum += tempBall.Eliminat();
                        //tempBall.GetComponent<ColorBall>().SetSubger();
                        //Debug.LogError(transform + "   " + needSpswnNum);
                        //GameManager.Instance.colorBalls.Remove(tempBall.GetComponent<ColorBall>());
                    }
                }
            }
        }

    }

  
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<Prop>()!=null)
        {
            Fusion(this, collision.transform.GetComponent<Prop>());
        }
        //if (!canTiggle) return;

        //if (collision != null && collision.transform.CompareTag("Prop")&& collision.transform.GetComponent<EliminateOrgne>()!=null)
        //{
        //    if (SizeType == Porp_Size.大) return;
        //    EliminateRing other = collision.transform.GetComponent<EliminateRing>();

        //    canTiggle = false;
        //    other.canTiggle = false;
        //    transform.DOMove(other.transform.position, 0.5f).SetEase(Ease.Linear).OnComplete(() => {

        //        Porp_Size newSize = ComplereSize(SizeType, other.SizeType);
        //        EliminateOrgne ElimiProp = Pool.Instance.Spawn(Pool.Prop_PoolName, Pool.EliminateOrgne).GetComponent<EliminateOrgne>();
        //        ElimiProp.SizeType = newSize;
        //        ElimiProp.transform.position = transform.position;
        //        float M = 0;
        //        switch (ElimiProp.SizeType)
        //        {
        //            case Porp_Size.小:
        //                M = 0.3f;
        //                break;
        //            case Porp_Size.中:
        //                M = 0.4f;
        //                break;
        //            case Porp_Size.大:
        //                M = 0.5f;
        //                break;
        //            default:
        //                break;
        //        }
        //        ElimiProp.transform.localScale = Vector3.one * M;
        //        canTiggle = true;
        //        if (other != null)
        //        {
        //            other.canTiggle = true;
        //            other.Eliminat();
        //            Eliminat();
        //        }
        //    });

        //}
    }
}
