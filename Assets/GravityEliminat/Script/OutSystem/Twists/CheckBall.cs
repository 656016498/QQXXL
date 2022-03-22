using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CheckBall : MonoBehaviour
{
    public Vector3[] paths;
    public Image ball;
    private Vector3 starPos;

    private void Start()
    {
        ball.gameObject.SetActive(false);
        starPos = transform.localPosition;
    }
    public void Moving(int type)
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Ball/"+type);
        ball.sprite = Resources.Load<Sprite>("Ball/" + type);
        transform.DOLocalPath(paths, 1, PathType.Linear).SetEase(Ease.Linear).OnComplete(()=> { Back(); });
    }

    private void Back()
    {
        transform.localPosition = starPos;
        ball.gameObject.SetActive(true);
        ball.DOFade(1, 1.5f).OnComplete(()=>{
            ball.gameObject.SetActive(false);
        });
    }
}
