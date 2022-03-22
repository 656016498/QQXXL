using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TornadoProp : Prop
{
    //public int Gaer;//等级
    public override void OnClick()
    {
        CreataeTornado();
        Pool.Instance.Despawn(Pool.Prop_PoolName, transform);
        Debug.LogError("龙卷风按钮");
    }
    public override void Init(object[] obj = null)
    {
        base.Init(obj);
        if (obj != null)
        {
            SizeType = (Porp_Size)obj[0];
            Gear = (int)obj[1];
            //transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PropSprite/" + PropType.ToString() + Gear);
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PropSprite/" + PropType.ToString() + Gear);

        }
    }


    public void CreataeTornado() {

        Transform transform = Pool.Instance.Spawn(Pool.Prop_PoolName, Pool.Tornado+(Gear));
        transform.localScale = Vector3.zero;
        Tornado tornado = transform.GetComponent<Tornado>();
        object[] vs = new object[1] { Gear - 1};
        tornado.Init(vs);
        transform.DOScale((1 + 0.5F * (int)SizeType),1F).OnComplete(() => {
            tornado.CanMove = true;
            for (int i = 0; i < (Gear - 1)*2; i++)
            {
                Transform transform2 = Pool.Instance.Spawn(Pool.Prop_PoolName, Pool.Tornado + (Gear));
                Tornado tornado2 = transform2.GetComponent<Tornado>();
                object[] vs2 = new object[1] { Gear - 1 };
                tornado2.Init(vs2);
                transform2.localScale = Vector3.one * (1 + 0.5F * (int)SizeType);
                transform2.position = this.transform.position + new Vector3(0, 0.5f, 0);
                tornado2.CanMove = true;
            }
        });
        transform.position = this.transform.position+new Vector3 (0,0.5f,0);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<Prop>() != null)
        {
            Fusion(this, collision.transform.GetComponent<Prop>());
        }
    }
    
}
