using ClockStone;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Util;

public class UIRoot : SingletonMonoBehaviour<UIRoot>
{

    // 所有UI列表
    [Header("所有UI列表")]
    public List<UIBase> m_AllUIs;
    [Header("UIRoot")]
    public Transform root;
    [Header("正常UIRoot")]
    public Transform normalRoot;
    [Header("固定UIRoot")]
    public Transform fixedRoot;
    [Header("弹出UIRoot")]
    public Transform popupRoot;
    [Header("UI相机")]
    public Camera uiCamera;

    public Transform mask;
    protected void Awake()
    {
        
    }
    private void Start()
    {
        InitUiManager();
        uiCamera.orthographicSize = Camera.main.orthographicSize;
        //TODO 展示默认界面


#if BB108
        UIManager.Instance.Show<MainPanel>(UIType.Normal);
#else
        GameManager.Instance.LoadLevel();
        //UIManager.Instance.Show<MainPanel>(UIType.Normal);

#endif
    }
    private void OnDestroy()
    {
        UIManager.Instance.Clear();
    }
     void InitUiManager()
    {
        foreach (UIBase ui in m_AllUIs)
        {
            UIManager.Instance.allUIs.Add(ui.ToString().Split(new char[] { '(', ')' })[1], ui);
            ui.Hide();
        }
        UIManager.Instance.root = this;
        UIManager.Instance.fixedRoot = fixedRoot;
        UIManager.Instance.normalRoot = normalRoot;
        UIManager.Instance.popupRoot = popupRoot;
        UIManager.Instance.uiCamera = uiCamera;
        //UIRoot.Instance.
       //UIRoot.Instance.wo
    }
    //世界坐标转UGUI坐标


    public void ShowMask() {

        mask.gameObject.SetActive(true);
    }

    public void HideMask() {

        mask.gameObject.SetActive(false);
    }
}
