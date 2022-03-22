using UnityEngine.UI;

public class TwistedQaustionUI : UIBase
{
    public Button btnClose1, btnClose2;
    private void Start()
    {
        btnClose1.onClick.AddListener(OnClose);
        btnClose2.onClick.AddListener(OnClose);
    }

    private void OnClose()
    {
        UIManager.Instance.Hide<TwistedQaustionUI>();
    }
}
