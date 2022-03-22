using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : Ball
{

    public BallType willType;
    public SortType willSotr;
    public int Gear;
    List<Hive> needClanHive = new List<Hive>();
    List<Hive> tempHive = new List<Hive>();
    [Header("镜头是否移动")]
    public bool NextStep;
    new private void Start()
    {
        isEliminat = false;
        ballType = BallType.Hive;
        sort = SortType.Default;
        needClanHive.Clear();
        //if (NextStep)
        //{
        //    transform.GetComponent<BoxCollider2D>().enabled = true;
        //    RangHive(this);
        //}
        typeName = ballType.ToString() + "_" + sort.ToString();
        
    }
    public override void Init(SortType sort, bool ISFix, int Gear = 0)
    {

        base.Init(sort, ISFix, Gear);

    }
    public override int Eliminat(int Numbase,bool N=false)
    {
        if (isEliminat) return 0;
        AudioMgr.Instance.PlaySFX("特殊方块--蜂蜜");
        Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.HiveBomb, transform.position);
        Gear--;
        //Debug.Log("蜜蜂消除" + Gear);
     
       

        if (Gear == 0)
        {
            isEliminat = true;
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
            Ball ball = Pool.Instance.Spawn(Pool.Ball_PoolName, GameManager.Instance.GetParfabName(willType)).GetComponent<Ball>();
            ball.Init(willSotr, false);
            ball.transform.position = transform.position;
            ball.transform.SetParent(GameManager.Instance.level.AllBallParent);
            //if (NextStep)
            //{
            //    foreach (var item in needClanHive)
            //    {
            //        if (item != null)
            //        {
            //            item.ClenSelf();
            //        }
            //    }
            //    GameManager.Instance.LevelStep++;
            //    Debug.Log("关卡步数"+ GameManager.Instance.LevelStep);
            //    EventManager.Instance.ExecuteEvent(MEventType.LevelNextStep, GameManager.Instance.LevelStep);
            //}
            Destroy(this.gameObject);
        }
        else
        if (Gear == 1)
        {

            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
        }

        else if (Gear == 2)
        {
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }

        return 0;
    }

    //检测周围蜂槽
    public void RangHive(Hive orginHive)
    {

        Collider2D[] hitRang = Physics2D.OverlapCircleAll(transform.position, transform.GetComponent<SpriteRenderer>().bounds.size.y / 2+0.4F);
        tempHive.Clear();
        for (int i = 0; i < hitRang.Length; i++)
        {
            Hive hive = hitRang[i].transform.GetComponent<Hive>();
            if (hive != null)
            {
                if (!needClanHive.Contains(hive))
                {
                    if (orginHive != hive)
                    {
                        tempHive.Add(hive);
                    }
                    else
                    {
                        needClanHive.Add(orginHive);
                    }
                }
            }
        }
        if (tempHive.Count > 0)
        {
            for (int i = 0; i < tempHive.Count; i++)
            {
                RangHive(tempHive[i]);
            }
        }
    }

    public void ClenSelf()
    {
        Gear = 1;
        Eliminat(0);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<Ball>()!=null&& collision.transform.GetComponent<Hive>()==null)
        {
            transform.GetComponent<CircleCollider2D>().enabled = false;
            foreach (var item in needClanHive)
            {
                if (item != null)
                {
                    item.ClenSelf();
                }
            }
        }
    }

    public void Update()
    {
        if (CameraManager.Instance.CanMoveCamera&&CameraManager.Instance.transform.position.y+2<transform.position.y)
        {
            ClenSelf();
        }
    }
}
