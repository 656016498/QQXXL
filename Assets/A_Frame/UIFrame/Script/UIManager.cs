using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UIType
{
    Normal,   // 普通ui，只会显示一个
    Fixed,    // 固定ui 一直显示
    PopUp,   // 弹窗ui
    None,      //独立的窗口
}


public class UIManager
{
    private const string path = "Prefabs/UI/";
    private static UIManager m_Instance = null;
    public static UIManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new UIManager();
            }
            return m_Instance;
        }
    }

    // 所有UI列表
    private Dictionary<string, UIBase> m_AllUIs = new Dictionary<string, UIBase>();

    //管理生命周期的列表
    private List<UIBase> listQueue = new List<UIBase>(11);
    public Dictionary<string, UIBase> allUIs
    { get { return m_AllUIs; } set { m_AllUIs = value; } }


    // 当前展示的Normal
    private UIBase currentNormalUI = null;

    // ui Root
    public UIRoot root;
    public Transform fixedRoot;
    public Transform normalRoot;
    public Transform popupRoot;
    public Camera uiCamera;

    // 展示UI
    [System.Obsolete("请使用Show<T>(string uiPath, UIType type)", true)]
    public void Show<T>()
    {
        Type t = typeof(T);
        string uiName = t.ToString();
        UIBase uiBase = null;
        if (allUIs.ContainsKey(uiName))
        {
            uiBase = allUIs[uiName];
            if (uiBase.type == UIType.Normal)
            {
                if (currentNormalUI != null)
                {
                    if (uiBase != currentNormalUI)
                    {
                        if (currentNormalUI.gameObject.activeSelf == true)
                        {
                            currentNormalUI.Hide();
                        }
                    }
                }
                currentNormalUI = uiBase;

            }
            uiBase.Show();
            uiBase.Refresh();
        }
        else
        {

            Debug.LogError(uiName + "  此UI不存在，请仔细查看。");
        }
    }

	//获取UI
    public T GetBase<T>() where T : UIBase
    {
        if (allUIs.ContainsKey(typeof(T).ToString()))
            return allUIs[typeof(T).ToString()] as T;
        else
            return null;
    }
    
    //尝试获取UI
    public bool TryGetBase<T>(out T ui) where T : UIBase
    {
        ui = null;
        if (allUIs.ContainsKey(typeof(T).ToString()))
        {
            ui = allUIs[typeof(T).ToString()] as T;
            return true;
        }
        return false;
    }
    //展示弹窗
    public T ShowPopUp<T>(string tempPath="") where T : UIBase
    {
        return ShowAndBackBase<T>(path+ tempPath + typeof(T), UIType.PopUp);
    }
    public void ShowPopUp<T>(Action<T> mScripts, string tempPath = "") where T : UIBase
    {
        var mPanel = ShowAndBackBase<T>(path + tempPath + typeof(T), UIType.PopUp);
        mScripts(mPanel);
    }
    //关闭所有弹窗
    public void HideAllPopUp()
    {
        foreach (UIBase uIBase in allUIs.Values)
        {
            if( uIBase.type==UIType.PopUp )
            {
                //if(uIBase.gameObject.activeSelf==true)
                //    uIBase.Hide();
                uIBase.Hide();
            }
        }
    }
    /// <summary>
    /// 有返回值的Show方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="uiPath">ui路径</param>
    /// <param name="type">ui类型</param>
    public void Show<T>(UIType type) where T : UIBase
    {
       ShowAndBackBase<T>(path+typeof(T), type);
    }

    public void Show<T>(UIType type,object OBJ) where T : UIBase
    {
        ShowAndBackBase<T>(path + typeof(T), type,OBJ);
    }

    public void Hide<T>() where T:UIBase
    {
        if (TryGetBase<T>(out T mUi))
        {
            mUi.Hide();
            Push(mUi);
        }
    }
    /// <summary>
    /// 入队:用于面板生命周期使用,如果面板容量大于一定的数量,会自动将最早使用过的面板进行销毁处理
    /// </summary>
    /// <param name="ui"></param>
    private void Push(UIBase ui)
    {
        if (listQueue.Contains(ui))
        {
            listQueue.Remove(ui);
        }
        listQueue.Insert(0, ui);

        if (listQueue.Count>10)
        {
            Destroy(listQueue[10].name);
            listQueue.RemoveAt(10);
            
        }
    
    }

    /// <summary>
    /// 有返回值的Show方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="uiPath"></param>
    /// <param name="type"></param>
    public T ShowAndBackBase<T>(string uiPath, UIType type) where T : UIBase
    {
       
        Type t = typeof(T);
        string uiName = t.ToString();
        UIBase uiBase = null;
        if (allUIs.ContainsKey(uiName))
        {
           
            uiBase = allUIs[uiName];
        }
        else
        {
            GameObject go = GameObject.Instantiate(Resources.Load(uiPath)) as GameObject;
            uiBase = AnchorUIGameObject(go, type);
            allUIs.Add(uiName, uiBase);
        }
        uiBase.Show();
        uiBase.Refresh();

        if (listQueue.Contains(uiBase))
        {
            listQueue.Remove(uiBase);
        }
        uiBase.transform.SetAsLastSibling();
        return uiBase as T;
    }

    public T ShowAndBackBase<T>(string uiPath, UIType type,object obj) where T : UIBase
    {

        Type t = typeof(T);
        string uiName = t.ToString();
        UIBase uiBase = null;
        if (allUIs.ContainsKey(uiName))
        {

            uiBase = allUIs[uiName];
        }
        else
        {
            GameObject go = GameObject.Instantiate(Resources.Load(uiPath)) as GameObject;
            uiBase = AnchorUIGameObject(go, type);
            allUIs.Add(uiName, uiBase);
        }
        uiBase.Show(obj);
        uiBase.Refresh();

        if (listQueue.Contains(uiBase))
        {
            listQueue.Remove(uiBase);
        }
        return uiBase as T;
    }

    protected UIBase AnchorUIGameObject(GameObject ui, UIType type)
    {

        Vector3 anchorPos = Vector3.zero;
        Vector2 sizeDel = Vector2.zero;
        Vector3 scale = Vector3.one;
        if (ui.GetComponent<RectTransform>() != null)
        {
            anchorPos = ui.GetComponent<RectTransform>().anchoredPosition;
            sizeDel = ui.GetComponent<RectTransform>().sizeDelta;
            scale = ui.GetComponent<RectTransform>().localScale;
        }
        else
        {
            anchorPos = ui.transform.localPosition;
            scale = ui.transform.localScale;
        }

        //Debug.Log("anchorPos:" + anchorPos + "|sizeDel:" + sizeDel);

        if (type == UIType.Fixed)
        {
            ui.transform.SetParent(fixedRoot);
        }
        else if (type == UIType.Normal)
        {
            ui.transform.SetParent(normalRoot);
        }
        else if (type == UIType.PopUp)
        {
            ui.transform.SetParent(popupRoot);
        }
        else if (type == UIType.None)
        {
            ui.transform.SetParent(root.transform);
        }


        if (ui.GetComponent<RectTransform>() != null)
        {
            ui.GetComponent<RectTransform>().anchoredPosition = anchorPos;
            ui.GetComponent<RectTransform>().sizeDelta = sizeDel;
            ui.GetComponent<RectTransform>().localScale = scale;
        }
        else
        {
            ui.transform.localPosition = anchorPos;
            ui.transform.localScale = scale;
        }
        //Debug.Log(ui.transform.localPosition);
        ui.transform.localPosition = Vector3.zero;
        return ui.GetComponent<UIBase>();
    }

    //刷新ui数据
    public void Refresh<T>()
    {

        Type t = typeof(T);
        string uiName = t.ToString();

        if (allUIs.ContainsKey(uiName))
        {
            allUIs[uiName].Refresh();
        }
    }


    /// <summary>
    /// 隱藏指定面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void HidePanel<T>() where T : UIBase
    {
        if (TryGetBase<T>(out T mt))
        {
            mt.Hide();
        }
    }

    public void Destroy<T>()
    {
        Type t = typeof(T);
        string uiName = t.ToString();
        UIBase ui = null;
        if (allUIs.ContainsKey(uiName))
        {
            ui = allUIs[uiName];
            allUIs.Remove(uiName);
            GameObject.Destroy(ui.gameObject);

        }
    }
    public void Destroy(string name)
    {
        UIBase ui = null;
        if (allUIs.ContainsKey(name))
        {
            ui = allUIs[name];
            allUIs.Remove(name);
            GameObject.Destroy(ui.gameObject);

        }
    }


    //清除所有UI面板
    public void Clear()
    {
        allUIs.Clear();
    }


}
