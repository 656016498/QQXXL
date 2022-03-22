using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BallsMoving : MonoBehaviour
{
    public Transform[] balls;
    public Transform mid;
    private Vector2[] starPoss;
    private List<Vector3> path;
    private System.Random random;
    private int steps = 3;
    // Start is called before the first frame update
    void Start()
    {
        path = new List<Vector3>();
        random = new System.Random();
        Vector2 minPos = new Vector2(mid.localPosition.x, mid.localPosition.y);
        //balls = transform.GetComponentsInChildren<Image>().;
        starPoss = new Vector2[balls.Length];
        for (int i = 0; i < balls.Length; i++)
        {
            starPoss[i] = balls[i].localPosition;
        }
        for (int i = 0; i < steps; i++)
        {
            path.Add(new Vector2(-200,random.Next(-90,90))+ minPos);
            
        }
        for (int i = 0; i < steps; i++)
        {
            path.Add(new Vector2(200, random.Next(-90, 90)) + minPos);

        }
        for (int i = 0; i < steps; i++)
        {
            path.Add(new Vector2(random.Next(-200, 200), -90) + minPos);

        }
        for (int i = 0; i < steps; i++)
        {
            path.Add(new Vector2(random.Next(-200, 200), 90) + minPos);

        }


    }
    public void Moving()
    {
        for (int i = 0; i < balls.Length; i++)
        {
            Vector3[] pa = path.OrderBy(c => Guid.NewGuid()).ToArray();
            pa[pa.Length - 1] = starPoss[i];
            balls[i].DOLocalPath(pa, 2, PathType.Linear).SetEase(Ease.OutSine);
        }
    }
   

    
}
