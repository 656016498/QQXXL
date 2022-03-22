using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UniRx;
public class MagicFly : MonoBehaviour
{
    List<Vector3> FlyPoint = new List<Vector3>();
    public Transform flyParent;
    public Button Magic;
    public Button selfBtn;
    Tween flyTween;
    public float Delay;
    public Vector2 defaultVct;
    // Start is called before the first frame update
    void Awake()
    {

        for (int i = 0; i < flyParent.childCount; i++)
        {
            FlyPoint.Add(flyParent.GetChild(i).transform.position);
        }

        //flyTween.Pause();
        Magic.onClick.AddListener(() => {
            Magic.gameObject.SetActive(false);
            flyTween.Kill();
            //AdControl.Instance.ShowRwAd("fly_hb_video", () => {
                //Debug.Log("/////llll" + willForm);

                if (willForm == "Game")
                {
                    UmengDisMgr.Instance.CountOnNumber("fly_hb_gq_get", GameManager.Instance.CurrentLevel.ToString());
                    //Debug.Log("/////llll" + willForm);
                }
                else if (willForm == "Main")
                {
                    UmengDisMgr.Instance.CountOnNumber("fly_hb_zy_get");
                }


                var popup1 = UIManager.Instance.ShowPopUp<OpenRedPopup3>();
                popup1.OnOpen("fly_hb_video", 0, () => {
                    Observable.TimeInterval(System.TimeSpan.FromSeconds(0)).Subscribe(_ => {
                        var rewardRedIcon = RedWithdrawData.Instance.GetRedIcon();
                        var rewardVoucher = (float)rewardRedIcon / (RedWithdrawData.Instance.NowCanCash * 10000);
                        var popup2 = UIManager.Instance.ShowPopUp<RedTwoPopup>();
                        popup2.OnOpen(rewardVoucher, rewardRedIcon, "", () =>
                        {
                            Magic.GetComponent<RectTransform>().anchoredPosition = defaultVct;
                            flyTween.Pause();
                            popup2.effect.SetActive(false);
                            Debug.Log("关闭红包二级界面");
                            if (!GameManager.Instance.isCash)
                            {
                                RedWithdrawData.Instance.UpdateRedIcon(rewardRedIcon);
                            }

                        });
                        popup1.defult.SetActive(false);

                    });
                   
                },()=> { 
                
                });

                //Observable.TimeInterval(System.TimeSpan.FromSeconds(0f)).Subscribe(_ =>
                //{
                    
                //});
               
            });
        selfBtn.onClick = Magic.onClick;
        //});
    }

    string willForm;
    public void BegainFly(string s)
    {
        Magic.GetComponent<RectTransform>().anchoredPosition = defaultVct;
        Magic.gameObject.SetActive(true);
        flyTween = Magic.transform.DOLocalPath(FlyPoint.ToArray(), 30, PathType.CatmullRom).SetDelay(Delay).OnComplete(() => {
            Magic.gameObject.SetActive(false);
            flyTween.Kill();
        });
       
        
        //for (int i = 0; i < flyParent.childCount; i++)
        //{
        //    FlyPoint.Add(flyParent.GetChild(i).transform.position);
        //}
        willForm = s;
        if (willForm == "Main")
        {
            UmengDisMgr.Instance.CountOnNumber("fly_hb_zy_show");
        }
        else if (willForm == "Game")
        {
            UmengDisMgr.Instance.CountOnNumber("fly_hb_gq_show",GameManager.Instance.CurrentLevel.ToString());
        }
        //flyTween = Magic.transform.DOPath(FlyPoint.ToArray(), 20, PathType.CatmullRom).SetDelay(Delay).OnComplete(() => {

        //    Magic.gameObject.SetActive(false);

        //});
        //flyTween.Play();

    }


    
    public void Pause() {
        flyTween.Kill();
        Magic.gameObject.SetActive(false);
        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.1F)).Subscribe(_ => { 
        Magic.GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width / 2 + 150, 419.5f);

        });
    }
    
}
