using GameTime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    // Start is called before the first frame update

    /// <summary>
    /// 游戏入口
    /// </summary>
    private void Awake()
    {
        //初始时间(默认系统时间)
        GameClock.Init(GameUseTime.ServerTime);
        TableMgr.Instance.Init();
        DataManager.Instance.Init();
        TimeMgr.Instance.Init();

        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = false;
        GameManager.Instance.Init();
        float scaling = (750f / 1334f * Screen.height / Screen.width);
        //Debug.Log(scaling+"???");
        Camera.main.orthographicSize = 6.67f * scaling;
        PropManger.Instance.Init();
     
    }

    private void Start()
    {
        //微信初始
        AudioMgr.Instance.InitWeiChat();
        //展示提现成功
        WithdrawSucManger.Instance.ShowSucPanel();
        //提现界面倒计时
#if BB108
        SignDataControl.Instance.Init();
#endif
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.ShowPopUp<ExitPanel>();
        }
    }
}

public class WillDown
{

    public int num;
    public BallType type;
    public SortType color;
    public bool isFix;
    public string BallTypeString;
    public int Gern;

    public WillDown(BallType ballType, SortType colorS, bool isFxsS, int y)
    {

        type = ballType;
        color = colorS;
        isFix = isFxsS;
        num = y;
        BallTypeString = ballType.ToString() + color.ToString();
    }
}
