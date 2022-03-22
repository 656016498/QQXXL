using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneMap : MonoBehaviour
{
    //public int Index;
  public   Transform itemP;
    //private void Start()
    //{
    //    itemP=transform.Find("CurrentLevel");
    //}
    // Start is called before the first frame update
    public void Refresh(int map) {

        for (int i = 0; i < itemP.childCount; i++)
        {
            itemP.GetChild(i).GetComponent<UILevelItem>().Refresh(map,i);
        }

    }
}
