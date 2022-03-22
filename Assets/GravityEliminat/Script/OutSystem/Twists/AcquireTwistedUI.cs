using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AcquireTwistedUI : UIBase
{
    public Button btnGet;
    public Image imgIcon;
    public Text txtInfo;
    public Sprite spriteRed, spriteIPhone, spriteIPad;
    private Action get;
    public Transform effet;
    public GameObject effectStar;
    private void Start()
    {
        btnGet.onClick.AddListener(OnGet);
        effet.DOLocalRotate(new Vector3(0, 0, 359), 4, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1); ;
    }
    public void OnShow(string info,int id,Action onGet)
    {
        txtInfo.text = info;
        get = onGet;
        switch (id)
        {
            case 1:
                imgIcon.sprite = spriteIPhone;
                break;
            case 2:
                imgIcon.sprite = spriteIPad;
                break;
            default:
                imgIcon.sprite = spriteRed;
                break;
        }
        effectStar.gameObject.SetActive(true);
    }
    private void OnGet()
    {
        UIManager.Instance.Hide<AcquireTwistedUI>();
        effectStar.gameObject.SetActive(false);
        get?.Invoke();
       
    }
}
