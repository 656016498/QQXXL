using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SliderControl : MonoBehaviour
{
    [Range(0, 1.0f)]
    [SerializeField]
    private float Slider;
    [SerializeField]
    private Image SliderValue;
    [SerializeField]
    private Text SliderText;


    private bool IsUpdateText = false;

    private void Start()
    {
        IsUpdateText = (SliderText == null || SliderText.gameObject.activeSelf);
        mLastValue = -1;
        UpdateSlider(0);        
    }

    public void SetSlider(float value)
    {
        value = Mathf.Clamp(value, 0, 1);
        Slider = value;        
    }

    private void Update()
    {
       UpdateSlider(Slider);
    }


    private float mLastValue;
    public void UpdateSlider(float value)
    {
        if (mLastValue == value) return;
        mLastValue = value;
        SliderValue.DOFillAmount(value,1f);
        if (IsUpdateText)
        {
            SliderText.text = string.Format("{0}%", (value * 100).ToString("f0"));
        }      
    }

    public void UpdateInfo(float value,string text)
    {
       
        SliderValue.fillAmount = value;
        SliderText.text = text;
        Debug.Log("SliderValue.fillAmount"+ SliderValue.fillAmount);
    }

}
