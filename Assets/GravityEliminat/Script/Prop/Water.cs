using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
public class Water : MonoBehaviour
{

    bool addFore=false;
    float fore;
    float offset;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddEvent (MEventType.WaterMove, Move);
        offset = CameraManager.Instance.transform.position.y - transform.position.y;
    }

    public void Move(object[] arg) {

        AudioMgr.Instance.PlaySFX("汽水增长");
        fore = (float)arg[0] / GameManager.Instance.level.riseNum;
        Debug.Log("WATERMove");
        transform.DOBlendableMoveBy(new Vector3(0, (float)arg[0], 0), 5).OnUpdate(()=> {

            //CameraManager.Instance.transform.position = transform.position + new Vector3(0, offset, -10);

        });
        ;
        //Observable.Timer(System.TimeSpan.FromSeconds(0.5F)).Subscribe(_ => {

        //    /*addFore = false; */
        //    //addFore = true;

        //});
    }
    private void OnDestroy()
    {
        EventManager.Instance.RemoveEvetn (MEventType.WaterMove, Move);

    }
    //private void (Collision2D collision)
    //{

    //}
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (addFore)
    //    {
    //        if (collision.transform.CompareTag("Ball")&& collision.transform.CompareTag("Prop"))
    //        {
    //            Debug.LogError("添加力");
    //            collision.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2 (0,90)*5);
    //        }
    //    }
    //}
    //    private void Update()
    //    {
    //        if (addFore)
    //        {
    //            EventManager.Instance.ExecuteEvent(MEventType.BallAddFore, fore);
    //        }
    //    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{

    //}
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.transform.CompareTag("Ball") || collision.transform.CompareTag("Prop"))
    //    {
    //        Debug.Log(00);
    //        collision.transform.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 20);
    //    }
    //}
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.transform.CompareTag("Ball") || collision.transform.CompareTag("Prop"))
    //    {
    //        Debug.Log(11);
    //        collision.transform.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 20);
    //    }
    //}

    //private void  (Collider2D collision)
    //{
    //    //if (addFore)
    //    //{

    //    //}
    //}

}
