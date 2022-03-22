using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServicePop : UIBase
{
    // Start is called before the first frame update
    public IButton sureBtn;
    public IButton closeBtn;
    void Start()
    {
        sureBtn.onClick.AddListener(Hide);
        closeBtn.onClick = sureBtn.onClick;
    }

    // Update is called once per frame
   
}
