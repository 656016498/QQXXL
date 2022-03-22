using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Linq;
public class Prop  : MonoBehaviour, CanClick
{


    public bool CanFusion = true;//可否融合
    protected float shakeLevel=3f;
    protected float shakeTimes=0.25f;
    public int Gear=1;//阶级
    public bool canTiggle = true;//可否触发
    public bool isReady = false;//带激发特效
    public float waitTimes=0.5f;
    public SortType PropType;
    public Porp_Size SizeType;
    public List<Ball> PropCollectList = new List<Ball>();
    public List<Transform> BombList = new List<Transform>();
    public List<Ball> willElimnt = new List<Ball>(); 
    int bombIndex = 0;
    public virtual void  Eliminat()
    {
        
        if (PropManger.Instance.allProp.Contains(this))
        {
            if (!GameManager.Instance.OverGame)
            {
                PropManger.Instance.allProp.Remove(this);
            }
        }
        if (transform.gameObject.activeSelf)
        {
            Pool.Instance.Despawn(Pool.Prop_PoolName, transform);
        }
    }
       
    public virtual void Init(object[] obj=null) {
        
        CanFusion = true;
        canTiggle = true;
        InitReady();
        if (!PropManger.Instance.allProp.Contains(this))
        {
            PropManger.Instance.allProp.Add(this);
        }
    }

    public virtual Collider2D[] DetectionRange() {
        return null;
    }
    public virtual Dictionary<int,List<Collider2D>> DetectionRange2()
    {
        return null;
    }

    public virtual void SetReady(Transform prop)
    {

        Prop otherProp = prop.GetComponent<Prop>();
        if (!otherProp.canTiggle) return;
        if (otherProp.isReady)
        {
            return;
        }
        otherProp.isReady = true;
        otherProp.CanFusion = false;
        PropManger.Instance.ReadyNum += 1f;
        float now = PropManger.Instance.ReadyNum;
#if !Animal
        if (prop.GetComponent<LightBall>() == null)
        {
            prop.GetChild(1).gameObject.SetActive(true);
        }
#endif
        prop.GetComponent<Prop>().CanFusion = false;
        otherProp.waitTimes = (PropManger.Instance.ReadyNum) * PropManger.Instance.times;
        Observable.TimeInterval(System.TimeSpan.FromSeconds(1)).Subscribe(_ =>
        {
            if (PropManger.Instance.ReadyNum == now)
            {
                PropManger.Instance.ReadyNum = 0;
            }
            willElimnt.Clear();
        });
    }



    public virtual void InitReady() {

        isReady = false;
        CanFusion = true;
#if !Animal
        transform.GetChild(1).gameObject.SetActive(false);
#endif

    }


    public void AddBatch(Dictionary<string, WillDown> temp,int needSpswnNum) 
    {
        //彩球添加
        //Debug.Log("道具消除 球掉落:"+needSpswnNum);
        List<WillDown> FixWillDown = new List<WillDown>();
        foreach (var item in temp.Values)
        {
            if (item.isFix && item.type == BallType.ColorBall)
            {
                FixWillDown.Add(item);
            }
        }
        var fixNum = GameManager.Instance.ByFixAddBatch(FixWillDown, needSpswnNum);
        DownRang downRang = GameManager.Instance.level.downRangs[GameManager.Instance.JudgeBatch(needSpswnNum)];
        GameManager.Instance.RomdbyWeights(needSpswnNum - fixNum, downRang.batch);
        GameManager.Instance.CreateBall(downRang.downInterval);
        //GameManager.Instance.EliminatPower.Value += needSpswnNum;
        PropCollectList.Clear();
    }






    public IEnumerator PropCollectBall()
    {
        PropCollectList.Clear();
        canTiggle = false;
        //Collider2D[] collider2Ds = DetectionRange();
        Dictionary<int,List<Collider2D>> collider2Ds = DetectionRange2();
        if (collider2Ds == null)
        {
            OnClick();
            //Debug.Log("temptemptempDDELLLLLL" );
        }
        else
        {
            var needSpswnNum = 0;
            Dictionary<string, WillDown> tempWillDown = new Dictionary<string, WillDown>();
            Dictionary<int, List<Collider2D>> refDic = new Dictionary<int, List<Collider2D>>();
            collider2Ds = (from temp in collider2Ds orderby temp.Key ascending select temp).
            ToDictionary(pair => pair.Key, pair => pair.Value);
            foreach (var item in collider2Ds)
            {
                refDic.Add(item.Key, item.Value);
            }
            //for (int i = 0; i < collider2Ds.Values.Count-1;i++)
            //{
            //    //Debug.LogError(collider2Ds[i]);
            //    Debug.LogError("a:"+i);
            //    Debug.Log("temptemptempDD" + collider2Ds.Values.Count + "EQWEQWE" + collider2Ds[i].Count);


            //    if (collider2Ds.ContainsKey(i))
            //    {
            //        //List<Collider2D> temp = new List<Collider2D>();
            //        //foreach (var B in collider2Ds[i])
            //        //{
            //        //    temp.Add(B);
            //        //}
            //        //Debug.LogError(collider2Ds[i]);
            //        PorpElimat(collider2Ds[i]);
            //    }
            //    //Debug.Log("temptemptemp" + collider2Ds[i].Count);
            //    yield return new WaitForSeconds(0.01f);
            //}

            foreach (var item in refDic.Values)
            {

                PorpElimat(item,ref tempWillDown,ref needSpswnNum);
                yield return new WaitForSeconds(0.025F);

            }
            Eliminat();
            AddBatch(tempWillDown, needSpswnNum);
            GameManager.Instance.ElimintNoColorlBall();
        }

    }





    public void PorpElimat(List<Collider2D> collider2Ds, ref Dictionary<string, WillDown> temp,ref int needSpswnNum) {
        PropCollectList.Clear();

        for (int i = 0; i < collider2Ds.Count; i++)
        {
            if (collider2Ds[i]==null)
            {
                continue;
            }
            if (collider2Ds[i].CompareTag("Ball") || collider2Ds[i].CompareTag("Soda"))
            {
                Ball tempBall = collider2Ds[i].gameObject.GetComponent<Ball>();
                if (!PropCollectList.Contains(tempBall) && tempBall != null && !tempBall.isEliminat)
                {
                    PropCollectList.Add(tempBall);
                }
            }


            //if (collider2Ds[i].transform.GetComponent<TicketBall>()!=null)
            //{
            //    collider2Ds[i].transform.GetComponent<TicketBall>().Elimnt();
            //}
            if (collider2Ds[i].gameObject.transform.GetComponent<MoneyBall>() != null)
            {
                collider2Ds[i].gameObject.transform.GetComponent<MoneyBall>().Eliminat();
            }
            if (collider2Ds[i].gameObject.name == "BlackCloud")
            {
                collider2Ds[i].GetComponent<BlackCloud>().Eliminat();
            }
            if (collider2Ds[i].CompareTag("Prop") && collider2Ds[i].transform != this.transform && !GameManager.Instance.OverGame)
            {
                if (!collider2Ds[i].transform.GetComponent<Prop>().isReady && collider2Ds[i].transform.GetComponent<Prop>().canTiggle)
                {
                    //设置待激发状态
                    //PropManger.Instance.ReadyNum++;
                    SetReady(collider2Ds[i].transform);

                }
            }
        }
    
        for (int i = 0; i < PropCollectList.Count; i++)
        {
            if (PropCollectList[i].ballType != BallType.ColorBall && !PropCollectList[i].isEliminat)
            {
                if (!GameManager.Instance.ClickSpecailBall.Contains(PropCollectList[i]))
                {
                    GameManager.Instance.ClickSpecailBall.Add(PropCollectList[i]);
                }
             
            }
            else
            {
                WillDown willDown = new WillDown(PropCollectList[i].ballType, PropCollectList[i].sort, PropCollectList[i].isFix, 1);
                if (temp.ContainsKey(willDown.BallTypeString))
                {
                    temp[willDown.BallTypeString].num++;
                }
                else
                {
                    temp.Add(willDown.BallTypeString, willDown);
                }

                if (!PropCollectList[i].isEliminat)
                {
                    needSpswnNum += PropCollectList[i].Eliminat();
                }
            }
        }

        GameManager.Instance.EliminatPower.Value += PropCollectList.Count;
        GameManager.Instance.GravityDisappear();

    }
    /// <summary>
    /// 道具连续爆炸
    /// </summary>
    public void ContinuousExplosion()
    {
        //if (bombIndex < BombList.Count)
        //{
        //    BombList[bombIndex].  PropCollectBall();
        //    bombIndex++;
        //}
        //else
        //{
        //    bombIndex = 0;
        //    BombList.Clear();
        //}
    }

    public virtual void OnClick()
    {
        //canTiggle = false;
        CanFusion = false;
        if (!GameManager.Instance.OverGame&&canTiggle)
        {
            UIRoot.Instance.ShowMask();
        }
        CameraManager.Instance.BeginMove();
        //Observable.Timer(System.TimeSpan.FromSeconds(1.5f)).Subscribe(_ => {
        //    CameraManager.Instance.pos = GameManager.Instance.GetBestDownPos();
        //    //Debug.LogError(">>>" + (GameManager.Instance.dowuY - 1) + ">>>" + CameraManager.Instance.pos.position.y);
        //    if (GameManager.Instance.dowuY - 1 < CameraManager.Instance.pos.position.y)
        //    {
        //        CameraManager.Instance.CanMoveCamera = true;
        //    }
        //    //Debug.LogError("摄像机" + CameraManager.Instance.CanMoveCamera);
        //    //UnityEditor.EditorApplication.isPaused = true;
        //});
    }


    public Porp_Size ComplereSize(Porp_Size psSelf, Porp_Size psOher)
    {
        if ((int)psOher - (int)psSelf > 0)
        {
            return psOher;
        }
        else if (psOher == psSelf)
        {
            return psOher;
        }
        else
        {
            return psSelf;
        }
    }

    public float GetSize(Porp_Size sizeType) {
      
        switch (sizeType)
        {
            case Porp_Size.小:
                return 1;
             
            case Porp_Size.中:
                return 1.2f;
                //break;
            case Porp_Size.大:
                return 1.5f;
                //break;
            default:
                return 1.2f;
        }
    }

    
    public Vector3 RomdRang(List<Ball> balls=null)
    {
        //Ball ball = null;
        Vector3 taget = Vector3.zero;
        var allNum = GameManager.Instance.allPlaneParent.childCount+ GameManager.Instance.allBallParent.childCount;
        if (allNum==0) 
        {
            //Eliminat();
            Debug.LogError("场景无物体");
            return taget;
        }
        int r = Random.Range(0, allNum);
        if (r < GameManager.Instance.allPlaneParent.childCount)
        {
            
            taget= GameManager.Instance.allPlaneParent.GetChild(r).position;

        }
        else
        {
            taget= GameManager.Instance.allBallParent.GetChild(r - GameManager.Instance.allPlaneParent.childCount).position;
        }
        if (!CameraManager.Instance.IsInScerrn(taget.y))
        {
            return RomdRang();
        }
        else {
            return taget;
        }
        //if (ball == null)
        //{
        //    return RomdRang();
        //}
        //else {
        //    return ball;
        //}
        //if (GameManager.Instance.allBallParent.childCount == 0)
        //{
        //    Eliminat();
        //}
        //if (GameManager.Instance.allBallParent.GetChild(r).GetComponent<Ball>() == null)
        //{
        //    return RomdRang();
        //}
        //else
        //{
        //    return GameManager.Instance.allBallParent.GetChild(r).GetComponent<Ball>();
        //}
        //if (GameManager.Instance.colorBalls.Count>0)
        //{
        //    return GameManager.Instance.colorBalls[r];
        //}
        //return null;
        //if (balls.Contains(GameManager.Instance.colorBalls[r]))
        //{
        //    return RomdRang(balls);
        //}
        //else
        //{
        //    return GameManager.Instance.colorBalls[r];
        //}
    }

    //Prop newProp = new Prop();
    public void Fusion<T>(T p1,T p2) {
        if (isReady) return;
        if (GameManager.Instance.OverGame) return;
        if (!canTiggle) return;
        SortType newProp;
        Porp_Size newSize;
        int newGear;
        Prop prop1 = p1 as Prop;
        Prop prop2 = p2 as Prop;
        if (prop1.PropType != prop2.PropType) return;
        if (prop1.Gear == 3 || prop2.Gear == 3) return;
        if (prop1.Gear != prop2.Gear) return;
        prop1.canTiggle = false;
        prop2.canTiggle = false;
        if (prop1.PropType == prop2.PropType)
        {
            newProp = prop1.PropType;
        }
        else
        {
            if (prop1.SizeType == prop2.SizeType)
            {
                int r = Random.Range(0, 1);
                if (r == 0)
                {
                    newProp = prop1.PropType;
                }
                else
                {
                    newProp = prop2.PropType;
                }
            }
            else if (prop1.SizeType > prop2.SizeType)
            {
                newProp = prop1.PropType;
            }
            else
            {
                newProp = prop2.PropType;
            }
        }
        newSize = ComplereSize(prop1.SizeType, prop2.SizeType);
        newGear = prop1.Gear+1;
        Vector3 cenPos = (prop2.transform.position + prop1.transform.position) / 2;
        CanFusion = false;
        prop2.CanFusion = false;
        prop1.transform.DOMove(cenPos,0.5F);
        prop2.transform.DOMove(cenPos,0.5F).OnComplete(() => {
            if (prop1.PropType != prop2.PropType) return;
            AudioMgr.Instance.PlaySFX("炸弹合成");
            prop1.Eliminat();
            prop2.Eliminat();

            //prop1.canTiggle = true;
            //prop2.canTiggle = true;

            //Debug.Log(newProp+"EEE"+newGear+"EEE"+ prop1.Gear);
            //if (newGear)
            //{

            //}
#if Animal
            string name = null;
            switch (newGear)
            {
                case 2:
                    name = Pool.ShiziProp;
                    break;
                case 3:
                    name = Pool.QQProp;
                    break;
                default:
                    break;
            }
                Prop prop = Pool.Instance.Spawn(Pool.Prop_PoolName, name).GetComponent<Prop>();
#else
            Prop prop = Pool.Instance.Spawn(Pool.Prop_PoolName, GetStorName(newProp,newGear)).GetComponent<Prop>();

#endif
        
            prop.transform.position = cenPos;
            prop.transform.localScale =Vector3.one*GetSize(newSize);
            prop.transform.SetParent(GameManager.Instance.level.AllBallParent);
            object[] all = new object[2] { newSize,newGear };
            prop.Init(all);
            prop1.canTiggle = true;
            prop2.canTiggle = true;
        });
       




    }

    public string GetStorName(SortType sortType,int Gear) {
        switch (sortType)
        {
            case SortType.Red:

                return Pool.HorizontalProp;
                //switch (Gear)
                //{
                //    case 1:
                        
                //    case 2:
                //        return Pool.CrossProp;
                //    case 3:
                //        return Pool.BombProp;
                //    default:
                //        break;
                //}
                break;
            case SortType.Yellow:
                return Pool.EliminateOrgne;
                //switch (Gear)
                //{
                //    case 1:
                       
                //    case 2:
                //        return Pool.XProp; 
                //    case 3:
                //        return Pool.BoxProp;
                //    default:
                //        break;
                //}
                break;
            case SortType.Blue:
                return   Pool.BlueProp;
            case SortType.Green:
                return Pool.PropGreen;
            case SortType.Orange:
                return Pool.UprihtProp;
            case SortType.Cyan:
                return Pool.LightBall;
            case SortType.Pink:
                return Pool.DelayedProp;
            case SortType.Purple:
                return Pool.PurpProp;
        }
        return null;
    }
    public void FixedUpdate()
    {
        if (isReady)
        {
            if (waitTimes > 0)
            {
                waitTimes -= Time.deltaTime;
            }
            else {
                isReady = false;
                waitTimes = 0;
                //GameManager.Instance.GravityDisappear();
                Physics2D.gravity = new Vector2(0, 5);
                Observable.TimeInterval(System.TimeSpan.FromSeconds(0.2f)).Subscribe(_X =>
                {
                    Physics2D.gravity = new Vector2(0, -9.81f);

                });
                Observable.TimeInterval(System.TimeSpan.FromSeconds(0.05f)).Subscribe(_X =>
                {
                    OnClick();

                });
               

            }
        }
    }
}
