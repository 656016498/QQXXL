using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIRoot))]
public class UIRootEditor : Editor
{
    private UIRoot uiRoot;

    private void OnEnable()
    {
        uiRoot = (UIRoot)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("获取所有对象"))
        {
            //uiRoot.m_AllUIs.Clear();
            //uiRoot.root = null;
            //uiRoot.normalRoot = null;
            //uiRoot.fixedRoot = null;
            //uiRoot.popupRoot = null;
            //uiRoot.uiCamera = null;


            //uiRoot.m_AllUIs = GetComponent<UIBase>().ToString().Split(new char[] { '(', ')' })[1]

            if (uiRoot.m_AllUIs == null) {

                //uiRoot.m_AllUIs = GameObject.FindObjectsOfType<UIBase>().ToList();
                uiRoot.m_AllUIs = uiRoot.gameObject.GetComponentsInChildren<UIBase>(true).ToList();
            }

            foreach (UIBase ui in uiRoot.gameObject.GetComponentsInChildren<UIBase>(true).ToList()) {

                if (!uiRoot.m_AllUIs.Contains(ui)) {

                    uiRoot.m_AllUIs.Add(ui);
                    switch (ui.transform.parent.name)
                    {
                        case "NormalRoot":
                            ui.type = UIType.Normal;
                            break;
                        case "FixedRoot":
                            ui.type = UIType.Fixed;
                            break;
                        case "PopupRoot":
                            ui.type = UIType.PopUp;
                            break;
                        default:
                            ui.type = UIType.None;
                            break;

                    }
                }
               
            }
            
            uiRoot.root = GameObject.Find("UIRoot").transform;
            uiRoot.normalRoot = GameObject.Find("NormalRoot").transform;
            uiRoot.fixedRoot = GameObject.Find("FixedRoot").transform;
            uiRoot.popupRoot = GameObject.Find("PopupRoot").transform;
            if(GameObject.Find("UICamera")!=null)
                uiRoot.uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        if (GUILayout.Button("清除所有对象"))
        {
            //uiRoot.m_AllUIs = GetComponent<UIBase>().ToString().Split(new char[] { '(', ')' })[1]
            uiRoot.m_AllUIs.Clear();
            uiRoot.root = null;
            uiRoot.normalRoot = null;
            uiRoot.fixedRoot = null;
            uiRoot.popupRoot = null;
            uiRoot.uiCamera = null;
        }

    }
}
