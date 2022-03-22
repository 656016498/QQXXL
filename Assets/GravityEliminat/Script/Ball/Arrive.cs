using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Arrive : Ball
{
    bool canDetect;
  new   private  void Start()
    {
        base.Start();
        Observable.TimeInterval(System.TimeSpan.FromSeconds(5)).Subscribe(_ => {

            canDetect = true;
        });
    }

    public override void Init(SortType sort, bool ISFix, int Gear = 0)
    {
        canDetect = false;
        Observable.TimeInterval(System.TimeSpan.FromSeconds(5)).Subscribe(_ => {

            canDetect = true;
        });
        base.Init(sort, ISFix, Gear);
    }
    public override int Eliminat(int soreBase = 1,bool L=false)
    {
        if (!canDetect) return 0;
        AudioMgr.Instance.PlaySFX("特殊方块--旋转球");
        return base.Eliminat(soreBase);
    }
}
