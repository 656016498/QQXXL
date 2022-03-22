using System;
using UnityEngine;
using UnityEngine.UI;

public class WDSiginUI : UIBase
{
    public Button btnClose, btnVideo;
    public Text txtReward, txtDayPro, txtDay, txtVideoPro, txtVideo;
    public Transform txtGet, sigin;
    private Action videoEnd;
    // Start is called before the first frame update
    protected  void Start()
    {
        btnClose.onClick.AddListener(OnClose);
        btnVideo.onClick.AddListener(OnVideo);
    }

    private void OnVideo()
    {
        var todayVideos = RedWithdrawData.Instance.redDayData.todayWithdrawVideos;
        if (todayVideos>=RedWithdrawData.Instance.daySiginVideo)
        {
            //弹出提示
            //ShowText("今日签到已达上限，请明日再来。");
            return;
        }
        AdControl.Instance.ShowRwAd("hundred_twenty_video", () => {
            videoEnd?.Invoke();
        });

    }
    public void OnShow(float reward,Action video)
    {
        txtReward.text = string.Format("{0}元",reward);
        videoEnd = video;
        UpdateUi();
        txtGet.gameObject.SetActive(false);
        sigin.gameObject.SetActive(true);
    }

    public void UpdateUi()
    {
        var siginTimes = RedWithdrawData.Instance.redData.withdrawSiginTimes;
        var todayVideos = RedWithdrawData.Instance.redDayData.todayWithdrawVideos;
        var allTimes = RedWithdrawData.Instance.needSiginDay;
        var allVideos = RedWithdrawData.Instance.daySiginVideo;
        txtDay.text = string.Format("需连续签到{0}天",allTimes);
        txtVideo.text = string.Format("看{0}次视频，完成签到", allVideos);
        txtDayPro.text = string.Format("当前进度：<color=#ffeb7a>{0}/{1}</color>天", siginTimes, allTimes);
        txtVideoPro.text = string.Format("当前进度：<color=#9fff65>{0}/{1}</color>次", todayVideos, allVideos);
    }
    public void ShowGet()
    {
        txtGet.gameObject.SetActive(true);
        sigin.gameObject.SetActive(false);
    }
    private void OnClose()
    {
        UIManager.Instance.Hide<WDSiginUI>();
    }

}
