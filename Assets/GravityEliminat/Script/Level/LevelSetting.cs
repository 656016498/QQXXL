using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DowPos {

  public  List<Transform> nowPos = new List<Transform>();

}

[Serializable]
public class DownRang {

    public int minRang;
    public int maxRang;
    /// <summary>
    /// 批数
    /// </summary>
    public int batch;
    /// <summary>
    /// 掉落间隔
    /// </summary>
    public float downInterval;

}

[Serializable]
public class BallWeights
{

    public BallType ballType;
    public SortType colorType;
    public bool isFixNum;
    public int fixNum;
    public int weights;
    public bool isSpecialDown;
    public List<Transform> specialDown=new List<Transform>();
    public int viewNum;
    public int Gern=1;

}

public enum WinType
{
    NULL,
    数量通关,
    通关线,
}

public enum SortType
{
    Default,
    Red,
    Yellow,
    Blue,
    Green,
    Coat,
    Orange,
    Cyan,
    Pink,
    Purple, 
    Coat2
}

public enum BallType
{
    NULL,
    ColorBall,
    SizeBall,
    Arrive,
    Soda,
    Chocolate,
    CandyCoat,
    CornKernel,
    PopCorn,
    SugarCube,
    Dye,
    BlackCloud,
    Hive, 
    GreenElimiate,
    FreezeBall
}
[Serializable]
/// <summary>
/// 通关条件
/// </summary>
public class ClearanceCondition { 
    public BallType ballType;
    public SortType colorType;
    public int num;
    //public bool IsSpecial;
    //public Transform specialTran;
    //public int viewNum;
}

[Serializable]
public class LevelSetting : MonoBehaviour
{
    public bool needMoveCamera = false;
    public bool startDownBall = true;//开局是否要掉落球
    public bool propModel=false;//道具模式
    public float riseNum;
    public bool haveWater;
    public Transform water;
    public List<DownRang> downRangs = new List<DownRang>();
    /// <summary>
    /// 掉落总球数
    /// </summary>
    public int fallAllBall;
    /// <summary>
    /// 旋转球通关线
    /// </summary>
    public Transform passLine;
    /// <summary>
    /// 特殊球掉落点
    /// </summary>
    public Transform specialPoint;


    /// <summary>
    /// 球掉落点位置
    /// </summary>
    public int dowPosCount;
    public List<DowPos> downPos = new List<DowPos>();
    /// <summary>
    /// 剩余步数
    /// </summary>
    public int RemainingSteps;
    /// <summary>
    /// 通关模式
    /// </summary>
    public WinType winType;
    /// <summary>
    /// 
    /// </summary>
    public int winNum;
    /// <summary>
    /// 通关条件
    /// </summary>
    public List<ClearanceCondition> cc = new List<ClearanceCondition>();
    
    /// <summary>
    /// 权重
    /// </summary>
    public List<BallWeights> ballWeights = new List<BallWeights>();



    public Transform AllBallParent;
    public Transform CameraParent;
    // Start is called before the first frame update
    void Awake()
    {
        AllBallParent = transform.Find("AllBall");
        CameraParent = transform.Find("CameraPoint");
    }


}
