using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AcquireLuckyUI : UIBase
{
    public Text txtGet;
    public Button btnGo;
    public Transform effet;
    Tween tween;
    // Start is called before the first frame update
    protected  void Start()
    {
        btnGo.onClick.AddListener(OnGo);
        tween=effet.DOLocalRotate(new Vector3(0, 0, 359),6, RotateMode.LocalAxisAdd).SetLoops(-1).SetEase(Ease.Linear);
    }

    private void OnGo()
    {
        UIManager.Instance.Hide<AcquireLuckyUI>();
        tween.Kill();
    }

    public void OnShow(float reward)
    {
        txtGet.text = string.Format("{0}元<size=30>提现机会</size>",reward);
        //UmengDisMgr.Instance.CountOnNumber("xstx_show", reward.ToString());
    }
}
