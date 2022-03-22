using BayatGames.SaveGamePro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
public class FrontSetPop : UIBase
{

    public IButton closeBtn;
    public IButton customerBtn;
    [Header("音乐")]
    public Toggle musicTog;
    [Header("音效")]
    public Toggle soundTog;
    [Header("震动")]
    public Toggle vibrateTog;
    [Header("当前版本")]
    public Text editionText;
    [Header("用户Id")]
    public Text userId;
    [Header("微信登陆文本")]
    public Text weiChatText;
    [Header("微信登陆")]
    public IButton weiChatLoginBtn;
    [Header("微信退出")]
    public IButton weiChatExitBtn;
    [Header("用户协议")]
    public IButton userXY; 
    [Header("用户政策")]
    public IButton userZC;
    
    public static bool IsFisrtShow = false; 
    void Start()
    {
        AddBtnLinser();
      
        //版本号
        AdControl.Instance.GetEdition((str) => {

            AudioMgr.Instance.SetEdition(str);
        });
        //安卓ID
        AdControl.Instance.GetAndroidID((inf) =>
        {
            AudioMgr.Instance.SetUserId(inf);
        });
        RefrishUi();
        IsFisrtShow = true;
    }

    public override void Show()
    {
           if (!IsFisrtShow) return;
            RefrishUi();
            base.Show();

        //展示信息流
        GameADControl.Instance.ShowMsg(true);
    }
    public override void Hide()
    {
        base.Hide();
        //展示信息流
        GameADControl.Instance.ShowMsg(false);
    }
    //添加按钮监听
    private void AddBtnLinser()
    {
        musicTog.onValueChanged.AddListener(MusicClick);
        soundTog.onValueChanged.AddListener(SoundClick);
        vibrateTog.onValueChanged.AddListener(vibrateClick);
        //weiChatLoginBtn.onClick.AddListener(() => {
        //    if (WeChatContral.Instance.mWexinIsLogin.Value)
        //    {
        //        ShowPublicTip.Instance.Show("已登录微信");
        //    }
        //    else
        //    {
        //        //微信登陆
        //        WeChatContral.Instance.MLogin(()=> {  RefrishUi();  Debug.Log("登陆成功刷新UI"); });
                
        //    }

           
        //});
        //weiChatExitBtn.onClick.AddListener(() => {

        //    UIManager.Instance.ShowPopUp<ExitPanel>();
        //});
        //userXY.onClick.AddListener(() => {
        //    Debug.Log("显示用户协议");
        //    AdControl.Instance.ShowUser();
        //});
        //userZC.onClick.AddListener(() => {
        //    AdControl.Instance.ShowProtocol();
        //    Debug.Log("显示隐私政策");
        //});
        closeBtn.onClick.AddListener(Hide);
        //customerBtn.onClick.AddListener(() => {

        //    UIManager.Instance.Show<ServicePop>(UIType.PopUp);
        //});
    }
    public void DoToMove(Transform t, bool isOn) {

        if (isOn)
        {
            t.DOLocalMoveX(200, 0.5F).SetEase(Ease.InOutBack);
        }
        else {

            t.DOLocalMoveX(105, 0.5F).SetEase(Ease.InOutBack);

        }

    }

    private void MusicClick(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("打开音乐");
            AudioMgr.Instance.SetTog(AudioMgr.ToggleType.MusicTog,true);
            AudioMgr.Instance.PlayMusic("BGM");
        }
        else
        {
            Debug.Log("关闭音乐");
            AudioMgr.Instance.SetTog(AudioMgr.ToggleType.MusicTog, false);
            AudioController.StopMusic();
        }

    }

    private void SoundClick(bool boo)
    {
        AudioMgr.Instance.SetTog(AudioMgr.ToggleType.SoundTog, boo);
        Debug.Log("是否开启音效");
    }
    private void vibrateClick(bool boo)
    {
        AudioMgr.Instance.SetTog(AudioMgr.ToggleType.vibrateTog, boo);
        Debug.Log("是否开启震动");
    } 

    public void RefrishUi()
    {
        var mdata = AudioMgr.Instance.mdate;
        editionText.text = string.Format("当前版本:{0}", mdata.editionId);
        userId.text = string.Format("当前用户ID:{0}", mdata.userId);
        weiChatText.text = WeChatContral.Instance.mWexinIsLogin.Value ? "已登录" : "登陆微信";
        if (WeChatContral.Instance.mWexinIsLogin.Value) weiChatLoginBtn.transform.HideCanvasGroup();
        else weiChatLoginBtn.transform.ShowCanvasGroup();
        //刷新音乐开关
        musicTog.isOn = mdata.isMusic;
        soundTog.isOn = mdata.isSound;
        vibrateTog.isOn = mdata.isVibrate;
    }
}
//设置界面数据类型
public class FrontSetPopData
{
    
        public bool isMusic;//开启音乐
        public bool isSound;//开启音效
        public bool isVibrate;//开启震动
        public string editionId;//版本号
        public string userId;//用户ID
        public Sprite icon;//用户头像
        public string weiChatName;//微信名称

        public FrontSetPopData() 
        {
            isMusic = false;
            isSound = true;
            isVibrate = true;
            editionId = "0.1";
            userId = "123456";
            icon =null;
            weiChatName = "XXX";
        }
   
}
//设置界面管理
public class AudioMgr
{
    public static AudioMgr mInstance;
    public static AudioMgr Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new AudioMgr();
            }
            return mInstance;
        }
    }

    public enum ToggleType
    {
        MusicTog,//音乐
        SoundTog,//音效 
        vibrateTog,//震动
    }

    public const string Localkey = "SetDate_Localkey";
    public FrontSetPopData mdate;

    public AudioMgr()
    {
        Init();
    }
    public  bool IsInited { get; private set; } = false;

    public  void Init()
    {
        if (IsInited)
        {
            return;
        }
        IsInited = true;
        LoadData();
    }
    private  void LoadData()
    {
        mdate = SaveGame.Load<FrontSetPopData>(Localkey);
        if (mdate == null)
        {
            mdate = new FrontSetPopData();
        }
        SaveData();
    }

    public  void SaveData()
    {
        SaveGame.Save(Localkey, mdate);
    }
  
    /// <summary>
    /// 设置开关
    /// </summary>
    public  void SetTog(ToggleType type, bool boo)
    {
        switch (type)
        {
            case ToggleType.MusicTog:
                mdate.isMusic = boo;
                break;
            case ToggleType.SoundTog:
                mdate.isSound = boo;
                break;
            case ToggleType.vibrateTog:
                mdate.isVibrate = boo;
                break;
        }
        SaveData();
    }


    //播放BGM
    public void PlayMusic(string musicID)
    {
        if (mdate.isMusic)
        {
            AudioController.PlayMusic(musicID);
        }
    }
    //播放音效
    public void PlaySFX(string SFXID)
    {
        //Debug.Log("音效" + SFXID);
        if (mdate.isSound)
        {
            //Debug.Log("音效14" + SFXID);

            AudioController.Play(SFXID);
            
        }
    }

    //停止播放音效
    public void StopPlaySFX(string SFXID)
    {
       
        AudioController.Stop(SFXID);
    }
    //开始震动
    public void vibrate()
    {
        if (mdate.isVibrate)
        {
            AdControl.Instance.CallShake();
        }
    }
    //设置用户ID
    public void SetUserId(string id)
    {  if (id != null)
        {
            mdate.userId = id;
            SaveData();
            Debug.Log("用户ID" + id);
        }
        
    }
    //设置版本号
    public void SetEdition(string id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            mdate.editionId = id;
            SaveData();
            Debug.Log("版本号" + id);
        }
      
    }

    //设置微信名称
    public void SetWeiChatName(string str)
    {
        if (!string.IsNullOrEmpty(str) && mdate.weiChatName!=str)
        {
            mdate.weiChatName = str;
            SaveData();
            Debug.Log("微信名称"+str);
        }
    }
    //微信头像
    public void SetWeiChatIcon(Sprite sp)
    {
        if (sp != null)
        {
            mdate.icon = sp;
            SaveData();
            Debug.Log("设置微信头像");
        }
    }
    //给某个界面添加按钮点击音效
    public void ButtonAddSound(Transform thisT)
    {
        UnityEngine.UI.Button[] pageButton = thisT.GetComponentsInChildren<UnityEngine.UI.Button>();
        for (int i = 0; i < pageButton.Length; i++)
        {
            pageButton[i].onClick.AddListener(() => {

                PlaySFX("onClick");
            });
        }
    }
    
    //初始微信
    public void InitWeiChat()
    {
        if (!AdControl.Instance.isShowAd) return;
        WeChatContral.Instance.Init();
        WeChatContral.Instance.UserUrl.Subscribe(URL =>
        {
            if (URL == null) return;
            ObservableWWW.GetWWW(URL)
            .Subscribe(value =>
            {
                Debug.Log("加载头像成功");
                var tex2d = value.texture;
                Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0, 0));
                SetWeiChatIcon(m_sprite);
              //  InfiniteScrollView.Instance.avatar.transform.Find("tx").GetComponent<Image>().sprite = m_sprite;
            });

        });
        
        WeChatContral.Instance.UserName.Subscribe(value =>
        {
           
            if (!string.IsNullOrEmpty(value))
            {
                SetWeiChatName(value);
            }
           
        });

    }

   
   
}