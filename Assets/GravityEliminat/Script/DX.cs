using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DX : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position -= new Vector3(0, Camera.main.orthographicSize - 6.67F + 0.1F, 0); /*transform.GetComponent<PolygonCollider2D>().S*/
    }

    // Update is called once per frame
   

}
