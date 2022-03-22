using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminateLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ball>()!=null)
        {
            collision.GetComponent<Ball>().Eliminat();
        }
        else if (collision.GetComponent<Prop>() != null)
        {
            Prop prop = collision.GetComponent<Prop>();
            prop.Eliminat();
            Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.BallEnimlit, prop.transform.position);
        }
        else if (collision.GetComponent<MoneyBall>() != null)
        {
            MoneyBall moneyBall= collision.GetComponent<MoneyBall>();
            Pool.Instance.Despawn(Pool.Ball_PoolName, moneyBall.transform);
            Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.BallEnimlit, moneyBall.transform.position);
        }
    }
}
