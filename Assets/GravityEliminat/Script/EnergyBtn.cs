using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBtn : MonoBehaviour
{
    public void PlayBig()
    {

        AudioMgr.Instance.PlaySFX("蓄力技能弹出");

    }

    public void PlaySmall()
    {

        AudioMgr.Instance.PlaySFX("蓄力技能收回");

    }
}
