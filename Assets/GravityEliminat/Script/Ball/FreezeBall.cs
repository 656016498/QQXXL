using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBall : Ball
{
    public int Gear=1;
    // Start is called before the first frame update
   public override void Start()
    {
        Debug.LogError("NeedInit" + NeedInit);
        base.Start();
    }
    public override void Init(SortType sort, bool ISFix, int Gear = 0)
    {
        this.Gear = Gear;
        base.Init(sort, ISFix, Gear);
        SetSprite();

    }

    public override int Eliminat(int soreBase = 1, bool needAddPower = false)
    {
        if (isEliminat)
        {
            return 0;
        }
        Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.BingKuai, transform.position);
        Gear--;
        if (Gear<=0)
        {
            //foreach (var item in GameManager.Instance.SpecailBallNum)
            //{
            //    //Debug.LogError(item.Key+"EEE"+item.Value);
            //}
            if ( GameManager.Instance.SpecailBallNum.ContainsKey(typeName))
            {
                GameManager.Instance.SpecailBallNum[typeName]--;
                GameManager.Instance.DownSpecailBall();
            }
            isEliminat = true;
            Ball ball = Pool.Instance.Spawn(Pool.Ball_PoolName, GameManager.Instance.GetParfabName(BallType.ColorBall)).GetComponent<Ball>();
            ball.Init(sort, false);
            ball.transform.position = transform.position;
            ball.transform.SetParent(GameManager.Instance.level.AllBallParent);
            GameManager.Instance.colorBalls.Add(ball.GetComponent<ColorBall>());
            Pool.Instance.Despawn(Pool.Ball_PoolName, transform);
            transform.gameObject.SetActive(false);
        }
        else
        {
            SetSprite();
        }
        return 0;
    }

    public void SetSprite()
    {
       transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BallSprite/Freeze_" +sort.ToString()+"_"+ Gear.ToString());
    }
}
