using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBall : Ball
{
   
    
    public override int Eliminat(int soreBase = 1, bool needAddPower = false)
    {
        if (isEliminat) return 0;
        isEliminat = true;
        DynamicMgr.Instance.WordPosFlyUI("Cat", transform.position, UIManager.Instance.GetBase<GamePanel>().CatBtn.transform.position,null);
        CatManager.Instance.catData.CatNum++;
        CatManager.Instance.NowCatNum--;
        CatManager.Instance.catData.now[1]++;
        CatManager.Instance.CanGet();
        Pool.Instance.Despawn(Pool.Ball_PoolName,transform);
        Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.BallEnimlit, transform.position);
        CatManager.Instance.SaveData();
        return 0;
    }

    
}
