using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chocolate : Ball
{
    [SerializeField]
    bool isPlace=false;//是否是摆放上去物体 
    public int Gear;
  new  public void Start()
    {
        Init(sort, isFix);
        isPlace = true;
    }

    public override void Init(SortType sort, bool ISFix,int i=0)
    {
        this.sort = sort;
        this.isFix = ISFix;
        typeName = ballType.ToString()+"_" + sort.ToString();
        //if (sort == SortType.Coat)
        //{
        transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BallSprite/" + typeName);
    //}
    }
    public override int Eliminat(int Numbase,bool V)
    {
        if (isEliminat) return 0;
        //Debug.LogError("EEEEEEEEEEEEEEE");
        if (sort== SortType.Coat2)
        {
            AudioMgr.Instance.PlaySFX("特殊方块--礼盒巧克力");
            Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.LiheBomb, transform.position);
            sort = SortType.Coat;
            //transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BallSprite/" + ballType.ToString() +);
            Init(sort, isFix);
        }else
        if (sort == SortType.Coat)
        {
            AudioMgr.Instance.PlaySFX("特殊方块--礼盒巧克力");
            Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.ChocolateCodyPoL, transform.position);
            sort = SortType.Default;
            //transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BallSprite/" + ballType.ToString() +);
            Init(sort, isFix);
            return 0;
        }
        else
        {
            AudioMgr.Instance.PlaySFX("特殊方块--巧克力");
            Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.ChocolatePoLie, transform.position);
            if (GameManager.Instance.IsCondition(typeName))
            {

                if (GameManager.Instance.nowConditionR[typeName] > 0)
                {
                    //GameManager.Instance.nowConditionR[typeName]--;
                    GameManager.Instance.ReduceTarget(typeName);
                    Fly(GameManager.Instance.GetCondition(typeName), ()=> {
                        
                        EventManager.Instance.ExecuteEvent(MEventType.PassConditon, GameManager.Instance.nowConditionR);

                    });
                }
            }
            //typeName = ballType.ToString() + SortType.Default.ToString();
            if (isPlace)
            {

                Destroy(this.gameObject);
                //GameManager.Instance.ReduceTarget(typeName);
                
                    //Debug.LogError("通关了");
                
                return 0;
            }
            else
            {
                isEliminat = true;
                Pool.Instance.Despawn(Pool.Ball_PoolName, transform);

                if (GameManager.Instance.nowConditionR[typeName] > 0)
                {
                    //GameManager.Instance.nowConditionR[typeName]--;
                    GameManager.Instance.ReduceTarget(typeName);
                    Fly(GameManager.Instance.GetCondition(typeName), () => {
                        EventManager.Instance.ExecuteEvent(MEventType.PassConditon, GameManager.Instance.nowConditionR);

                    });
                }
                //{
                //Debug.LogError("通关了");
                //GameManager.Instance.PassGame();

                //}


                

            }

            if (GameManager.Instance.SpecailBallNum.ContainsKey(ballType.ToString() + "_" + SortType.Coat))
            {
                GameManager.Instance.SpecailBallNum[ballType.ToString() + "_" + SortType.Coat]--;
                GameManager.Instance.DownSpecailBall();
            }
            else if (GameManager.Instance.SpecailBallNum.ContainsKey(ballType.ToString() + "_" + SortType.Coat2))
            {
                GameManager.Instance.SpecailBallNum[ballType.ToString() + "_" + SortType.Coat2]--;
                GameManager.Instance.DownSpecailBall();
            }
         
        }

        return 1;
    }

}
