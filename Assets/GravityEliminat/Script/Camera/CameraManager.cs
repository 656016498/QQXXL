using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
public class CameraManager : SinglMonoBehaviour<CameraManager>
{
    public Transform downLine;
    public bool tow = false;
    public float shakeLevel = 10f;// 震动幅度
    public float setShakeTime = 0.5f;   // 震动时间
    public float shakeFps = 45f;    // 震动的FPS

    public bool isshakeCamera = false;// 震动标志
    private float fps;
    private float shakeTime = 0.0f;
    private float frameTime = 0.0f;
    private float shakeDelta = 2;
    private Camera selfCamera;
    public float cameraOffset;
    public float lastDownY;//最下面球位置
    public Vector3 originalPos;
    void OnEnable()
    {
        selfCamera = gameObject.GetComponent<Camera>();
    }


    public void SetShake(float xshakeLevel,float xshakeTime) {

        Camera.main.DOShakePosition(xshakeTime,new Vector3(0.05F,0.05F)* xshakeLevel,10).OnComplete(()=> {

            transform.localPosition = Vector3.zero;

        });
        //originalPos = transform.position;
        //shakeLevel = xshakeLevel;
        //isshakeCamera = true;
        //shakeTime = xshakeTime;
        //fps = shakeFps;
        //frameTime = 0.03f;
        //shakeDelta = 0.02f;
        //yOffset = 0F;
    }

    public void EndShake() {

       
        transform.localPosition = Vector3.zero;
        //transform.position = originalPos;
        selfCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        isshakeCamera = false;

    }

    void OnDisable()
    {

        selfCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        isshakeCamera = false;

    }

    // Update is called once per frame
    float yOffset = 0f;
    float time = 0;
    public bool CanMoveCamera = false;
    float tempY;
    public Transform pos = null;

    /// <summary>
    /// 是否移动相机
    /// </summary>
    public void BeginMove() {

        if (GameManager.Instance.level.needMoveCamera)
        {
            Observable.TimeInterval(System.TimeSpan.FromSeconds(0.5f)).Subscribe(_ =>
        {
            if (GameManager.Instance.OverGame) return;
            pos = GameManager.Instance.GetBestDownPos();
            Debug.Log(">>>>" + (lastDownY - 1) + "当前最低点位置" + pos.position.y + GameManager.Instance.OverGame);
            if ( pos.position.y< lastDownY)
            {
                CanMoveCamera = true;
                tow = true;
            }

            Observable.TimeInterval(System.TimeSpan.FromSeconds(0.5f)).Subscribe(_x => {
                if (CanMoveCamera) return;
                pos = GameManager.Instance.GetBestDownPos();
                Debug.Log(">>>>" + (lastDownY - 1) + "当前最低点位置" + pos.position.y + GameManager.Instance.OverGame);
                if (pos.position.y < lastDownY)
                {
                    CanMoveCamera = true;
                    tow = true;
                }
                Observable.TimeInterval(System.TimeSpan.FromSeconds(1.5f)).Subscribe(_x3 => {
                    if (CanMoveCamera) return;
                    pos = GameManager.Instance.GetBestDownPos();
                    Debug.Log(">>>>" + (lastDownY - 1) + "当前最低点位置" + pos.position.y + GameManager.Instance.OverGame);
                    if (pos.position.y < lastDownY)
                    {
                        CanMoveCamera = true;
                        tow = true;
                    }

                });
            });
            
        });
        }

    }
      

    void Update()
    {
        //if (isshakeCamera)
        //{
        //    if (shakeTime > 0)
        //    {
        //        shakeTime -= Time.deltaTime;
        //        if (shakeTime <= 0)
        //        {
        //            //enabled = false;
        //        }
        //        else
        //        {

        //            frameTime += Time.deltaTime;
        //            if (frameTime > 1.0 / fps)
        //            {
        //                frameTime = 0;
        //                //selfCamera.rect = new Rect(0, 0.005f * (-1.0f + shakeLevel * Random.value), 1.0f, 1.0f);
        //                //yOffset += ;
        //                //if (GameManager.Instance.level.haveWater)
        //                //{
        //                //    yOffset = 0;
        //                //}
        //                //else
        //                //{
        //                //    
        //                //}
        //                yOffset += shakeDelta * (-1.0f + shakeLevel * Random.value);
        //                selfCamera.transform.localPosition = new Vector3(shakeDelta * (-1.0f + shakeLevel * Random.value),/*transform.position.y*/  yOffset, -10);

        //            }
        //        }
        //    }
        //    else
        //    {
        //        EndShake();

        //    }
        //}

        if (!GameManager.Instance.OverGame && CanMoveCamera)
        {
            pos = GameManager.Instance.GetBestDownPos();
            tempY = pos.position.y + cameraOffset;
            if (tempY< transform.parent.position.y)
            {
                if (pos.position.y < lastDownY)
                {
                    transform.parent.position = new Vector3(0, Vector3.Lerp(transform.parent.position, new Vector3(0, tempY, 0), Time.deltaTime * 5).y, -10);
                }
            }
            else
            {
                lastDownY = pos.position.y;
                time += Time.deltaTime;
                if (time >= 2)
                {
                    CanMoveCamera = false;
                    time = 0;
                }
            }
        }
        //if (GameManager.Instance.level.haveWater)
        //{
        //    //if (transform.position.y < y)
        //    //{
        //    Debug.Log("EEEE:"+ GameManager.Instance.level.water.transform.position.y + cameraOffset);
        //        transform.position = new Vector3(0, Vector3.Lerp(transform.position, new Vector3(0, GameManager.Instance.level.water.transform.position.y+ cameraOffset, 0), Time.deltaTime * 5).y, -10);
        //        //MoveWater = false;
        //    //}
        //    //else { 
            
        //    //    MoveWater = true;

        //    //}
        //}
    }
    bool MoveWater = false;
    float msy = 0;
    public void StartLevelMove() {
        lastDownY = 0;
        if (GameManager.Instance.level.needMoveCamera)
        {
            Observable.TimeInterval(System.TimeSpan.FromSeconds(1.5F)).Subscribe(_ => {
                pos = GameManager.Instance.GetBestDownPos();
                transform.parent.DOMoveY(pos.position.y + cameraOffset, 1.5F).OnComplete(()=> {
                    lastDownY = pos.position.y;
                });
            });
        }
    }
    private void Start()
    {
        RecoverPos();
        transform.Find("Sky").localScale = Vector3.one *((750f / 1334f * Screen.height / Screen.width)+0.5f);
        downLine = transform.Find("DownLine");
        //EventManager.Instance.AddEvent(MEventType.LevelNextStep, MoveStep);
        EventManager.Instance.AddEvent(MEventType.WaterMove, Move);
    }
    public void MoveStep(object[] agr) {
        //RecoverPos();
        int R = 0;
        if (GameManager.Instance.level.haveWater)
        {
            R = GameManager.Instance.level.CameraParent.childCount - 1;
            transform.position = GameManager.Instance.level.CameraParent.GetChild(R).position + new Vector3(0, 0, -10);
            Observable.TimeInterval(System.TimeSpan.FromSeconds(1)).Subscribe(_ =>
            {
                MoveCamera(GameManager.Instance.level.CameraParent.GetChild(0).position);
            });
        }
        else
        {
            R = GameManager.Instance.level.CameraParent.childCount - 1 - (int)agr[0];
            //}
            Observable.TimeInterval(System.TimeSpan.FromSeconds(1)).Subscribe(_ =>
            {
                MoveCamera(GameManager.Instance.level.CameraParent.GetChild(R).position);
            });
        }
    }

    

    public void MoveCamera(Vector3 target) {
        transform.DOMoveY(target.y, 2f)/*.SetDelay(1)*/;
    }
  

    public void SetCameraPos(Vector3 target)
    {
        transform.parent.position =new Vector3 (0, target.y, -10);
    }

    public void RecoverPos() { 
        
        transform.parent.position = new Vector3(0, 0, -10);
        downLine.gameObject.SetActive(false);


    }
    float y = 0; 
    public void Move(object[] arg)
    {
        //y= originalPos.y+ (float)arg[0];
        //MoveWater = true;
        transform.parent.DOBlendableMoveBy(new Vector3(0, (float)arg[0], 0), 5).OnUpdate(() =>
        {
            //originalPos = transform.position;
        });
    }

    /// <summary>
    /// 开启道具遮罩
    /// </summary>
    public void ShowPropMask() {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HidePropMask() {

        transform.GetChild(0).gameObject.SetActive(false);
    }

    public bool IsInScerrn(float orgin) {

        if (orgin>(transform.position.y- Screen.height/100 / 2)&& orgin < (transform.position.y + Screen.height/100 / 2))
        {
            return true;
        }
        return false;
    }


    public void SetDownLinePos(System.Action callBall) {

        if (GameManager.Instance.allBallParent.childCount == 0)
        {
            callBall();
            return;
        }
       
        List<Transform> all = new List<Transform>();
        for (int i = 0; i < GameManager.Instance.allBallParent.childCount; i++)
        {
            all.Add(GameManager.Instance.allBallParent.GetChild(i));
        }
        all.Sort((x, y) => -x.transform.position.y.CompareTo(y.transform.position.y));
        downLine.transform.position = all[0].transform.position+new Vector3 (0,0.5F,0);
        downLine.gameObject.SetActive(true);
        downLine.DOMove(new Vector3 (0,-Camera.main.orthographicSize, downLine.transform.position.z),1).OnComplete(()=> {

            if (callBall!=null)
            {
                callBall();
            }
            //downLine.gameObject.SetActive(true) = false;
            downLine.gameObject.SetActive(false);
        });
    }
}
