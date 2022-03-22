using BayatGames.SaveGamePro;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singlton<DataManager>
{
    public int LevelFoldNum;//总关卡数
    private const string GameDataPath = "PlayerData_NoMain";
    public GameData data;
    //public string Version;
    public void Init() {
#if Easy
        LevelFoldNum = /*GetFoleNumber();*/250;

#else
 LevelFoldNum = /*GetFoleNumber();*/102;
#endif
        InitData();
    }

    public int GetTargetBox() {


        if (data.BXLevek.Count > 0)
        {
            return data.BXLevek[0];
        }
        else {

            return 5;
        }

    }

    public int GetFoleNumber() {

        string path = Application.dataPath + "/Resources/Prefabs/UI/";
        DirectoryInfo direction = new DirectoryInfo(path);
        FileInfo[] files = direction.GetFiles("Level", SearchOption.AllDirectories);
        Debug.Log("关卡数量"+ files.Length/2);
        return files.Length/2;
    }

    public void InitData() {
        data = SaveGame.Load<GameData>(GameDataPath);
        if (data==null)
        {
            data = new GameData();
            //for (int i = 0; i < LevelFoldNum; i++)
            //{
            //    data.levelStar.Add(0);
            //}
            data.TicketLevel = TableMgr.Instance.InitTicketLevel();
            //data.MoneyLevel = TableMgr.Instance.InitMoneyLevel();
            //data.GemLevel = TableMgr.Instance.InitGemData();
            data.NeedGudieMap = TableMgr.Instance.GuideData();
            //foreach (var item in data.GemLevel)
           
 //{
            //    Debug.Log("彩石管卡" + item);
            //}
            SaveGame.Save(GameDataPath, data);
        }
    }
    public void SaveGameData()
    {
        Debug.Log("//////数据保存///////");
        SaveGame.Save(GameDataPath, data);
    }
}
