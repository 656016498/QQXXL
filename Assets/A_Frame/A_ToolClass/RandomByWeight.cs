using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomByWeight
{

    

    public static int RomdIndex(int[] Weight) {
        var index = 0;
        var aLLWegiht = 0;
        
        for (int i = 0; i < Weight.Length; i++)
        {
            aLLWegiht += Weight[i];
        }
        var r = Random.Range(0, aLLWegiht);
        var now = 0;
        for (int i = 0; i < Weight.Length; i++)
        {
            now += Weight[i];
            if (r < now)
            {
                index = i;
                break;
            }
        }
        return index;



    }

}
