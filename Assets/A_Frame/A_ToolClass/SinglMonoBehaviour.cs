using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglMonoBehaviour<T>: MonoBehaviour where T :MonoBehaviour
{
    private static T _Instance;
    //private GameObject obj=null;
    private static readonly object syslock = new object();
   public static T Instance {

        get {
            if (_Instance==null)
            {
                lock (syslock) {
                    _Instance =FindObjectOfType<T>();
                    if (_Instance == null)
                    {
                        _Instance = (T)Activator.CreateInstance(typeof(T), true);
                    }
                }
            }
            return _Instance;
        }
    }
}
