using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ActiveExtension
{

    /// <summary>
    /// 执行委托:简化委托为空的逻辑
    /// </summary>
    /// <param name="action"></param>
    public static void Run(this Action action)
    {
        if (action != null)
        {
            action();
        }
    }

    public static void Run<T>(this Action<T> action,T mt)
    {
        if (action!=null)
        {
            action(mt);
        }
    }

    public static void Run(this Action<int,int> action,int i1,int i2)
    {
        if (action!=null)
        {
            action(i1, i2);
        }
    }

}
