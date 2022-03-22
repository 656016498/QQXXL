using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SugarCube : Ball
{
    //档位
    public int Gear;
    public Sprite[] sugerSprite;
    public float[] size;
    // Start is called before the first frame update
  new public  void Start()
    {
        base.Start();
    }
    public override void Init(SortType sort, bool ISFix,int LE)
    {
        Gear = LE;
        base.Init(sort, ISFix);
        isPut = false;
        transform.GetComponent<SpriteRenderer>().sprite = sugerSprite[Gear-1];
        Destroy(transform.GetComponent<CircleCollider2D>());
        gameObject.AddComponent<CircleCollider2D>();
    }

    public override int Eliminat(int numBase=1,bool f=false)
    {
        if (Gear == 1)
        {
            FlyRomdIce();
            if (isPut)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Pool.Instance.Despawn(Pool.Ball_PoolName, transform);
            }
            return 0;
        }
        else {
            Gear--;
            FlyRomdIce();
            transform.GetComponent<SpriteRenderer>().sprite = sugerSprite[Gear-1];
            Destroy(transform.GetComponent<CircleCollider2D>());
            gameObject.AddComponent<CircleCollider2D>();
            return 0;
        }

    }


    public void FlyRomdIce()
    {
        if (GameManager.Instance.OverGame) return;
        for (int i = 0; i < 5; i++)
        {
            Fly();
        }



     //GameManager.Instance.  NeedSpwanIce += 6;
     //   if (GameManager.Instance.colorBalls.Count > 6)
     //   {
     //       for (int i = 0; i < GameManager.Instance.NeedSpwanIce; i++)
     //       {
     //           Fly();
     //       }
     //       GameManager.Instance.NeedSpwanIce -= 6;
     //   }
     //   else
     //   {
     //       GameManager.Instance.NeedSpwanIce -= GameManager.Instance.colorBalls.Count;
     //       for (int i = 0; i < GameManager.Instance.colorBalls.Count; i++)
     //       {

                
     //           var R = UnityEngine.Random.Range(0, GameManager.Instance.colorBalls.Count);
     //           DynamicMgr.Instance.LineFly(Pool.Ball_PoolName, Pool.IceFly, transform.position, GameManager.Instance.colorBalls[R].transform.position);
     //           GameManager.Instance.colorBalls[R].SetSubger();
     //       }
     //       GameManager.Instance.colorBalls.Clear();

     //   }
    }
   

    public void Fly() {

        var R = RandomNoIce();
        if (R==-1)
        {
            XDebug.LogError("没有彩球可以消除");
            return;
        }
        Transform ice = Pool.Instance.Spawn(Pool.Ball_PoolName, Pool.IceFly);
        ice.transform.position = transform.position;
        ice.SetParent(GameManager.Instance.colorBalls[R].transform);
        ice.DOLocalMove(Vector3.zero,0.75F).SetEase(Ease.InCirc).OnComplete(()=> {
            Pool.Instance.Despawn(Pool.Ball_PoolName, ice);
            GameManager.Instance.colorBalls[R].SetSubger();
        });
    }


    public int RandomNoIce() {
        //if (!GameManager.Instance.OverGame) return;
        if (GameManager.Instance.colorBalls.Count-GameManager.Instance.IceNum==0)
        {
            GameManager.Instance.NeedSpwanIce++;
            return -1;
        }
        var R = Random.Range(0, GameManager.Instance.colorBalls.Count);
        if (!GameManager.Instance.colorBalls[R].IsIce )
        {
            return R;
        }
        else {
            return RandomNoIce();
        }



    }
         
}
