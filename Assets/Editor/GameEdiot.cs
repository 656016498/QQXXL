using BayatGames.SaveGamePro;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameEdiot : EditorWindow
{
	[MenuItem("Tools/清除数据")]
	public static void ClanData()
	{
		SaveGame.Clear();
				
	}

	[MenuItem("Tools/GameSetting")]
	public static void AddWindow()
	{
		Rect wr = new Rect(0, 0, 500, 500);
		GameEdiot windows = (GameEdiot)EditorWindow.GetWindowWithRect(typeof(GameEdiot), wr, true, "GameSetting");
		windows.Show();
	}
	bool restBool;
	int chainNum = 0;
	bool button1;
	private void OnGUI()
	{

		BallType tset1Ball = BallType.NULL;
		SortType test1Sort = SortType.Default;
		int i = 0;
		EditorGUILayout.BeginHorizontal("Box");
		DataManager.Instance.Init();
		//DataManager.Instance.data.CurrentLevel= EditorGUILayout.IntField("设置关卡", DataManager.Instance.data.CurrentLevel);
		//button1 = GUILayout.Button("确定");
		DataManager.Instance.data.useWho= EditorGUILayout.Toggle("True：AllLevel，False：Level", DataManager.Instance.data.useWho);


        //if (button1)
        //      {
        //	DataManager.Instance.SaveGameData();

        //}
        DataManager.Instance.SaveGameData();
        EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal("Box");
		chainNum = EditorGUILayout.IntField("创建链小节", chainNum);
		bool sureBtn = GUILayout.Button("sure");
		if (sureBtn)
		{
			Debug.Log(chainNum);
			CloneChain(chainNum);
		}
		EditorGUILayout.EndHorizontal();


		//EditorGUILayout.BeginHorizontal("Box");
		//EditorGUILayout.Foldout(restBool, "设置蜂蜜");
		//tset1Ball=(BallType) EditorGUILayout.EnumPopup()


		//foreach (GameObject item in Selection.objects)
		//{
		//	if (item.transform.GetComponent<Hive>() != null)
		//	{
				

		//	}
		//	else {

		//		Debug.LogError("选中的道具不是蜂巢");
		//	}
  //      }
		//EditorGUILayout.EndHorizontal();

	}

	public void CloneChain(int num) {
		
	
		GameObject chain = Instantiate(new GameObject());
		chain.transform.position = Vector3.zero;
		chain.transform.localScale = Vector3.one * 0.4F;
		chain.name = "chains";

		for (int i = 0; i < num; i++)
        {
			GameObject chainJoint = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/chain"+ i%2), chain.transform);
			chainJoint.transform.localPosition = new Vector3(0.8F * i,0);
		}

		for (int i = 0; i < chain.transform.childCount; i++)
        {
            if (i< chain.transform.childCount-1)
            {
				chain.transform.GetChild(i).GetComponent<HingeJoint2D>().connectedBody = chain.transform.GetChild(i + 1).GetComponent<Rigidbody2D>();
			}
		}
		
		chain.transform.GetChild(0).GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
		chain.transform.GetChild(num-1).GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
		chain.transform.SetParent(GameObject.Find("Terrain").transform);

	}
}
