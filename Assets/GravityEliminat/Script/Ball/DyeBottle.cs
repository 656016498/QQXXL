using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class DyeBottle : Ball
{
    public float DefSize;
    private void Awake()
    {
        DefSize = transform.localScale.x;

    }
    public override void Start()
    {
        base.Start();
    }
    public override void Init(SortType sort, bool ISFix, int Gear = 0)
    {

        base.Init(sort, ISFix, Gear);
        NeedInit = false;
        transform.GetComponent<SpriteRenderer>().sprite= Resources.Load<Sprite>("BallSprite/"+ typeName);
        transform.localScale = Vector3.one * DefSize;
    }

    public override int Eliminat(int numBase,bool B)
    {
        GameManager.Instance.DyeChangeColor(sort, transform.position);
        Pool.Instance.SpawnEffect(Pool.Effect_PoolName,Pool.DyeBoom+sort,transform.position);
        return base.Eliminat(numBase);
    }
}
