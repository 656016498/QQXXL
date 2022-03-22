using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
public enum TreasureType {
    Starlight,
    Challenge
}
public class Treasure: MonoBehaviour
{
    public bool IsGamePass;
    public Text Numtext;
    Button selfBtn;
    //bool canClick=true;
    public Image pro;
    public Animator animator;
    public Text tipText;
    public Transform effectTran;
    public Text dirText;
    private void Start()
    {

        selfBtn = transform.GetComponent<Button>();
        animator.SetBool("canShake",false);
       
        selfBtn.onClick.AddListener(() => {
            AudioMgr.Instance.PlaySFX("onClick");
            //if (GameManager.Instance.StarShineStarSub.Value>=10)
            //{
            //    //canClick = false;
            //    //TODO 打开界面
            //    UIManager.Instance.Show<TreasurePop>(UIType.PopUp, TreasureType.Starlight);
            //}
           
                ShowPublicTip.Instance.Show( string.Format("再过{0}关即可打开宝箱！", DataManager.Instance.GetTargetBox() - GameManager.Instance.StarShineStarSub.Value) );

        });


        GameManager.Instance.StarShineStarSub.Subscribe(vaule => {

            Init();

        });
        oldTreasure = DataManager.Instance.data.StarshineStar;
    }

    int oldTreasure;
    public void Init()
    {
        int target = DataManager.Instance.GetTargetBox();
        if (GameManager.Instance.StarShineStarSub.Value < 10)
        {
            Numtext.text = string.Format("{0}/{1}", GameManager.Instance.StarShineStarSub.Value, target);
            tipText.transform.parent.HideCanvasGroup();
            animator.SetBool("canShake", false);
            transform.localEulerAngles = Vector3.zero;
            effectTran.gameObject.SetActive(false);
        }
        else
        {
            //Numtext.text = "点击开启";
            Numtext.text = string.Format("{0}/{1}", GameManager.Instance.StarShineStarSub.Value, target);
            animator.SetBool("canShake", true);
            tipText.transform.parent.ShowCanvasGroup();
            tipText.text = string.Format("剩余{0}个", GameManager.Instance.StarShineStarSub.Value / 10);
            effectTran.gameObject.SetActive(true);
        }
       
        pro.fillAmount = (float)(oldTreasure > target ? target : oldTreasure) / target;
        pro.DOFillAmount((float)(GameManager.Instance.StarShineStarSub.Value > target ? target : GameManager.Instance.StarShineStarSub.Value) / target, 1).SetDelay(1);
        var can = DataManager.Instance.GetTargetBox() - GameManager.Instance.StarShineStarSub.Value;
        if (can == 0)
        {
            dirText.text = string.Format("点击【打开宝箱】可获得现金奖励！");
            
        }
        else {

            dirText.text = string.Format("再过<color=#FF0E07><size=28>{0}</size></color>关开宝箱，可获得<color=#FF0E07>现金奖励</color>！", can);

        }
      
        oldTreasure = GameManager.Instance.StarShineStarSub.Value;
    }

}
