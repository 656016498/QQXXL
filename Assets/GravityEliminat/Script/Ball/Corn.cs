using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corn : Ball
{
    // Start is called before the first frame update
    new public void Start() {

        base.Start();
    }

    public  void Init(BallType ball, SortType sort, bool ISFix)
    {
        ballType = ball;
        base.Init(sort, ISFix);
    }

    public override int Eliminat(int numBase,bool B)
    {
        
        switch (ballType)
        {
            case BallType.CornKernel:
                AudioMgr.Instance.PlaySFX("特殊方块--玉米粒");
                Pool.Instance.SpawnEffect(Pool.Effect_PoolName,Pool.CornKernelBomb,transform.position);
                Transform cornTran= Pool.Instance.Spawn(Pool.Ball_PoolName,Pool.Ball_PopCorn);
                cornTran.position = transform.position;
                cornTran.transform.SetParent(GameManager.Instance.level.AllBallParent);
                cornTran.GetComponent<Corn>().Init(BallType.PopCorn,SortType.Default,isFix);
                if (isPut)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    Pool.Instance.Despawn(Pool.Ball_PoolName, transform);
                }
                return 0;
            case BallType.PopCorn:
                AudioMgr.Instance.PlaySFX("特殊方块--爆米花");
                Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.CornKernelBomb, transform.position);
                ballType = BallType.CornKernel;
                return base.Eliminat(numBase);


            default:
                return 0;
        }
        
    }
}
