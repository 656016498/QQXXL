using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HuxiScript : MonoBehaviour
{
    public float s = 1.1f;

    private bool isBig;

    private Vector3 a;

    private Vector3 b = Vector3.one;

    

    private void Start()
    {
        isBig = true;
        a = new Vector3(s, s, s);
    }

    // Update is called once per frame
    void Update()
    {
        if (isBig && transform.localScale == b)
        {
            transform.DOScale(s, s);
            isBig = false;
        }
        if (!isBig && transform.localScale == a)
        {
            transform.DOScale(1f, 1f);
            isBig = true;
        }
    }
}
