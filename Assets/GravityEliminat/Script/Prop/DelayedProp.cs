using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedProp : Prop
{

    public bool CanClick;
    public List<Vector3> balls = new List<Vector3>();

    public override void Init(object[] obj = null)
    {
        base.Init(obj);
        CanClick = true;
    }

    public override void OnClick()
    {
        base.OnClick();
        balls.Clear();
        WillDO();
    }

    public void WillDO()
    {

        for (int i = 0; i < (int)Gear * 2; i++)
        {
            int p = i;
            balls.Add(RomdRang());
            DynamicMgr.Instance.FlyEffectLine(transform.position, balls[p], Pool.LiuGuang, 1,() =>
        {
            DelayedBomb bomb = Pool.Instance.Spawn(Pool.Prop_PoolName, Pool.DelayedBomb).GetComponent
            <DelayedBomb>();
            bomb.Gear = Gear;
            bomb.transform.position = balls[p];
            bomb.transform.localScale = Vector3.one * GetSize(SizeType);
            if (i < 2)
            {
                bomb.Radius = 2 + (int)SizeType * 0.5f; bomb.DelayeNum = 1;
            }
            else if (i < 4)
            {
                bomb.Radius = 3 + (int)SizeType * 0.5f; bomb.DelayeNum = 2;
            }
            else if (i < 6)
            {
                bomb.Radius = 4 + (int)SizeType * 0.5f; bomb.DelayeNum = 3;
            }
            if (p == Gear * 2 - 1)
            {
                Eliminat();
            }
        });
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<Prop>() != null)
        {
            Fusion(this, collision.transform.GetComponent<Prop>());
        }
    }

}
