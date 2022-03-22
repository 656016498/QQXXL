using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Singlton<T> where T:class
{
    private static T _Instance;
    private static readonly object SyncObject = new object();

    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                lock (SyncObject)
                {
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
