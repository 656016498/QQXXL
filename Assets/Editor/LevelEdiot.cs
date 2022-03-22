using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(LevelSetting))]
public class LevelEdiot : Editor
{
    LevelSetting setting;
    Vector2 scrollPos=Vector2.zero;
    public LevelEdiot()
    {
        //this.titleContent = new GUIContent("关卡编辑器");
    }
    int typeNum;
    int ballTypeNum;
    bool open6;
    bool open1;
    bool open2;
    bool open0;
    bool open4;
    int temp1;
    int canShowType = 0;
    int pointDop = 0;
    bool open3;
    int lc = 0;
    int carmerPNum=0;
    bool open5;
    Transform transform1 = null;
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
        setting = target as LevelSetting;
        setting.needMoveCamera = EditorGUILayout.Toggle("是否需要移动摄像头", setting.needMoveCamera);
        setting.startDownBall =EditorGUILayout.Toggle("开局是否掉落球", setting.startDownBall);
        setting.RemainingSteps = EditorGUILayout.IntField("剩余步数", setting.RemainingSteps);
        setting.fallAllBall = EditorGUILayout.IntField("关卡总球数", setting.fallAllBall);
        #region 出现球概率
        //open2 = EditorGUILayout.Foldout(open2, "权重");\
        open2 = true;
        if (open2)
        {
            #region 掉落位置
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            open0 = EditorGUILayout.Foldout(open0, "掉落位置");
            EditorGUILayout.BeginVertical();
            if (open0)
            {
                pointDop = setting.downPos.Count;
                //Debug.Log("1" + pointDop);
                ////Debug.Log("???"+setting.downPos.Count);
                pointDop = EditorGUILayout.IntField("球掉落位置组数", pointDop);
                //Debug.Log("2" + pointDop);

                int count0 = setting.dowPosCount - pointDop;
                if (count0 < 0)
                {
                    for (int i = 0; i < Mathf.Abs(count0); i++)
                    {
                        setting.downPos.Add(new DowPos());
                    }
                }
                int length0 = setting.downPos.Count;
                setting.downPos.RemoveRange(pointDop, length0 - pointDop);
                for (int i = 0; i < setting.downPos.Count; i++)
                {
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.Foldout(open5, string.Format("第{0}视角",i+1));
                    var addBtn = GUILayout.Button("add");
                    if (addBtn)
                    {
                        Transform transform = null;
                        transform = (Transform)EditorGUILayout.ObjectField("掉落位置", transform, typeof(Transform), true);
                        setting.downPos[i].nowPos.Add(transform);
                    }
                    for (int j = 0; j < setting.downPos[i].nowPos.Count; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        setting.downPos[i].nowPos[j]= (Transform)EditorGUILayout.ObjectField("掉落位置", setting.downPos[i].nowPos[j], typeof(Transform), true);
                        var deletBtn = GUILayout.Button("x");
                        if (deletBtn)
                        {
                            setting.downPos[i].nowPos.RemoveAt(j);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndVertical();
                    //for (int j = 0; j < setting.downPos[i].Count; j++)
                    //{
                    //    var deletBtn = GUILayout.Button("x");
                    //    if (deletBtn)
                    //    {
                    //        setting.downPos[i].RemoveAt(j);
                    //    }
                    //    EditorGUILayout.EndVertical();
                    //}
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            #endregion
            #region 批次
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            open5 = EditorGUILayout.Foldout(open5,"批次");
            EditorGUILayout.EndHorizontal();

            if (open5)
            {
                
                var addBtn = GUILayout.Button("add");
                //if (addBtn)
                //{

                //    EditorGUILayout.BeginHorizontal("Box");
                //    setting.downRangs[i].minRang = EditorGUILayout.IntField("最小范围", downRang.minRang);
                //    setting.downRangs[i].minRang = EditorGUILayout.IntField("最大范围", downRang.maxRang);
                //    downRang.batch = EditorGUILayout.IntField("批数", downRang.batch);
                //    downRang.downInterval = EditorGUILayout.FloatField("间隔", downRang.downInterval);
                //    bool closeBtn = GUILayout.Button("remove");
                //    if (closeBtn)
                //    {
                //        if (setting.downRangs.Contains(downRang))
                //        {
                //            setting.downRangs.Remove(downRang);
                //        }
                //    }
                //    EditorGUILayout.EndHorizontal();
                //}
                //if (setting.downRangs.Count>0)
                //{
                if (addBtn)
                {
                    DownRang downRang = new DownRang();
                    setting.downRangs.Add(downRang);
                }
                for (int i = 0; i < setting.downRangs.Count; i++)
                {
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.BeginHorizontal("Box");
                    setting.downRangs[i].minRang = EditorGUILayout.IntField("最小范围", setting.downRangs[i].minRang);
                    setting.downRangs[i].maxRang = EditorGUILayout.IntField("最大范围", setting.downRangs[i].maxRang);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal("Box");
                    setting.downRangs[i].batch = EditorGUILayout.IntField("每批个数", setting.downRangs[i].batch);
                    setting.downRangs[i].downInterval = EditorGUILayout.FloatField("间隔", setting.downRangs[i].downInterval);
                    EditorGUILayout.EndHorizontal();
                    bool closeBtn = GUILayout.Button("remove",GUILayout.Width(70));
                    if (closeBtn)
                    {
                        if (setting.downRangs.Contains(setting.downRangs[i]))
                        {
                            setting.downRangs.Remove(setting.downRangs[i]);
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            #endregion
            canShowType = setting.ballWeights.Count;
            canShowType = EditorGUILayout.IntField("出现可以出现的种类", canShowType);
            int count1 = setting.ballWeights.Count - canShowType;
            if (count1 < 0)
            {
                for (int x = 0; x < Mathf.Abs(count1); x++)
                {
                    setting.ballWeights.Add(new BallWeights());
                }
            }
            for (int i = 0; i < canShowType; i++)
            {
                EditorGUILayout.BeginVertical("box");
                setting.ballWeights[i].ballType = (BallType)EditorGUILayout.EnumPopup("球类型", setting.ballWeights[i].ballType);
                if (setting.ballWeights[i].ballType==BallType.SugarCube|| setting.ballWeights[i].ballType == BallType.FreezeBall)
                {
                    setting.ballWeights[i].Gern = (int)EditorGUILayout.IntField("等级1-4", setting.ballWeights[i].Gern);
                }
                setting.ballWeights[i].colorType = (SortType)EditorGUILayout.EnumPopup("分类", setting.ballWeights[i].colorType);
                setting.ballWeights[i].isFixNum = EditorGUILayout.Toggle("是否固定", setting.ballWeights[i].isFixNum);
                if (setting.ballWeights[i].isFixNum)
                {
                    setting.ballWeights[i].fixNum = EditorGUILayout.IntField("固定数量", setting.ballWeights[i].fixNum);
                    setting.ballWeights[i].weights = 0;
                }
                else
                {
                    setting.ballWeights[i].weights = EditorGUILayout.IntField("权重", setting.ballWeights[i].weights);
                    setting.ballWeights[i].fixNum = 0;
                    setting.ballWeights[i].isFixNum = false;
                }

                setting.ballWeights[i].isSpecialDown = EditorGUILayout.Toggle("是否特殊", setting.ballWeights[i].isSpecialDown);
                if (setting.ballWeights[i].isSpecialDown)
                {
                    setting.ballWeights[i].isFixNum = true;
                    setting.ballWeights[i].viewNum = EditorGUILayout.IntField("视野数量", setting.ballWeights[i].viewNum);
                    bool addBtn = GUILayout.Button("添加位置");
                    if (addBtn)
                    {
                        Transform transform = null;
                        transform = (Transform)EditorGUILayout.ObjectField("掉落位置", transform, typeof(Transform), true);
                        setting.ballWeights[i].specialDown.Add(transform);
                    }

                    //Debug.Log(setting.ballWeights[i].specialDown.Count);
                    for (int j = 0; j < setting.ballWeights[i].specialDown.Count; j++)
                    {
                        EditorGUILayout.BeginHorizontal("Box");
                        setting.ballWeights[i].specialDown[j] = (Transform)EditorGUILayout.ObjectField("特殊掉落位置", setting.ballWeights[i].specialDown[j], typeof(Transform), true);
                        bool deletn = GUILayout.Button("x");
                        if (deletn)
                        {
                            setting.ballWeights[i].specialDown.RemoveAt(j);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                else {
                    setting.ballWeights[i].specialDown.Clear();
                }
                int length1 = setting.ballWeights.Count;
                setting.ballWeights.RemoveRange(canShowType, length1 - canShowType);
                EditorGUILayout.EndVertical();
            }
        }
        #endregion
        #region 通关条件
        open1 = EditorGUILayout.Foldout(open1, "关卡设置");
        if (open1)
        {
            setting.winType = (WinType)EditorGUILayout.EnumPopup("通关类型:", setting.winType);
            switch (setting.winType)
            {
                case WinType.数量通关:
                    typeNum = setting.cc.Count;
                    typeNum = EditorGUILayout.IntField("通关球种类", typeNum);
                    int count2 = setting.cc.Count - typeNum;
                    if (count2 < 0)
                    {
                        for (int i = 0; i < Mathf.Abs(count2); i++)
                        {
                            setting.cc.Add(new ClearanceCondition());
                        }
                    }
                    else
                    {
                        for (int i = 0; i < typeNum; i++)
                        {
                            EditorGUILayout.BeginVertical("box");
                            setting.cc[i].ballType = (BallType)EditorGUILayout.EnumPopup("球类型:", setting.cc[i].ballType);
                            setting.cc[i].colorType = (SortType)EditorGUILayout.EnumPopup("分类:", setting.cc[i].colorType);
                            setting.cc[i].num = EditorGUILayout.IntField("数量", setting.cc[i].num);

                            EditorGUILayout.EndVertical();
                        }
                    }
                    int length2 = setting.cc.Count;
                    setting.cc.RemoveRange(typeNum, length2 - typeNum);
                    break;
                case WinType.通关线:
                    setting.winNum = EditorGUILayout.IntField("目标数量", setting.winNum);
                    setting.passLine = (Transform)EditorGUILayout.ObjectField("通关线", setting.passLine, typeof(Transform), true);
                    break;

                //case WinType.巧克力:
                //    setting.winNum = EditorGUILayout.IntField("目标数量", setting.winNum);
                //    setting.specialPoint = (Transform)EditorGUILayout.ObjectField("特殊点掉落位置", setting.specialPoint, typeof(Transform), true);
                //    break;

                default:
                    GUILayout.Label("未选择通关类型", EditorStyles.boldLabel);
                    break;
            }
            setting.haveWater  = EditorGUILayout.Toggle("是否是有水", setting.haveWater);
            if (setting.haveWater)
            {
                //setting.water = (Transform)EditorGUILayout.ObjectField("水", setting.water,typeof(Transform),true);
                setting.riseNum = EditorGUILayout.FloatField("水位上升高度",setting.riseNum);
            }
        }
        #endregion

        //open4 = EditorGUILayout.Foldout(open4, "摄像机位置");
        //if (open4)
        //{
        //    carmerPNum = setting.carmeraPoint.Count;
        //    carmerPNum = EditorGUILayout.IntField("位置数量", carmerPNum);
        //    int count2 = setting.carmeraPoint.Count - carmerPNum;
        //    if (count2 < 0)
        //    {
        //        for (int i = 0; i < Mathf.Abs(count2); i++)
        //        {
        //            Transform transform = null;
        //            transform = (Transform)EditorGUILayout.ObjectField("摄像机点", transform, typeof(Transform), true);
        //            setting.carmeraPoint.Add(transform);
        //        }
        //    }
        //    int count = setting.carmeraPoint.Count;
        //    setting.carmeraPoint.RemoveRange(carmerPNum, count - carmerPNum);
        //    for (int i = 0; i < setting.carmeraPoint.Count; i++)
        //    {
        //        setting.carmeraPoint[i] = (Transform)EditorGUILayout.ObjectField("摄像机点", setting.carmeraPoint[i], typeof(Transform), true);
        //    }
        //}
        #region 道具设置
        //open6 = EditorGUILayout.Foldout(open6, "道具设置");
        //if (open6)
        //{
            EditorGUILayout.BeginVertical("Box");
            transform1 = (Transform)EditorGUILayout.ObjectField("道具", transform1, typeof(Transform), true);
        if (transform1 != null)
        {
            Debug.Log(transform1.GetComponent<Furnace>());
            if (transform1.GetComponent<Furnace>() != null)
            {
                Furnace furnace = transform1.GetComponent<Furnace>();
                furnace.NowStep = (int)EditorGUILayout.IntField("当前属于第几视角", furnace.NowStep);
                furnace.clearType = (SortType)EditorGUILayout.EnumPopup("对应球类型", furnace.clearType);
                furnace.num = (int)EditorGUILayout.IntField("对应数量", furnace.num);
                var addBtn = GUILayout.Button("添加对应链子数");
                lc = furnace.chain.Count;
                if (addBtn)
                {
                    furnace.chain.Add(null);
                }
                for (int i = 0; i < lc; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    furnace.chain[i] = (Transform)EditorGUILayout.ObjectField("链子" + i, furnace.chain[i], typeof(Transform), true);
                    var removeBtn = GUILayout.Button("x");
                    if (removeBtn)
                    {
                        furnace.chain.RemoveAt(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            else if (transform1.GetComponent<SizeBall>() != null)
            {
                SizeBall sizeBall = transform1.GetComponent<SizeBall>();
                sizeBall.ReduceTimes = EditorGUILayout.IntField("变小次数", sizeBall.ReduceTimes);
                sizeBall.transform.localScale = Vector3.one * (sizeBall.ReduceTimes / 2 * 0.107F);
            }
        }
        setting.propModel = EditorGUILayout.Toggle("道具模式ture：特定要求", setting.propModel);
        EditorGUILayout.EndVertical();
        //}
        #endregion
        if (GUI.changed)
        {
            EditorUtility.SetDirty(setting);
        }
        EditorGUILayout.EndVertical();
    }

  
}
