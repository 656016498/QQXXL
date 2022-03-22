using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InGamePause : UIBase
{
    public IButton closeBtn;
    public IButton exitLevelBtn;
    public IButton playAgainBtn;
    public IButton ServiceBtn;
    public IButton GoMainBtn;
    [Header("音乐")]
    public Toggle musicTog;
    [Header("音效")]
    public Toggle soundTog;
    [Header("震动")]
    public Toggle shockTog;
    void Start()
    {

#if BB108
        exitLevelBtn.gameObject.SetActive(false);

#else
       GoMainBtn.gameObject.SetActive(false);
#endif
        GoMainBtn.onClick.AddListener(() => {

            AdControl.Instance.Parameter("pause_exit_half");
            InfiniteScrollView.Instance.JoinLevel = -1;
            DestoryLevel();
            Hide();

        });
        ServiceBtn.onClick.AddListener(() => {
            UIManager.Instance.Show<ServicePop>(UIType.PopUp);
        });
        closeBtn.onClick.AddListener(Hide);

        exitLevelBtn.onClick.AddListener(() => {

            Hide();
        });
        playAgainBtn.onClick.AddListener(() => {

            AdControl.Instance.Parameter("pause_restart_half");
        
            //UIManager.Instance.GetBase<GamePanel>().FlyHBFun();
            //UIManager.Instance.Show<AgainPop>(UIType.PopUp);
            GameManager.Instance.DestoryLevel();
            GameManager.Instance.LoadLevel();
            Hide();
        });

        musicTog.onValueChanged.AddListener((bool isOn) => {

            if (isOn)
            {
                Debug.Log("打开音乐");
                AudioMgr.Instance.SetTog(AudioMgr.ToggleType.MusicTog, true);
                AudioMgr.Instance.PlayMusic("战斗界面BGM");
            }
            else
            {
                Debug.Log("关闭音乐");
                AudioMgr.Instance.SetTog(AudioMgr.ToggleType.MusicTog, false);
                AudioController.StopMusic();
            }
        });
        soundTog.onValueChanged.AddListener((bool isOn) => {

            AudioMgr.Instance.SetTog(AudioMgr.ToggleType.SoundTog, isOn);
            Debug.Log("是否开启音效");
        });
        shockTog.onValueChanged.AddListener((bool isOn) => {

            AudioMgr.Instance.SetTog(AudioMgr.ToggleType.vibrateTog, isOn);
            Debug.Log("是否开启震动");
        });
    }

    public void DestoryLevel() {

        GameManager.Instance.DestoryLevel();
        Hide();
        UIManager.Instance.Hide<GamePanel>();
        UIManager.Instance.Show<MainPanel>(UIType.Normal);
    
        InfiniteScrollView.Instance.RefreshLevelMap();
    }

    private void RefrishUi()
    {

        var mdata = AudioMgr.Instance.mdate;
        //刷新音乐开关
        musicTog.isOn = mdata.isMusic;
        soundTog.isOn = mdata.isSound;
        shockTog.isOn = mdata.isVibrate;

    }

    public override void Show()
    {
        RefrishUi();
        base.Show();
        GameADControl.Instance.ShowMsg(true);
    //public void ShowMsg(bool isShow)

    }
    public override void Hide()
    {
        base.Hide();
        AdControl.Instance.Parameter("pause_close_half");
        GameADControl.Instance.ShowMsg(false);


    }
}
