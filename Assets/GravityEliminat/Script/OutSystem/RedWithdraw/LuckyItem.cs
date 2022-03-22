using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuckyItem : MonoBehaviour
{
    private Image imgHL;
    private Text txtReward;
    //闪烁间隔
    private float inv = 0.2f;
    private Action onend;
    private void Awake()
    {
        imgHL = transform.Find("imgHightLight").GetComponent<Image>();
        txtReward = transform.Find("txtReward").GetComponent<Text>();

    }
    // Start is called before the first frame update
    void Start()
    {
        SetHL(false);
    }
    //设置是否高亮
    public void SetHL(bool isHL)
    {
        imgHL.gameObject.SetActive(isHL);
    }
    public void SetValue(string v)
    {
        if (v == "0") v = "??";
        txtReward.text = v+"元";
    }
    public void OnBlink(Action end)
    {
        onend = end;
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        yield return new WaitForSeconds(inv);
        SetHL(false);
        yield return new WaitForSeconds(inv);
        SetHL(true);
        yield return new WaitForSeconds(inv);
        SetHL(false);
        yield return new WaitForSeconds(inv);
        SetHL(true);
        yield return new WaitForSeconds(inv);
        SetHL(false);
        yield return new WaitForSeconds(inv);
        SetHL(true);
        yield return new WaitForSeconds(inv);
        SetHL(false);
        onend?.Invoke();
    }
}
