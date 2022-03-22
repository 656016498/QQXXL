using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HierarchyMenu
{


	[MenuItem("GameObject/LevelSetting/创建/球落点", false, 1)]
	public static void CreateDownPoint()
	{
		GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/DownLine"));
		obj.transform.SetParent((Selection.objects[0] as GameObject).transform);
		obj.transform.position = Vector3.zero;
	}

	//[MenuItem("GameObject/LevelSetting/创建/大小球", false, 2)]
	//public static void CreateBigBall() {

	//	GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Ball/SizeBall"));
	//	obj.transform.SetParent((Selection.objects[0] as GameObject).transform);
	//	obj.transform.position = Vector3.zero;

	//}
	[MenuItem("GameObject/LevelSetting/摄像机/设置摄像机位置", false, 3)]
	public static void MoveCamera()
	{
		CameraManager.Instance.SetCameraPos((Selection.objects[0] as GameObject).transform.position);
	}

	[MenuItem("GameObject/LevelSetting/摄像机/重置位置", false, 3)]
	public static void RecoverCamera()
	{
		CameraManager.Instance.RecoverPos();
	}

	[MenuItem("GameObject/LevelSetting/清点当前球数", false, 9)]
	public static void NowBallNum()
	{

		GameObject parent = Selection.objects[0] as GameObject;
		Debug.Log("所有球:" + parent.transform.childCount);
		Dictionary<SortType, int> TEM = new Dictionary<SortType, int>();
		for (int i = 0; i < parent.transform.childCount; i++)
		{
			Ball ball = parent.transform.GetChild(i).GetComponent<Ball>();
			if (TEM.ContainsKey(ball.sort))

			{
				TEM[ball.sort]++;

			}
			else
			{

				TEM.Add(ball.sort,1);
			}
		}
		foreach (var item in TEM.Keys)
		{
			Debug.Log("球类型:" + item.ToString() + "    个数:" + TEM[item]);
		}
	}

	[MenuItem("GameObject/LevelSetting/创建/缩小球", false, 9)]
	public static void CreateSizeBall()
	{

		GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Ball/SizeBall"));
		obj.transform.SetParent((Selection.objects[0] as GameObject).transform);
		obj.transform.position = Vector3.zero;
	}
	[MenuItem("GameObject/LevelSetting/创建/关卡", false, 9)]
	public static void CreateLevelBase()
	{

		GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("AllLevel/LevelBase"));
		obj.transform.SetParent((Selection.objects[0] as GameObject).transform);
		obj.transform.position = Vector3.zero;

	}

	[MenuItem("GameObject/LevelSetting/创建/摄像机点", false, 9)]

	public static void CreateCameraPoint()
	{

		GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/cameraPoint"));
		obj.transform.SetParent((Selection.objects[0] as GameObject).transform);
		obj.transform.position = Vector3.zero;
	}

	[MenuItem("GameObject/UI/创建IButton", false, 9)]

	public static void CreateUIButton()
	{
		GameObject parent =(GameObject)Selection.objects[0];
		GameObject img = new GameObject("IButton");
		UnityEditor.Undo.RegisterCreatedObjectUndo(img,"");
		img.AddComponent<UnityEngine.UI.Image>();

		//SpriteRenderer spr = img.AddComponent(typeof(IMAHE)) as SpriteRenderer;
		//spr.transform.position = new Vector2(x, y);
		img.transform.SetParent(parent.transform);
		img.transform.localPosition = Vector3.zero;
		img.AddComponent<IButton>();
        //GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/cameraPoint"));
        //obj.transform.SetParent((Selection.objects[0] as GameObject).transform);
        //obj.transform.position = Vector3.zero;
    }

	[MenuItem("GameObject/LevelSetting/刷新蜂槽", false, 9)]

	public static void RestHive()
	{
		int x = 0;
		foreach (GameObject item in Selection.objects)
		{
			item.transform.GetComponent<SpriteRenderer>().sortingOrder = (int)((3.575f - item.transform.localPosition.y) / 0.4875F) + 3;
			for (int i = 0; i < item.transform.childCount; i++)
			{
				item.transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = item.transform.GetComponent<SpriteRenderer>().sortingOrder + i + 1;

			}
			switch (item.transform.GetComponent<Hive>().Gear)
			{

				case 1:
					item.transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BallSprite/蜂巢/Token_mtpn_01");
					item.transform.GetChild(1).gameObject.SetActive(false);
					item.transform.GetChild(2).gameObject.SetActive(false);
					//item.transform.GetChild().gameObject.SetActive(false);

					break;
				case 2:
					item.transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BallSprite/蜂巢/Token_mtpn_02");
					item.transform.GetChild(2).gameObject.SetActive(false);
					item.transform.GetChild(1).gameObject.SetActive(true);
					break;

				case 3:
					item.transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BallSprite/蜂巢/Token_mtpn_03");
					item.transform.GetChild(2).gameObject.SetActive(true);
					item.transform.GetChild(1).gameObject.SetActive(false);
					break;

				default: break;
			}

            if (item.transform.GetComponent<Hive>().willType == BallType.ColorBall)
            {
                item.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BallSprite/蜂巢/Hive_" + item.transform.GetComponent<Hive>().willSotr.ToString());
            }
            else if (item.transform.GetComponent<Hive>().willType == BallType.CornKernel)
            {

				Debug.Log(item.transform.GetComponent<Hive>().willType.ToString());
				item.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BallSprite/蜂巢/Hive_CornKernel");
            }
        }
	}

	[MenuItem("GameObject/LevelSetting/刷新彩球", false, 9)]
	public static void RestColorBall()
	{

		foreach (GameObject item in Selection.objects)
		{
			Ball itemBall = item.GetComponent<ColorBall>();

			if (itemBall!=null)
            {
				itemBall.Init(itemBall.sort, itemBall.isFix);
			}






		}
	}

	[MenuItem("GameObject/LevelSetting/刷新冰球", false, 9)]
	public static void RestIceBall()
	{
		foreach (GameObject item in Selection.objects)
		{
			SugarCube itemBall = item.GetComponent<SugarCube>();

			if (itemBall != null)
			{
				itemBall.Init(itemBall.sort, itemBall.isFix,itemBall.Gear);
			}
		}
	}

}
