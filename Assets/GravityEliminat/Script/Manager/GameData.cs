using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public bool FristJoinGame;
    public bool useWho;
    public int UnlockLevel;//解锁的关卡
    public int Love;//爱心
    public int StarshineStar;//星星
    public int ChallengeStar;//挑战星星
    public int Diamond;//钻石
    public int AllDay;//累计总天数
    public int AllStep;//累积总步数
    public int AllStar;//累积总星星
    public int[] BombProp;//道具
    public int HBCoin;
 
    public int addStepN;
    public int addBombN;
    public int addRefreshN;
    public int bestScore;//最高分数
    public List<int> levelStar;//关卡星星
    public System.DateTime ExitTime;
    public System.DateTime UseLoveTime;//使用爱心时间
    public List<int> GemLevel;//宝石关卡
    public List<int> NeedGudieMap;
    public List<int> TicketLevel;//出现抽奖券关卡
                                 //public List<int> MoneyLevel;//出现现金关卡
    public List<int> SDKSend;
    public int cashNum;
    public int VideoTimes;
    public List<int> BXLevek;


    public GameData() {
        BXLevek = new List<int> { 3,3,4 };
        FristJoinGame = true;
        useWho = false;
        UnlockLevel = 1;
        Love = 10;
        StarshineStar = 0;
        ChallengeStar = 0;
        Diamond = 0;
        AllDay = 0;
        AllStep = 0;
        AllStar = 0;
        HBCoin = 0;
        addStepN = 1;
        addBombN = 1;
        addRefreshN = 1;
        bestScore = 0;
        levelStar = new List<int>(1) { 0};
        ExitTime = System.DateTime.Now;
        UseLoveTime = System.DateTime.Now;
        BombProp = new int[3] {1,1,1};
        GemLevel = new List<int>();
        NeedGudieMap = new List<int>();
        TicketLevel = new List<int>();
        VideoTimes = 0;
        cashNum = 0;
        SDKSend = new List<int>();
        //MoneyLevel = new List<int>();
    }
}
