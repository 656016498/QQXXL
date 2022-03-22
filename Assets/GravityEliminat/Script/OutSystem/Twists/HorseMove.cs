using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorseMove : MonoBehaviour
{
    public List<RectTransform> items;
    public float speed = 1;
    private List<Tween> tweens;
    private Vector3  start;
    private float end;
    private void Awake()
    {
        //Debug.Log(items[0].GetComponent<Image>().rectTransform.sizeDelta.x);
        end = items[0].localPosition.x - items[0].GetComponent<Image>().rectTransform.sizeDelta.x + 10;
        start = items[items.Count - 1].localPosition+new Vector3(30,0,0);
        tweens = new List<Tween>();
        
    }
    private void OnEnable()
    {
        for (int i = 0; i < items.Count; i++)
        {
            tweens.Add(MoveItem(items[i],i));
        }
    }
    private Tween MoveItem(RectTransform item,int i)
    {
        float time = (item.localPosition.x - end) / speed;
        return item.DOLocalMoveX(end, time).SetEase(Ease.Linear).OnComplete(()=> {
            item.localPosition = start;
            tweens[i]= MoveItem(item, i);
        });
    }
}
