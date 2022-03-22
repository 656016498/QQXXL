using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEffect : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem mParticleSystem;
    void Start()
    {
        
    }

    
    /// <summary>
    /// 播放特效
    /// </summary>
    public void PlayEffects() 
    {
        if (mParticleSystem != null)
        {
            mParticleSystem.Play();
        }
        //XDebug.Log("播放特效————————");
    }
    
}
