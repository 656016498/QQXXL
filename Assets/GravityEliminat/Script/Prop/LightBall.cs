using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class LightBall : Prop
{
    public bool beginMove;
    //public bool isScattered=false;//散落
    public bool direction = true;
    public Dictionary<string, WillDown> temp = new Dictionary<string, WillDown>();
    public int needSpswnNum = 0;
    public int collisionNum = 1;
    public bool UP;
    public Vector3 upPos; public bool isRewardTime = true;
    public BlueProp desBall;
    public int one;
    public float roatY;
    Vector3 dir;

    Vector2 oneP;
    Vector2 twoP;

    public override void Init(object[] obj = null)
    {
        //Can = true;
        canTiggle = false;
        int R = Random.Range(0, 360);
        roat = 20 * Mathf.Cos(R * Mathf.PI / 180);
        roatY = 20 * Mathf.Sin(R * Mathf.PI / 180);


        Debug.Log(roat + "///" + roatY);

        dir = new Vector3(roat, roatY, 0);
        //transform.localEulerAngles = dir;
        //transform.GetComponent<Rigidbody2D>().AddForce(dir * 0.01F);
        one = 1;
        isRewardTime = true;
        //transform.GetComponent<CircleCollider2D>().enabled = true;

        //isScattered = false;
        //direction = PropManger.Instance.GetDir(transform.position);
        //temp.Clear();
        //smallNum = 0;
        upPos = UIManager.Instance.GetBase<GamePanel>().propDirTran.transform.position;
        //Debug.LogError(upPos);
        UP = false;
        needSpswnNum = 0;
        if (obj != null)
        {
            SizeType = (Porp_Size)obj[0];
            Gear = (int)obj[1];
            //transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PropSprite/" + PropType.ToString() + Gear);


            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }


            Debug.Log("光球阶级" + Gear);
            transform.GetChild(Gear - 1).gameObject.SetActive(true);
            collisionNum = (int)PropManger.Instance.GetRang(PropType.ToString(), 1, SizeType)[0];
        }
         all .Clear();
        AllDir .Clear();

        posIndex = 0;
        transform.localScale = Vector3.one * PropManger.Instance.GetRang(PropType.ToString(), 2, SizeType)[0];
        all.Add(transform.position);
        AllDir.Add(dir);
        for (int i = 0; i < collisionNum; i++)
        {
            //Debug.Log(i);
            var point = all[i];
            //Vector2 tempDir = DynamicMgr.Instance.PointRotate(all[i], newDir, 90).normalized * 0.3F;
            //Vector2 vector = point + tempDir;
            //Vector2 vecto2r = point - tempDir;
            //Debug.DrawLine(vector, vecto2r, Color.red, 10);
            //Debug.Log("EQWEQ" + vector +"WW"+ vecto2r);
            //Debug.Log("碰撞" + hit.collider);
            Vector2 newDir = AllDir[i];
            newDir =HitLine(point,newDir);
            AllDir.Add(newDir);
            //HitLine(vector, newDir);
            //HitLine(vecto2r, newDir);

        }

    }
    List<Vector2> all= new List<Vector2>();
    List<Vector2> AllDir= new List<Vector2>();
    int posIndex;
    public Vector2 HitLine(Vector2 orgin,Vector2 dir ) {
        var hit = Physics2D.Raycast(orgin, dir, 100, LayerMask.GetMask("Terrain"));

        if (hit.collider != null )
        {

            //oneP = all[i];
            //twoP = hit.point - newDir.normalized * 0.1f;
            //Debug.Log(oneP + "碰撞" + twoP + hit);
            all.Add(hit.point - dir.normalized * 0.2f);
            //Debug.DrawLine(orgin, hit.point - dir.normalized * 0.2f, Color.red, 10);
            //Debug.Log("碰撞qian" + newDir.normalized + "法线" + hit.normal);
            return Vector2.Reflect(dir.normalized, hit.normal);
            //Debug.Log("碰撞hou" + newDir);



        }
        else {


            Debug.LogError("未检测");
                }
        return Vector2.zero;
    }

        private void Start()
    {
        //isScattered = false;
        //direction = true;
        temp.Clear();
        needSpswnNum = 0;

    }
    public override void OnClick()
    {
        beginMove = true;
        //Physics2D.gravity = new Vector2(0, -1f);
        //Physics2D.positionIterations = 0;

    }


    public void DeleTimes()
    {
        
        //dir = l.normalized;
        //if (collisionNum > 0)
        //{
        //    //one *= -1;
        //    //int R = Random.Range(0, 360);


        //    //Debug.Log("DIR" + dir);
        //    //roat = 20 * Mathf.Cos(R * Mathf.PI / 180);
        //    //roatY = 20 * Mathf.Sin(R * Mathf.PI / 180);
        //    //var Range = 90;
        //    //if ( dir.x > dir.y)
        //    //{
        //    //    Range = 180;
        //    //}
        //    //else {
        //    //    Range = -90;
        //    //}
        //    //dir = DynamicMgr.Instance.PointRotate(new Vector2 (transform.position.x,transform.position.y),dir, 150);
        //    //int R = Random.Range(0, 360);
        //    //int R2 = Random.Range(0, 360);


        //    //Debug.Log(roat + "///" + roatY);

        //    ////dir = new Vector3(R, R2);
        //    //var Rang = 45;
        //    //if (transform.position.x > 0)
        //    //{
        //    //    Rang = Random.Range(60, 80);
        //    //}
        //    //else
        //    //{
        //    //    Rang = Random.Range(60, 80);

        //    //}
        //    //if (true)
        //    //{

        //    //}

        //    //transform.GetComponent<Rigidbody2D>().AddForce(l * 0.01F);
        //    //dir = l;

         
        //    collisionNum--;
        //    Debug.Log(Pool.HitWallEffect + Gear);
        //    Transform t = Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.HitWallEffect + Gear, transform.position, 0.01F);
        //    //t.eulerAngles = new Vector3(0, 0, -roat*0.5F);
        //}
        //else
        //{

            //Pool.Instance.Despawn(Pool.Prop_PoolName, transform);

            Eliminat();
            beginMove = false;
            AddBatch(temp, needSpswnNum);
            GameManager.Instance.ElimintNoColorlBall();
           //Physics2D.gravity = new Vector2(0, -9.81f);x
            //Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.HitWallEffect + Gear, transform.position, 0.01F);
        //}
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (beginMove && collision.transform.CompareTag("Wall") || collision.transform.GetComponent<SizeBall>()!=null)
    //    {
    //        //Can = false;
    //        //Observable.Timer(System.TimeSpan.FromSeconds(1F)).Subscribe(_ => {
    //        //    Can = true;
    //        //});
    //        //ContactPoint2D[] contactPoint = new ContactPoint2D[10];
    //        //int count= collision.GetContacts(contactPoint);
    //        //Vector2 add = Vector2.zero ;
    //        //for (int i = 0; i < count; i++)
    //        //{
    //        //    Debug.Log("碰撞点"+i + ":  碰撞点位置：" + contactPoint[i].point+"  "+ "  法线向量" + contactPoint[i].normal);
    //        //    add += contactPoint[i].normal;
    //        //}

    //        ////Debug
    //        //Vector2 newDir = Vector2.Reflect(dir, add.normalized);
    //        //Debug.Log("碰撞次数:" + collisionNum+"  碰撞点数量："+count +/*+  自己位置" +transform.position*/"  当前方向:"+dir.normalized+"   法线向量："+ add.normalized + "  改变后方向:"+newDir.normalized);
    //        //if (dir.normalized==)
    //        //{

    //        //}
    //        //DeleTimes(newDir);
    //        //Debug.Log("碰撞次数Dir"+dir.normalized);
    //        //return;
    //    }
    //}

    bool Can = true;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (beginMove)
        {
            if (collision.transform.CompareTag("Ball") || collision.transform.CompareTag("Soda"))
            {
                Ball tempBall = collision.transform.GetComponent<Ball>();
                if (tempBall.isEliminat) return;
                if (tempBall.ballType != BallType.ColorBall)
                {
                    GameManager.Instance.ClickSpecailBall.Add(tempBall);
                }
                else
                {
                    WillDown willDown = new WillDown(tempBall.ballType, tempBall.sort, tempBall.isFix, 1);
                    if (temp.ContainsKey(willDown.BallTypeString))
                    {
                        temp[willDown.BallTypeString].num++;
                    }
                    else
                    {
                        temp.Add(willDown.BallTypeString, willDown);
                    }
                        needSpswnNum += tempBall.Eliminat();
                }
            } 
            else if (collision.transform.CompareTag("Prop"))
            {
                Prop tempProp = collision.transform.GetComponent<Prop>();
                if (!tempProp.isReady)
                {
                    tempProp.SetReady(tempProp.transform);
                }
            }
            //else if (collision.transform.CompareTag("Sky"))
            //{
            //    //DeleTimes(-dir.normalized);
            //}
        }
    }
    //private void OnTriggerStay2D(Collider2D collision)
    //{


    public  float roat;
    Vector2 upDatadir;
    //}
    void Update()
    {


        if (beginMove)
        {
            transform.Translate(AllDir[posIndex].normalized * Time.deltaTime * 15);
            //if (posIndex + 1> all.Count)
            //{

            //}
            upDatadir = (Vector3)all[posIndex + 1]- transform.position ;

            float dic = Vector2.Distance(transform.position, all[posIndex + 1]);
            //if(dir.normalized!= AllDir[posIndex].normalized)
            //{
            //    Debug.Log("当前方向" + dir.normalized);
            //    Debug.Log("旧方向" + AllDir[posIndex].normalized);
            //    Debug.LogError("方向不一致，跳下一个目标");
            //}
            if (dic < 0.35F|| upDatadir.normalized != AllDir[posIndex].normalized)
            {
                Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.HitWallEffect + Gear, transform.position);
                AudioMgr.Instance.PlaySFX("蓝色技能--碰撞");
                transform.position = all[posIndex + 1];
                posIndex++;
                if (!GameManager.Instance.OverGame)
                {
                    CameraManager.Instance.SetShake(PropManger.Instance.GetShackLevel(PropType.ToString(), Gear, SizeType), PropManger.Instance.GetShackTime(PropType.ToString(), Gear, SizeType));
                }
                //Debug.Log("XIAOB"+posIndex);
                if (posIndex>=AllDir.Count-1)
                {
                    beginMove = false;
                    DeleTimes();
                    return;
                }
                //Debug.Log("方向"+oldDic.normalized);
            }
            //else if (dic > 0.35F)
            //{

            //}
            //if (Gear == 3 && UP)
            //{
            //    transform.GetComponent<Rigidbody2D>().MovePosition(transform.position + new Vector3(0, direction ? -90 : 90, 0) * Time.deltaTime * 0.25F);
            //}
            //else
            //{
            //transform.GetComponent<Rigidbody2D>().MovePosition(transform.position + dir * Time.deltaTime * 0.6F * one);
            //}|

        }
        //if (UP&&transform.position.y> upPos.y&&isRewardTime)
        //{
        //    //UP = false;
        //    Debug.Log("EQWEQWEQWEQWRRR");
        //    DeleTimes();
        //    isRewardTime = false;
        //    //Observable.Timer(System.TimeSpan.FromSeconds(1)).Subscribe(_ => {
        //    //    isRewardTime = true;
        //    //});
        //}


    }

    public override void InitReady()
    {

        isReady = false;
        CanFusion = true;

    }
  
}



