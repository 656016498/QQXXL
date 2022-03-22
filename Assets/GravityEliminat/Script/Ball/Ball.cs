using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UniRx;
using ClockStone;

interface CanClick {


}
public class Ball : MonoBehaviour
{
    //[HideInInspector]
    public bool isPut = false;    //是否是摆在地图上的物体
    //[HideInInspector]
    protected bool NeedInit = true;   //是否需要初始化
    [HideInInspector]
    public bool isSpecail = false;  //是否是特殊
    public bool isEliminat = false;//用于黑云判断
    //bool highlight = false;
    public BallType ballType; //类型
    public SortType sort;//颜色类别
    public bool isFix;//是否是固定数量
    public string typeName;
    float waitTime;
    int returnScore;
    
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="sort">类别</param>
    /// <param name="ISFix">是否固定</param>
    /// <param name="Gear">档位</param>

    public virtual void  Start()
    {
        if (NeedInit)
        {
            Init(sort, false);
            isPut = true;
        }
    }
    public virtual void Init(SortType sort, bool ISFix,int Gear= 0) {

        NeedInit = false;
        isEliminat = false;
        this.sort = sort;
        isFix = ISFix;
        typeName = ballType.ToString() + "_" + sort.ToString();

    }
    //public Ball(BallType ballType, SortType color,bool isFix) {
    //    this.ballType = ballType;
    //    this.color = color;
    //    this.isFix = isFix;
    //}

    /// <summary>
    /// 返回是否需要重新填充球
    /// </summary>
    public virtual int Eliminat(int soreBase = 1,bool needAddPower=false)
    {
        if (isEliminat)
        {
            return 0;
        }
        isEliminat = true;
        if (soreBase < 8)
        {
            waitTime = soreBase * 0.085F/*+(soreBase)*0.08f*/;
        }
        else
        {
            waitTime = 7 * 0.085F +0.015f* soreBase;
        }
        //Debug.LogError("/////eee");
        if (GameManager.Instance.IsCondition(typeName))
        {
            var targetNum = GameManager.Instance.nowConditionR[typeName];

            GameManager.Instance.ReduceTarget(typeName);
            Observable.TimeInterval(System.TimeSpan.FromSeconds(waitTime)).Subscribe(_ =>
            {
                if (targetNum > 0)
                {
                    Fly(GameManager.Instance.GetCondition(typeName), ()=> {
                        EventManager.Instance.ExecuteEvent(MEventType.PassConditon, GameManager.Instance.nowConditionR);
                        //EventManager.Instance.ExecuteEvent(MEventType.PassConditon, GameManager.Instance.nowConditionR);
                        //GameManager.Instance.IsPass()
                    });
                    //if (isPut)
                    //{
                    //    Destroy(gameObject);
                    //}
                    //else
                    //{
                    //    Pool.Instance.Despawn(Pool.Ball_PoolName, transform);
                    //}
                }
                //else {
                    //if (isPut)
                    //{
                    //    Destroy(gameObject);
                    //}
                    //else
                    //{
                    //    Pool.Instance.Despawn(Pool.Ball_PoolName, transform);
                    //}
                //}

            });

           

        }
        //else
        //{
            Observable.TimeInterval(System.TimeSpan.FromSeconds(waitTime)).Subscribe(_ => {
                //球球消除特效
                Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.BallEnimlit, transform.position);

                transform.DOScale(Vector3.one * GameManager.Instance.ballSize * 0.085F, 0.1F).SetEase(Ease.Linear).OnComplete(()=> {
                   
                   transform.localScale = Vector3.one * GameManager.Instance.ballSize;
               });
                if (ballType==BallType.ColorBall)
                {
                    AudioMgr.Instance.PlaySFX("消除方块");
                }
                Observable.TimeInterval(System.TimeSpan.FromSeconds(0.04F)).Subscribe(_1 =>
                {

                    //if (soundObj != null) soundObj.completelyPlayedDelegate = OnAudioCompleteleyPlayed;
                    
                    //if (!GameManager.Instance.OverGame)
                    //{
                    //    if (needAddPower && DataManager.Instance.data.UnlockLevel >= 6)
                    //    {
                    //        DynamicMgr.Instance.FlyEffectLine(transform.position, UIManager.Instance.GetBase<GamePanel>().energybtn.transform.position, Pool.StarFM, 1, null, 2f);
                    //    }
                    //}

                    if (isPut)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        Pool.Instance.Despawn(Pool.Ball_PoolName, transform);
                    }
                });
            });

        if (DataManager.Instance.data.UnlockLevel>=6&&!GameManager.Instance.OverGame)
        {

            DynamicMgr.Instance.FlyEffectLine(transform.position, UIManager.Instance.GetBase<GamePanel>().energybtn.transform.position, Pool.StarFM, 1, null, 2f);

        }

        //DynamicMgr.Instance.FlyEffectLineUI(transform.position, UIManager.Instance.GetBase<GamePanel>().SorePro.transform.parent.position, Pool.XXFM,1, null, 0.5f);




        //if (GameManager.Instance.IsCondition(typeName))
        //{

        //    Pool.Instance.Despawn(Pool.Ball_PoolName, transform);
        //    

        //}
        //else
        //{




        //飞星星粉末

        //});
        //}
        //}
        returnScore = soreBase * 10;
        if (returnScore>=50)
        {
            returnScore = 50;
        }
        GameManager.Instance.LevelScore.Value += returnScore;
            //飞分数
            //DynamicMgr.Instance.FlyText(transform.position,soreBase*100);
            //Debug.Log("GameManager.Instance.LevelScore.Value" + GameManager.Instance.LevelScore.Value);
          

            //    else
            //    {
            //        return 0;
            //    }
            //}
            //else { 
            //return 0;
            //}
        //}
        return 1;
    }
   


    public virtual void Fly(int index,System.Action action) {

        
        DynamicMgr.Instance.WordPosFlyUI(typeName,transform.position, UIManager.Instance.GetBase<GamePanel>().PassParent.GetChild(index).transform.position,action);


    }

    public void DelayedFly() { 
    
    

    }


    public void SetHight() {

        Transform effect = Pool.Instance.SpawnEffectByParent(Pool.Effect_PoolName, Pool.HightL,transform,Vector3.zero);

       
    }
    
}
