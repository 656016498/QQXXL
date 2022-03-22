using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SodaBall : Ball
{

    new public void Start() {

        base.Start();
    }

    public override void Init(SortType color, bool ISFix,int i=0)
    {
        base.Init(SortType.Default, ISFix);
    }
    public override int Eliminat(int i,bool V=false)
    {
        if (isPut)
        {
            Destroy(this.gameObject);
        }
        else { 
        Pool.Instance.Despawn(Pool.Ball_PoolName, transform);

        }
        AudioMgr.Instance.PlaySFX("特殊方块--汽水罐");
        Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.SodaBomb,transform.position);
        return 1;
    }
}
