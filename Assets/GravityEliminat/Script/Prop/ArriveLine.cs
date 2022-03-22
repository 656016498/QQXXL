using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class ArriveLine : MonoBehaviour
{
    private void Start()
    {
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<Ball>() != null)
        {
            Ball arriveBall = collision.transform.GetComponent<Ball>();
            if (arriveBall.ballType == BallType.Arrive)
            {
                arriveBall.Eliminat();
            }
        }
    }
}
