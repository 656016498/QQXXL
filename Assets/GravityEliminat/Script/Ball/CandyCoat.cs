using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyCoat : Ball
{
    public override void Init(SortType sort, bool ISFix,int i=0)
    {
        base.Init(sort,ISFix);
        isSpecail = true;
        transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BallSprite/" +typeName);
    }
    public override int Eliminat(int Numbase=1,bool S=false)
    {
        Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.CandyCoat,transform.position) ;
        Ball ball = Pool.Instance.Spawn(Pool.Ball_PoolName, Pool.Ball_Color).GetComponent<Ball>();
        ball.Init(sort,isFix);
        ball.transform.position = transform.position;
        ball.transform.parent = transform.parent;
        ball.typeName = this.typeName;
        ball.isSpecail = true;
        Pool.Instance.Despawn(Pool.Ball_PoolName, transform);
        return 0;
    }
}
